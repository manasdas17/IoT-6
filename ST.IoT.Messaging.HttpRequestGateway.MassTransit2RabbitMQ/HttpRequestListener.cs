using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Messaging.HttpRequestGateway.Interfaces;

namespace ST.IoT.Messaging.HttpRequestGateway.MassTransit2RabbitMQ
{
    public class HttpRequestListener : IHttpRequestListener
    {
        public Task<System.Net.Http.HttpResponseMessage> ProcessAsync(System.Net.Http.HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }
    }
}
