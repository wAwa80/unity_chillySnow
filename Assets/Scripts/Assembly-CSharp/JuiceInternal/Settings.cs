using UnityEngine;

namespace JuiceInternal
{
	public sealed class Settings : ScriptableObject
	{
		private static Settings instance;

		[Header("General")]
		[SerializeField]
		public bool debug = true;

		[SerializeField]
		public bool resetOnStart = true;

		[Space]
		[SerializeField]
		public string gameName = "DEFAULT NAME";

		[SerializeField]
		public string gameBundle = "com.default.bundle";

		[Space]
		[SerializeField]
		public string gameVersion = "1.0";

		[SerializeField]
		public int gameBuild = 1;

		[Space]
		[Header("Analytics")]
		[SerializeField]
		public string[] ABTestCohorts = new string[0];

		[SerializeField]
		public float ABTestCohortPercentage = 0.05f;

		[Header("Facebook")]
		[SerializeField]
		public string facebookAppID = string.Empty;

		[Header("GameAnalytics")]
		[SerializeField]
		public string iOSGameKey = string.Empty;

		[SerializeField]
		public string iOSSecretKey = string.Empty;

		[SerializeField]
		public string androidGameKey = string.Empty;

		[SerializeField]
		public string androidSecretKey = string.Empty;

		[SerializeField]
		public string[] customDimensions01 = new string[0];

		[SerializeField]
		public string[] customDimensions02 = new string[0];

		[SerializeField]
		public string[] customDimensions03 = new string[0];

		[SerializeField]
		public string[] resourceCurrencies = new string[0];

		[SerializeField]
		public string[] resourceItemTypes = new string[0];

		[Space]
		[Header("Game Center")]
		[SerializeField]
		public string androidResourcesString = string.Empty;

		[Space]
		[Header("In App Purchases")]
		[SerializeField]
		public string[] consumableProducts = new string[0];

		[SerializeField]
		public string[] nonConsumableProducts = new string[0];

		[SerializeField]
		public string[] subscriptions = new string[0];

		[Space]
		[Header("Premium")]
		//// 高级版产品ID
		[SerializeField]
		public string premium = string.Empty;

		[SerializeField]
		public string premiumDeal = string.Empty;
		//// 弹窗显示频率控制
		[SerializeField]
		public int dealPopFirstAtTick = 10;	// 第10次运行时首次显示

		[SerializeField]
		public int dealPopThenEveryTick = 10;	// 第10次运行时首次显示

		[Space]
		[Header("Ads")]
		[SerializeField]
		public string ironSourceIOSAppID = string.Empty;

		[SerializeField]
		public string ironSourceAndroidAppID = string.Empty;

		[Space]
		[SerializeField]
		public string iOSAdMobAppID = string.Empty;

		[SerializeField]
		public string androidAdMobAppID = string.Empty;

		[Space]
		[SerializeField]
		public int firstAdPopAtSecond = 40;

		[SerializeField]
		public int adThenPopEverySecond = 35;

		[Space]
		[Header("Rating")]
		[SerializeField]
		public string appStoreID = string.Empty;

		[SerializeField]
		public int ratingShowsAtTick = 8;

		public static Settings Get()
		{
			if (!instance)
			{
				instance = Resources.Load<Settings>("JuiceSettings");
			}
			return instance;
		}
	}
}
