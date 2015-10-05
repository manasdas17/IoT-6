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
using ST.IoT.Messaging.Messages.REST.Routing;
using ST.IoT.Messaging.Security;

namespace ST.IoT.API.REST.Proxy.OWIN
{
    public class RestRequestForwarder : DelegatingHandler
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private IRestAuthorizer _authorizer;
        private IRequestReplySendEndpoint<RestRequestToRouterMessage, RestRouterReplyMessage> _forwarder;

        public RestRequestForwarder(
            IRequestReplySendEndpoint<RestRequestToRouterMessage, RestRouterReplyMessage> forwarder,
            IRestAuthorizer authorizer)
        {
            _authorizer = authorizer;
            _forwarder = forwarder;
        }

        // called by OWIN when a request is availbable
        // THIS is wherre REST requests are first handled in the system
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var body = request.Content.ReadAsStringAsync().Result;
            return await _authorizer.ProcessAsync(request, cancellationToken, process);
        }

        private async Task<HttpResponseMessage> process(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var outgoing = new RestRequestToRouterMessage(request);

            _logger.Info("Sending to router");

            var reply = await _forwarder.SendAsync(outgoing);

            _logger.Info("Received back from router");

            return reply.HttpResponse;

        }
    }
}
