using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace EasyMobile
{
	[AddComponentMenu("")]
	public class IAPManager : MonoBehaviour
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action m_InitializeSucceeded;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action m_InitializeFailed;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<IAPProduct> m_PurchaseCompleted;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<IAPProduct> m_PurchaseFailed;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action m_RestoreCompleted;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action m_RestoreFailed;

		public static IAPManager Instance { get; private set; }

		public static event Action InitializeSucceeded
		{
			add
			{
				Action action = IAPManager.m_InitializeSucceeded;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref IAPManager.m_InitializeSucceeded, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = IAPManager.m_InitializeSucceeded;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref IAPManager.m_InitializeSucceeded, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action InitializeFailed
		{
			add
			{
				Action action = IAPManager.m_InitializeFailed;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref IAPManager.m_InitializeFailed, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = IAPManager.m_InitializeFailed;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref IAPManager.m_InitializeFailed, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action<IAPProduct> PurchaseCompleted
		{
			add
			{
				Action<IAPProduct> action = IAPManager.m_PurchaseCompleted;
				Action<IAPProduct> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref IAPManager.m_PurchaseCompleted, (Action<IAPProduct>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<IAPProduct> action = IAPManager.m_PurchaseCompleted;
				Action<IAPProduct> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref IAPManager.m_PurchaseCompleted, (Action<IAPProduct>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action<IAPProduct> PurchaseFailed
		{
			add
			{
				Action<IAPProduct> action = IAPManager.m_PurchaseFailed;
				Action<IAPProduct> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref IAPManager.m_PurchaseFailed, (Action<IAPProduct>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<IAPProduct> action = IAPManager.m_PurchaseFailed;
				Action<IAPProduct> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref IAPManager.m_PurchaseFailed, (Action<IAPProduct>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action RestoreCompleted
		{
			add
			{
				Action action = IAPManager.m_RestoreCompleted;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref IAPManager.m_RestoreCompleted, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = IAPManager.m_RestoreCompleted;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref IAPManager.m_RestoreCompleted, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action RestoreFailed
		{
			add
			{
				Action action = IAPManager.m_RestoreFailed;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref IAPManager.m_RestoreFailed, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = IAPManager.m_RestoreFailed;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref IAPManager.m_RestoreFailed, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		private void Awake()
		{
			if (Instance != null)
			{
				UnityEngine.Object.Destroy(this);
			}
			else
			{
				Instance = this;
			}
		}

		private void Start()
		{
		}

		public static void InitializePurchasing()
		{
			Debug.Log("InitializePurchasing FAILED: IAP module is not enabled.");
		}

		public static bool IsInitialized()
		{
			return false;
		}

		public static void Purchase(IAPProduct product)
		{
			if (product != null && product.Id != null)
			{
				PurchaseWithId(product.Id);
			}
			else
			{
				Debug.Log("Purchase FAILED: Either the product or its id is invalid.");
			}
		}

		public static void Purchase(string productName)
		{
			IAPProduct iAPProductByName = GetIAPProductByName(productName);
			if (iAPProductByName != null && iAPProductByName.Id != null)
			{
				PurchaseWithId(iAPProductByName.Id);
			}
			else
			{
				Debug.Log("PurchaseWithName FAILED: Not found product with name: " + productName + " or its id is invalid.");
			}
		}

		public static void PurchaseWithId(string productId)
		{
			Debug.Log("PurchaseWithId FAILED: IAP module is not enabled.");
		}

		public static void RestorePurchases()
		{
			Debug.Log("RestorePurchases FAILED: IAP module is not enabled.");
		}

		public static bool IsProductOwned(string productName)
		{
			Debug.Log("IsProductOwned FAILED: IAP module is not enabled.");
			return false;
		}

		public static void RefreshAppleAppReceipt(Action<string> successCallback, Action errorCallback)
		{
			Debug.Log("RefreshAppleAppReceipt FAILED: IAP module is not enabled.");
		}

		public static IAPProduct[] GetAllIAPProducts()
		{
			return EM_Settings.InAppPurchasing.Products;
		}

		public static IAPProduct GetIAPProductByName(string productName)
		{
			IAPProduct[] products = EM_Settings.InAppPurchasing.Products;
			foreach (IAPProduct iAPProduct in products)
			{
				if (iAPProduct.Name.Equals(productName))
				{
					return iAPProduct;
				}
			}
			return null;
		}

		public static IAPProduct GetIAPProductById(string productId)
		{
			IAPProduct[] products = EM_Settings.InAppPurchasing.Products;
			foreach (IAPProduct iAPProduct in products)
			{
				if (iAPProduct.Id.Equals(productId))
				{
					return iAPProduct;
				}
			}
			return null;
		}
	}
}
