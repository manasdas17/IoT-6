using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.IoT.Messaging.Endpoints.MTRMQ.Send.ThingUpdated;
using ST.IoT.Messaging.Messages.Push;

namespace ST.IoT.Spikes.MinionUpdateMessagePublisherConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var updateSender = new ThingUpdatedSendEndpoint();

            while (true)
            {
                Console.WriteLine("Press enter to send");
                Console.ReadLine();
                updateSender.ThingWasUpdated("{\"ID\": \"seamless-thingies-1\"}");
                Console.WriteLine("sent");
            }
        }
    }
}
