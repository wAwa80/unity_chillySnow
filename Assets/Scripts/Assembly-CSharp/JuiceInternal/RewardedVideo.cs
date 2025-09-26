using System;
using UnityEngine;

namespace JuiceInternal
{
	public sealed class RewardedVideo : MonoBehaviour
	{
		private const string LOG_PREFIX = "Ads - Rewarded Video";

		private const string FORMATTER = "{0} event received";

		private const string FORMATTER_WITH_ERROR = "{0} event received with error \"{1}\" (Code {2} - Error {3})";

		private bool ready;

		private Action<bool> callback;

		private bool pleaseFireCallback;

		private float willCancelReward;

		private void LogEvent(string eventName)
		{
			Log.Message("Ads - Rewarded Video", $"{eventName} event received");
		}

		private void LogEvent(string eventName, IronSourceError error)
		{
			Log.Message("Ads - Rewarded Video", $"{eventName} event received with error \"{error.getDescription()}\" (Code {error.getCode()} - Error {error.getErrorCode()})");
		}

		public void RegisterEvents()
		{
			IronSourceEvents.onRewardedVideoAdOpenedEvent += OnRewardedVideoOpenedEvent;
			IronSourceEvents.onRewardedVideoAdClosedEvent += OnRewardedVideoClosedEvent;
			IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += OnRewardedVideoAvailabilityChangedEvent;
			IronSourceEvents.onRewardedVideoAdStartedEvent += OnRewardedVideoStartedEvent;
			IronSourceEvents.onRewardedVideoAdEndedEvent += OnRewardedVideoEndedEvent;
			IronSourceEvents.onRewardedVideoAdRewardedEvent += OnRewardedVideoRewardedEvent;
			IronSourceEvents.onRewardedVideoAdShowFailedEvent += OnRewardedVideoFailedToShowEvent;
			IronSourceEvents.onRewardedVideoAdClickedEvent += OnRewardedVideoClickedEvent;
		}

		private void Update()
		{
			if (pleaseFireCallback)
			{
				Log.Message("DEBUG RVS", "pleaseFireCallback");
				if (callback != null)
				{
					callback(obj: true);
				}
				callback = null;
				pleaseFireCallback = false;
				willCancelReward = 0f;
			}
			else if (willCancelReward > 0f && Time.unscaledTime >= willCancelReward)
			{
				Log.Message("DEBUG RVS", "willCancelReward");
				if (callback != null)
				{
					callback(obj: false);
				}
				callback = null;
				pleaseFireCallback = false;
				willCancelReward = 0f;
			}
		}

		public bool IsReady()
		{
			return IronSource.Agent.isRewardedVideoAvailable();
		}

		public void Show(Action<bool> callback)
		{
			if (!IsReady())
			{
				callback(obj: false);
				return;
			}
			pleaseFireCallback = false;
			willCancelReward = 0f;
			this.callback = callback;
			IronSource.Agent.showRewardedVideo();
			Invoke("InCaseShowFailed", 2f);
		}

		private void InCaseShowFailed()
		{
			pleaseFireCallback = true;
		}

		private void OnRewardedVideoOpenedEvent()
		{
			LogEvent("OnRewardedVideoOpenedEvent");
			CancelInvoke();
			Module<Analytics>.GetInstance().SendDesignEvent("Ads:RewardedVideo:Shown");
		}

		private void OnRewardedVideoClosedEvent()
		{
			LogEvent("OnRewardedVideoClosedEvent");
			willCancelReward = Time.unscaledTime + 1f;
			Module<Ads>.GetInstance().interstitial.ResetTimer();
		}

		private void OnRewardedVideoAvailabilityChangedEvent(bool available)
		{
			if (available)
			{
				LogEvent("OnRewardedVideoAvailabilityChangedEvent (true)");
			}
			else
			{
				LogEvent("OnRewardedVideoAvailabilityChangedEvent (false)");
			}
		}

		private void OnRewardedVideoStartedEvent()
		{
			LogEvent("OnRewardedVideoStartedEvent");
		}

		private void OnRewardedVideoClickedEvent(IronSourcePlacement placement)
		{
			LogEvent("OnRewardedVideoClickedEvent");
			Module<Analytics>.GetInstance().SendDesignEvent("Ads:RewardedVideo:Clicked");
		}

		private void OnRewardedVideoEndedEvent()
		{
			LogEvent("OnRewardedVideoEndedEvent");
		}

		private void OnRewardedVideoRewardedEvent(IronSourcePlacement placement)
		{
			LogEvent("OnRewardedVideoRewardedEvent");
			pleaseFireCallback = true;
		}

		private void OnRewardedVideoFailedToShowEvent(IronSourceError error)
		{
			LogEvent("OnRewardedVideoFailedToShowEvent", error);
			CancelInvoke();
			willCancelReward = Time.unscaledTime;
		}
	}
}
