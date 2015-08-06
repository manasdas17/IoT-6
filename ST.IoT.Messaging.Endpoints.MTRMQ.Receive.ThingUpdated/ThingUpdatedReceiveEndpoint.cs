using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using MassTransit;
using MassTransit.RabbitMqTransport;
using ST.IoT.Messaging.Endpoints.Interfaces;
using ST.IoT.Messaging.Messages.Push;

namespace ST.IoT.Messaging.Endpoints.MTRMQ.Receive.ThingUpdated
{
    public class ThingUpdatedReceiveEndpoint : IThingUpdated
    {
        private IBusControl _bus;
        private BusHandle _handle;
        private IRabbitMqHost _host;

        private const string _address_base = "rabbitmq://localhost/";
        private const string _update_queue_spec = "rabbitmq://localhost/minion_updated";

        public class ThingUpdatedMessageConsumer : IConsumer<ThingUpdatedMessage>
        {
            private Action<string> _handler;

            public ThingUpdatedMessageConsumer(Action<string> handler)
            {
                _handler = handler;
            }
            public async Task Consume(ConsumeContext<ThingUpdatedMessage> context)
            {
                _handler(context.Message.Thing);
            }
        }

        private ThingUpdatedMessageConsumer _consumer = null;
        private Action<string> _thingUpdatedHandler = null;

        public ThingUpdatedReceiveEndpoint(Action<string> thingUpdatedHandler = null)
        {
            _thingUpdatedHandler = thingUpdatedHandler;
            var hostAddress = new Uri(_address_base);

            _consumer = new ThingUpdatedMessageConsumer(ThingWasUpdated);

            _bus = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                _host = x.Host(hostAddress, h =>
                {
                    h.Username("iot");
                    h.Password("iot");
                });

                x.ReceiveEndpoint(_host, "minion_updated_" + Guid.NewGuid().ToString()
                    , configure =>
                {
                    configure.AutoDelete(true);
                    configure.Consumer(typeof (ThingUpdatedMessageConsumer), _ => _consumer);
                });
            });

            _handle = _bus.Start();
        }

        public virtual void ThingWasUpdated(string thing_json)
        {
            if (_thingUpdatedHandler != null) _thingUpdatedHandler(thing_json);
        }
    }
}
