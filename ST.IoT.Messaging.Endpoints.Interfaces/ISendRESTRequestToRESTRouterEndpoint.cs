using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ST.IoT.Messaging.Endpoints.Interfaces
{
    public interface ISendRESTRequestToRESTRouterEndpoint : IRequestReplySendEndpoint
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }
}
