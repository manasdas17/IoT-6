using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using MassTransit.RabbitMqTransport;
using ST.IoT.Common;

namespace ST.IoT.Messaging.BusFactories.RabbitMQ
{
    public class RabbitBusFactory : IRabbitBusFactory
    {
        private IBusControl _busControl;
        public IBusControl BusControl { get { return _busControl; } }

        private BusHandle _busHandle;
        public BusHandle BusHandle { get { return _busHandle; } }

        private IRabbitMqHost _host;
        public IRabbitMqHost Host { get { return _host; } }

        private IRabbitMqBusFactoryConfigurator _configurator;
        public IRabbitMqBusFactoryConfigurator Configurator { get { return _configurator; } }

        private Uri _baseUrl;
        private Dictionary<string, Func<IConsumer>> _consumers;

        public RabbitBusFactory() : this(RabbitMQConstants.FullBaseUrl, RabbitMQConstants.Username, RabbitMQConstants.Password,
            null)
        {
        }

        public RabbitBusFactory(Uri baseUrl, string username, string password, Dictionary<string, Func<IConsumer>> consumers) 
        {
            _baseUrl = baseUrl;
            _consumers = consumers;

            _busControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                _configurator = x;
                _host = x.Host(baseUrl, h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
                if (_consumers != null)
                {
                    foreach (var consumer in _consumers)
                    {
                        _configurator.ReceiveEndpoint(_host, consumer.Key, e =>
                        {
                            e.Consumer(() => consumer.Value());
                        });
                    }
                }
            });
        }

        public void AddReceiveEndpoint<T>(string queueName, IConsumer<T> consumer) where T : class
        {
            /*
            _configurator.ReceiveEndpoint(_host, "rest_api_requests", e =>
            {
                //e.Consumer(() => consumer);
                //e.Consumer<RequestConsumer>();
            });
             * */
        }

        public void Start()
        {
            _busHandle = _busControl.Start();
        }

        public void Stop()
        {
            if (_busHandle != null) _busHandle.Stop(TimeSpan.FromSeconds(30));
        }

        public IRequestClient<T, V> CreateRequestClient<T, V>(string queueName) where T : class where V : class
        {
            var result = _busControl.CreateRequestClient<T, V>(new Uri(_baseUrl + queueName), TimeSpan.FromSeconds(50));
            return result; 
        }
    }
}
