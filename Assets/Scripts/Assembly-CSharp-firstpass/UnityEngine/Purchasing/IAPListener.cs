using UnityEngine;
using System;
using UnityEngine.Events;

namespace UnityEngine.Purchasing
{
	public class IAPListener : MonoBehaviour
	{
		[Serializable]
		public class OnPurchaseCompletedEvent : UnityEvent<Product>
		{
		}

		[Serializable]
		public class OnPurchaseFailedEvent : UnityEvent<Product, PurchaseFailureReason>
		{
		}

		public bool consumePurchase;
		public bool dontDestroyOnLoad;
		public OnPurchaseCompletedEvent onPurchaseComplete;
		public OnPurchaseFailedEvent onPurchaseFailed;
	}
}
