using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using NLog;
using ST.IoT.Messaging.Busses.Factories.MTRMQ;
using ST.IoT.Messaging.Endpoints.Interfaces;
using ST.IoT.Messaging.Messages.REST.Routing;

namespace ST.IoT.Messaging.Endpoints.Send.RESTRequestForProcessing
{
    [Export(typeof(ISendRESTRequestToRESTRouterEndpoint))]
    public class RabbitMQViaMassTransit : ISendRESTRequestToRESTRouterEndpoint
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private IBusControl _busControl;
        private MassTransit.RabbitMqTransport.IRabbitMqHost _host;
        private string _baseUrl;
        private string _password;
        private BusHandle _busHandle;
        private IMassTransitRabbitMQFactory _factory;
        private IRequestClient<RestProxyToRouterMessage, RestRouterReplyMessage> _client;

        [ImportingConstructor]
        public RabbitMQViaMassTransit(IMassTransitRabbitMQFactory factory)
        {
            _logger.Info("Starting bus factory");

            _factory = factory;
            _client = _factory.CreateRequestClient<RestProxyToRouterMessage, RestRouterReplyMessage>("rest_api_requests");
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.Info("Forwarding request to rabbit - serializing request");
            var serialized = new HttpMessageContent(request).ReadAsByteArrayAsync().Result;
            _logger.Info("Serialized request");

            HttpResponseMessage response = null;
            try
            {
                var the_request = new RestProxyToRouterMessage()
                {
                    HttpRequest = serialized
                };
                _logger.Info("Sending request");

                var reply = await _client.Request(the_request, cancellationToken);
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

        public Task<Reply> SendAsync<Request, Reply>(Request request, CancellationToken cancellationToken = default(CancellationToken))
            where Request : class
            where Reply : class
        {
            throw new NotImplementedException();
        }

        public void Rez()
        {
        }

        public void DeRez()
        {
        }
    }
}
