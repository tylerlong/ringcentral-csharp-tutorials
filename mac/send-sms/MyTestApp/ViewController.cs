using System;
using System.Collections.Generic;

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
		private const string appKey = "33zQZlyQSPadH22cFIT0sw";
		private const string appSecret = "TgihqTjaSg2_63CmrNq6tA8n2riQTCQwGl4VoD0mRYTA";
		private const string username = "17322764403";
		private const string extension = "";
		private const string password = "RNG94405";
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

		partial void SendMessage (Foundation.NSObject sender)
		{
			var requestBody = new Dictionary<string, dynamic> {
				{ "text", messageTextField.Text },
				{ "from", new Dictionary<string, string>{ { "phoneNumber", username } } }, {
					"to",
					new List<Dictionary<string, string>> { new Dictionary<string, string> { {
								"phoneNumber",
								receiverNumberTextField.Text
							}
						}
					}
				} 
			};
			var jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject (requestBody);
			var request = new RingCentral.SDK.Http.Request ("/restapi/v1.0/account/~/extension/~/sms", jsonBody);
			var response = platform.Post (request);
			Console.WriteLine ("Sms sent, status code is: " + response.GetStatus ());
		}
	}
}
