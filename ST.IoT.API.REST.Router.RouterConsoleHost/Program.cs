using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Ninject;
using ST.IoT.Common;
using ST.IoT.Messaging.BusFactories.RabbitMQ;
using ST.IoT.Messaging.Messages.REST.Routing;

namespace ST.IoT.API.REST.Router.RouterConsoleHost
{
    class Program
    {
        private static IKernel _kernel;

        static void Main(string[] args)
        {
            _kernel = new StandardKernel();

            _kernel.Bind<IRabbitBusFactory>()
                .ToMethod(c =>
                {
                    var factory = new RabbitBusFactory();
                    factory.BaseUrl("rabbitmq://localhost").Username("iot").Password("iot");
                    /*
                    factory.ConfigureHook = (h, c2) =>
                    {
                        c2.ReceiveEndpoint(h, "rest_api_requests", e =>
                        {
                            e.Consumer<RequestConsumer2>();
                        });
                    };
                     * */
                    /*
                    factory.ConfigureHook = (h, c2) =>
                    {
                        c2.ReceiveEndpoint(h, "rest_api_requests", e =>
                        {
                            e.Consumer<ST.IoT.Messaging.RabbitMQ.RequestConsumer<RestProxyToRouterMessage>>();
                        });
                    };
                    */
                    factory.AddConsumer<RestProxyToRouterMessage>("rest_api_requests", processRestMessage);
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

        static object processRestMessage(ConsumeContext<RestProxyToRouterMessage> context)
        {
            Console.WriteLine(context.Message);

            return new RestRouterReplyMessage()
            {
                CorrelatedRequestInternalMessageID = context.Message.InternalMessageID
            };
        }
    }
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
}
