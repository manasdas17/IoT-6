using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace ST.IoT.Messaging.Busses.Factories.MTRMQ
{
    public class MessageConsumer<Request, Reply> : IConsumer<Request> where Request : class where Reply : class
    {
        public ConsumeContext<Request> Context { get; set; }
        private Func<MessageConsumer<Request, Reply>, Task<Reply>> _handler;

        public async Task Consume(ConsumeContext<Request> context)
        {
            var reply = await _handler(this);
            context.RespondAsync(reply);
        }
    }
}
