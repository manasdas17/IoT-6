using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoT.Spikes.MassTransit.Mono
{
    class Program
    {
        static void Main(string[] args)
        {
            var hostAddress = new Uri("rabbitmq://localhost");

            var bus = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                x.Host(hostAddress, h =>
                {
                    h.Username("iot");
                    h.Password("iot");
                });
            });

            var handle = bus.Start();
            Console.WriteLine("Running");
            Console.ReadLine();
            handle.Stop();
        }
    }
}
