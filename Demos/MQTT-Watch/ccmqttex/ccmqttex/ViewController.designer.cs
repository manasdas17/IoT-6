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

namespace ccmqttex
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnConnect { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnPush { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lbConnected { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lbStatus { get; set; }

		[Action ("btnConnect_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void btnConnect_TouchUpInside (UIButton sender);

		[Action ("btnPush_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void btnPush_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (btnConnect != null) {
				btnConnect.Dispose ();
				btnConnect = null;
			}
			if (btnPush != null) {
				btnPush.Dispose ();
				btnPush = null;
			}
			if (lbConnected != null) {
				lbConnected.Dispose ();
				lbConnected = null;
			}
			if (lbStatus != null) {
				lbStatus.Dispose ();
				lbStatus = null;
			}
		}
	}
}
