using System;
using UnityEngine;

namespace EasyMobile
{
	[Serializable]
	public class AdSettings
	{
		[Serializable]
		public struct DefaultAdNetworks
		{
			public BannerAdNetwork bannerAdNetwork;

			public InterstitialAdNetwork interstitialAdNetwork;

			public RewardedAdNetwork rewardedAdNetwork;

			public DefaultAdNetworks(BannerAdNetwork banner, InterstitialAdNetwork interstitial, RewardedAdNetwork rewarded)
			{
				bannerAdNetwork = banner;
				interstitialAdNetwork = interstitial;
				rewardedAdNetwork = rewarded;
			}
		}

		[Serializable]
		public struct AdColonyConfig
		{
			public string appId;

			public string interstitialAdId;

			public string rewardedAdId;
		}

		[Serializable]
		public struct AdMobConfig
		{
			public string bannerAdId;

			public string interstitialAdId;

			public string rewardedAdId;
		}

		public enum AdMobChildDirectedTreatment
		{
			Yes,
			No,
			Unspecified
		}

		public enum AdOrientation
		{
			AdOrientationPortrait,
			AdOrientationLandscape,
			AdOrientationAll
		}

		[SerializeField]
		private AdColonyConfig _iosAdColonyConfig;

		[SerializeField]
		private AdColonyConfig _androidAdColonyConfig;

		[SerializeField]
		private AdOrientation _adColonyAdOrientation = AdOrientation.AdOrientationAll;

		[SerializeField]
		private bool _adColonyShowRewardedAdPrePopup;

		[SerializeField]
		private bool _adColonyShowRewardedAdPostPopup;

		[SerializeField]
		private AdMobConfig _iosAdMobConfig;

		[SerializeField]
		private AdMobConfig _androidAdMobConfig;

		[SerializeField]
		private bool _admobDesignedForFamilies;

		[SerializeField]
		private AdMobChildDirectedTreatment _adMobChildDirected = AdMobChildDirectedTreatment.Unspecified;

		[SerializeField]
		private bool _admobEnableTestMode;

		[SerializeField]
		private string[] _admobTestDeviceIds;

		[SerializeField]
		private string _heyzapPublisherId;

		[SerializeField]
		private bool _heyzapShowTestSuite;

		[SerializeField]
		private bool _autoLoadDefaultAds = true;

		[SerializeField]
		private float _adCheckingInterval = 10f;

		[SerializeField]
		private float _adLoadingInterval = 20f;

		[SerializeField]
		private DefaultAdNetworks _iosDefaultAdNetworks = new DefaultAdNetworks(BannerAdNetwork.None, InterstitialAdNetwork.None, RewardedAdNetwork.None);

		[SerializeField]
		private DefaultAdNetworks _androidDefaultAdNetwork = new DefaultAdNetworks(BannerAdNetwork.None, InterstitialAdNetwork.None, RewardedAdNetwork.None);

		public AdColonyConfig AdColonyIds
		{
			get
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					return _androidAdColonyConfig;
				}
				if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					return _iosAdColonyConfig;
				}
				return default(AdColonyConfig);
			}
		}

		public bool AdColonyShowRewardedAdPrePopup => _adColonyShowRewardedAdPrePopup;

		public bool AdColonyShowRewardedAdPostPopup => _adColonyShowRewardedAdPostPopup;

		public AdOrientation AdColonyAdOrientation => _adColonyAdOrientation;

		public AdMobConfig AdMobIds
		{
			get
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					return _androidAdMobConfig;
				}
				if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					return _iosAdMobConfig;
				}
				return default(AdMobConfig);
			}
		}

		[Obsolete("This property is now obsolete. Use AdMobIds obtain cross-platform AdMob IDs.")]
		public AdMobConfig IosAdMobConfig => _iosAdMobConfig;

		[Obsolete("This property is now obsolete. Use AdMobIds to obtain cross-platform AdMob IDs.")]
		public AdMobConfig AndroidAdMobConfig => _androidAdMobConfig;

		public bool AdMobDesignedForFamilies => _admobDesignedForFamilies;

		public AdMobChildDirectedTreatment AdMobChildDirected => _adMobChildDirected;

		public bool AdMobEnableTestMode => _admobEnableTestMode;

		public string[] AdMobTestDeviceIds => _admobTestDeviceIds;

		public string HeyzapPublisherId => _heyzapPublisherId;

		public bool HeyzapShowTestSuite => _heyzapShowTestSuite;

		public bool IsAutoLoadDefaultAds
		{
			get
			{
				return _autoLoadDefaultAds;
			}
			set
			{
				_autoLoadDefaultAds = value;
			}
		}

		public float AdCheckingInterval
		{
			get
			{
				return _adCheckingInterval;
			}
			set
			{
				_adCheckingInterval = value;
			}
		}

		public float AdLoadingInterval
		{
			get
			{
				return _adLoadingInterval;
			}
			set
			{
				_adLoadingInterval = value;
			}
		}

		public DefaultAdNetworks IosDefaultAdNetworks => _iosDefaultAdNetworks;

		public DefaultAdNetworks AndroidDefaultAdNetworks => _androidDefaultAdNetwork;
	}
}
