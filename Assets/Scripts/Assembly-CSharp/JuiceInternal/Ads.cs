using UnityEngine;
using UnityEngine.Advertisements;

namespace JuiceInternal
{
	public sealed class Ads : Module<Ads>
	{
		private bool initialized;

		public static bool forceNoFuckingAds;

		public Banner banner { get; private set; }

		public Interstitial interstitial { get; private set; }

		public RewardedVideo rewardedVideo { get; private set; }

		protected override void OnSetup()
		{
			GDPR.onTermsConsentChanged.AddListener(OnTermsConsentChanged);
			GDPR.onPersonalizedAdsConsentChanged.AddListener(OnPersonalizedAdsConsentChanged);
			banner = base.gameObject.AddComponent<Banner>();
			interstitial = base.gameObject.AddComponent<Interstitial>();
			rewardedVideo = base.gameObject.AddComponent<RewardedVideo>();
		}

		protected override void OnGameStarted()
		{
			//IronSource.Agent.setConsent(GDPR.perosnalizedAdsConsent);
			Module<Store>.GetInstance().RegisterProductBoughtEvent(OnPurchased);
			if (GDPR.termsConsent)
			{
				Initialize();
			}
		}

		private void OnPersonalizedAdsConsentChanged(bool consent)
		{
			//IronSource.Agent.setConsent(consent);
		}

		private void OnTermsConsentChanged(bool consent)
		{
			if (consent)
			{
				//IronSource.Agent.setConsent(GDPR.perosnalizedAdsConsent);
				Initialize();
			}
		}

		private void Initialize()
		{
			Module<Ads>.LogMessage("Initializing...");
			if (Settings.Get().debug)
			{
				//IronSource.Agent.setAdaptersDebug(enabled: true);
				//IronSource.Agent.validateIntegration();
			}
			//MetaData metaData = new MetaData("gdpr");
			//metaData.Set("consent", "true");
			//Advertisement.SetMetaData(metaData);
			//IronSource.Agent.init(Settings.Get().ironSourceAndroidAppID);
			//IronSource.Agent.shouldTrackNetworkState(track: true);
			// IronSource 事件预制体可能未放入 Resources，缺资源时跳过，避免 Instantiate(null) 抛 ArgumentException
			GameObject ironSourceEventsPrefab = Resources.Load<GameObject>("IronSourceEventsPrefab");
			if (ironSourceEventsPrefab != null)
			{
				Object.DontDestroyOnLoad(Object.Instantiate(ironSourceEventsPrefab));
			}
			else
			{
				Module<Ads>.LogMessage("Resources.Load(\"IronSourceEventsPrefab\") 返回 null，跳过广告事件预制体实例化。// TODO: [User Action] 若需 IronSource，请将 IronSourceEventsPrefab 放入 Resources");
			}
			if (Module<Premium>.GetInstance().IsPremium() || forceNoFuckingAds)
			{
				Module<Ads>.LogMessage("User is premium : not initializing banners and interstitials");
			}
			else
			{
				Module<Ads>.LogMessage("User is NOT premium : initializing banners and interstitials");
				banner.TurnOn();
				interstitial.TurnOn();
			}
			rewardedVideo.RegisterEvents();
			initialized = true;
		}

		public void ForceNoFuckingAds()
		{
			forceNoFuckingAds = true;
			banner.TurnOff();
			interstitial.TurnOff();
		}

		private void OnPurchased(string productID, bool newPurchase)
		{
			if (initialized && (!(productID != Settings.Get().premium) || !(productID != Settings.Get().premiumDeal)))
			{
				Module<Ads>.LogMessage("User just became premium, destroying banner and interstitial ads.");
				banner.TurnOff();
				interstitial.TurnOff();
			}
		}

		private void OnApplicationPause(bool isPaused)
		{
			if (initialized)
			{
				//IronSource.Agent.onApplicationPause(isPaused);
			}
		}
	}
}
