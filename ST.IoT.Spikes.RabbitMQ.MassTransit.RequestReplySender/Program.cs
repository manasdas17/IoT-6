using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace ST.IoT.Spikes.RabbitMQ.MassTransit.RequestReplySender
{
    class Program
    {
        private static void Main(string[] args)
        {
            new Program().run().Wait();
        }

        private async Task run()
        {
            Console.WriteLine("I am the producer");
            var url = "rabbitmq://localhost";

            var busControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                var host = x.Host(new Uri(url), h =>
                {
                    h.Username("iot");
                    h.Password("iot");
                });
            });

            Console.WriteLine("Starting bus");
            var handle = busControl.Start();
            Console.WriteLine("Started bus");

            var client = busControl.CreateRequestClient<string, string>(
                new Uri(url + "/" + "test_queue"),
                TimeSpan.FromSeconds(2));

            try
            {
                Console.WriteLine(await client.Request("1"));
                Console.WriteLine(await client.Request("2"));
                Console.WriteLine(await client.Request("3"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("OMG!!! {0}", ex.Message);
            }
            finally
            {
                Console.WriteLine("Stopping queue");
                handle.Stop();
                handle.Dispose();
                Console.WriteLine("Queue stopped");
            }
        }
    }
}
