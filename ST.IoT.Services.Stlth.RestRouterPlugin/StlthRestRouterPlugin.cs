using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NLog;
using ST.IoT.API.REST.Router.Plugins.Interfaces;
using ST.IoT.Services.Stlth.Endpoints;
using ST.IoT.Services.Stlth.Messages;

namespace ST.IoT.Services.Stlth.RestRouterPlugin
{
    [Export(typeof(IRestRouterPlugin))]
    public class StlthRestRouterPlugin : IRestRouterPlugin
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly ForwardToServiceEndpoint _forwarder;

        [ImportingConstructor]
        public StlthRestRouterPlugin([Import] IForwardToServiceEndpoint forwarder)
        {
            _forwarder = forwarder as ForwardToServiceEndpoint;
        }

        public bool CanHandle(HttpRequestMessage request)
        {
            var uri = request.RequestUri.Host.ToString().ToLower();
            return uri.StartsWith("stlth.") || uri.StartsWith("command.stlth.") || uri.StartsWith("data.stlth.") || uri.StartsWith("social.api.stlth.") || uri.StartsWith("core.api.stlth.");
        }

        public async Task<HttpResponseMessage> HandleAsync(HttpRequestMessage request)
        {
            _logger.Info("Handling message");

            var stlthRequest = new RestRequest(request);
            var reply = await _forwarder.SendAsync(stlthRequest);

            _logger.Info("Finished handling message");

            return reply.Response;
        }

        public void Start()
        {
            _logger.Info("Starting STLTH plugin");

            _forwarder.Start();

            _logger.Info("Started STLTH plugin");
        }

        public void Stop()
        {
            _logger.Info("Stopping STLTH plugin");

            _forwarder.Stop();

            _logger.Info("Stopped STLTH plugin");
        }
    }
}
