using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Ninject;
using NLog;
using NLog.LayoutRenderers.Wrappers;
using ST.IoT.API.REST.Router.Plugins.Interfaces;
using ST.IoT.Common;
using ST.IoT.Messaging.BusFactories.RabbitMQ;
using ST.IoT.Messaging.Messages.REST.Routing;

namespace ST.IoT.API.REST.Router.RouterConsoleHost
{
    class Program
    {
        private static IKernel _kernel;

        [ImportMany] public IEnumerable<Lazy<IRestRouterPlugin, IRestRouterPluginMetadata>> _plugins = null;

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            new Program().run();
        }

        private void run()
        {
            _logger.Info("Starting REST router");
            _kernel = new StandardKernel();

            var aggCatalog = new AggregateCatalog();
            var directoryCatalog = new DirectoryCatalog(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "ST.IoT.API.REST.Router.Plugins.*.dll");

            aggCatalog.Catalogs.Add(directoryCatalog);

            var container = new CompositionContainer(aggCatalog);

            _logger.Info("Composing router plugings");
            container.ComposeParts(this);
            _logger.Info("Found the following plugins:");
            _plugins.ToList().ForEach(p => Console.WriteLine(p.Value.GetType().Name));

            _kernel.Bind<IRabbitBusFactory>()
                .ToMethod(c =>
                {
                    var factory = new RabbitBusFactory();
                    factory.BaseUrl("rabbitmq://localhost")
                        .Username("iot")
                        .Password("iot")
                        .AddConsumer<RestProxyToRouterMessage>("rest_api_requests", processRestMessage);
                    return factory;
                })
                .WhenInjectedInto<RestApiRouterService>();
                
            _kernel.Bind<IRestApiRouterService>().To<RestApiRouterService>().InSingletonScope();

            var router = _kernel.Get<IRestApiRouterService>();

            _logger.Info("Starting router");
            router.Start();
            _logger.Info("Started router, press enter to exit");

            Console.ReadLine();

            _logger.Info("Stopping");
            router.Stop();
            _logger.Info("stopped");
        }

        async Task<object> processRestMessage(ConsumeContext<RestProxyToRouterMessage> context)
        {
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

            if (request.RequestUri.Host.StartsWith("minions.") || request.RequestUri.Host.Contains("the-mionions.io"))
            {
                _logger.Info("got a request for minions");
                var plugin = _plugins.FirstOrDefault(p => p.Metadata.Services == "minions");
                if (plugin != null)
                {
                    var http_response = await plugin.Value.HandleAsync(request);
                    response.HttpResponse = encode(http_response);
                }
                else
                {
                    _logger.Info("There is no service to handle minions");
                }
            }
            else
            {
                _logger.Warn("Didn't recognize how to route: {0}", request.RequestUri.Host);
            }

            _logger.Info("Replying");
            _logger.Info(response);

            return context.RespondAsync(response);
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
