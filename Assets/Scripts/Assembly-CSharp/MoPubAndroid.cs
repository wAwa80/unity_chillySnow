using UnityEngine;

public class MoPubAndroid
{
	private static readonly AndroidJavaClass _pluginClass = new AndroidJavaClass("com.mopub.unity.MoPubUnityPlugin");

	public static void addFacebookTestDeviceId(string hashedDeviceId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_pluginClass.CallStatic("addFacebookTestDeviceId", hashedDeviceId);
		}
	}

	public static void setLocationAwareness(MoPubLocationAwareness locationAwareness)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_pluginClass.CallStatic("setLocationAwareness", locationAwareness.ToString());
		}
	}

	public static void reportApplicationOpen()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_pluginClass.CallStatic("reportApplicationOpen");
		}
	}
}
