using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using NLog;
using ST.IoT.API.REST.Router.Plugins.Interfaces;
using ST.IoT.API.REST.Util.UrlPatterns;
using ST.IoT.Services.Minions.Endpoints;
using ST.IoT.Services.Minions.Messages;

namespace ST.IoT.API.REST.Router.Plugins.Minions
{
    [Export(typeof(IRestRouterPlugin))]
    [ExportMetadata("Services", "minions")]
    public class MinionRestRouterPlugin : IRestRouterPlugin
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private UrlPatternDispatcher _patternDispatcher;
        private ForwardToMinionsServiceEndpoint _forwarder;

        [ImportingConstructor]
        public MinionRestRouterPlugin([Import] IForwardToMinionsServiceEndpoint forwarder)
        {
            _logger.Info("Creating send endpoint");
            
            _patternDispatcher = new UrlPatternDispatcher(
                                new [] {
                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Put,
                        Parts = new[] {"/", "quote/", "for/"},
                        Handler = doQuoteFor,
                    },
                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Get,
                        Parts = new[] {"/", "get/", "latest/", "quote/", "for/"},
                        Handler = doGetLatestQuoteFor,
                    },
                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Get,
                        Parts = new[] {"/", "get/", "quotes/", "for/"},
                        Handler = doGetQuotesFor,
                    },
                    new UrlPatternDispatcher.UriPattern()
                    {
                        Method = HttpMethod.Get,
                        Parts = new[] {"/", "listen/", "for/", "quotes/", "from/"},
                        Handler = doGetQuotesFor,
                    }
                }, null);

            _forwarder = forwarder as ForwardToMinionsServiceEndpoint;
        }

        public bool CanHandle(HttpRequestMessage request)
        {
            return request.RequestUri.Host.StartsWith("minions.local") || request.RequestUri.Host.Contains("the-mionions.io");
        }

        public async Task<HttpResponseMessage> HandleAsync(HttpRequestMessage request)
        {
            _logger.Info("received message");
            _logger.Info(request);

            var segments = getSegments(request);

            var handler = _patternDispatcher.getHandler(request, segments);
            if (handler == null)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Content = new StringContent(String.Format("Unknown request of your minions: {0} {1}", request.Method, request.RequestUri.LocalPath));
                return response;
            }

            var result = await handler.Handler(request, segments);
            return result;
        }

        private string[] getSegments(HttpRequestMessage request)
        {
            var urlSegments = request.RequestUri.Segments;
            var trimmed = urlSegments.Select(s => s.TrimEnd(new char[] { '/' })).ToArray();
            var nonEmpty = trimmed.Where(s => s.Length > 0).ToArray();
            return nonEmpty;
        }


        public async Task<HttpResponseMessage> doQuoteFor(HttpRequestMessage request, string[] segments)
        {
            var uri = request.RequestUri;
            if (uri.Segments.Length != 4)
            {
                return buildResponse(HttpStatusCode.BadRequest,
                    string.Format("Too many parts in the path (expected four): {0}", uri.LocalPath));
            }

            _logger.Info("Sending the message to the minions service");

            try
            {
                var content = (await request.Content.ReadAsStringAsync()).Trim();

                try
                {
                    JObject.Parse(content);
                }
                catch (Exception)
                {
                    return buildResponse(HttpStatusCode.BadRequest,
                        string.Format("Body was not valid json: {0}", content));
                }

                var template = File.ReadAllText("message_templates/put_minion_template.json");
                var withsubs = template.Replace("{name}", uri.Segments[3]).Replace("{quote}", content);
                var json = JObject.Parse(withsubs);
                json["content"] = content;

                var reply = await _forwarder.SendAsync(new MinionsRequestMessage(json.ToString()));

                // TODO: need to analyze the minions response and put it in the HttpResponseMessage

                _logger.Info("Got response from minions service");

                var response = new HttpResponseMessage(reply.StatusCode);
                response.Content = new StringContent(reply.Response);

                _logger.Info("Reply");
                _logger.Info(response);
                return response;
            }
            catch (Exception ex)
            {
                return buildResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<HttpResponseMessage> doGetLatestQuoteFor(HttpRequestMessage request, string[] segments)
        {
            var uri = request.RequestUri;
            if (uri.Segments.Length != 6)
            {
                return buildResponse(HttpStatusCode.BadRequest,
                    string.Format("Too many parts in the path (expected five): {0}", uri.LocalPath));
            }

            _logger.Info("Sending the message to the minions service");

            try
            {
                var template = File.ReadAllText("message_templates/get_latest_quote_for_minion_template.json");
                var withsubs = template.Replace("{name}", uri.Segments[5]);
                var json = JObject.Parse(withsubs);

                var reply = await _forwarder.SendAsync(new MinionsRequestMessage(json.ToString()));

                // TODO: need to analyze the minions response and put it in the HttpResponseMessage

                _logger.Info("Got response from minions service");

                var response = new HttpResponseMessage(reply.StatusCode);
                response.Content = new StringContent(reply.Response);

                _logger.Info("Reply");
                _logger.Info(response);
                return response;
            }
            catch (Exception ex)
            {
                return buildResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<HttpResponseMessage> doGetQuotesFor(HttpRequestMessage request, string[] segments)
        {
            var uri = request.RequestUri;
            if (uri.Segments.Length != 5)
            {
                return buildResponse(HttpStatusCode.BadRequest,
                    string.Format("Too many parts in the path (expected five): {0}", uri.LocalPath));
            }

            _logger.Info("Sending the message to the minions service");

            try
            {
                var template = File.ReadAllText("message_templates/get_quotes_for_minion_template.json");
                var withsubs = template.Replace("{name}", uri.Segments[4]);
                var json = JObject.Parse(withsubs);

                var reply = await _forwarder.SendAsync(new MinionsRequestMessage(json.ToString()));

                // TODO: need to analyze the minions response and put it in the HttpResponseMessage

                _logger.Info("Got response from minions service");

                var response = new HttpResponseMessage(reply.StatusCode);
                response.Content = new StringContent(reply.Response);

                _logger.Info("Reply");
                _logger.Info(response);
                return response;
            }
            catch (Exception ex)
            {
                return buildResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private static HttpResponseMessage buildResponse(HttpStatusCode httpStatusCode, string content)
        {
            var response = new HttpResponseMessage(httpStatusCode);
            response.Content = new StringContent(content);
            return response;
        }

        public void Start()
        {
            _forwarder.Start();
        }

        public void Stop()
        {
            _forwarder.Stop();
        }
    }
}
