
public sealed class Analytics : Singleton<Analytics>
{
	private const string SESSION = "Destroy Pines (1.2)";

	private bool sendEvents;

	private static string forcedABTest;

	private void Start()
	{
		if (!Data.HasKey("analyticsSession"))
		{
			FirstTime();
		}
		else if (Data.LoadString("analyticsSession") == "Destroy Pines (1.2)")
		{
			sendEvents = true;
		}
	}

	private void OnEnable()
	{
		MoPubManager.onRewardedVideoShownEvent += onRewardedVideoShown;
		MoPubManager.onRewardedVideoClickedEvent += onRewardedVideoClickedEvent;
		MoPubManager.onInterstitialShownEvent += onInterstitialShown;
		MoPubManager.onInterstitialClickedEvent += onInterstitialClicked;
		MoPubManager.onAdLoadedEvent += onBannerShown;
		MoPubManager.onAdClickedEvent += onBannerClicked;
	}

	private void OnDisable()
	{
		MoPubManager.onRewardedVideoShownEvent -= onRewardedVideoShown;
		MoPubManager.onRewardedVideoClickedEvent -= onRewardedVideoClickedEvent;
		MoPubManager.onInterstitialShownEvent -= onInterstitialShown;
		MoPubManager.onInterstitialClickedEvent -= onInterstitialClicked;
		MoPubManager.onAdLoadedEvent -= onBannerShown;
		MoPubManager.onAdClickedEvent -= onBannerClicked;
	}

	private void FirstTime()
	{
		Data.SaveString("analyticsSession", "Destroy Pines (1.2)");
		//GameAnalytics.NewDesignEvent("First Time:App Launched");
		//if (VoodooAnalytics.GetPlayerCohort() != null)
		//{
		//	sendEvents = true;
		//	GameAnalytics.NewDesignEvent(string.Format("AB Test:{0}:Installation:{1}", "Destroy Pines (1.2)", VoodooAnalytics.GetPlayerCohort()));
		//}
	}

	protected override void OnNewGame()
	{
		//VoodooSauce.OnGameStarted();
	}

	protected override void OnGameOver(bool canUseSecondChance)
	{
		if (!canUseSecondChance)
		{
			//VoodooSauce.OnGameFinished(Stats.GetTop().score);
			//GameAnalytics.NewDesignEvent("Game Played", Stats.GetTop().score);
			//if (sendEvents && VoodooAnalytics.GetPlayerCohort() != null)
			//{
			//	GameAnalytics.NewDesignEvent(string.Format("AB Test:{0}:Game Played:{1}", "Destroy Pines (1.2)", VoodooAnalytics.GetPlayerCohort()));
			//}
		}
	}

	public void Rated(int stars)
	{
		//GameAnalytics.NewDesignEvent("Rated", stars);
		//if (sendEvents && VoodooAnalytics.GetPlayerCohort() != null)
		//{
		//	GameAnalytics.NewDesignEvent(string.Format("AB Test:{0}:Rated:{1}", "Destroy Pines (1.2)", VoodooAnalytics.GetPlayerCohort()), stars);
		//}
	}

	protected override void OnPurchased(string ProductID)
	{
		//GameAnalytics.NewDesignEvent($"Purchased:{ProductID}");
		//if (sendEvents && VoodooAnalytics.GetPlayerCohort() != null)
		//{
		//	GameAnalytics.NewDesignEvent(string.Format("AB Test:{0}:Purchased:{1}:{2}", "Destroy Pines (1.2)", ProductID, VoodooAnalytics.GetPlayerCohort()));
		//}
	}

	public void AdShown(string adType)
	{
		//GameAnalytics.NewDesignEvent($"Ads:Ad Shown:{adType}");
		//VoodooSauce.TrackCustomEvent("Ad Shown");
		//if (sendEvents && VoodooAnalytics.GetPlayerCohort() != null)
		//{
		//	GameAnalytics.NewDesignEvent(string.Format("AB Test:{0}:Ad Shown:{1}:{2}", "Destroy Pines (1.2)", VoodooAnalytics.GetPlayerCohort(), adType));
		//}
	}

	public void AdClicked(string adType)
	{
		//GameAnalytics.NewDesignEvent($"Ads:Ad Clicked:{adType}");
		//if (sendEvents && VoodooAnalytics.GetPlayerCohort() != null)
		//{
		//	GameAnalytics.NewDesignEvent(string.Format("AB Test:{0}:Ad Clicked:{1}:{2}", "Destroy Pines (1.2)", VoodooAnalytics.GetPlayerCohort(), adType));
		//}
	}

	private void onRewardedVideoShown(string adUnit)
	{
		AdShown("Rewarded Video");
	}

	private void onRewardedVideoClickedEvent(string adUnit)
	{
		AdClicked("Rewarded Video");
	}

	private void onInterstitialShown(string adUnitId)
	{
		AdShown("Interstitial");
	}

	private void onInterstitialClicked(string adUnitId)
	{
		AdClicked("Interstitial");
	}

	private void onBannerShown(float height)
	{
		AdShown("Banner");
	}

	private void onBannerClicked(string adUnitId)
	{
		AdClicked("Banner");
	}

	public static void ForceABTest(string ABTest)
	{
		forcedABTest = ABTest;
	}

	public static string GetCohort()
	{
		if (forcedABTest != null)
		{
			return forcedABTest;
		}
		return null;// VoodooSauce.GetPlayerCohort();
	}
}
