using System;

using RingCentral.SDK;

using UIKit;

namespace MyTestApp
{
	public partial class ViewController : UIViewController
	{
		public ViewController (IntPtr handle) : base (handle)
		{
		}

		private Platform platform;
		private const string appKey = "appKey";
		private const string appSecret = "appSecret";
		private const string username = "username";
		private const string extension = "extension";
		private const string password = "password";
		private const string sandboxServer = "https://platform.devtest.ringcentral.com";
		// private const string productionServer = "https://platform.ringcentral.com";

		private void Authenticate ()
		{
			if (platform == null) {
				var sdk = new SDK (appKey, appSecret, sandboxServer, "MyTestApp", "1.0.0");
				platform = sdk.GetPlatform ();
			}
			if (!platform.IsAuthorized ()) {
				platform.Authorize (username, extension, password, true);
			}
		}

		private void PrintAuthenticationStatus ()
		{
			if (platform != null && platform.IsAuthorized ()) {
				Console.WriteLine ("App is authenticated");
			} else {
				Console.WriteLine ("App is not authenticated");
			}
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.

			// do the authentication
			Authenticate ();

			// make sure authentication is successful
			PrintAuthenticationStatus ();
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}
