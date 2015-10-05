using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ST.IoT.Messaging.Security
{
    public interface IRestAuthorizer
    {
        Task<HttpResponseMessage> ProcessAsync(HttpRequestMessage request, CancellationToken cancellationToken, 
            Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> successContinution);
    }
}