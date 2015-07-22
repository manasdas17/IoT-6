using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using MassTransit.RabbitMqTransport;
using ST.IoT.Spikes.RabbitMQ.MassTransit.Messages;

namespace ST.IoT.Spikes.RabbitMQ.MassTransit.RequestReplyReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("I am the consumer");
            var busControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                var url = "rabbitmq://localhost";
                var queue = "test_queue";

                var host = x.Host(new Uri(url), h =>
                {
                    h.Username("iot");
                    h.Password("iot");
                });

                x.ReceiveEndpoint(host, queue, e =>
                {
                    e.Consumer<RequestConsumer>();
                    //e.AutoDelete(true);
                    //e.Exclusive(true);
                });
            });

            Console.WriteLine("Starting bus");
            var handle = busControl.Start();
            Console.WriteLine("Bus started");

            Console.ReadLine();

            Console.WriteLine("Stopping bus");
            handle.Stop();
            //handle.Dispose();
            Console.WriteLine("Stopped");
        }
    }

    public class RequestConsumer : IConsumer<IRequestMessage>
    {
        public async Task Consume(ConsumeContext<IRequestMessage> context)
        {
            Console.WriteLine("Got: " + context.Message.TheMessage);
            context.Respond(new ReplyMesssge() {TheReply = "reply to: " + context.Message.TheMessage});
            await Task.FromResult(0);
        }
    }
 
    public class RequestConsumer2 : IConsumer<string>
    {
        public async Task Consume(ConsumeContext<string> context)
        {
            Console.WriteLine("Got: " + context.Message);
            context.Respond(new ReplyMesssge() { TheReply = "reply to: " + context.Message });
            await Task.FromResult(0);
        }
    }
}
