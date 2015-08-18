using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Ninject;
using NLog;
using ST.IoT.API.REST.Proxy.Interfaces;
using ST.IoT.API.REST.Proxy.OWIN;
using ST.IoT.API.REST.Router.Messaging.Endpoints;
using ST.IoT.Hosts.Interfaces;
using ST.IoT.Messaging.Bus.Core;
using ST.IoT.Messaging.Messages.REST.Routing;

namespace ST.IoT.Hosts.RestProxyService
{
    [Export(typeof(IHostableService))]
    public class RestProxyServiceHost : IRestProxyServiceHost
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IRestApiProxyHost _proxy = null;
        private static IKernel _kernel;
        private static IRestProxyServiceHost _host;

        public IRestApiProxyHost RestApiProxyHost { get; set; }

        [ImportingConstructor]
        public RestProxyServiceHost([Import] IRestApiProxyHost proxy)
        {
            _logger.Info("Created using " + proxy.ToString());
            _proxy = proxy;
        }

        public void Start()
        {
            _logger.Info("Starting");
            _proxy.Start();
            _logger.Info("Started");
        }

        public void Stop()
        {
            _logger.Info("Stopping");
            _proxy.Stop();
            _logger.Info("Stopped");
        }

        public static void wire(StandardKernel kernel)
        {
            _kernel = kernel;
            _kernel.Bind<ISendToRestRouterEndpoint>().To<SendToRestRouterEndpoint>();
            _kernel.Bind<IRestApiProxyHost>().To<OwinRestApiProxyHost>();
            _kernel.Bind<IRestProxyServiceHost>().To<RestProxyServiceHost>();
        }

        public static void start(IKernel kernel = null)
        {
            var k = kernel ?? _kernel;
            if (k == null) throw new Exception("Not wired!");

            _host = _kernel.Get<IRestProxyServiceHost>();
            _host.Start();
        }

        public static void stop()
        {
            _host.Stop();
        }
    }
}
