using ST.IoT.Messaging.Bus.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6
{
    class Program
    {
        public class foo
        {

        }

        static void Main(string[] args)
        {
            Console.WriteLine("HI2");
            EndpointParameters.Default = new EndpointParameters(new Uri("rabbitmq://192.168.0.87"), "iot", "iot", "stlth");

            var consumer = new ConsumeEndpoint<foo>("foo_q");
            consumer.Start();
            Console.ReadLine();
            consumer.Stop();
        }
    }
}
