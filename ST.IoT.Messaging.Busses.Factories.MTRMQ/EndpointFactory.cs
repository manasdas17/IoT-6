using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;
using NLog;
using ST.IoT.Messaging.Busses.Factories.Core;

namespace ST.IoT.Messaging.Busses.Factories.MTRMQ
{
    public class EndpointFactory
    {
        public class Wiring
        {
            public IBusControl BusControl { get; set; }
            public IRabbitMqHost Host { get; set; }
            public BusHandle BusHandle { get; set; }
        }
        public List<Wiring> _wirings = new List<Wiring>(); 

        private readonly Uri _baseAddress;
        private readonly string _username;
        private readonly string _password;
        private readonly string _virtualHost;


        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public EndpointFactory(Uri baseAddress, string username, string password, string virtualHost = "", bool inMemory = false)
        {
            _baseAddress = baseAddress;
            _username = username;
            _password = password;
            _virtualHost = virtualHost;

            if (!string.IsNullOrEmpty(virtualHost))
            {
                _baseAddress = new Uri(_baseAddress + "/" + virtualHost);
            }
        }

        public SendEndpoint CreateSendEndpoint(string queueName)
        {
            return new SendEndpoint();
        }

        public ReceiveEndpoint CreateReceiveEndpoint(IReceiveEndpointParameters parameters)
        {
            throw new NotImplementedException();
        }

        public RequestReplySendEndpoint<Request, Reply> CreateRequestReplySendEndpoint<Request, Reply>(IRequestReplyEndpointParameters parameters)
            where Request : class
            where Reply : class
        {
            _logger.Info("Wiring bus");

            var wiring = new Wiring();

            //var m = new MessageRequestClient();

            wiring.BusControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                _logger.Info("Creating host");
                wiring.Host = x.Host(_baseAddress, h => 
                {
                    h.Username(_username);
                    h.Password(_password);
                });

            });

            _logger.Info("Starting bus");

            wiring.BusHandle = wiring.BusControl.Start();

            _wirings.Add(wiring);
            
            _logger.Info("Started bus");

            var endpoint = new RequestReplySendEndpoint<Request, Reply>();
            return endpoint;
        }

        public RequestReplyReceiveEndpoint<Request, Reply> CreateRequestReplyReceiveEndpoint<Request, Reply>(IRequestReplyEndpointParameters parameters)
            where Request : class
            where Reply : class
        {
            var endpoint = new RequestReplyReceiveEndpoint<Request, Reply>();
            return endpoint;
        }
    }
}
