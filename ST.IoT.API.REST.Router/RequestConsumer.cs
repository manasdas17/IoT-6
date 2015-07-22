using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace ST.IoT.API.REST.Router
{
    public class RequestConsumer<T> : IConsumer<T> where T : class
    {
        public RequestConsumer()
        {
            
        }

        public async Task Consume(ConsumeContext<T> context)
        {
            Console.WriteLine("Yeah, I got a message");
            await context.RespondAsync("Back at ya!");
        }
    }
}
