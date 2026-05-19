using UnityEngine;


namespace EndlessMode
{
	public class MoPubAndroidBanner
	{
		private readonly AndroidJavaObject _bannerPlugin;

		public MoPubAndroidBanner(string adUnitId)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				_bannerPlugin = new AndroidJavaObject("com.mopub.unity.MoPubBannerUnityPlugin", adUnitId);
			}
		}

		public void createBanner(MoPubAdPosition position)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				_bannerPlugin.Call("createBanner", (int)position);
			}
		}

		public void destroyBanner()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				_bannerPlugin.Call("destroyBanner");
			}
		}

		public void showBanner(bool shouldShow)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				_bannerPlugin.Call("hideBanner", !shouldShow);
			}
		}

		public void setBannerKeywords(string keywords)
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				_bannerPlugin.Call("setBannerKeywords", keywords);
			}
		}
	}
}
