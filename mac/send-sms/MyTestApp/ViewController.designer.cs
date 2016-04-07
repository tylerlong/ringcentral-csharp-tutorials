// WARNING
//
// This file has been generated automatically by Xamarin Studio Enterprise to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MyTestApp
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UITextField messageTextField { get; set; }

		[Outlet]
		UIKit.UITextField receiverNumberTextField { get; set; }

		[Action ("SendMessage:")]
		partial void SendMessage (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (receiverNumberTextField != null) {
				receiverNumberTextField.Dispose ();
				receiverNumberTextField = null;
			}

			if (messageTextField != null) {
				messageTextField.Dispose ();
				messageTextField = null;
			}
		}
	}
}
