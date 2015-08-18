using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Ninject;
using NLog;
using ST.IoT.API.REST.Proxy.Interfaces;
using ST.IoT.API.REST.Proxy.OWIN;
using ST.IoT.Hosts.Minions;
using ST.IoT.Hosts.RestProxyService;
using ST.IoT.Hosts.RestRouter;
using ST.IoT.Hosts.SignalR.Things;
using ST.IoT.Hosts.Stlth;
using ST.IoT.Messaging.Bus.Core;
using ST.IoT.Messaging.Endpoints.Things.Updates;

namespace ST.IoT.Aggregates.Stlth
{

    internal class Program
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            new Program().run();
        }

        private void run()
        {
            //routerOnly();
            //proxyAndRouter();
            //justStlth();
            runStlthAndMinions();
        }

        private void runStlthAndMinions()
        {
            _logger.Info("Starting");

            var kernel = new StandardKernel();

            EndpointParameters.Default = new EndpointParameters(new Uri("rabbitmq://localhost"), "iot", "iot", "stlth");

            kernel.Bind<IThingUpdatedPublishEndpoint>().To<ThingUpdatedPublishEndpoint>();

            // wire them
            RestProxyServiceHost.wire(kernel);
            RestRouterHost.wire(kernel);
            MinionsServiceHost.wire(kernel);
            StlthServiceHost.wire(kernel);

            //start em up!
            RestProxyServiceHost.start();
            RestRouterHost.start();
            MinionsServiceHost.start();
            StlthServiceHost.start();

            //var sr = new SignalRThingsUpdateHost();

            _logger.Info("Up and running");

            Console.ReadLine();

            _logger.Info("Shutting down");

            RestRouterHost.stop();
            MinionsServiceHost.stop();
            RestProxyServiceHost.stop();
            StlthServiceHost.stop();

            _logger.Info("Stopped");
        }

        private void justStlth()
        {
            var kernel = new StandardKernel();

            EndpointParameters.Default = new EndpointParameters(new Uri("rabbitmq://localhost"), "iot", "iot", "stlth");

            // wire them
            StlthServiceHost.wire(kernel);

            //start em up!
            StlthServiceHost.start();

            Console.ReadLine();

            StlthServiceHost.stop();
        }

        private void proxyAndRouter()
        {
            var kernel = new StandardKernel();

            EndpointParameters.Default = new EndpointParameters(new Uri("rabbitmq://localhost"), "iot", "iot", "stlth");

            // wire them
            RestProxyServiceHost.wire(kernel);
            RestRouterHost.wire(kernel);

            //start em up!
            RestProxyServiceHost.start();
            RestRouterHost.start();

            Console.ReadLine();

            RestRouterHost.stop();
            RestProxyServiceHost.stop();

        }

        private void routerOnly()
        {
            var kernel = new StandardKernel();

            EndpointParameters.Default = new EndpointParameters(new Uri("rabbitmq://localhost"), "iot", "iot", "stlth");

            // wire them
            RestRouterHost.wire(kernel);

            //start em up!
            RestRouterHost.start();

            Console.ReadLine();

            RestRouterHost.stop();
        }

        private void proxyOnly()
        {
            var kernel = new StandardKernel();

            EndpointParameters.Default = new EndpointParameters(new Uri("rabbitmq://localhost"), "iot", "iot", "stlth");

            // wire them
            RestProxyServiceHost.wire(kernel);

            //start em up!
            RestProxyServiceHost.start();

            Console.ReadLine();

            RestProxyServiceHost.stop();
        }


        public class foo
        {
            public string Message { get; set; }
            public override string ToString()
            {
                return Message;
            }
        }
    }
}
