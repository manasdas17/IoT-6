using System;
using System.Drawing;
using System.Net;
using Foundation;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using UIKit;
using Sockets.Plugin;
using System.Threading.Tasks;
using MqttLib;

namespace M2Mqtt.iOS.App
{
    public partial class RootViewController : UIViewController
    {
		private IMqtt _client;
		private static string _connectionString = "tcp://m11.cloudmqtt.com:12360";
		private static string _clientId = "1";
		private static string _username = "mike";
		private static string _password = "cloudmqtt";
        public RootViewController(IntPtr handle) : base(handle)
        {
			/*

			*/

			_client = MqttClientFactory.CreateClient(_connectionString, _clientId, _username, _password);

			// Setup some useful client delegate callbacks
			_client.Connected += _client_Connected;
			_client.ConnectionLost += _client_ConnectionLost;
			_client.PublishArrived += ClientOnPublishArrived;

			_client.Connect(true);

			Task.Run (async() => {
				//await connect2 ();
			});
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
			_client.Publish("mqttdotnet/pubtest", "Hello MQTT World", QoS.BestEfforts, false);
		}



		async Task connect()
		{
			var address = "192.168.0.50";
			var port = 1883;
			var client = new TcpSocketClient();
			await client.ConnectAsync (address, port);
			await client.DisconnectAsync ();
		}

		async Task connect2()
		{
			// create client instance 
			var client = new MqttClient("192.168.0.50");

			// register to message received 
			client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

			string clientId = Guid.NewGuid().ToString();
			client.Connect(clientId);

			// subscribe to the topic "/home/temperature" with QoS 2 
			client.Subscribe(new string[] {"/home/temperature"}, new byte[] {MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});
		}

		private static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // handle message received 
        }


        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        #endregion
    }
}