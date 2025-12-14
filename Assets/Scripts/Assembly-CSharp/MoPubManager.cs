using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MoPubManager : MonoBehaviour
{
	public class RewardedVideoData
	{
		public string adUnitId;

		public string currencyType;

		public float amount;

		public RewardedVideoData(string json)
		{
			//if (Json.Deserialize(json) is Dictionary<string, object> dictionary)
			//{
			//	if (dictionary.ContainsKey("adUnitId"))
			//	{
			//		adUnitId = dictionary["adUnitId"].ToString();
			//	}
			//	if (dictionary.ContainsKey("currencyType"))
			//	{
			//		currencyType = dictionary["currencyType"].ToString();
			//	}
			//	if (dictionary.ContainsKey("amount"))
			//	{
			//		amount = float.Parse(dictionary["amount"].ToString());
			//	}
			//}
		}

		public override string ToString()
		{
			return $"adUnitId: {adUnitId}, currencyType: {currencyType}, amount: {amount}";
		}
	}

	public class MoPubReward
	{
		private readonly string _label;

		private readonly int _amount;

		public string Label => _label;

		public int Amount => _amount;

		public MoPubReward(string label, int amount)
		{
			_label = label;
			_amount = amount;
		}

		public override string ToString()
		{
			return $"\"{Amount} {Label}\"";
		}
	}

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<float> m_onAdLoadedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_onAdFailedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_onAdClickedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_onAdExpandedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_onAdCollapsedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_onInterstitialLoadedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_onInterstitialFailedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_onInterstitialDismissedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_onInterstitialExpiredEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_onInterstitialShownEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_onInterstitialClickedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_onRewardedVideoLoadedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_onRewardedVideoFailedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_onRewardedVideoExpiredEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_onRewardedVideoShownEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_onRewardedVideoClickedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_onRewardedVideoFailedToPlayEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<RewardedVideoData> m_onRewardedVideoReceivedRewardEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_onRewardedVideoClosedEvent;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static Action<string> m_onRewardedVideoLeavingApplicationEvent;

	public static event Action<float> onAdLoadedEvent
	{
		add
		{
			Action<float> action = MoPubManager.m_onAdLoadedEvent;
			Action<float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onAdLoadedEvent, (Action<float>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<float> action = MoPubManager.m_onAdLoadedEvent;
			Action<float> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onAdLoadedEvent, (Action<float>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onAdFailedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_onAdFailedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onAdFailedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_onAdFailedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onAdFailedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onAdClickedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_onAdClickedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onAdClickedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_onAdClickedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onAdClickedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onAdExpandedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_onAdExpandedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onAdExpandedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_onAdExpandedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onAdExpandedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onAdCollapsedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_onAdCollapsedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onAdCollapsedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_onAdCollapsedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onAdCollapsedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onInterstitialLoadedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_onInterstitialLoadedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onInterstitialLoadedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_onInterstitialLoadedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onInterstitialLoadedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onInterstitialFailedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_onInterstitialFailedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onInterstitialFailedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_onInterstitialFailedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onInterstitialFailedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onInterstitialDismissedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_onInterstitialDismissedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onInterstitialDismissedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_onInterstitialDismissedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onInterstitialDismissedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onInterstitialExpiredEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_onInterstitialExpiredEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onInterstitialExpiredEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_onInterstitialExpiredEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onInterstitialExpiredEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onInterstitialShownEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_onInterstitialShownEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onInterstitialShownEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_onInterstitialShownEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onInterstitialShownEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onInterstitialClickedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_onInterstitialClickedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onInterstitialClickedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_onInterstitialClickedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onInterstitialClickedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onRewardedVideoLoadedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_onRewardedVideoLoadedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onRewardedVideoLoadedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_onRewardedVideoLoadedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onRewardedVideoLoadedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onRewardedVideoFailedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_onRewardedVideoFailedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onRewardedVideoFailedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_onRewardedVideoFailedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onRewardedVideoFailedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onRewardedVideoExpiredEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_onRewardedVideoExpiredEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onRewardedVideoExpiredEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_onRewardedVideoExpiredEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onRewardedVideoExpiredEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onRewardedVideoShownEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_onRewardedVideoShownEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onRewardedVideoShownEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_onRewardedVideoShownEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onRewardedVideoShownEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onRewardedVideoClickedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_onRewardedVideoClickedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onRewardedVideoClickedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_onRewardedVideoClickedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onRewardedVideoClickedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onRewardedVideoFailedToPlayEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_onRewardedVideoFailedToPlayEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onRewardedVideoFailedToPlayEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_onRewardedVideoFailedToPlayEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onRewardedVideoFailedToPlayEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<RewardedVideoData> onRewardedVideoReceivedRewardEvent
	{
		add
		{
			Action<RewardedVideoData> action = MoPubManager.m_onRewardedVideoReceivedRewardEvent;
			Action<RewardedVideoData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onRewardedVideoReceivedRewardEvent, (Action<RewardedVideoData>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<RewardedVideoData> action = MoPubManager.m_onRewardedVideoReceivedRewardEvent;
			Action<RewardedVideoData> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onRewardedVideoReceivedRewardEvent, (Action<RewardedVideoData>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onRewardedVideoClosedEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_onRewardedVideoClosedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onRewardedVideoClosedEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_onRewardedVideoClosedEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onRewardedVideoClosedEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	public static event Action<string> onRewardedVideoLeavingApplicationEvent
	{
		add
		{
			Action<string> action = MoPubManager.m_onRewardedVideoLeavingApplicationEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onRewardedVideoLeavingApplicationEvent, (Action<string>)Delegate.Combine(action2, value), action);
			}
			while (action != action2);
		}
		remove
		{
			Action<string> action = MoPubManager.m_onRewardedVideoLeavingApplicationEvent;
			Action<string> action2;
			do
			{
				action2 = action;
				action = Interlocked.CompareExchange(ref MoPubManager.m_onRewardedVideoLeavingApplicationEvent, (Action<string>)Delegate.Remove(action2, value), action);
			}
			while (action != action2);
		}
	}

	static MoPubManager()
	{
		Type typeFromHandle = typeof(MoPubManager);
		try
		{
			MonoBehaviour monoBehaviour = UnityEngine.Object.FindObjectOfType(typeFromHandle) as MonoBehaviour;
			if (!(monoBehaviour != null))
			{
				GameObject gameObject = new GameObject(typeFromHandle.ToString());
				gameObject.AddComponent(typeFromHandle);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				Debug.Log("Created MoPuManager");
			}
		}
		catch (UnityException)
		{
			Debug.LogWarning(string.Concat("It looks like you have the ", typeFromHandle, " on a GameObject in your scene. Please remove the script from your scene."));
		}
	}

	public static void Init()
	{
		Debug.Log("MoPuManager initialized");
	}

	private void onAdLoaded(string height)
	{
		//if (MoPubManager.onAdLoadedEvent != null)
		//{
		//	MoPubManager.onAdLoadedEvent(float.Parse(height));
		//}
	}

	private void onAdFailed(string errorMsg)
	{
		//if (MoPubManager.onAdFailedEvent != null)
		//{
		//	MoPubManager.onAdFailedEvent(errorMsg);
		//}
	}

	private void onAdClicked(string adUnitId)
	{
		//if (MoPubManager.onAdClickedEvent != null)
		//{
		//	MoPubManager.onAdClickedEvent(adUnitId);
		//}
	}

	private void onAdExpanded(string adUnitId)
	{
		//if (MoPubManager.onAdExpandedEvent != null)
		//{
		//	MoPubManager.onAdExpandedEvent(adUnitId);
		//}
	}

	private void onAdCollapsed(string adUnitId)
	{
		//if (MoPubManager.onAdCollapsedEvent != null)
		//{
		//	MoPubManager.onAdCollapsedEvent(adUnitId);
		//}
	}

	private void onInterstitialLoaded(string adUnitId)
	{
		//if (MoPubManager.onInterstitialLoadedEvent != null)
		//{
		//	MoPubManager.onInterstitialLoadedEvent(adUnitId);
		//}
	}

	private void onInterstitialFailed(string errorMsg)
	{
		//if (MoPubManager.onInterstitialFailedEvent != null)
		//{
		//	MoPubManager.onInterstitialFailedEvent(errorMsg);
		//}
	}

	private void onInterstitialDismissed(string adUnitId)
	{
		//if (MoPubManager.onInterstitialDismissedEvent != null)
		//{
		//	MoPubManager.onInterstitialDismissedEvent(adUnitId);
		//}
	}

	private void interstitialDidExpire(string adUnitId)
	{
		//if (MoPubManager.onInterstitialExpiredEvent != null)
		//{
		//	MoPubManager.onInterstitialExpiredEvent(adUnitId);
		//}
	}

	private void onInterstitialShown(string adUnitId)
	{
		//if (MoPubManager.onInterstitialShownEvent != null)
		//{
		//	MoPubManager.onInterstitialShownEvent(adUnitId);
		//}
	}

	private void onInterstitialClicked(string adUnitId)
	{
		//if (MoPubManager.onInterstitialClickedEvent != null)
		//{
		//	MoPubManager.onInterstitialClickedEvent(adUnitId);
		//}
	}

	//private void onRewardedVideoLoaded(string adUnitId)
	//{
	//	if (MoPubManager.onRewardedVideoLoadedEvent != null)
	//	{
	//		MoPubManager.onRewardedVideoLoadedEvent(adUnitId);
	//	}
	//}

	//private void onRewardedVideoFailed(string errorMsg)
	//{
	//	if (MoPubManager.onRewardedVideoFailedEvent != null)
	//	{
	//		MoPubManager.onRewardedVideoFailedEvent(errorMsg);
	//	}
	//}

	//private void onRewardedVideoExpired(string adUnitId)
	//{
	//	if (MoPubManager.onRewardedVideoExpiredEvent != null)
	//	{
	//		MoPubManager.onRewardedVideoExpiredEvent(adUnitId);
	//	}
	//}

	//private void onRewardedVideoShown(string adUnitId)
	//{
	//	if (MoPubManager.onRewardedVideoShownEvent != null)
	//	{
	//		MoPubManager.onRewardedVideoShownEvent(adUnitId);
	//	}
	//}

	//private void onRewardedVideoClicked(string adUnitId)
	//{
	//	if (MoPubManager.onRewardedVideoClickedEvent != null)
	//	{
	//		MoPubManager.onRewardedVideoClickedEvent(adUnitId);
	//	}
	//}

	//private void onRewardedVideoFailedToPlay(string errorMsg)
	//{
	//	if (MoPubManager.onRewardedVideoFailedToPlayEvent != null)
	//	{
	//		MoPubManager.onRewardedVideoFailedToPlayEvent(errorMsg);
	//	}
	//}

	//private void onRewardedVideoReceivedReward(string json)
	//{
	//	if (MoPubManager.onRewardedVideoReceivedRewardEvent != null)
	//	{
	//		MoPubManager.onRewardedVideoReceivedRewardEvent(new RewardedVideoData(json));
	//	}
	//}

	//private void onRewardedVideoClosed(string adUnitId)
	//{
	//	if (MoPubManager.onRewardedVideoClosedEvent != null)
	//	{
	//		MoPubManager.onRewardedVideoClosedEvent(adUnitId);
	//	}
	//}

	//private void onRewardedVideoLeavingApplication(string adUnitId)
	//{
	//	if (MoPubManager.onRewardedVideoLeavingApplicationEvent != null)
	//	{
	//		MoPubManager.onRewardedVideoLeavingApplicationEvent(adUnitId);
	//	}
	//}
}
