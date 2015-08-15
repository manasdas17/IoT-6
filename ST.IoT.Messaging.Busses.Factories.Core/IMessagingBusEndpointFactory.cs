using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Messaging.Busses.Factories.Core
{
    public interface IMessagingBusEndpointFactory<F, S, R, Req, Rep> where Req : class where Rep : class
    {
        F Initialize(IEndpointFactoryInitializationParameters parameters);
        S CreateSendEndpoint(ISendEndpointParameters parameters);
        R CreateReceiveEndpoint(IReceiveEndpointParameters parameters);

        IRequestReplyEndpoint<Req, Rep> CreateRequestReplyEndpoint<Request, Reply>(
            IRequestReplyEndpointParameters parameters) where Request : class where Reply : class;
    }
}
