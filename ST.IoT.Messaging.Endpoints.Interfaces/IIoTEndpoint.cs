using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ST.IoT.Messaging.Endpoints.Interfaces
{
    public interface IIoTEndpoint
    {
        void Rez();
        void DeRez();
    }

    public interface IRequestReplySendEndpoint : IIoTEndpoint
    {
        Task<Reply> SendAsync<Request, Reply>(Request request, CancellationToken cancellationToken = default(CancellationToken)) where Request : class where Reply : class;
    }

    public interface IRequestReplyReceiveEndpoint<Request, Reply> : IRequestReplySendEndpoint
         where Request : class where Reply : class
    {
        Task<Reply> ProcessRequestAsync<Request, Reply>(Request request);

        event EventHandler<EventArgs> ReceivedMessage;
    }
}
