using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using RabbitMQ.Client.Framing.Impl;

namespace ST.IoT.Messaging.Busses.Factories.MTRMQ
{
    public interface IMassTransitRabbitMQFactory
    {
        IRequestClient<T1, T2> CreateRequestClient<T1, T2>(string queueName) where T1 : class where T2 : class;

        MassTransitRabbitMQFactory.Consumer<T, R> CreateConsumer<T, R>(string queue_name, 
            Func<MassTransitRabbitMQFactory.Consumer<T, R>, Task<R>> handler) where T : class where R : class;

        void Start();
        void Stop();
    }
}
