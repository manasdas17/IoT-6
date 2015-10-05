using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NLog;
using ST.IoT.API.REST.Proxy.Interfaces;
using ST.IoT.API.REST.Router.Messaging.Endpoints;
using ST.IoT.Messaging.Bus.Core;
using ST.IoT.Messaging.Messages.REST.Routing;
using ST.IoT.Messaging.Security;

namespace ST.IoT.API.REST.Proxy.OWIN
{
    [Export(typeof(IRestApiProxyHost))]
    public class OwinRestApiProxyHost : IRestApiProxyHost
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();
        public string BaseAddress { get; set; }


        private SendToRestRouterEndpoint _forwarder;
        private IRestAuthorizer _authorizer;

        [ImportingConstructor]
        public OwinRestApiProxyHost(
            ISendToRestRouterEndpoint forwarder,
            IRestAuthorizer authorizer,
            string baseAddress = "http://*:8080/")
        {
            // defaults
            BaseAddress = baseAddress;

            _forwarder = forwarder as SendToRestRouterEndpoint;
            _authorizer = authorizer;

            _logger.Info("Created on address: " + BaseAddress);
            _logger.Info("Using a " + forwarder);
        }

        public void Start()
        {
            _logger.Info("Starting");

            ServicePointManager.DefaultConnectionLimit = 20;
            ServicePointManager.MaxServicePointIdleTime = 10000;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;

            Proxy.Start(BaseAddress, _forwarder, _authorizer);

            _forwarder.Start();

            _logger.Info("Started on " + BaseAddress);
        }

        public void Stop()
        {
            _logger.Info("Stopping");

            Proxy.Stop();

            _logger.Info("Stopped");
        }
    }
}
