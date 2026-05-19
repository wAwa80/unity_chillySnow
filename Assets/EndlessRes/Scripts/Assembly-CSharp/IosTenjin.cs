using UnityEngine;


namespace EndlessMode
{
	public class IosTenjin : BaseTenjin
	{
		public override void Init(string apiKey)
		{
			Debug.Log("iOS Initializing " + apiKey);
			base.ApiKey = apiKey;
		}

		public override void Connect()
		{
			Debug.Log("iOS Connecting " + base.ApiKey);
		}

		public override void Connect(string deferredDeeplink)
		{
			Debug.Log("Connecting with deferredDeeplink " + deferredDeeplink);
		}

		public override void SendEvent(string eventName)
		{
			Debug.Log("iOS Sending Event " + eventName);
		}

		public override void SendEvent(string eventName, string eventValue)
		{
			Debug.Log("iOS Sending Event " + eventName + " : " + eventValue);
		}

		public override void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature)
		{
			Debug.Log("iOS Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + transactionId + ", " + receipt + ", " + signature);
		}

		public override void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate)
		{
			Debug.Log("Sending IosTenjin::GetDeeplink");
		}
	}
}
