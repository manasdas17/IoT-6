using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Messaging.Endpoints.Interfaces
{
    public interface IEndpointFactory
    {
        IRequestReplySendEndpoint CreateRequestReplySendEndpoint<Request, Reply>();
    }
}
