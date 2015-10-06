using System;

using UIKit;
using WormHoleSharp;
using MqttLib;

namespace ccmqttex
{
	public partial class ViewController : UIViewController
	{
		private IMqtt _mqttClient;
		private bool _connected = false;
		private int _i = 1;
		public ViewController (IntPtr handle) : base (handle)
		{
		}

		private Wormhole _wormHole;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.

			btnConnect.Enabled = true;
			btnPush.Enabled = false;

			lbConnected.TextColor = UIColor.Red;
			lbConnected.Text = "Not connected";
			lbStatus.Text = "...";

			_wormHole = new Wormhole ("group.tech.seamlessthingies.ccmqttex", "messageDir");
			_wormHole.ListenForMessage<string> ("emotion", (message) => {
				_mqttClient.Publish("mqttdotnet/pubtest", "Hi from your watch!", QoS.BestEfforts, false);
				InvokeOnMainThread(() => { lbStatus.Text = "got message from watch: " + _i.ToString();});
				_i++;
			});

			_mqttClient = MqttClientFactory.CreateClient ("tcp://m11.cloudmqtt.com:12360", Guid.NewGuid ().ToString (), "mike", "cloudmqtt");
			_mqttClient.Connected += (object sender, EventArgs e) => {
				InvokeOnMainThread (() => {
					btnConnect.Enabled = false;
					btnPush.Enabled = true;
					lbConnected.TextColor = UIColor.Green;
					lbConnected.Text = "Connected";
					_connected = true;
				});
			};
				
			_mqttClient.ConnectionLost += (object sender, EventArgs e) => {
				InvokeOnMainThread (() => {
					btnConnect.Enabled = true;
					btnPush.Enabled = false;
					lbConnected.TextColor = UIColor.Red;
					lbConnected.Text = "Not connected";
					_connected = false;
				});
			};

			_mqttClient.Connect (true);
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		partial void btnPush_TouchUpInside (UIButton sender)
		{
			if (_connected)
				_mqttClient.Publish("mqttdotnet/pubtest", "Hello MQTT World", QoS.BestEfforts, false);
		}

		partial void btnConnect_TouchUpInside (UIButton sender)
		{
			_mqttClient.Connect(true);
		}
	}

}

