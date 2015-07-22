using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ST.IoT.Messaging.HttpRequestGateway.Interfaces
{
    public interface IHttpRequestGateway
    {
        Task<HttpResponseMessage> ForwardAndGetResponseAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }
}
