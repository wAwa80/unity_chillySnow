using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace EasyMobile
{
	[AddComponentMenu("")]
	public class AdManager : MonoBehaviour
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<InterstitialAdNetwork, AdLocation> m_InterstitialAdCompleted;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<RewardedAdNetwork, AdLocation> m_RewardedAdCompleted;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action m_AdsRemoved;

		private const string UNITYADS_REWARDED_ZONE_ID = "rewardedVideo";

		private static float lastInterstitialAdLoadTimestamp;

		private static float lastRewardedAdLoadTimestamp;

		private static List<BannerAdNetwork> activeBannerAdNetworks;

		private const string AD_REMOVE_STATUS_PPKEY = "EM_REMOVE_ADS";

		private const int AD_ENABLED = 1;

		private const int AD_DISABLED = -1;

		private static IEnumerator autoLoadAdsCoroutine;

		private static bool isAutoLoadDefaultAds;

		public static AdManager Instance { get; private set; }

		public static event Action<InterstitialAdNetwork, AdLocation> InterstitialAdCompleted
		{
			add
			{
				Action<InterstitialAdNetwork, AdLocation> action = AdManager.m_InterstitialAdCompleted;
				Action<InterstitialAdNetwork, AdLocation> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref AdManager.m_InterstitialAdCompleted, (Action<InterstitialAdNetwork, AdLocation>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<InterstitialAdNetwork, AdLocation> action = AdManager.m_InterstitialAdCompleted;
				Action<InterstitialAdNetwork, AdLocation> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref AdManager.m_InterstitialAdCompleted, (Action<InterstitialAdNetwork, AdLocation>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action<RewardedAdNetwork, AdLocation> RewardedAdCompleted
		{
			add
			{
				Action<RewardedAdNetwork, AdLocation> action = AdManager.m_RewardedAdCompleted;
				Action<RewardedAdNetwork, AdLocation> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref AdManager.m_RewardedAdCompleted, (Action<RewardedAdNetwork, AdLocation>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<RewardedAdNetwork, AdLocation> action = AdManager.m_RewardedAdCompleted;
				Action<RewardedAdNetwork, AdLocation> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref AdManager.m_RewardedAdCompleted, (Action<RewardedAdNetwork, AdLocation>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action AdsRemoved
		{
			add
			{
				Action action = AdManager.m_AdsRemoved;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref AdManager.m_AdsRemoved, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = AdManager.m_AdsRemoved;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref AdManager.m_AdsRemoved, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		private void Awake()
		{
			if (Instance != null)
			{
				UnityEngine.Object.Destroy(this);
			}
			else
			{
				Instance = this;
			}
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		private void Start()
		{
			isAutoLoadDefaultAds = EM_Settings.Advertising.IsAutoLoadDefaultAds;
			if (isAutoLoadDefaultAds)
			{
				autoLoadAdsCoroutine = CRAutoLoadAds();
				StartCoroutine(autoLoadAdsCoroutine);
			}
		}

		private void Update()
		{
			if (isAutoLoadDefaultAds != EM_Settings.Advertising.IsAutoLoadDefaultAds)
			{
				SetAutoLoadDefaultAds(EM_Settings.Advertising.IsAutoLoadDefaultAds);
			}
		}

		public static bool IsAutoLoadDefaultAds()
		{
			return EM_Settings.Advertising.IsAutoLoadDefaultAds;
		}

		public static void SetAutoLoadDefaultAds(bool isAutoLoad)
		{
			isAutoLoadDefaultAds = isAutoLoad;
			EM_Settings.Advertising.IsAutoLoadDefaultAds = isAutoLoad;
			if (!isAutoLoad)
			{
				if (autoLoadAdsCoroutine != null)
				{
					Instance.StopCoroutine(autoLoadAdsCoroutine);
					autoLoadAdsCoroutine = null;
				}
			}
			else if (autoLoadAdsCoroutine == null)
			{
				autoLoadAdsCoroutine = CRAutoLoadAds();
				Instance.StartCoroutine(autoLoadAdsCoroutine);
			}
		}

		public static void ShowBannerAd(BannerAdPosition position)
		{
			switch (Application.platform)
			{
			case RuntimePlatform.Android:
				ShowBannerAd(EM_Settings.Advertising.AndroidDefaultAdNetworks.bannerAdNetwork, position, BannerAdSize.SmartBanner);
				break;
			case RuntimePlatform.IPhonePlayer:
				ShowBannerAd(EM_Settings.Advertising.IosDefaultAdNetworks.bannerAdNetwork, position, BannerAdSize.SmartBanner);
				break;
			}
		}

		public static void ShowBannerAd(BannerAdPosition position, BannerAdSize size)
		{
			switch (Application.platform)
			{
			case RuntimePlatform.Android:
				ShowBannerAd(EM_Settings.Advertising.AndroidDefaultAdNetworks.bannerAdNetwork, position, size);
				break;
			case RuntimePlatform.IPhonePlayer:
				ShowBannerAd(EM_Settings.Advertising.IosDefaultAdNetworks.bannerAdNetwork, position, size);
				break;
			}
		}

		public static void ShowBannerAd(BannerAdNetwork adNetwork, BannerAdPosition position, BannerAdSize size)
		{
			if (IsAdRemoved())
			{
				Debug.Log("ShowBannerAd FAILED: Ads were removed.");
				return;
			}
			switch ((AdNetwork)adNetwork)
			{
			case AdNetwork.AdMob:
				Debug.LogError("SDK missing. Please import Google Mobile Ads plugin.");
				break;
			case AdNetwork.Heyzap:
				Debug.LogError("SDK missing. Please import Heyzap plugin.");
				break;
			}
		}

		public static void HideBannerAd()
		{
			switch (Application.platform)
			{
			case RuntimePlatform.Android:
				HideBannerAd(EM_Settings.Advertising.AndroidDefaultAdNetworks.bannerAdNetwork);
				break;
			case RuntimePlatform.IPhonePlayer:
				HideBannerAd(EM_Settings.Advertising.IosDefaultAdNetworks.bannerAdNetwork);
				break;
			}
		}

		public static void HideBannerAd(BannerAdNetwork adNetwork)
		{
			switch ((AdNetwork)adNetwork)
			{
			case AdNetwork.AdMob:
				Debug.LogError("SDK missing. Please import Google Mobile Ads plugin.");
				break;
			case AdNetwork.Heyzap:
				Debug.LogError("SDK missing. Please import Heyzap plugin.");
				break;
			}
		}

		public static void DestroyBannerAd()
		{
			switch (Application.platform)
			{
			case RuntimePlatform.Android:
				DestroyBannerAd(EM_Settings.Advertising.AndroidDefaultAdNetworks.bannerAdNetwork);
				break;
			case RuntimePlatform.IPhonePlayer:
				DestroyBannerAd(EM_Settings.Advertising.IosDefaultAdNetworks.bannerAdNetwork);
				break;
			}
		}

		public static void DestroyBannerAd(BannerAdNetwork adNetwork)
		{
			switch ((AdNetwork)adNetwork)
			{
			case AdNetwork.AdMob:
				Debug.LogError("SDK missing. Please import Google Mobile Ads plugin.");
				break;
			case AdNetwork.Heyzap:
				Debug.LogError("SDK missing. Please import Heyzap plugin.");
				break;
			}
		}

		public static bool IsShowingBannerAd()
		{
			return activeBannerAdNetworks.Count > 0;
		}

		public static BannerAdNetwork[] GetActiveBannerAdNetworks()
		{
			return activeBannerAdNetworks.ToArray();
		}

		public static void LoadInterstitialAd()
		{
			switch (Application.platform)
			{
			case RuntimePlatform.Android:
				LoadInterstitialAd(EM_Settings.Advertising.AndroidDefaultAdNetworks.interstitialAdNetwork, AdLocation.Default);
				break;
			case RuntimePlatform.IPhonePlayer:
				LoadInterstitialAd(EM_Settings.Advertising.IosDefaultAdNetworks.interstitialAdNetwork, AdLocation.Default);
				break;
			}
		}

		public static void LoadInterstitialAd(InterstitialAdNetwork adNetwork, AdLocation location)
		{
			if (!IsAdRemoved())
			{
				switch ((AdNetwork)adNetwork)
				{
				case AdNetwork.AdColony:
					Debug.LogError("SDK missing. Please import AdColony plugin.");
					break;
				case AdNetwork.AdMob:
					Debug.LogError("SDK missing. Please import Google Mobile Ads plugin.");
					break;
				case AdNetwork.Chartboost:
					Debug.LogError("SDK missing. Please import Chartboost plugin.");
					break;
				case AdNetwork.Heyzap:
					Debug.LogError("SDK missing. Please import Heyzap plugin.");
					break;
				case AdNetwork.UnityAds:
					Debug.LogError("SDK missing. Please enable Unity Ads service.");
					break;
				}
			}
		}

		public static bool IsInterstitialAdReady()
		{
			return Application.platform switch
			{
				RuntimePlatform.Android => IsInterstitialAdReady(EM_Settings.Advertising.AndroidDefaultAdNetworks.interstitialAdNetwork, AdLocation.Default), 
				RuntimePlatform.IPhonePlayer => IsInterstitialAdReady(EM_Settings.Advertising.IosDefaultAdNetworks.interstitialAdNetwork, AdLocation.Default), 
				_ => false, 
			};
		}

		public static bool IsInterstitialAdReady(InterstitialAdNetwork adNetwork, AdLocation location)
		{
			if (IsAdRemoved())
			{
				return false;
			}
			return (AdNetwork)adNetwork switch
			{
				AdNetwork.AdColony => false, 
				AdNetwork.AdMob => false, 
				AdNetwork.Chartboost => false, 
				AdNetwork.Heyzap => false, 
				AdNetwork.UnityAds => false, 
				_ => false, 
			};
		}

		public static void ShowInterstitialAd()
		{
			switch (Application.platform)
			{
			case RuntimePlatform.Android:
				ShowInterstitialAd(EM_Settings.Advertising.AndroidDefaultAdNetworks.interstitialAdNetwork, AdLocation.Default);
				break;
			case RuntimePlatform.IPhonePlayer:
				ShowInterstitialAd(EM_Settings.Advertising.IosDefaultAdNetworks.interstitialAdNetwork, AdLocation.Default);
				break;
			}
		}

		public static void ShowInterstitialAd(InterstitialAdNetwork adNetwork, AdLocation location)
		{
			if (IsAdRemoved())
			{
				Debug.Log("ShowInterstitialAd FAILED: Ads were removed.");
				return;
			}
			if (!IsInterstitialAdReady(adNetwork, location))
			{
				Debug.Log("ShowInterstitialAd FAILED: Interstitial ad is not loaded.");
				return;
			}
			switch ((AdNetwork)adNetwork)
			{
			case AdNetwork.AdColony:
				Debug.LogError("SDK missing. Please import AdColony plugin.");
				break;
			case AdNetwork.AdMob:
				Debug.LogError("SDK missing. Please import Google Mobile Ads plugin.");
				break;
			case AdNetwork.Chartboost:
				Debug.LogError("SDK missing. Please import Chartboost plugin.");
				break;
			case AdNetwork.Heyzap:
				Debug.LogError("SDK missing. Please import Heyzap plugin.");
				break;
			case AdNetwork.UnityAds:
				Debug.LogError("SDK missing. Please enable Unity Ads service.");
				break;
			}
		}

		public static void LoadRewardedAd()
		{
			switch (Application.platform)
			{
			case RuntimePlatform.Android:
				LoadRewardedAd(EM_Settings.Advertising.AndroidDefaultAdNetworks.rewardedAdNetwork, AdLocation.Default);
				break;
			case RuntimePlatform.IPhonePlayer:
				LoadRewardedAd(EM_Settings.Advertising.IosDefaultAdNetworks.rewardedAdNetwork, AdLocation.Default);
				break;
			}
		}

		public static void LoadRewardedAd(RewardedAdNetwork adNetwork, AdLocation location)
		{
			switch ((AdNetwork)adNetwork)
			{
			case AdNetwork.AdColony:
				Debug.LogError("SDK missing. Please import AdColony plugin.");
				break;
			case AdNetwork.AdMob:
				Debug.LogError("SDK missing. Please import Google Mobile Ads plugin.");
				break;
			case AdNetwork.Chartboost:
				Debug.LogError("SDK missing. Please import Chartboost plugin.");
				break;
			case AdNetwork.Heyzap:
				Debug.LogError("SDK missing. Please import Heyzap plugin.");
				break;
			case AdNetwork.UnityAds:
				Debug.LogError("SDK missing. Please enable Unity Ads service.");
				break;
			}
		}

		public static bool IsRewardedAdReady()
		{
			return Application.platform switch
			{
				RuntimePlatform.Android => IsRewardedAdReady(EM_Settings.Advertising.AndroidDefaultAdNetworks.rewardedAdNetwork, AdLocation.Default), 
				RuntimePlatform.IPhonePlayer => IsRewardedAdReady(EM_Settings.Advertising.IosDefaultAdNetworks.rewardedAdNetwork, AdLocation.Default), 
				_ => false, 
			};
		}

		public static bool IsRewardedAdReady(RewardedAdNetwork adNetwork, AdLocation location)
		{
			return (AdNetwork)adNetwork switch
			{
				AdNetwork.AdColony => false, 
				AdNetwork.AdMob => false, 
				AdNetwork.Chartboost => false, 
				AdNetwork.Heyzap => false, 
				AdNetwork.UnityAds => false, 
				_ => false, 
			};
		}

		public static void ShowRewardedAd()
		{
			switch (Application.platform)
			{
			case RuntimePlatform.Android:
				ShowRewardedAd(EM_Settings.Advertising.AndroidDefaultAdNetworks.rewardedAdNetwork, AdLocation.Default);
				break;
			case RuntimePlatform.IPhonePlayer:
				ShowRewardedAd(EM_Settings.Advertising.IosDefaultAdNetworks.rewardedAdNetwork, AdLocation.Default);
				break;
			}
		}

		public static void ShowRewardedAd(RewardedAdNetwork adNetwork, AdLocation location)
		{
			if (!IsRewardedAdReady(adNetwork, location))
			{
				Debug.Log("ShowRewardedAd FAILED: Rewarded ad is not loaded.");
				return;
			}
			switch ((AdNetwork)adNetwork)
			{
			case AdNetwork.AdColony:
				Debug.LogError("SDK missing. Please import AdColony plugin.");
				break;
			case AdNetwork.AdMob:
				Debug.LogError("SDK missing. Please import Google Mobile Ads plugin.");
				break;
			case AdNetwork.Chartboost:
				Debug.LogError("SDK missing. Please import Chartboost plugin.");
				break;
			case AdNetwork.Heyzap:
				Debug.LogError("SDK missing. Please import Heyzap plugin.");
				break;
			case AdNetwork.UnityAds:
				Debug.LogError("SDK missing. Please enable Unity Ads service.");
				break;
			}
		}

		public static bool IsAdRemoved()
		{
			return PlayerPrefs.GetInt("EM_REMOVE_ADS", 1) == -1;
		}

		public static void RemoveAds()
		{
            UnityEngine.Debug.Log("******* REMOVING ADS... *******");
			DestroyBannerAd();
			PlayerPrefs.SetInt("EM_REMOVE_ADS", -1);
			PlayerPrefs.Save();
			//AdManager.AdsRemoved();
		}

		public static void ResetRemoveAds()
		{
			Debug.Log("******* RESET REMOVE ADS STATUS... *******");
			PlayerPrefs.SetInt("EM_REMOVE_ADS", 1);
			PlayerPrefs.Save();
		}

		private static IEnumerator CRAutoLoadAds()
		{
			while (true)
			{
				IEnumerator enumerator = Enum.GetValues(typeof(AdType)).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						switch ((AdType)enumerator.Current)
						{
						case AdType.Interstitial:
							if (!IsInterstitialAdReady() && !IsAdRemoved() && Time.realtimeSinceStartup - lastInterstitialAdLoadTimestamp >= EM_Settings.Advertising.AdLoadingInterval)
							{
								LoadInterstitialAd();
								lastInterstitialAdLoadTimestamp = Time.realtimeSinceStartup;
							}
							break;
						case AdType.Rewarded:
							if (!IsRewardedAdReady() && Time.realtimeSinceStartup - lastRewardedAdLoadTimestamp >= EM_Settings.Advertising.AdLoadingInterval)
							{
								LoadRewardedAd();
								lastRewardedAdLoadTimestamp = Time.realtimeSinceStartup;
							}
							break;
						}
					}
				}
				finally
				{
					IDisposable disposable;
					IDisposable disposable2 = (disposable = enumerator as IDisposable);
					if (disposable != null)
					{
						disposable2.Dispose();
					}
				}
				yield return new WaitForSeconds(EM_Settings.Advertising.AdCheckingInterval);
			}
		}

		static AdManager()
		{
			//AdManager.InterstitialAdCompleted = delegate
			//{
			//};
			//AdManager.RewardedAdCompleted = delegate
			//{
			//};
			//AdManager.AdsRemoved = delegate
			//{
			//};
			lastInterstitialAdLoadTimestamp = -1000f;
			lastRewardedAdLoadTimestamp = -1000f;
			activeBannerAdNetworks = new List<BannerAdNetwork>();
		}
	}
}
