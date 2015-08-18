using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Messaging.Bus.Core;
using ST.IoT.Messaging.Messages.REST.Routing;

namespace ST.IoT.API.REST.Router.Messaging.Endpoints
{
    public interface IConsumeRestRequestEndpoint
    {
        
    }

    public class ConsumeRestRequestEndpoint : RequestReplyConsumeEndpoint<RestRequestToRouterMessage, RestRouterReplyMessage>, IConsumeRestRequestEndpoint
    {
        public ConsumeRestRequestEndpoint(string queueName = "rest_api_requests") : base(queueName)
        {
        }
    }
}
