using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace M2Mqtt.PortableTest.ConsoleRunner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            /*
            // create client instance 
            MqttClient client = new MqttClient("192.168.0.50");

            // register to message received 
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // subscribe to the topic "/home/temperature" with QoS 2 
            client.Subscribe(new string[] {"/home/temperature"}, new byte[] {MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});
            */

            while (true)
            {
                Console.ReadLine();

                // create client instance 
                MqttClient publisher = new MqttClient("192.168.0.50");

                string publisherId = Guid.NewGuid().ToString();
                publisher.Connect(publisherId);

                // publish a message on "/home/temperature" topic with QoS 2 
                publisher.Publish("/home/temperature", Encoding.UTF8.GetBytes("HI there mike!"),
                    MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            }
        }

        private static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var msg = Encoding.UTF8.GetString(e.Message);
            Console.WriteLine(msg);
        }
    }
}
