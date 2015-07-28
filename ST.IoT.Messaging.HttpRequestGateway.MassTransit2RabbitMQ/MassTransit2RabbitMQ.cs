using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using NLog;
using ST.IoT.Messaging.BusFactories.RabbitMQ;
using ST.IoT.Messaging.HttpRequestGateway.Interfaces;
using ST.IoT.Messaging.Messages.REST.Routing;

namespace bSeamless.IoT.Messaging.HttpRequestGateway
{
    public class MassTransit2RabbitMQ : DelegatingHandler, IHttpRequestGateway
    {
        private IRabbitBusFactory _busFactory;

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public MassTransit2RabbitMQ(IRabbitBusFactory busFactory)
        {
            _busFactory = busFactory;

            _logger.Info("Starting bus factory");
            _busFactory.Start();
            _logger.Info("Started log factory");
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.Info("Preparing request to rabbit");
            _logger.Info(request);
            var result = await ForwardAndGetResponseAsync(request, cancellationToken);
            _logger.Info("Got result from rabbit");
            _logger.Info(request);
            return result;
        }

        public async Task<HttpResponseMessage> ForwardAndGetResponseAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.Info("Forwarding request to rabbit - serializing request");
            var serialized = new HttpMessageContent(request).ReadAsByteArrayAsync().Result;
            _logger.Info("Serialized request");

            HttpResponseMessage response = null;

            try
            {
                _logger.Info("Creaing request client rest_api_request");
                var client = _busFactory.CreateRequestClient<RestProxyToRouterMessage, RestRouterReplyMessage>("rest_api_requests");
                _logger.Info("Preparing rest proxy to router message");
                var the_request = new RestProxyToRouterMessage()
                {
                    HttpRequest = serialized
                };
                _logger.Info("Sending request");
                
                var reply = await client.Request(the_request, cancellationToken);
                _logger.Info("Sent message, waiting for task to complete");
                _logger.Info("Completed task");

                _logger.Info("Deserializing response");
                var ms = new MemoryStream(reply.HttpResponse);
                var r1 = new HttpResponseMessage();
                r1.Content = new ByteArrayContent(ms.ToArray());
                r1.Content.Headers.Add("Content-Type", "application/http;msgtype=response");
                response = r1.Content.ReadAsHttpResponseMessageAsync().Result;
                _logger.Info("The response is: {0}", response);
            }
            catch (Exception ex)
            {
                _logger.Error("Exception processing request: {0}", ex.Message);

                response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            return response;
        }
    }
}
