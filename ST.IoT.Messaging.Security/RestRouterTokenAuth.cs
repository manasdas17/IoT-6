using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ST.IoT.Messaging.Security
{
    public class RestRouterTokenAuth : IRestRouterTokenAuth
    {
        public async Task<HttpResponseMessage> ProcessAsync(
            HttpRequestMessage request, CancellationToken cancellationToken, 
            Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> successContinution)
        {
            var headers = request.Headers;
            if (headers.Authorization == null)
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            var auth = headers.Authorization;
            if (auth.Scheme != "Token")
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            if (auth.Parameter != @"token='48a3b0c8-aed7-4ced-99ef-ddbd46e12a14'")
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            request.Headers.Add("TenantID", "stlth");

            return await successContinution(request, cancellationToken);
        }
    }
}
