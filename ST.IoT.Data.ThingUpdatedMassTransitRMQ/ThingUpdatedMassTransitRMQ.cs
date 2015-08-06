using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using ST.IoT.Data.Interfaces;
using ST.IoT.Messaging.Messages.Push;

namespace ST.IoT.Data.ThingUpdatedMassTransitRMQ
{
    public class ThingUpdatedMassTransitRMQ : IThingUpdated
    {
        private IBusControl _bus;
        private BusHandle _handle;
        private const string _address_base = "rabbitmq://localhost/";
        private const string _update_queue_spec = "rabbitmq://localhost/minion_updated";

        public ThingUpdatedMassTransitRMQ()
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
                var endpoint = await _bus.GetSendEndpoint(new Uri(_update_queue_spec));
                endpoint.Send(new ThingUpdatedMessage(thing_json));
            }
            catch (Exception)
            {
            }
        }
    }
}
