using UnityEngine;

namespace VoodooSauceInternal
{
	internal class VoodooSettings : ScriptableObject
	{
		public string MixpanelProdToken;
		public string MixpanelDevToken;
		public float MixpanelUsersPercent;
		public float MixpanelUsersPercentPerCohort;
		public float MixpanelUsersPercentForPeople;
		public string GameAnalyticsIosGameKey;
		public string GameAnalyticsIosSecretKey;
		public string GameAnalyticsAndroidGameKey;
		public string GameAnalyticsAndroidSecretKey;
		public AdUnits IosAdUnits;
		public AdUnits AndroidAdUnits;
		public string TapjoyIosSdkKey;
		public string TapjoyAndroidSdkKey;
		public int DelayInSecondsBeforeFirstInterstitialAd;
		public int DelayInSecondsBetweenInterstitialAds;
		public int MaxGamesBetweenInterstitialAds;
		public string[] RunningABTests;
		public DebugForcedCohort DebugForcedCohort;
		public ProductDescriptor[] Products;
	}
}
