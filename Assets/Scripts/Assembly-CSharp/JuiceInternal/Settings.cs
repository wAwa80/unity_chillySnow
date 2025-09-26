using UnityEngine;

namespace JuiceInternal
{
	public class Settings : ScriptableObject
	{
		[SerializeField]
		public bool debug;
		[SerializeField]
		public bool resetOnStart;
		[SerializeField]
		public string gameName;
		[SerializeField]
		public string gameBundle;
		[SerializeField]
		public string gameVersion;
		[SerializeField]
		public int gameBuild;
		[SerializeField]
		public string[] ABTestCohorts;
		[SerializeField]
		public float ABTestCohortPercentage;
		[SerializeField]
		public string facebookAppID;
		[SerializeField]
		public string iOSGameKey;
		[SerializeField]
		public string iOSSecretKey;
		[SerializeField]
		public string androidGameKey;
		[SerializeField]
		public string androidSecretKey;
		[SerializeField]
		public string[] customDimensions01;
		[SerializeField]
		public string[] customDimensions02;
		[SerializeField]
		public string[] customDimensions03;
		[SerializeField]
		public string[] resourceCurrencies;
		[SerializeField]
		public string[] resourceItemTypes;
		[SerializeField]
		public string androidResourcesString;
		[SerializeField]
		public string[] consumableProducts;
		[SerializeField]
		public string[] nonConsumableProducts;
		[SerializeField]
		public string[] subscriptions;
		[SerializeField]
		public string premium;
		[SerializeField]
		public string premiumDeal;
		[SerializeField]
		public int dealPopFirstAtTick;
		[SerializeField]
		public int dealPopThenEveryTick;
		[SerializeField]
		public string ironSourceIOSAppID;
		[SerializeField]
		public string ironSourceAndroidAppID;
		[SerializeField]
		public string iOSAdMobAppID;
		[SerializeField]
		public string androidAdMobAppID;
		[SerializeField]
		public int firstAdPopAtSecond;
		[SerializeField]
		public int adThenPopEverySecond;
		[SerializeField]
		public string appStoreID;
		[SerializeField]
		public int ratingShowsAtTick;
	}
}
