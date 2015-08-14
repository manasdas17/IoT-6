using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NLog;
using ST.IoT.API.REST.Proxy.Interfaces;
using ST.IoT.API.REST.Proxy.OWIN;
using ST.IoT.Hosts.Interfaces;

namespace ST.IoT.Hosts.RestProxyService
{
    [Export(typeof(IHostableService))]
    public class RestProxyServiceHost : IRestProxyServiceHost, IHostableService
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IRestApiProxyHost _proxy = null;

        public IRestApiProxyHost RestApiProxyHost { get; set; }

        [ImportingConstructor]
        public RestProxyServiceHost(IRestApiProxyHost proxy)
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
    }
}
