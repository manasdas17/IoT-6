using ST.IoT.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Messaging.Endpoints.MTRMQ.ThingUpdated
{
    public class ThingUpdatedEndpoint : IThingUpdated
    {
        public ThingUpdatedEndpoint()
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

        public void ThingWasUpdated(string thing_json)
        {
        }
    }
}
