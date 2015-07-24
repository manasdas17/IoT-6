﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using ST.IoT.Messaging.BusFactories.RabbitMQ;
using ST.IoT.Messaging.HttpRequestGateway.Interfaces;
using ST.IoT.Messaging.Messages.REST.Routing;

namespace bSeamless.IoT.Messaging.HttpRequestGateway
{
    public class MassTransit2RabbitMQ : DelegatingHandler, IHttpRequestGateway
    {
        private IRabbitBusFactory _busFactory;

        public MassTransit2RabbitMQ(IRabbitBusFactory busFactory)
        {
            _busFactory = busFactory;
            _busFactory.Start();
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return ForwardAndGetResponseAsync(request, cancellationToken);
        }

        public async Task<HttpResponseMessage> ForwardAndGetResponseAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var serialized = new HttpMessageContent(request).ReadAsByteArrayAsync().Result;

            try
            {
                var client = _busFactory.CreateRequestClient<RestProxyToRouterMessage, RestRouterReplyMessage>("rest_api_requests");
                var reply = await client.Request(
                    new RestProxyToRouterMessage()
                    {
                        HttpRequest =  serialized
                    },
                    cancellationToken);

                Console.WriteLine(reply);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            var response = new HttpResponseMessage(HttpStatusCode.Accepted);
            return response;
        }
    }
}
