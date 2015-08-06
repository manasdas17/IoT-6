using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;
using RabbitMQ.Client;
using ST.IoT.Messaging.Endpoints.Interfaces;
using ST.IoT.Messaging.Messages.Push;

namespace ST.IoT.Messaging.Endpoints.MTRMQ.Send.ThingUpdated
{
    public class ThingUpdatedSendEndpoint : IThingUpdated
    {
        private IBusControl _bus;
        private BusHandle _handle;
        private IRabbitMqHost _host;

        private const string _address_base = "rabbitmq://localhost/";
        private const string _update_queue_spec = "rabbitmq://localhost/minion_updated";

        public ThingUpdatedSendEndpoint()
        {
            var hostAddress = new Uri(_address_base);

            _bus = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                x.Host(hostAddress, h =>
                {
                    h.Username("iot");
                    h.Password("iot");
                });
            });

            _handle = _bus.Start();
        }

        public async void ThingWasUpdated(string thing_json)
        {
            try
            {
                _bus.Publish(new ThingUpdatedMessage(thing_json));

                //var endpoint = await _bus.GetSendEndpoint(new Uri(_update_queue_spec));
                //endpoint.Send(new ThingUpdatedMessage(thing_json));
            }
            catch (Exception)
            {
            }
        }
    }
}
