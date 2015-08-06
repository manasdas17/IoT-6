using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ST.IoT.Messaging.Endpoints.MTRMQ.Receive.ThingUpdated;

namespace ST.IoT.API.REST.PushRequestHttpHandler
{
    public class MinionsChunkedWebRequestHandler : DelegatingHandler
    {
        private UpdateStatusManager _updateManager;

        public MinionsChunkedWebRequestHandler()
        {
            _updateManager = new UpdateStatusManager();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request.RequestUri.LocalPath.StartsWith("/listen/for/quotes/from/"))
            {
                return await startListening(request, cancellationToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }

        private Timer _t;

        private async Task<HttpResponseMessage> startListening(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting subscription");
            var response = request.CreateResponse(HttpStatusCode.Accepted);

            var thingID = request.RequestUri.Segments[5];

            response.Content = _updateManager.subscribe(thingID, cancellationToken);
            return response;
        }
    }
}
