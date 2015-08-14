using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ST.IoT.Messaging.Endpoints.Interfaces;

namespace ST.IoT.Messaging.Endpoints.Generic.Core
{
    public abstract class RequestReplyReceiveEndpoint<Request, Reply> : IRequestReplyReceiveEndpoint<Request, Reply>, IDisposable
            where Request : class
            where Reply : class
    {
        public class RequestReplyEndpointReceivedMessageEventArgs : EventArgs
        {
            public object Message { get; private set; }
            public object Response { get; set; }

            public RequestReplyEndpointReceivedMessageEventArgs(object request)
            {
                Message = request;
            }

        }
/*
        public async Task<Reply> ProcessRequestAsync(Request request) 
        {
            if (ReceivedMessage != null)
            {
                var evt = new RequestReplyEndpointReceivedMessageEventArgs(request);
                ReceivedMessage(this, evt);
                return await Task.FromResult((Reply)evt.Response);
            }
            return await Task.FromResult(default(Reply));
        }
        */

        public async Task<Reply> ProcessRequestAsync<Request, Reply>(Request request)
        {
            if (ReceivedMessage != null)
            {
                var evt = new RequestReplyEndpointReceivedMessageEventArgs(request);
                ReceivedMessage(this, evt);
                return await Task.FromResult((Reply)evt.Response);
            }
            return await Task.FromResult(default(Reply));
        }

        public void Rez()
        {
        }

        public void DeRez()
        {
        }

        public void Dispose()
        {
        }


        public event EventHandler<EventArgs> ReceivedMessage;

        public Task<Reply> SendAsync<Request, Reply>(Request request, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
            where Request : class
            where Reply : class
        {
            throw new NotImplementedException();
        }
    }
}
