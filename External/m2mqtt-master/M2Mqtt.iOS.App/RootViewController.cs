using System;
using System.Drawing;
using System.Net;
using Foundation;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using UIKit;
using Sockets.Plugin;
using System.Threading.Tasks;

namespace M2Mqtt.iOS.App
{
    public partial class RootViewController : UIViewController
    {
        public RootViewController(IntPtr handle) : base(handle)
        {
			/*

			*/

			Task.Run (async() => {
				await connect2 ();
			});
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