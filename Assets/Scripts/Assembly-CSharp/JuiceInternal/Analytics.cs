using GameAnalyticsSDK;
using UnityEngine;

namespace JuiceInternal
{
	public sealed class Analytics : Module<Analytics>
	{
		private const string FIRST_VERSION_INSTALLED_KEY = "81754984509845987198";

		private string firstVersion;

		private bool initialized;

		private bool canSendEvents;

		private const string NO_COHORT_DATA_KEY = "79089970046461313324";

		private const string COHORT_DATA_KEY = "99567348125007843353";

		private const string CONTROL_COHORT = "Control ({0})";

		private string cohort;

		private bool cohortWasForced;

		private string lastEventSent;

		private float levelTime;

		protected override void OnSetup()
		{
			if (PlayerPrefs.HasKey("81754984509845987198"))
			{
				firstVersion = PlayerPrefs.GetString("81754984509845987198");
			}
			else
			{
				firstVersion = Application.version;
				PlayerPrefs.SetString("81754984509845987198", firstVersion);
			}
			Module<Analytics>.LogMessage("First installed version is " + firstVersion);
			SetCohort();
			PlayerPrefs.Save();
			GDPR.onTermsConsentChanged.AddListener(OnTermsConsentChanged);
			GDPR.onPersonalizedAdsConsentChanged.AddListener(OnPersonalizedAdsConsentChanged);
			GDPR.onAnalyticsConsentChanged.AddListener(OnAnalyticsConsentChanged);
			canSendEvents = GDPR.analyticsConsent;
			if (canSendEvents && GDPR.termsConsent)
			{
				InitializeSDK();
			}
		}

		private void OnAnalyticsConsentChanged(bool consent)
		{
			if (!consent)
			{
				SendDesignEvent("Player:Chose:AnalyticsOff");
			}
			canSendEvents = consent;
			if (consent && GDPR.termsConsent)
			{
				InitializeSDK();
			}
			if (consent)
			{
				SendDesignEvent("Player:Chose:AnalyticsOn");
			}
		}

		private void OnPersonalizedAdsConsentChanged(bool consent)
		{
			if (consent)
			{
				SendDesignEvent("Player:Chose:PersonalizedAds");
			}
			else
			{
				SendDesignEvent("Player:Chose:NonPersonalizedAds");
			}
		}

		private void OnTermsConsentChanged(bool consent)
		{
			if (consent)
			{
				canSendEvents = GDPR.analyticsConsent;
				if (canSendEvents)
				{
					InitializeSDK();
				}
				SendDesignEvent("Player:AcceptedAllTerms");
			}
		}

		private void InitializeSDK()
		{
			if (!initialized)
			{
				Module<Analytics>.LogMessage("Initializing SDK");
				Object.DontDestroyOnLoad(new GameObject("JUICE MODULE GameAnalytics", typeof(GameAnalytics)));
				GameAnalytics.Initialize();
				initialized = true;
			}
		}

		private bool CanSendEvents()
		{
			return canSendEvents && initialized;
		}

		private void SetCohort()
		{
			if (cohortWasForced)
			{
				return;
			}
			if (PlayerPrefs.HasKey("79089970046461313324"))
			{
				Module<Analytics>.LogMessage("Player has previously been assigned to no cohort");
				return;
			}
			string[] aBTestCohorts = Settings.Get().ABTestCohorts;
			if (aBTestCohorts.Length == 0)
			{
				Module<Analytics>.LogMessage("No AB Test is currently setup, skipping cohort assignation");
				PlayerPrefs.SetInt("79089970046461313324", 1);
				if (PlayerPrefs.HasKey("99567348125007843353"))
				{
					PlayerPrefs.DeleteKey("99567348125007843353");
				}
				return;
			}
			string text = $"Control ({aBTestCohorts[0]})";
			if (PlayerPrefs.HasKey("99567348125007843353"))
			{
				cohort = PlayerPrefs.GetString("99567348125007843353");
				if (cohort == text)
				{
					Module<Analytics>.LogMessage("Player is already in Control cohort");
					return;
				}
				for (int i = 0; i < aBTestCohorts.Length; i++)
				{
					if (cohort == aBTestCohorts[i])
					{
						Module<Analytics>.LogMessage($"Player is already in {cohort} cohort");
						return;
					}
				}
				PlayerPrefs.SetInt("79089970046461313324", 1);
				PlayerPrefs.DeleteKey("99567348125007843353");
				Module<Analytics>.LogMessage("Player's cohort is outdated. Putting in no cohort");
			}
			else
			{
				float value = Random.value;
				float aBTestCohortPercentage = Settings.Get().ABTestCohortPercentage;
				if (value < aBTestCohortPercentage)
				{
					cohort = text;
					Module<Analytics>.LogMessage("Player has no cohort yet, assigning to Control cohort");
					PlayerPrefs.SetString("99567348125007843353", text);
				}
				else if (value >= (float)(aBTestCohorts.Length + 1) * aBTestCohortPercentage)
				{
					Module<Analytics>.LogMessage("Player has no cohort yet, and it will forever remain so");
					PlayerPrefs.SetInt("79089970046461313324", 1);
				}
				else
				{
					cohort = aBTestCohorts[Mathf.FloorToInt((value - aBTestCohortPercentage) / aBTestCohortPercentage)];
					Module<Analytics>.LogMessage($"Player has no cohort yet, assigning to {cohort} cohort");
					PlayerPrefs.SetString("99567348125007843353", text);
				}
			}
		}

