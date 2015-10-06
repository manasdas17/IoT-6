using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MqttLib;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ST.IoT.Spikes.CloudMQTT.TestClient
{
    class Program
    {
        private IMqtt _client;
        private MqttClient _mqtt;

        private static void Main(string[] args)
        {
            new Program().run();
        }

        private void run()
        { 
            /*
            _client = MqttClientFactory.CreateClient("tcp://m11.cloudmqtt.com:12360", "1", "mike", "cloudmqtt");

            // Setup some useful client delegate callbacks
            _client.Connected += _client_Connected;
            _client.ConnectionLost += _client_ConnectionLost;
            _client.PublishArrived += ClientOnPublishArrived;
            
            _client.Connect(true);
            */

            _mqtt = new MqttClient("m11.cloudmqtt.com", 12360, false, MqttSslProtocols.None);
            _mqtt.ConnectionClosed += (sender, args) => { };
            _mqtt.MqttMsgSubscribed += (sender, args) => { };
            _mqtt.MqttMsgPublishReceived += (sender, args) =>
            {

            };
            _mqtt.Connect("1", "mike", "cloudmqtt");
            _mqtt.Subscribe(new[] {"mqttdotnet/pubtest/#"}, new[] {MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE});
            Console.ReadLine();

            _client.Disconnect();

            Console.ReadLine();
        }

        private bool ClientOnPublishArrived(object sender, PublishArrivedArgs e)
        {
            Console.WriteLine("Publish arrived");
            Console.WriteLine("Received Message");
            Console.WriteLine("Topic: " + e.Topic);
            Console.WriteLine("Payload: " + e.Payload);
            Console.WriteLine();
            return true;
        }

        private void _client_ConnectionLost(object sender, EventArgs e)
        {
            Console.WriteLine("Disconnected");
        }

        private void _client_Connected(object sender, EventArgs e)
        {
            Console.WriteLine("Connected");

            _client.Subscribe("mqttdotnet/pubtest/#", QoS.BestEfforts);
            //_client.Publish("mqttdotnet/pubtest", "Hello MQTT World", QoS.BestEfforts, false);
        }
    }
}
