using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

namespace JuiceInternal
{
	/// <summary>
	/// 应用内购买系统
	/// 
	/// 与Premium的关联：
	/// 产品所有权验证：Store.IsOwned() 方法被Premium用来检查购买状态
	/// 购买功能：Premium的Buy()方法调用Store的购买功能（当前被注释）
	///	数据持久化：通过PlayerPrefs保存已购买产品列表
	/// </summary>
	public sealed class Store : Module<Store> //, IStoreListener
	{
		private class ProductBoughtEvent : UnityEvent<string, bool>
		{
		}

		private const string PROVIDE_MODE_KEY = "ghpofroighjiruhferiu";

		private const string OWNED_PRODUCTS_KEY = "89401891065336555980";

		private static readonly HashSet<string> OWNED_PRODUCTS = new HashSet<string>();

		private static bool dataLoaded;

		//private IStoreController m_StoreController;

		//private IExtensionProvider m_StoreExtensionProvider;

		private ProductBoughtEvent OnProductBought = new ProductBoughtEvent();

		private bool initialized;

		private static void LoadData()
		{
			if (PlayerPrefs.GetInt("ghpofroighjiruhferiu", 2) < 2)
			{
				OWNED_PRODUCTS.Add("chilly_noads");
			}
			if (PlayerPrefs.HasKey("89401891065336555980"))
			{
				OWNED_PRODUCTS.UnionWith(PlayerPrefs.GetString("89401891065336555980").Split('|'));
			}
			dataLoaded = true;
		}

		private static void SaveData()
		{
			if (!dataLoaded)
			{
				LoadData();
			}
			string[] array = new string[OWNED_PRODUCTS.Count];
			int num = 0;
			foreach (string oWNED_PRODUCT in OWNED_PRODUCTS)
			{
				array[num] = oWNED_PRODUCT;
				num++;
			}
			PlayerPrefs.SetString("89401891065336555980", string.Join("|", array));
			PlayerPrefs.Save();
		}

		public static bool IsOwned(string productID)
		{
			if (!dataLoaded)
			{
				LoadData();
			}
			return OWNED_PRODUCTS.Contains(productID);
		}

		protected override void OnSetup()
		{
			LoadData();
            //ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            Settings settings = Settings.Get();
			string[] consumableProducts = settings.consumableProducts;
			foreach (string text in consumableProducts)
			{
				if (!string.IsNullOrEmpty(text))
				{
					//configurationBuilder.AddProduct(text, ProductType.Consumable);
					Module<Store>.LogMessage(string.Format("Added consumable product \"{0}\" : {1}", text, (!IsOwned(text)) ? "NOT OWNED" : "OWNED"));
				}
			}
			string[] nonConsumableProducts = settings.nonConsumableProducts;
			foreach (string text2 in nonConsumableProducts)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					//configurationBuilder.AddProduct(text2, ProductType.NonConsumable);
					Module<Store>.LogMessage(string.Format("Added non consumable product \"{0}\" : {1}", text2, (!IsOwned(text2)) ? "NOT OWNED" : "OWNED"));
				}
			}
			string[] subscriptions = settings.subscriptions;
			foreach (string text3 in subscriptions)
			{
				if (!string.IsNullOrEmpty(text3))
				{
					//configurationBuilder.AddProduct(text3, ProductType.Subscription);
					Module<Store>.LogMessage(string.Format("Added subscription product \"{0}\" : {1}", text3, (!IsOwned(text3)) ? "NOT OWNED" : "OWNED"));
				}
			}
			Module<Store>.LogMessage("Configuring Unity IAP");
			//UnityPurchasing.Initialize(this, configurationBuilder);
		}

		public void RegisterProductBoughtEvent(UnityAction<string, bool> callback)
		{
			OnProductBought.AddListener(callback);
		}

		public void UnregisterProductBoughtEvent(UnityAction<string, bool> callback)
		{
			OnProductBought.RemoveListener(callback);
		}

		//public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
		//{
		//	Module<Store>.LogMessage("Unity IAP Initialized successfully");
		//	m_StoreController = controller;
		//	m_StoreExtensionProvider = extensions;
		//	Product[] all = controller.products.all;
		//	foreach (Product product in all)
		//	{
		//		if (product.hasReceipt && !OWNED_PRODUCTS.Contains(product.definition.id))
		//		{
		//			OnProductPurchased(product.definition.id);
		//		}
		//	}
		//	initialized = true;
		//}

		public bool IsInitialized()
		{
			return false;// m_StoreController != null;
		}

		//public void OnInitializeFailed(InitializationFailureReason error)
		//{
		//	Module<Store>.LogDebugWarning($"Failed to properly initialize Unity IAP. Reason was \"{error.ToString()}\"");
		//}

		public void Buy(string productID)
		{
			if (!initialized)
			{
				Module<Store>.LogDebugWarning($"Cannot buy product \"{productID}\" while the store is not initialized");
				return;
			}
			//Product product = m_StoreController.products.WithID(productID);
			//if (product == null || !product.availableToPurchase)
			//{
			//	Module<Store>.LogDebugWarning($"Uknown or unavailable product \"{productID}\"");
			//	return;
			//}
			//Module<Store>.LogMessage($"Purchasing product \"{productID}\"");
			//if (Settings.Get().debug)
			//{
			//	OnProductPurchased(productID);
			//}
			//else
			//{
			//	m_StoreController.InitiatePurchase(product);
			//}
		}

		public void RestorePurchases()
		{
			if (!initialized)
			{
				Module<Store>.LogDebugWarning("Cannot restore purchases while the store is not initialized");
			}
			else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
			{
				Module<Store>.LogMessage("Purchase restore request received");
				//IAppleExtensions extension = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
				//extension.RestoreTransactions(delegate(bool result)
				//{
				//	Module<Store>.LogMessage($"Restoring pruchases : \"{result}\"");
				//});
			}
			else
			{
				Module<Store>.LogDebugWarning($"Cannot restore purchases on platform \"{Application.platform}\"");
			}
		}

		//public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
		//{
		//	string id = args.purchasedProduct.definition.id;
		//	OnProductPurchased(id);
		//	Module<Analytics>.GetInstance().SendDesignEvent("IAP:Purchased:" + id);
		//	return PurchaseProcessingResult.Complete;
		//}

		private void OnProductPurchased(string productID)
		{
			Module<Store>.LogMessage($"Successfully bought product \"{productID}\"");
			if (!OWNED_PRODUCTS.Contains(productID))
			{
				OWNED_PRODUCTS.Add(productID);
				SaveData();
			}
			OnProductBought.Invoke(productID, arg1: true);
		}

		//public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
		//{
		//	if (failureReason == PurchaseFailureReason.UserCancelled)
		//	{
		//		Module<Analytics>.GetInstance().SendDesignEvent("IAP:Canceled:" + product.definition.id);
		//	}
		//	else
		//	{
		//		Module<Store>.LogDebugWarning($"Failed to buy product \"{product.definition.storeSpecificId}\" for reason \"{failureReason}\"");
		//	}
		//}

		public string GetLocalizedProductPrice(string productID)
		{
			return null;
			//if (m_StoreController == null)
			//{
			//	return null;
			//}
			//Product product = m_StoreController.products.WithID(productID);
			//if (product == null)
			//{
			//	return null;
			//}
			//string localizedPriceString = product.metadata.localizedPriceString;
			//if (string.IsNullOrEmpty(localizedPriceString))
			//{
			//	return "-,--";
			//}
			//return localizedPriceString;
		}
	}
}
