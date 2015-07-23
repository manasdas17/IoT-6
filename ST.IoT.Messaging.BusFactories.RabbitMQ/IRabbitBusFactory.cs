using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace ST.IoT.Messaging.BusFactories.RabbitMQ
{
    public interface IRabbitBusFactory
    {
        IRabbitBusFactory BaseUrl(string baseUrl);
        IRabbitBusFactory Username(string username);
        IRabbitBusFactory Password(string password);
        IRabbitBusFactory AddConsumer<T>(string queueName, Func<ConsumeContext<T>, object> handler) where T : class;

        void Start();
        void Stop();

        void AddReceiveEndpoint<T>(string name, IConsumer<T> consumer) where T : class;
        IRequestClient<T, V> CreateRequestClient<T, V>(string queueName) where T : class where V : class;

        Action<IRabbitMqHost, IRabbitMqBusFactoryConfigurator> ConfigureHook { get; set; }
    }
}
