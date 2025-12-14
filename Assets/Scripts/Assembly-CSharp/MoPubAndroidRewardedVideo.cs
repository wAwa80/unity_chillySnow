using System.Collections.Generic;
using UnityEngine;

public class MoPubAndroidRewardedVideo
{
	private static readonly AndroidJavaClass _pluginClass = new AndroidJavaClass("com.mopub.unity.MoPubRewardedVideoUnityPlugin");

	private readonly AndroidJavaObject _plugin;

	private Dictionary<MoPubManager.MoPubReward, AndroidJavaObject> _rewardsDict = new Dictionary<MoPubManager.MoPubReward, AndroidJavaObject>();

	private MoPubManager.MoPubReward _selectedReward;

	public MoPubAndroidRewardedVideo(string adUnitId)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin = new AndroidJavaObject("com.mopub.unity.MoPubRewardedVideoUnityPlugin", adUnitId);
		}
	}

	public static void initializeRewardedVideo()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_pluginClass.CallStatic("initializeRewardedVideo");
		}
	}

	public void requestRewardedVideo(List<MoPubMediationSetting> mediationSettings = null, string keywords = null, double latitude = 99999.0, double longitude = 99999.0, string customerId = null)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			//string text = ((mediationSettings != null) ? Json.Serialize(mediationSettings) : null);
			//_plugin.Call("requestRewardedVideo", text, keywords, latitude, longitude, customerId);
		}
	}

	public void showRewardedVideo()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			_plugin.Call("showRewardedVideo");
		}
	}

	public bool hasRewardedVideo()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return false;
		}
		return _plugin.Call<bool>("hasRewardedVideo", new object[0]);
	}

	public List<MoPubManager.MoPubReward> getAVailableRewards()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return null;
		}
		_rewardsDict.Clear();
		using (AndroidJavaObject androidJavaObject = _plugin.Call<AndroidJavaObject>("getAvailableRewards", new object[0]))
		{
			AndroidJavaObject[] array = AndroidJNIHelper.ConvertFromJNIArray<AndroidJavaObject[]>(androidJavaObject.GetRawObject());
			if (array.Length > 1)
			{
				AndroidJavaObject[] array2 = array;
				foreach (AndroidJavaObject androidJavaObject2 in array2)
				{
					string label = androidJavaObject2.Call<string>("getLabel", new object[0]);
					int amount = androidJavaObject2.Call<int>("getAmount", new object[0]);
					_rewardsDict.Add(new MoPubManager.MoPubReward(label, amount), androidJavaObject2);
				}
			}
		}
		return new List<MoPubManager.MoPubReward>(_rewardsDict.Keys);
	}

	public void selectReward(MoPubManager.MoPubReward selectedReward)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			if (_rewardsDict.TryGetValue(selectedReward, out var value))
			{
				_plugin.Call("selectReward", value);
			}
			else
			{
				Debug.LogWarning($"Selected reward {selectedReward} is not available.");
			}
		}
	}
}
