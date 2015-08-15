using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Messaging.Busses.Factories.MTRMQ;
using ST.IoT.Messaging.Endpoints.Generic.Core;

namespace ST.IoT.Messaging.Endpoints.MTRMQ
{
    /*
    public class RequestReplyMessageReceivedEvent<Request, Reply> : EventArgs
        where Request : class
        where Reply : class
    {

        public Request Message { get; private set; }
        public Reply Response { get; set; }

        public RequestReplyMessageReceivedEvent(Request request)
        {
            Message = request;
        }
    }
    */
    
    public class MTRMQRequestReplyReceiveEndpoint<Request, Reply> : ST.IoT.Messaging.Endpoints.Generic.Core.RequestReplyReceiveEndpoint<Request, Reply>
        where Request : class
        where Reply : class
    {
        private IMassTransitRabbitMQFactory _factory;
        public MTRMQRequestReplyReceiveEndpoint(IMassTransitRabbitMQFactory factory)
        {
            _factory = factory;

            _factory.CreateConsumer<Request, Reply>("minion_requests", handler);
        }

        private async Task<Reply> handler(MassTransitRabbitMQFactory.Consumer<Request, Reply> consumer)
        {
            var reply = await ProcessRequestAsync<Request, Reply>(consumer.Context.Message);
            return reply;
        }
    }
}
