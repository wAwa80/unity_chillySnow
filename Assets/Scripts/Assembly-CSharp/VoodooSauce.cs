using System;
using System.Collections.Generic;

public static class VoodooSauce
{
	public const string VERSION = "2.1";

	public static string GetPlayerCohort()
	{
		return "";// VoodooAnalytics.GetPlayerCohort();
	}

	public static void OnGameStarted()
	{
		//VoodooAnalytics.OnGameStarted();
	}

	public static void OnGameFinished(float score)
	{
		OnGameFinished(levelComplete: true, score, null);
	}

	public static void OnGameFinished(bool levelComplete, float score)
	{
		OnGameFinished(levelComplete, score, null);
	}

	public static void OnGameFinished(bool levelComplete, float score, Dictionary<string, object> eventProperties)
	{
		//VoodooAnalytics.OnGameFinished(levelComplete, score, eventProperties);
		//VoodooAds.OnGamePlayed();
	}

	public static void TrackCustomEvent(string eventName, Dictionary<string, object> eventProperties)
	{
		//VoodooAnalytics.TrackCustomEvent(eventName, eventProperties);
	}

	public static void TrackCustomEvent(string eventName)
	{
		TrackCustomEvent(eventName, null);
	}

	public static void RegisterPurchaseDelegate(IPurchaseDelegate purchaseDelegate)
	{
		//VoodooIAP.SetPurchaseDelegate(purchaseDelegate);
	}

	public static void Purchase(string productId)
	{
		//VoodooIAP.BuyProduct(productId);
	}

	public static void RestorePurchases()
	{
		//VoodooIAP.RestorePurchases();
	}

	public static void ShowBanner(Action<float> onBannerDisplayed)
	{
		//VoodooAds.ShowBanner(onBannerDisplayed);
	}

	public static void HideBanner()
	{
		//VoodooAds.HideBanner();
	}

	public static void ShowInterstitial(Action onComplete)
	{
		//VoodooAds.ShowInterstitial(onComplete);
	}

	public static bool IsRewardedVideoAvailable()
	{
		return false;// VoodooAds.IsRewardedVideoAvailable();
	}

	public static void ShowRewardedVideo(Action<bool> onComplete)
	{
		//VoodooAds.ShowRewardedVideo(onComplete);
	}

	public static void SetInterstitialAdsDisplayConditions(int delayInSecondsBeforeFirstInterstitialAd, int delayInSecondsBetweenInterstitialAds, int maxGamesBetweenInterstitialAds)
	{
		//VoodooAds.SetInterstitialAdsDisplayConditions(delayInSecondsBeforeFirstInterstitialAd, delayInSecondsBetweenInterstitialAds, maxGamesBetweenInterstitialAds);
	}

	//public static void SetAdUnit(AdUnitType adUnitType, string adUnit)
	//{
	//	//VoodooAds.SetAdUnit(adUnitType, adUnit);
	//}

	public static void EnablePremium()
	{
		//VoodooPremium.EnablePremium();
	}

	public static bool IsPremium()
	{
		return false;// VoodooPremium.IsPremium();
	}
}
