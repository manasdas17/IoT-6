using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ST.IoT.Messaging.Endpoints.Interfaces;

namespace ST.IoT.Messaging.Endpoints.Generic.Core
{
    public abstract class RequestReplySendEndpoint : IRequestReplySendEndpoint, IDisposable
    {
        public abstract Task<Reply> SendAsync<Request, Reply>(Request request, CancellationToken cancellationToken = default(CancellationToken))
            where Request : class
            where Reply : class;

        public virtual void Rez()
        {
        }

        public virtual void DeRez()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}
