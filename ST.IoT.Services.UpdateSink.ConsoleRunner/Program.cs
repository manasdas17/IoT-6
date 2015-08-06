using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Messaging.Endpoints.MTRMQ.Receive.ThingUpdated;

namespace ST.IoT.Services.UpdateSink.ConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Console update message sink running");
            var receiver = new ThingUpdatedReceiveEndpoint(Console.WriteLine);
            Console.ReadLine();
        }
    }
}
