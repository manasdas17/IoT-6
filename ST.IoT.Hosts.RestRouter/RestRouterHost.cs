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
using ST.IoT.API.REST.Router.Messaging.Endpoints;
using ST.IoT.API.REST.Router.Plugins.Interfaces;
using ST.IoT.Hosts.Interfaces;
using ST.IoT.Messaging.Bus.Core;
using ST.IoT.Messaging.Busses.Factories.MTRMQ;
using ST.IoT.Messaging.Messages.REST.Routing;

namespace ST.IoT.Hosts.RestRouter
{
    public interface IRestRouterHost : IHostableService
    {
        void Start();
        void Stop();
    }

    [Export(typeof(IHostableService))]
    public class RestRouterHost : IRestRouterHost
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private IRestApiRouterService _routerService;
        private static IKernel _kernel;

        private static IRestRouterHost _host;

        public RestRouterHost(IRestApiRouterService routerService)
        {
            _routerService = routerService;
        }

        public void Start()
        {
            _routerService.Start();
        }

        public void Stop()
        {
            _routerService.Stop();
        }

        public static void start(IKernel kernel = null)
        {
            var k = kernel ?? _kernel;
            if (k == null) throw new Exception("Not wired!");

            _host = _kernel.Get<IRestRouterHost>();
            _host.Start();

        }

        public static void stop()
        {
            _host.Stop();
        }

        public static void wire(IKernel kernel)
        {
            _logger.Info("Starting wiring");

            _kernel = kernel;
            _kernel.Bind<IRestRouterHost>().To<RestRouterHost>();
            _kernel.Bind<IRestApiRouterService>().To<RestRouterService>();
            _kernel.Bind<IConsumeRestRequestEndpoint>().To<ConsumeRestRequestEndpoint>();

            _logger.Info("Finished wiring");
        }
    }
}
