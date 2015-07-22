using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace ST.IoT.Messaging.BusFactories.RabbitMQ
{
    public interface IRabbitBusFactory
    {
        void Start();
        void Stop();

        void AddReceiveEndpoint<T>(string name, IConsumer<T> consumer) where T : class;
        IRequestClient<T, V> CreateRequestClient<T, V>(string queueName) where T : class where V : class;
    }
}
