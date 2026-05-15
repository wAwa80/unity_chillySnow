using System.Collections.Generic;
using UnityEngine;

public static class MoPub
{
	public const double LAT_LONG_SENTINEL = 99999.0;

	public const string ADUNIT_NOT_FOUND_MSG = "AdUnit {0} not found: no plugin was initialized";

	private static Dictionary<string, MoPubAndroidBanner> _bannerPluginsDict = new Dictionary<string, MoPubAndroidBanner>();

	private static Dictionary<string, MoPubAndroidInterstitial> _interstitialPluginsDict = new Dictionary<string, MoPubAndroidInterstitial>();

	private static Dictionary<string, MoPubAndroidRewardedVideo> _rewardedVideoPluginsDict = new Dictionary<string, MoPubAndroidRewardedVideo>();

	public static void clearLoadedPlugins()
	{
		_bannerPluginsDict.Clear();
		_interstitialPluginsDict.Clear();
		_rewardedVideoPluginsDict.Clear();
	}

	public static void loadBannerPluginsForAdUnits(string adUnit)
	{
		loadBannerPluginsForAdUnits(new string[1] { adUnit });
	}

	public static void loadBannerPluginsForAdUnits(string[] bannerAdUnitIds)
	{
		foreach (string text in bannerAdUnitIds)
		{
			_bannerPluginsDict.Add(text, new MoPubAndroidBanner(text));
		}
		Debug.Log(bannerAdUnitIds.Length + " banner AdUnits loaded for plugins:\n" + string.Join(", ", bannerAdUnitIds));
	}

	public static void loadInterstitialPluginsForAdUnits(string adUnit)
	{
		loadInterstitialPluginsForAdUnits(new string[1] { adUnit });
	}

	public static void loadInterstitialPluginsForAdUnits(string[] interstitialAdUnitIds)
	{
		foreach (string text in interstitialAdUnitIds)
		{
			_interstitialPluginsDict.Add(text, new MoPubAndroidInterstitial(text));
		}
		Debug.Log(interstitialAdUnitIds.Length + " interstitial AdUnits loaded for plugins:\n" + string.Join(", ", interstitialAdUnitIds));
	}

	public static void loadRewardedVideoPluginsForAdUnits(string adUnit)
	{
		loadRewardedVideoPluginsForAdUnits(new string[1] { adUnit });
	}

	public static void loadRewardedVideoPluginsForAdUnits(string[] rewardedVideoAdUnitIds)
	{
		foreach (string text in rewardedVideoAdUnitIds)
		{
			_rewardedVideoPluginsDict.Add(text, new MoPubAndroidRewardedVideo(text));
		}
		Debug.Log(rewardedVideoAdUnitIds.Length + " rewarded video AdUnits loaded for plugins:\n" + string.Join(", ", rewardedVideoAdUnitIds));
	}

	public static void enableLocationSupport(bool shouldUseLocation)
	{
		MoPubAndroid.setLocationAwareness(MoPubLocationAwareness.NORMAL);
	}

	public static void reportApplicationOpen(string iTunesAppId = null)
	{
		MoPubAndroid.reportApplicationOpen();
	}

	public static void createBanner(string adUnitId, MoPubAdPosition position)
	{
		if (_bannerPluginsDict.TryGetValue(adUnitId, out var value))
		{
			value.createBanner(position);
		}
		else
		{
			Debug.LogWarning($"AdUnit {adUnitId} not found: no plugin was initialized");
		}
	}

	public static void destroyBanner(string adUnitId)
	{
		if (_bannerPluginsDict.TryGetValue(adUnitId, out var value))
		{
			value.destroyBanner();
		}
		else
		{
			Debug.LogWarning($"AdUnit {adUnitId} not found: no plugin was initialized");
		}
	}

	public static void showBanner(string adUnitId, bool shouldShow)
	{
		if (_bannerPluginsDict.TryGetValue(adUnitId, out var value))
		{
			value.showBanner(shouldShow);
		}
		else
		{
			Debug.LogWarning($"AdUnit {adUnitId} not found: no plugin was initialized");
		}
	}

	public static void requestInterstitialAd(string adUnitId, string keywords = "")
	{
		if (_interstitialPluginsDict.TryGetValue(adUnitId, out var value))
		{
			value.requestInterstitialAd(keywords);
		}
		else
		{
			Debug.LogWarning($"AdUnit {adUnitId} not found: no plugin was initialized");
		}
	}

	public static void showInterstitialAd(string adUnitId)
	{
		if (_interstitialPluginsDict.TryGetValue(adUnitId, out var value))
		{
			value.showInterstitialAd();
		}
		else
		{
			Debug.LogWarning($"AdUnit {adUnitId} not found: no plugin was initialized");
		}
	}

	public static void initializeRewardedVideo()
	{
		MoPubAndroidRewardedVideo.initializeRewardedVideo();
	}

	public static void requestRewardedVideo(string adUnitId, List<MoPubMediationSetting> mediationSettings = null, string keywords = null, double latitude = 99999.0, double longitude = 99999.0, string customerId = null)
	{
		if (_rewardedVideoPluginsDict.TryGetValue(adUnitId, out var value))
		{
			value.requestRewardedVideo(mediationSettings, keywords, latitude, longitude, customerId);
		}
		else
		{
			Debug.LogWarning($"AdUnit {adUnitId} not found: no plugin was initialized");
		}
	}

	public static void showRewardedVideo(string adUnitId)
	{
		if (_rewardedVideoPluginsDict.TryGetValue(adUnitId, out var value))
		{
			value.showRewardedVideo();
		}
		else
		{
			Debug.LogWarning($"AdUnit {adUnitId} not found: no plugin was initialized");
		}
	}

	public static bool hasRewardedVideo(string adUnitId)
	{
		if (_rewardedVideoPluginsDict.TryGetValue(adUnitId, out var value))
		{
			return value.hasRewardedVideo();
		}
		Debug.LogWarning($"AdUnit {adUnitId} not found: no plugin was initialized");
		return false;
	}

	public static List<MoPubManager.MoPubReward> getAVailableRewards(string adUnitId)
	{
		if (_rewardedVideoPluginsDict.TryGetValue(adUnitId, out var value))
		{
			return value.getAVailableRewards();
		}
		Debug.LogWarning($"AdUnit {adUnitId} not found: no plugin was initialized");
		return null;
	}

	public static void selectReward(string adUnitId, MoPubManager.MoPubReward selectedReward)
	{
		if (_rewardedVideoPluginsDict.TryGetValue(adUnitId, out var value))
		{
			value.selectReward(selectedReward);
		}
		else
		{
			Debug.LogWarning($"AdUnit {adUnitId} not found: no plugin was initialized");
		}
	}
}
