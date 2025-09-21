using System;
using UnityEngine;

namespace EasyMobile
{
	[Serializable]
	public class AdSettings
	{
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

		[Serializable]
		public struct DefaultAdNetworks
		{
			public BannerAdNetwork bannerAdNetwork;
			public InterstitialAdNetwork interstitialAdNetwork;
			public RewardedAdNetwork rewardedAdNetwork;
		}

		public enum AdOrientation
		{
			AdOrientationPortrait = 0,
			AdOrientationLandscape = 1,
			AdOrientationAll = 2,
		}

		public enum AdMobChildDirectedTreatment
		{
			Yes = 0,
			No = 1,
			Unspecified = 2,
		}

		[SerializeField]
		private AdColonyConfig _iosAdColonyConfig;
		[SerializeField]
		private AdColonyConfig _androidAdColonyConfig;
		[SerializeField]
		private AdOrientation _adColonyAdOrientation;
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
		private AdMobChildDirectedTreatment _adMobChildDirected;
		[SerializeField]
		private bool _admobEnableTestMode;
		[SerializeField]
		private string[] _admobTestDeviceIds;
		[SerializeField]
		private string _heyzapPublisherId;
		[SerializeField]
		private bool _heyzapShowTestSuite;
		[SerializeField]
		private bool _autoLoadDefaultAds;
		[SerializeField]
		private float _adCheckingInterval;
		[SerializeField]
		private float _adLoadingInterval;
		[SerializeField]
		private DefaultAdNetworks _iosDefaultAdNetworks;
		[SerializeField]
		private DefaultAdNetworks _androidDefaultAdNetwork;
	}
}
