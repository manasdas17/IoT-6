using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bSeamless.IoT.Messaging.HttpRequestGateway;
using Ninject;
using NLog;
using ST.IoT.API.REST.Proxy.Interfaces;
using ST.IoT.API.REST.Proxy.OWIN;
using ST.IoT.Messaging.BusFactories.RabbitMQ;
using ST.IoT.Messaging.HttpRequestGateway.Interfaces;

namespace ST.IoT.API.REST.Proxy.RestProxyConsoleHost
{
    class Program
    {
        private static IKernel _kernel;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            _logger.Info("Starting");

            _kernel = new StandardKernel();
            _kernel.Bind<IRabbitBusFactory>()
                .ToMethod(c =>
                {
                    _logger.Info("Initializing bus factory");
                    var factory = new RabbitBusFactory();
                    factory.BaseUrl("rabbitmq://localhost").Username("iot").Password("iot");
                    _logger.Info("Configured bus factory");
                    return factory;
                })
                .WhenInjectedInto<MassTransit2RabbitMQ>();

            _kernel.Bind<IHttpRequestGateway>().To<MassTransit2RabbitMQ>();
            _kernel.Bind<IRestApiProxyHost>().To<OwinRestApiProxyHost>().InSingletonScope();

            _logger.Info("Creating and starting proxy host");
            var host = _kernel.Get<IRestApiProxyHost>();
            host.Start();
            _logger.Info("Started proxy host");

            _logger.Info("Press enter to stop");
            Console.ReadLine();

            _logger.Info("Starting shutdown");
            host.Stop();

            foreach (var target in LogManager.Configuration.AllTargets) target.Dispose();

            _logger.Info("Now exiting");
        }
    }
}
