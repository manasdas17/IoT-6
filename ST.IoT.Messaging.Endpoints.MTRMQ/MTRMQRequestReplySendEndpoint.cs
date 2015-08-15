using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using ST.IoT.Messaging.Busses.Factories.MTRMQ;
using ST.IoT.Messaging.Endpoints.Generic.Core;

namespace ST.IoT.Messaging.Endpoints.MTRMQ
{
    public class MTRMQRequestReplySendEndpoint<Request, Reply> : RequestReplySendEndpoint, IMTRMQRequestReplySendEndpoint where Request : class where Reply : class
    {
        private IMassTransitRabbitMQFactory _factory;
        private bool _needsDeRez = false;
        private IRequestClient<Request, Reply> _client;

        [ImportingConstructor]
        public MTRMQRequestReplySendEndpoint(IMassTransitRabbitMQFactory factory)
        {
            _factory = factory;

            _client = _factory.CreateRequestClient<Request, Reply>("minion_requests");
        }

        public async override Task<Reply> SendAsync<Request, Reply>(Request request, CancellationToken cancellationToken = default(CancellationToken))
        {
            /*y = await _client.Request(request);
            return reply;
             * */
            return null;
        }

        public override void Rez()
        {
            base.Rez();
        }

        public override void DeRez()
        {
            base.DeRez();
        }

        public override void Dispose()
        {
            DeRez();
        }
    }
}
