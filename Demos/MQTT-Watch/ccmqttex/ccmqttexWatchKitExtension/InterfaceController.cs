using System;

using WatchKit;
using Foundation;
using WormHoleSharp;

namespace ccmqttexWatchKitExtension
{
	public partial class InterfaceController : WKInterfaceController
	{
		private Wormhole _wormHole;

		public InterfaceController (IntPtr handle) : base (handle)
		{
		}

		public override void Awake (NSObject context)
		{
			base.Awake (context);

			// Configure interface objects here.
			Console.WriteLine ("{0} awake with context", this);

			_wormHole = new Wormhole ("group.tech.seamlessthingies.ccmqttex", "messageDir");
		}

		public override void WillActivate ()
		{
			// This method is called when the watch view controller is about to be visible to the user.
			Console.WriteLine ("{0} will activate", this);
		}

		public override void DidDeactivate ()
		{
			// This method is called when the watch view controller is no longer visible to the user.
			Console.WriteLine ("{0} did deactivate", this);
		}

		partial void OnHappyPressed (WatchKit.WKInterfaceButton sender)
		{
			_wormHole.PassMessage("emotion", "happy");
		}
	}
}

