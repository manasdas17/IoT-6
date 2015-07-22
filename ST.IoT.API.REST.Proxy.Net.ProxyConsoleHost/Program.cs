using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bSeamless.IoT.Messaging.HttpRequestGateway;
using Ninject;
using ST.IoT.API.REST.Proxy.Interfaces;
using ST.IoT.API.REST.Proxy.OWIN;
using ST.IoT.Messaging.HttpRequestGateway.Interfaces;

namespace ST.IoT.API.REST.Proxy.RestProxyConsoleHost
{
    class Program
    {
        private static IKernel _kernel;

        static void Main(string[] args)
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IRestApiProxyHost>().To<OwinRestApiProxyHost>().InSingletonScope();
            _kernel.Bind<IHttpRequestGateway>().To<MassTransit2RabbitMQ>();

            var host = _kernel.Get<IRestApiProxyHost>();
            host.Start();

            Console.ReadLine();

            host.Stop();
        }
    }
}
