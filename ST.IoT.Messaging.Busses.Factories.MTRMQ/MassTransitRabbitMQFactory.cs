using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MassTransit;
using MassTransit.RabbitMqTransport;
using NLog;

namespace ST.IoT.Messaging.Busses.Factories.MTRMQ
{
    [Export(typeof(IMassTransitRabbitMQFactory))]
    public class MassTransitRabbitMQFactory : IMassTransitRabbitMQFactory
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private string _baseURL = "rabbitmq://localhost";
        private string _username = "iot";
        private string _password = "iot";

        public class Wiring
        {
            public IBusControl BusControl { get; set; }
            public IRabbitMqHost Host { get; set; }
            public BusHandle BusHandle { get; set; }
        }

        public class Consumer<T, R> : IConsumer<T> where T : class where R : class
        {
            public ConsumeContext<T> Context { get; set; }
            private Func<Consumer<T, R>, Task<R>> _handler;
            public Consumer(Func<Consumer<T, R>, Task<R>> handler)
            {
                _handler = handler;
            }

            public async Task Consume(ConsumeContext<T> context)
            {
                this.Context = context;
                var result = await _handler(this);
                context.RespondAsync(result);
            }
        }

        public List<Wiring> _wirings = new List<Wiring>(); 

        public MassTransitRabbitMQFactory()
        {
            _logger.Info("Created");
        }

        public void Start()
        {
            
        }

        public void Stop()
        {
            _wirings.ForEach(w =>
            {
                w.BusHandle.Stop();
            });
        }

        public IRequestClient<T, V> CreateRequestClient<T, V>(string queueName)
            where T : class
            where V : class
        {
            var the_wire = wire();

            _logger.Info("Creating request client: {0}", queueName);
            var address = _baseURL + "/" + queueName;
            var result = the_wire.BusControl.CreateRequestClient<T, V>(new Uri(address), TimeSpan.FromSeconds(500));
            _logger.Info("Created");
            return result;
        }

        public Consumer<T,R> CreateConsumer<T, R>(string queue_name, Func<Consumer<T, R>, Task<R>> handler) where T : class where R : class
        {
            var wiring = new Wiring();

            var consumer = new Consumer<T,R>(handler);
            _logger.Info("Wiring bus for consumer");

            wiring.BusControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                _logger.Info("Creating host");

                wiring.Host = x.Host(new Uri(_baseURL), h =>
                {
                    h.Username(_username);
                    h.Password(_password);
                });
                x.ReceiveEndpoint(wiring.Host, queue_name, e =>
                {
                    _logger.Info("Adding receive consumer: {0}", typeof (T));
                    e.Consumer(consumer.GetType(), _ => consumer);
                });
            });

            _logger.Info("Starting bus");
            wiring.BusHandle = wiring.BusControl.Start();
            _logger.Info("Started bus");

            _wirings.Add(wiring);

            return consumer;
        }

        private Wiring wire()
        {
            var wiring = new Wiring();

            _logger.Info("Wiring bus");

            wiring.BusControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                _logger.Info("Creating host");

                wiring.Host = x.Host(new Uri(_baseURL), h =>
                {
                    h.Username(_username);
                    h.Password(_password);
                });
            });

            _logger.Info("Starting bus");
            wiring.BusHandle = wiring.BusControl.Start();
            _logger.Info("Started bus");

            _wirings.Add(wiring);
            return wiring;
        }
    }
}
