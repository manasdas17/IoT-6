using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using ST.IoT.Messaging.Bus.Core;
using ST.IoT.Messaging.Endpoints.Interfaces;

namespace ST.IoT.API.REST.Proxy.OWIN
{
    public class RestRequestForwarder : DelegatingHandler, ISendRESTRequestToRESTRouterEndpoint
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private IRequestReplySendEndpoint<HttpRequestMessage, HttpResponseMessage> _forwarder;

        public RestRequestForwarder(IRequestReplySendEndpoint<HttpRequestMessage, HttpResponseMessage> forwarder)
        {
            _forwarder = forwarder;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await ((ISendRESTRequestToRESTRouterEndpoint)this).SendAsync(request, cancellationToken);
        }


        async Task<HttpResponseMessage> ISendRESTRequestToRESTRouterEndpoint.SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.Info("Forwarding request");
            _logger.Info(request);

            var response = await _forwarder.SendAsync(request, cancellationToken);

            _logger.Info("Got result for request");
            _logger.Info(request);
            return response;
        }

        public Task<Reply> SendAsync<Request, Reply>(Request request, CancellationToken cancellationToken = default(CancellationToken))
            where Request : class
            where Reply : class
        {
            throw new NotImplementedException();
        }

        public void Rez()
        {
        }

        public void DeRez()
        {
        }
    }
}
