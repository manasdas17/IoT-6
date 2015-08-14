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
using ST.IoT.Messaging.Bus.Core;
using ST.IoT.Messaging.Endpoints.Interfaces;
using ST.IoT.Messaging.HttpRequestGateway.Interfaces;

namespace ST.IoT.API.REST.Proxy.OWIN
{
    [Export(typeof(IRestApiProxyHost))]
    public class OwinRestApiProxyHost : IRestApiProxyHost
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();
        public string BaseAddress { get; set; }

        /*
        public ISendRESTRequestToRESTRouterEndpoint RestRouterEndpoint
        {
            get { return Proxy.HttpRequestGateway as ISendRESTRequestToRESTRouterEndpoint; }
        }

        private ISendRESTRequestToRESTRouterEndpoint _endpoint;
        */

        private IRequestReplySendEndpoint<HttpRequestMessage, HttpResponseMessage>  _forwarder;

        [ImportingConstructor]
        public OwinRestApiProxyHost(IRequestReplySendEndpoint<HttpRequestMessage, HttpResponseMessage> forwarder,
            string baseAddress = "http://*:8080/")
//            ISendRESTRequestToRESTRouterEndpoint forwarder)
        {
            // defaults
            BaseAddress = baseAddress;
            _forwarder = forwarder;

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

            Proxy.Start(BaseAddress, _forwarder);

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
