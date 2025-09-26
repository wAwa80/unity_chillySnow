using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnityEngine.Purchasing
{
	public class IAPButton : MonoBehaviour
	{
		[Serializable]
		public class OnPurchaseCompletedEvent : UnityEvent<Product>
		{
		}

		[Serializable]
		public class OnPurchaseFailedEvent : UnityEvent<Product, PurchaseFailureReason>
		{
		}

		public enum ButtonType
		{
			Purchase = 0,
			Restore = 1,
		}

		public string productId;
		public ButtonType buttonType;
		public bool consumePurchase;
		public OnPurchaseCompletedEvent onPurchaseComplete;
		public OnPurchaseFailedEvent onPurchaseFailed;
		public Text titleText;
		public Text descriptionText;
		public Text priceText;
	}
}
