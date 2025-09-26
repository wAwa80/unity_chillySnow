using System;
using UnityEngine;

namespace JuiceInternal
{
	public sealed class Interstitial : MonoBehaviour
	{
		private const string LOG_PREFIX = "Ads - Interstitial";

		private const string FORMATTER = "{0} event received";

		private const string FORMATTER_WITH_ERROR = "{0} event received with error \"{1}\" (Code {2} - Error {3})";

		private bool turnedOn;

		private bool requesting;

		private float lockRequestTimer;

		private bool ready;

		private Action callback;

		private bool pleaseFireCallback;

		private float interstitialTimer;

		private void LogEvent(string eventName)
		{
			Log.Message("Ads - Interstitial", $"{eventName} event received");
		}

		//private void LogEvent(string eventName, IronSourceError error)
		//{
		//	Log.Message("Ads - Interstitial", $"{eventName} event received with error \"{error.getDescription()}\" (Code {error.getCode()} - Error {error.getErrorCode()})");
		//}

		private void Awake()
		{
			base.enabled = false;
		}

		public void TurnOn()
		{
			if (!turnedOn && !Ads.forceNoFuckingAds && !Module<Premium>.GetInstance().IsPremium())
			{
				turnedOn = true;
				//IronSourceEvents.onInterstitialAdReadyEvent += OnInterstitialLoadedEvent;
				//IronSourceEvents.onInterstitialAdLoadFailedEvent += OnInterstitialFailedToLoadEvent;
				//IronSourceEvents.onInterstitialAdShowSucceededEvent += OnInterstitialShownEvent;
				//IronSourceEvents.onInterstitialAdShowFailedEvent += OnInterstitialFailedToShowEvent;
				//IronSourceEvents.onInterstitialAdOpenedEvent += OnInterstitialOpenedEvent;
				//IronSourceEvents.onInterstitialAdClickedEvent += OnInterstitialClickedEvent;
				//IronSourceEvents.onInterstitialAdClosedEvent += OnInterstitialClosedEvent;
				base.enabled = true;
				interstitialTimer = Time.unscaledTime + (float)Settings.Get().firstAdPopAtSecond;
			}
		}

		public void TurnOff()
		{
			if (turnedOn)
			{
				turnedOn = false;
				//IronSourceEvents.onInterstitialAdReadyEvent -= OnInterstitialLoadedEvent;
				//IronSourceEvents.onInterstitialAdLoadFailedEvent -= OnInterstitialFailedToLoadEvent;
				//IronSourceEvents.onInterstitialAdShowSucceededEvent -= OnInterstitialShownEvent;
				//IronSourceEvents.onInterstitialAdShowFailedEvent -= OnInterstitialFailedToShowEvent;
				//IronSourceEvents.onInterstitialAdOpenedEvent -= OnInterstitialOpenedEvent;
				//IronSourceEvents.onInterstitialAdClickedEvent -= OnInterstitialClickedEvent;
				//IronSourceEvents.onInterstitialAdClosedEvent -= OnInterstitialClosedEvent;
				base.enabled = false;
			}
		}

		private void Update()
		{
			if (pleaseFireCallback)
			{
				Log.Message("DEBUG INTERSTITIALS", "pleaseFireCallback");
				if (callback != null)
				{
					callback();
				}
				callback = null;
				pleaseFireCallback = false;
			}
			if (!ready && !requesting && !(Time.unscaledTime < lockRequestTimer))
			{
				if (Application.internetReachability == NetworkReachability.NotReachable)
				{
					lockRequestTimer = Time.unscaledTime + 5f;
					return;
				}
				requesting = true;
				//IronSource.Agent.loadInterstitial();
			}
		}

		public bool IsReady()
		{
			return base.enabled && ready;
		}

		public void Show(Action callback)
		{
			if (!IsReady() || Time.unscaledTime < interstitialTimer || Ads.forceNoFuckingAds || Module<Premium>.GetInstance().IsPremium())
			{
				callback();
				return;
			}
			this.callback = callback;
			//IronSource.Agent.showInterstitial();
			Invoke("InCaseShowFailed", 2f);
		}

		public void ResetTimer()
		{
			interstitialTimer = Time.unscaledTime + (float)Settings.Get().adThenPopEverySecond;
		}

		private void InCaseShowFailed()
		{
			pleaseFireCallback = true;
		}

		private void OnInterstitialLoadedEvent()
		{
			LogEvent("OnInterstitialLoadedEvent");
			requesting = false;
			ready = true;
		}

		//private void OnInterstitialFailedToLoadEvent(IronSourceError error)
		//{
		//	LogEvent("OnInterstitialFailedToLoadEvent", error);
		//	requesting = false;
		//}

		private void OnInterstitialShownEvent()
		{
			LogEvent("OnInterstitialShownEvent");
			Module<Analytics>.GetInstance().SendDesignEvent("Ads:Interstitial:Shown");
			CancelInvoke();
		}

		//private void OnInterstitialFailedToShowEvent(IronSourceError error)
		//{
		//	LogEvent("OnInterstitialFailedToShowEvent", error);
		//	ready = false;
		//}

		private void OnInterstitialOpenedEvent()
		{
			LogEvent("OnInterstitialOpenedEvent");
		}

		private void OnInterstitialClickedEvent()
		{
			LogEvent("OnInterstitialClickedEvent");
			Module<Analytics>.GetInstance().SendDesignEvent("Ads:Interstitial:Clicked");
		}

		private void OnInterstitialClosedEvent()
		{
			LogEvent("OnInterstitialClosedEvent");
			ready = false;
			pleaseFireCallback = true;
			ResetTimer();
		}
	}
}
