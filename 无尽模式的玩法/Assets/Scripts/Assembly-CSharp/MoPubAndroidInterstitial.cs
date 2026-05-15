using UnityEngine;

public class MoPubAndroidInterstitial
{
	private readonly AndroidJavaObject _interstitialPlugin;

	public MoPubAndroidInterstitial(string adUnitId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_interstitialPlugin = new AndroidJavaObject("com.mopub.unity.MoPubInterstitialUnityPlugin", adUnitId);
		}
	}

	public void requestInterstitialAd(string keywords = "")
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_interstitialPlugin.Call("requestInterstitialAd", keywords);
		}
	}

	public void showInterstitialAd()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_interstitialPlugin.Call("showInterstitialAd");
		}
	}
}
