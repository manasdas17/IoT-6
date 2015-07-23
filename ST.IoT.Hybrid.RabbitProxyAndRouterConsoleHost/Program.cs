using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using bSeamless.IoT.Messaging.HttpRequestGateway;
using MassTransit;
using Ninject;
using ST.IoT.API.REST.Proxy.Interfaces;
using ST.IoT.API.REST.Proxy.OWIN;
using ST.IoT.API.REST.Router;
using ST.IoT.Common;
using ST.IoT.Messaging.BusFactories.RabbitMQ;
using ST.IoT.Messaging.HttpRequestGateway.Interfaces;

namespace ST.IoT.Hybrid.RabbitProxyAndRouterConsoleHost
{
    class Program
    {
        private static IKernel _kernel;

        static void Main(string[] args)
        {
            /*
            _kernel = new StandardKernel();

            _kernel.Bind<IRabbitBusFactory>().ToMethod(
                c => 
                    new RabbitBusFactory(
                        RabbitMQConstants.FullBaseUrl, RabbitMQConstants.Username, RabbitMQConstants.Password,
                        new Dictionary<string, Func<IConsumer>>()
                        {
                            { "", () => new RequestConsumer<string>() },
                        }
                    ))
                .WhenInjectedInto(typeof (RestApiRouterService));

            _kernel.Bind<IHttpRequestGateway>().To<MassTransit2RabbitMQ>();
            _kernel.Bind<IRestApiProxyHost>().To<OwinRestApiProxyHost>().InSingletonScope();
            _kernel.Bind<IRestApiRouterService>().To<RestApiRouterService>().InSingletonScope();
            
            //var proxy = _kernel.Get<IRestApiProxyHost>();
            var router = _kernel.Get<IRestApiRouterService>();

            //proxy.Start();
            router.Start();

            Console.ReadLine();

            //proxy.Stop();
            router.Stop();
            */
            Console.WriteLine("Bye!");
        }
    }
}
