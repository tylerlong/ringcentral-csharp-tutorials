using System;
using System.Collections.Generic;
using RingCentral.SDK;
using UIKit;

namespace MyTestApp
{
	public partial class ViewController : UIViewController
	{
		private string syncToken;

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


			// get initial syncToken
			var request = new RingCentral.SDK.Http.Request ("/restapi/v1.0/account/~/extension/~/message-sync",
				              new List<KeyValuePair<string, string>> {
					new KeyValuePair<string, string> ("dateFrom", DateTime.UtcNow.AddHours (-1).ToString ("o")),
					new KeyValuePair<string, string> ("syncType", "FSync"),
				});
			var response = platform.Get (request);
			syncToken = response.GetJson ().SelectToken ("syncInfo.syncToken").ToString ();


			// subscribe
			var sub = new RingCentral.Subscription.SubscriptionServiceImplementation () { _platform = platform };
			sub.AddEvent ("/restapi/v1.0/account/~/extension/~/message-store");
			sub.Subscribe (ActionOnNotification, null, null);
		}

		void ActionOnNotification (object message)
		{
			var request = new RingCentral.SDK.Http.Request ("/restapi/v1.0/account/~/extension/~/message-sync",
				              new List<KeyValuePair<string, string>> {
					new KeyValuePair<string, string> ("syncToken", syncToken),
					new KeyValuePair<string, string> ("syncType", "ISync"),
				});
			var response = platform.Get (request);
			syncToken = response.GetJson ().SelectToken ("syncInfo.syncToken").ToString ();

			foreach (var m in ExtractMessages(response.GetJson())) {
				InvokeOnMainThread (() => {
					consoleTextView.Text += "\n" + m;
				});
			}
		}

		private HashSet<string> ids = new HashSet<string> ();

		private List<string> ExtractMessages (Newtonsoft.Json.Linq.JObject jo)
		{
			var result = new List<string> ();

			foreach (var jt in jo.SelectToken ("records")) {
				var subject = jt.SelectToken ("subject").ToString ();
				var direction = jt.SelectToken ("direction").ToString ();
				var id = jt.SelectToken ("id").ToString ();
				if (!ids.Contains (id)) {
					result.Add (string.Format ("{0} : {1}", direction, subject));
					ids.Add (id);
				}
			}
			result.Reverse ();

			return result;
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
