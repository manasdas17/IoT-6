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
using ST.IoT.Services.Minions.Endpoints.Send.MTRMQ;
using ST.IoT.Services.Minions.Messages;

namespace ST.IoT.API.REST.Router.Plugins.Minions
{
    [Export(typeof(IRestRouterPlugin))]
    [ExportMetadata("Services", "minions")]
    public class MinionRestRouterPlugin : IRestRouterPlugin
    {
        private MinionsSendEndpointMTRMQ _minionsServiceFacade;

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public class UrlPatternDispatcher
        {
            public class UriPattern
            {
                public HttpMethod Method { get; set; }
                public string[] Parts { get; set; }
                public Func<HttpRequestMessage, Task<HttpResponseMessage>> Handler { get; set; }


                public bool match(HttpRequestMessage request)
                {
                    if (request.Method != Method)
                    {
                        return false;
                    }
                    var segments = request.RequestUri.Segments;
                    if (segments.Length < Parts.Length)
                    {
                        return false;
                    }
                    var i = 0;
                    foreach (var part in Parts)
                    {
                        if (part != segments[i]) return false;
                        i++;
                    }
                    return true;
                }
            }

            private List<UriPattern> _patterns;

            public UrlPatternDispatcher(MinionRestRouterPlugin outer)
            {
                _patterns = new List<UriPattern>()
                {
                    new UriPattern()
                    {
                        Method = HttpMethod.Put,
                        Parts = new[] {"/", "quote/", "for/"},
                        Handler = outer.doQuoteFor,
                    },
                    new UriPattern()
                    {
                        Method = HttpMethod.Get,
                        Parts = new[] {"/", "get/", "latest/", "quote/", "for/"},
                        Handler = outer.doGetLatestQuoteFor,
                    },
                    new UriPattern()
                    {
                        Method = HttpMethod.Get,
                        Parts = new[] {"/", "get/", "quotes/", "for/"},
                        Handler = outer.doGetQuotesFor,
                    },
                    new UriPattern()
                    {
                        Method = HttpMethod.Get,
                        Parts = new[] {"/", "listen/", "for/", "quotes/", "from/"},
                        Handler = outer.doGetQuotesFor,
                    },
                };
            }

            public UriPattern handle(HttpRequestMessage request)
            {
                foreach (var pattern in _patterns)
                {
                    if (pattern.match(request))
                    {
                        return pattern;
                    }
                }
                return null;
            }
        }

        private UrlPatternDispatcher _patternDispatcher;


        public MinionRestRouterPlugin()
        {
            _logger.Info("Creating send endpoint");
            /*
            _patternDispatcher = new UrlPatternDispatcher(this);

            _minionsServiceFacade = new MinionsSendEndpointMTRMQ();
            _minionsServiceFacade.Start();
             * */
        }

        public bool CanHandle(HttpRequestMessage request)
        {
            return request.RequestUri.Host.StartsWith("minions.local") || request.RequestUri.Host.Contains("the-mionions.io");
        }

        public async Task<HttpResponseMessage> HandleAsync(HttpRequestMessage request)
        {
            _logger.Info("received message");
            _logger.Info(request);

            var handler = _patternDispatcher.handle(request);
            if (handler == null)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Content = new StringContent(String.Format("Unknown request of your minions: {0} {1}", request.Method, request.RequestUri.LocalPath));
                return response;
            }

            var result = await handler.Handler(request);
            return result;
        }

        public async Task<HttpResponseMessage> doQuoteFor(HttpRequestMessage request)
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

                var reply = await _minionsServiceFacade.ProcessRequestAsync(new MinionsRequestMessage(json.ToString()));

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

        public async Task<HttpResponseMessage> doGetLatestQuoteFor(HttpRequestMessage request)
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

                var reply = await _minionsServiceFacade.ProcessRequestAsync(new MinionsRequestMessage(json.ToString()));

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

        public async Task<HttpResponseMessage> doGetQuotesFor(HttpRequestMessage request)
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

                var reply = await _minionsServiceFacade.ProcessRequestAsync(new MinionsRequestMessage(json.ToString()));

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
    }
}
