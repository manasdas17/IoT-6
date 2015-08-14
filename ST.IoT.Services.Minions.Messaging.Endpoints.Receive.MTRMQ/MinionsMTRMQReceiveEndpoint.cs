using ST.IoT.Services.Minions.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;
using ST.IoT.Messaging.Busses.Factories.MTRMQ;
using ST.IoT.Messaging.Endpoints.MTRMQ;
using ST.IoT.Services.Minions.Messages;

namespace ST.IoT.Services.Minions.Messaging.Endpoints.Receive.MTRMQ
{
    public class MinionsMTRMQReceiveEndpoint : MTRMQRequestReplyReceiveEndpoint<MinionsRequestMessage, MinionsResponseMessage>
    {
        //private const string _address = "rabbitmq://localhost/minions";

        [ImportingConstructor]
        public MinionsMTRMQReceiveEndpoint(IMassTransitRabbitMQFactory factory)
            : base(factory)
        {
        }

        public Task<MinionsResponseMessage> ProcessRequestAsync(MinionsRequestMessage request)
        {
            /*
            MinionsResponseMessage response;

            if (ReceivedRequestMessage != null)
            {
                var evtArgs = new MinionsRequestMessageReceivedEventArgs(this, request);
                ReceivedRequestMessage(this, evtArgs);
                response = evtArgs.ResponseMessage;
                return Task.FromResult(response);
            }
            return Task.FromResult(new MinionsResponseMessage(HttpStatusCode.Forbidden, "No endpoint attached"));
             * */
            return null;
        }

        public void Start()
        {
            /*
            Console.WriteLine("Creating bus");

            _bus = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                _host = x.Host(new Uri(_address), h =>
                {
                    h.Username("minion_boss");
                    h.Password("minion_boss");
                });

                x.ReceiveEndpoint(_host, "minion_requests",
                    e =>
                    {
                        e.Consumer(typeof (MinionsRequestMessageConsumer), _ => new MinionsRequestMessageConsumer(this));
                        //e.Consumer<MinionsRequestMessageConsumer>();
                    });
            });

            Console.WriteLine("Starting bus");
            _handle = _bus.Start();
            Console.WriteLine("Started bus");
             * */
        }

        public void Stop()
        {
            /*
            if (_handle != null)
            {
                _handle.Stop();
            }
             * */
        }


        public event EventHandler<MinionsRequestMessageReceivedEventArgs> ReceivedRequestMessage;
    }
}
