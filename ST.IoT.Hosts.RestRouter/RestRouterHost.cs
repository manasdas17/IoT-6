using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MassTransit.Context;
using Ninject;
using NLog;
using RabbitMQ.Client.Events;
using ST.IoT.API.REST.Router;
using ST.IoT.API.REST.Router.Plugins.Interfaces;
using ST.IoT.Hosts.Interfaces;
using ST.IoT.Messaging.Busses.Factories.MTRMQ;
using ST.IoT.Messaging.Endpoints.Interfaces;
using ST.IoT.Messaging.Messages.REST.Routing;

namespace ST.IoT.Hosts.RestRouter
{
    [Export(typeof (IHostableService))]
    public class RestRouterHost : IHostableService
    {
        private static IKernel _kernel;

  ///      [ImportMany]
//        public IEnumerable<Lazy<IRestRouterPlugin, IRestRouterPluginMetadata>> _plugins = null;
        [ImportMany]
        public IEnumerable<IRestRouterPlugin> _plugins = null;

        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private IReceiveRESTRequestForRoutingEndpoint _endpoint;

        [ImportingConstructor]
        public RestRouterHost(IReceiveRESTRequestForRoutingEndpoint endpoint)
        {
            _endpoint = endpoint;
            _endpoint.Handler = processRestMessage;
        }

        public void Start()
        {
            _logger.Info("Starting REST router");
            _kernel = new StandardKernel();

            loadPlugins();

            _logger.Info("REST router started");
        }

        public void Stop()
        {
            _logger.Info("REST router stopped");
        }


        private void loadPlugins()
        {
            _logger.Info("Loading plugins");
            var aggCatalog = new AggregateCatalog();
            var directoryCatalog = new DirectoryCatalog(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "ST.IoT.API.REST.Router.Plugins.*.dll");

            aggCatalog.Catalogs.Add(directoryCatalog);

            var container = new CompositionContainer(aggCatalog);

            _logger.Info("Composing router plugings");
            container.ComposeParts(this);
            _logger.Info("Found the following plugins:");
            _plugins.ToList().ForEach(p => Console.WriteLine(p.GetType().Name));

            _logger.Info("Started router");
        }

        private async Task<RestRouterReplyMessage> processRestMessage(MassTransitRabbitMQFactory.Consumer<RestProxyToRouterMessage, RestRouterReplyMessage> consumer)
        {
            var context = consumer.Context;

            _logger.Info("Deserializing message");
            var ms = new MemoryStream(context.Message.HttpRequest);
            var r1 = new HttpRequestMessage();
            r1.Content = new ByteArrayContent(ms.ToArray());
            r1.Content.Headers.Add("Content-Type", "application/http;msgtype=request");
            var request = r1.Content.ReadAsHttpRequestMessageAsync().Result;

            _logger.Info("Message is routed to {0}", request.RequestUri.Host);
            _logger.Info(request.ToString());

            var response = new RestRouterReplyMessage()
            {
                CorrelatedRequestInternalMessageID = context.Message.InternalMessageID
            };

            HttpResponseMessage http_response = null;

            foreach (var p in _plugins)
            {
                if (p.CanHandle(request))
                {
                    http_response = await p.HandleAsync(request);
                    break;
                }
            }

            if (http_response == null)
            {
                _logger.Warn("Didn't recognize how to route: {0}", request.RequestUri.Host);
                response.HttpResponse =
                    encode(createHttpErrorResponse(HttpStatusCode.InternalServerError,
                        "No services opted to handle this request: " + request.RequestUri));
            }

            _logger.Info("Replying");
            _logger.Info(response);

            return response;
        }

        HttpResponseMessage createHttpErrorResponse(HttpStatusCode code, string message)
        {
            var r = new HttpResponseMessage(code);
            r.Content = new StringContent(message);
            return r;
        }

        private byte[] encode(HttpResponseMessage httpResponseMessage)
        {

            _logger.Info("Encoding response message");
            var serialized = new HttpMessageContent(httpResponseMessage).ReadAsByteArrayAsync().Result;
            _logger.Info("Done encoding");
            return serialized;
        }
    }
}
