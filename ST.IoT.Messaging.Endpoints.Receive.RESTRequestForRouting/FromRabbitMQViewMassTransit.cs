using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Messaging.Busses.Factories.MTRMQ;
using ST.IoT.Messaging.Endpoints.Interfaces;
using ST.IoT.Messaging.Messages.REST.Routing;

namespace ST.IoT.Messaging.Endpoints.Receive.RESTRequestForRouting
{
    [Export(typeof(IReceiveRESTRequestForRoutingEndpoint))]
    public class FromRabbitMQViewMassTransit : IReceiveRESTRequestForRoutingEndpoint
    {
        private IMassTransitRabbitMQFactory _factory;

        [ImportingConstructor]
        public FromRabbitMQViewMassTransit(IMassTransitRabbitMQFactory factory)
        {
            _factory = factory;

            _factory.CreateConsumer<RestRequestToRouterMessage, RestRouterReplyMessage>("rest_api_requests", handler);
        }

        private Task<RestRouterReplyMessage> handler(MassTransitRabbitMQFactory.Consumer<RestRequestToRouterMessage, RestRouterReplyMessage> consumer)
        {
            if (Handler != null) return Handler(consumer);
            return Task.FromResult(new RestRouterReplyMessage());
        }

        public Func<MassTransitRabbitMQFactory.Consumer<RestRequestToRouterMessage, RestRouterReplyMessage>, Task<RestRouterReplyMessage>> Handler { get; set; }
    }
}
