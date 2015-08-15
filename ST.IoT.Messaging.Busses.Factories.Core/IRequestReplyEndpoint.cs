using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Messaging.Busses.Factories.Core
{
    public interface IRequestReplyEndpoint<Request, Reply> where Request : class where Reply : class
    {
        Task<Reply> SendAsync(Request request);
    }
}
