using UnityEngine;

public class DebugTenjin : BaseTenjin
{
	public override void Connect()
	{
		Debug.Log("Connecting " + base.ApiKey);
	}

	public override void Connect(string deferredDeeplink)
	{
		Debug.Log("Connecting with deferredDeeplink " + deferredDeeplink);
	}

	public override void Init(string apiKey)
	{
		Debug.Log("Initializing " + apiKey);
		base.ApiKey = apiKey;
	}

	public override void SendEvent(string eventName)
	{
		Debug.Log("Sending Event " + eventName);
	}

	public override void SendEvent(string eventName, string eventValue)
	{
		Debug.Log("Sending Event " + eventName + " : " + eventValue);
	}

	public override void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature)
	{
		Debug.Log("Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + transactionId + ", " + receipt + ", " + signature);
	}

	public override void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate)
	{
		Debug.Log("Sending DebugTenjin::GetDeeplink");
	}
}
