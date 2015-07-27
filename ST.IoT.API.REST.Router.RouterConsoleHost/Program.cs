using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Ninject;
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

        private static void Main(string[] args)
        {
            new Program().run();
        }

        private void run()
        {
            _kernel = new StandardKernel();

            var aggCatalog = new AggregateCatalog();
            var directoryCatalog = new DirectoryCatalog(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "ST.IoT.API.REST.Router.Plugins.*.dll");

            aggCatalog.Catalogs.Add(directoryCatalog);

            var container = new CompositionContainer(aggCatalog);
            container.ComposeParts(this);

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

            Console.WriteLine("Starting router");
            router.Start();
            Console.WriteLine("Started router");

            Console.ReadLine();

            router.Stop();
        }

        

        async Task<object> processRestMessage(ConsumeContext<RestProxyToRouterMessage> context)
        {
            var ms = new MemoryStream(context.Message.HttpRequest);
            var r1 = new HttpRequestMessage();
            r1.Content = new ByteArrayContent(ms.ToArray());
            r1.Content.Headers.Add("Content-Type", "application/http;msgtype=request");
            var request = r1.Content.ReadAsHttpRequestMessageAsync().Result;

            Console.WriteLine(request.RequestUri.Host);

            var response = new RestRouterReplyMessage()
            {
                CorrelatedRequestInternalMessageID = context.Message.InternalMessageID
            };

            if (request.RequestUri.Host.StartsWith("minions."))
            {
                var plugin = _plugins.FirstOrDefault(p => p.Metadata.Services == "minions");
                if (plugin != null)
                {
                    var resp = await plugin.Value.HandleAsync(request);
                    response.HttpResponse = encode(resp);
                }
            }
            return context.RespondAsync(response);
        }

        private byte[] encode(HttpResponseMessage httpResponseMessage)
        {
            var serialized = new HttpMessageContent(httpResponseMessage).ReadAsByteArrayAsync().Result;
            return serialized;
        }
    }
    /*
    public class RequestConsumer2 : IConsumer<RestProxyToRouterMessage>
    {
        public RequestConsumer2()
        {
            
        }
        public async Task Consume(ConsumeContext<RestProxyToRouterMessage> context)
        {
            Console.WriteLine("Got: " + context.Message);
            //context.Respond(new ReplyMesssge() { TheReply = "reply to: " + context.Message });
            await Task.FromResult(0);
        }
    }
     * */
}
