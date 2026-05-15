using UnityEngine;

public abstract class BaseTenjin : MonoBehaviour
{
	protected string apiKey;

	public string ApiKey
	{
		get
		{
			return apiKey;
		}
		set
		{
			apiKey = value;
		}
	}

	public abstract void Connect();

	public abstract void Connect(string deferredDeeplink);

	public abstract void Init(string apiKey);

	public abstract void SendEvent(string eventName);

	public abstract void SendEvent(string eventName, string eventValue);

	public abstract void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature);

	public abstract void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate);
}
