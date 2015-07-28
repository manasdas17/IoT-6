using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Context;
using NLog;

namespace ST.IoT.Messaging.RabbitMQ
{
    public class RequestConsumer<T> : IConsumer<T> where T : class
    {
        private Func<ConsumeContext<T>, Task<object>> _consumer;

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public RequestConsumer()
        {
            
        }

        public RequestConsumer(Func<ConsumeContext<T>, Task<object>> consumer)
        {
            _consumer = consumer;
        }

        public async Task Consume(ConsumeContext<T> context)
        {
            _logger.Info("Consuming message: {0}", context.Message);
            var response = await _consumer(context);
            _logger.Info("Response is: {0}", response);
            context.RespondAsync(response);
            _logger.Info("Sent response");
        }
    }
}