		public void ForceCohort(string newCohort)
		{
			cohortWasForced = true;
			cohort = newCohort;
			Module<Analytics>.LogMessage($"Player has been forced on cohort {newCohort}");
			PlayerPrefs.SetString("99567348125007843353", newCohort);
			PlayerPrefs.Save();
		}

		public string GetCohort()
		{
			return cohort;
		}

		private void OnApplicationPause(bool pause)
		{
			if (pause && lastEventSent != null && (lastEventSent.Length < 16 || lastEventSent.Substring(0, 16) != "LastSessionEvent"))
			{
				SendDesignEvent("LastSessionEvent:" + lastEventSent);
			}
		}

		private void OnApplicationQuit()
		{
			if (lastEventSent != null && (lastEventSent.Length < 16 || lastEventSent.Substring(0, 16) != "LastSessionEvent"))
			{
				SendDesignEvent("LastSessionEvent:" + lastEventSent);
			}
		}

		public void SendAppropriateProgressionEvent(GAProgressionStatus status, string pEvent)
		{
			string[] array = pEvent.Split(':');
			if (array.Length == 1)
			{
				SendProgressionEvent(status, array[0]);
			}
			else if (array.Length == 2)
			{
				SendProgressionEvent(status, array[0], array[1]);
			}
			else
			{
				SendProgressionEvent(status, array[0], array[1], array[2]);
			}
		}

		public void SendDesignEvent(string eventName)
		{
			if (CanSendEvents())
			{
				lastEventSent = eventName;
				GameAnalytics.NewDesignEvent(eventName);
				if (cohort != null)
				{
					GameAnalytics.NewDesignEvent($"AB Test:{firstVersion}:{eventName}:{cohort}");
				}
			}
		}

		public void SendDesignEvent(string eventName, float value)
		{
			if (CanSendEvents())
			{
				lastEventSent = eventName;
				GameAnalytics.NewDesignEvent(eventName, value);
				if (cohort != null)
				{
					GameAnalytics.NewDesignEvent($"AB Test:{firstVersion}:{eventName}:{cohort}", value);
				}
			}
		}

		public void SendProgressionEvent(GAProgressionStatus status, string progression01)
		{
			if (CanSendEvents())
			{
				lastEventSent = $"ProgressEvent:{status}:{progression01}";
				GameAnalytics.NewProgressionEvent(status, progression01);
			}
		}

		public void SendProgressionEvent(GAProgressionStatus status, string progression01, int score)
		{
			if (CanSendEvents())
			{
				lastEventSent = $"ProgressEvent:{status}:{progression01}";
				GameAnalytics.NewProgressionEvent(status, progression01, score);
			}
		}

		public void SendProgressionEvent(GAProgressionStatus status, string progression01, string progression02)
		{
			if (CanSendEvents())
			{
				lastEventSent = $"ProgressEvent:{status}:{progression01}:{progression02}";
				GameAnalytics.NewProgressionEvent(status, progression01, progression02);
			}
		}

		public void SendProgressionEvent(GAProgressionStatus status, string progression01, string progression02, int score)
		{
			if (CanSendEvents())
			{
				lastEventSent = $"ProgressEvent:{status}:{progression01}:{progression02}";
				GameAnalytics.NewProgressionEvent(status, progression01, progression02, score);
			}
		}

		public void SendProgressionEvent(GAProgressionStatus status, string progression01, string progression02, string progression03)
		{
			if (CanSendEvents())
			{
				lastEventSent = $"ProgressEvent:{status}:{progression01}:{progression02}";
				GameAnalytics.NewProgressionEvent(status, progression01, progression02, progression03);
			}
		}

		public void SendProgressionEvent(GAProgressionStatus status, string progression01, string progression02, string progression03, int score)
		{
			if (CanSendEvents())
			{
				lastEventSent = $"ProgressEvent:{status}:{progression01}:{progression02}";
				GameAnalytics.NewProgressionEvent(status, progression01, progression02, progression03, score);
			}
		}

		public void SendResourceEvent(GAResourceFlowType flowType, string currency, float amount, string itemType, string itemID)
		{
			if (CanSendEvents())
			{
				lastEventSent = $"BusinessEvent:{itemType}:{itemID}:{amount}";
				GameAnalytics.NewResourceEvent(flowType, currency, amount, itemType, itemID);
			}
		}

		public void SendErrorEvent(GAErrorSeverity severity, string message)
		{
			if (CanSendEvents())
			{
				GameAnalytics.NewErrorEvent(severity, message);
			}
		}

		public void SendBusinessEvent(string currency, int amount, string itemType, string itemID, string cartType)
		{
			if (CanSendEvents())
			{
				lastEventSent = $"BusinessEvent:{itemType}:{itemID}:{amount}";
				GameAnalytics.NewBusinessEvent(currency, amount, itemType, itemID, cartType);
			}
		}
	}
}
