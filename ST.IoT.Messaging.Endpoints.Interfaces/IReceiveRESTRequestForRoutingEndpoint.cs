using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Messaging.Busses.Factories.MTRMQ;
using ST.IoT.Messaging.Messages.REST.Routing;

namespace ST.IoT.Messaging.Endpoints.Interfaces
{
    public interface IReceiveRESTRequestForRoutingEndpoint
    {
        Func<MassTransitRabbitMQFactory.Consumer<RestProxyToRouterMessage, RestRouterReplyMessage>, Task<RestRouterReplyMessage>> Handler { get; set; }
    }
}
