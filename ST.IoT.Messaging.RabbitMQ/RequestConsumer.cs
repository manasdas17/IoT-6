using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Context;

namespace ST.IoT.Messaging.RabbitMQ
{
    public class RequestConsumer<T> : IConsumer<T> where T : class
    {
        private Func<ConsumeContext<T>, object> _consumer;

        public RequestConsumer()
        {
            
        }

        public RequestConsumer(Func<ConsumeContext<T>, object> consumer)
        {
            _consumer = consumer;
        }

        public Task Consume(ConsumeContext<T> context)
        {
            var response = _consumer(context);
            return context.RespondAsync(response);
        }
    }
}
