// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ccmqttexWatchKitExtension
{
	[Register ("InterfaceController")]
	partial class InterfaceController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		WatchKit.WKInterfaceButton btnHappy { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		WatchKit.WKInterfaceLabel thelabel { get; set; }

		[Action ("OnHappyPressed:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void OnHappyPressed (WatchKit.WKInterfaceButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (btnHappy != null) {
				btnHappy.Dispose ();
				btnHappy = null;
			}
			if (thelabel != null) {
				thelabel.Dispose ();
				thelabel = null;
			}
		}
	}
}
