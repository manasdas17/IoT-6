using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace FooBar
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Creating bus...");

            var busControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                var url = "rabbitmq://API.IOT.BSEAMLESS.COM/IoT";
                var queue = "rest_api_requests";

                IRabbitMqHost host = x.Host(new Uri(url), h =>
                {
                    h.Username("iot");
                    h.Password("iot");
                });

                x.ReceiveEndpoint(host, queue, e =>
                {
                    //e.Consumer<RequestConsumer>();
                });
            });

            Console.WriteLine("Starting bus...");

            var busHandle = busControl.Start();

            Console.WriteLine("Bus started");

            Console.ReadLine();

            busHandle.Stop();
        }
    }

    public interface ISimpleRequest
    {
        DateTime Timestamp { get; }
        string CustomerId { get; }
    }

    public class RequestConsumer :
    IConsumer<ISimpleRequest>
    {

        public async Task Consume(ConsumeContext<ISimpleRequest> context)
        {
            context.Respond(new SimpleResponse
            {
                CusomerName = string.Format("Customer Number {0}", context.Message.CustomerId)
            });
        }

        public interface ISimpleResponse
        {
            string CusomerName { get; }
        }

        class SimpleResponse : ISimpleResponse
        {
            public string CusomerName { get; set; }
        }
    }
}
