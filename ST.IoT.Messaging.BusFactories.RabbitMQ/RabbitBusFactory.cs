using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using MassTransit.RabbitMqTransport;
using ST.IoT.Common;
using ST.IoT.Messaging.RabbitMQ;

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

        public RabbitBusFactory()
        {
            
        }

        private string _baseUrl;
        public IRabbitBusFactory BaseUrl(string baseUrl)
        {
            _baseUrl = baseUrl;
            return this;
        }

        private string _username;
        public IRabbitBusFactory Username(string username)
        {
            _username = username;
            return this;
        }

        private string _password;
        public IRabbitBusFactory Password(string password)
        {
            _password = password;
            return this;
        }

        private class ConsumerInfo
        {
            public Func<IConsumer> Factory { get; set; }
            public string QueueName { get; set; }
            public object Consumer { get; set; }
            public Type ConsumerType { get; set; }
        }

        private List<ConsumerInfo> _consumers = new List<ConsumerInfo>();

        public IRabbitBusFactory AddConsumer<T>(string queueName, Func<ConsumeContext<T>, object> handler) where T : class
        {
            _consumers.Add(new ConsumerInfo()
            {
                Factory = () => new RequestConsumer<T>(handler),
                QueueName = queueName,
                ConsumerType = typeof(RequestConsumer<T>)
            });

            return this;
        }

        public Action<IRabbitMqHost, IRabbitMqBusFactoryConfigurator> ConfigureHook { get; set; }

        public void Start()
        {
            _busControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                _configurator = x;
                _host = x.Host(new Uri(_baseUrl), h =>
                {
                    h.Username(_username);
                    h.Password(_password);
                });
                
                if (ConfigureHook != null)
                {
                    ConfigureHook(_host, _configurator);
                }
                else
                {
                    foreach (var consumer in _consumers)
                    {
                        var consumer1 = consumer;
                        _configurator.ReceiveEndpoint(_host, consumer.QueueName, e =>
                        {
                            e.Consumer(consumer1.ConsumerType, _ => consumer1.Factory());
                        });
                    }
                }
            });

            _busHandle = _busControl.Start();
        }

        public void AddReceiveEndpoint<T>(string name, IConsumer<T> consumer) where T : class
        {
            
        }


        public void Stop()
        {
            if (_busHandle != null) _busHandle.Stop(TimeSpan.FromSeconds(30));
        }

        public IRequestClient<T, V> CreateRequestClient<T, V>(string queueName) where T : class where V : class
        {
            var address = _baseUrl + "/" + queueName;
            var result = _busControl.CreateRequestClient<T, V>(new Uri(address), TimeSpan.FromSeconds(50));
            return result; 
        }
    }
}
