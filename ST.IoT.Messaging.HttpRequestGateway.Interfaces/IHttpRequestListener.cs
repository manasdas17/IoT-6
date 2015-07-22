using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Messaging.HttpRequestGateway.Interfaces
{

    public interface IHttpRequestListener
    {
        Task<HttpResponseMessage> ProcessAsync(HttpRequestMessage request);
    }
}
