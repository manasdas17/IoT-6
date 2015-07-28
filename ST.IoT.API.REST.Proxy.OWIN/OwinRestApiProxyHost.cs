using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NLog;
using ST.IoT.API.REST.Proxy.Interfaces;
using ST.IoT.Messaging.HttpRequestGateway.Interfaces;

namespace ST.IoT.API.REST.Proxy.OWIN
{
    public class OwinRestApiProxyHost : IRestApiProxyHost
    {
        private const string _address = @"http://*:8080/";

        private Logger _logger = LogManager.GetCurrentClassLogger();

        public OwinRestApiProxyHost(IHttpRequestGateway webRequestGateway)
        {
            Proxy.HttpRequestGateway = webRequestGateway;
        }

        public void Start()
        {
            _logger.Info("Starting");

            ServicePointManager.DefaultConnectionLimit = 20;
            ServicePointManager.MaxServicePointIdleTime = 10000;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.Expect100Continue = false;

            Proxy.Start(_address);

            _logger.Info("Started");
        }

        public void Stop()
        {
            _logger.Info("Stopping");

            Proxy.Stop();

            _logger.Info("Stopped");
        }
    }
}
