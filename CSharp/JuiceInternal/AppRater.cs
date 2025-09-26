using System;
using PaperPlaneTools;
using UnityEngine;

namespace JuiceInternal
{
	public sealed class AppRater : Module<AppRater>
	{
		private const string TICK_COUNT_KEY = "01956855734958154650";

		private int ticks;

		private bool rateBoxReady;

		protected override void OnSetup()
		{
			ticks = PlayerPrefs.GetInt("01956855734958154650", 0);
		}

		private void Start()
		{
			InitRateBox();
		}

		private void OnApplicationPause(bool pause)
		{
			if (pause)
			{
				rateBoxReady = false;
			}
			else
			{
				InitRateBox();
			}
		}

		public void TryShow()
		{
			ticks++;
			int ratingShowsAtTick = Settings.Get().ratingShowsAtTick;
			if (ticks >= ratingShowsAtTick)
			{
				try
				{
					DisplayAppRater();
					ticks = 0;
					PlayerPrefs.SetInt("01956855734958154650", ticks);
					PlayerPrefs.Save();
					return;
				}
				catch (Exception ex)
				{
					Module<AppRater>.LogWarning($"Could not display the App Rater. Exception was thrown : \"{ex.Message}\" ({ex.StackTrace})");
					return;
				}
			}
			Module<AppRater>.LogMessage($"Cannot display the App Rater because condition is not met (tick : {ticks}/{ratingShowsAtTick})");
		}

		public void ForceShow()
		{
			try
			{
				DisplayAppRater();
				ticks = 0;
				PlayerPrefs.SetInt("01956855734958154650", ticks);
				PlayerPrefs.Save();
			}
			catch (Exception ex)
			{
				Module<AppRater>.LogWarning($"Could not display the App Rater. Exception was thrown : \"{ex.Message}\" ({ex.StackTrace})");
			}
		}

		private void InitRateBox()
		{
			if (rateBoxReady)
			{
				return;
			}
			try
			{
				RateBox.Instance.Init(RateBox.GetStoreUrl(Settings.Get().appStoreID, Application.identifier), new RateBoxConditions
				{
					MinSessionCount = 0,
					MinCustomEventsCount = 0,
					DelayAfterInstallInSeconds = 60,
					DelayAfterLaunchInSeconds = 60,
					PostponeCooldownInSeconds = 300,
					RequireInternetConnection = true
				}, new RateBoxTextSettings
				{
					Title = $"Like {Settings.Get().gameName}?",
					RateButtonTitle = "Rate",
					RejectButtonTitle = "No, thanks",
					PostponeButtonTitle = "Later",
					Message = "Please take a moment to rate us."
				}, new RateBoxSettings
				{
					UseIOSReview = true
				});
				rateBoxReady = true;
			}
			catch (Exception ex)
			{
				Module<AppRater>.LogWarning($"Could not load the App Rater. Exception was thrown : \"{ex.Message}\" ({ex.StackTrace})");
				rateBoxReady = false;
			}
		}

		private void DisplayAppRater()
		{
			if (rateBoxReady)
			{
				Module<AppRater>.LogMessage("Displaying...");
				try
				{
					RateBox.Instance.Show();
					return;
				}
				catch (Exception ex)
				{
					Module<AppRater>.LogWarning($"Could not display the App Rater. Exception was thrown : \"{ex.Message}\" ({ex.StackTrace})");
					rateBoxReady = false;
					return;
				}
			}
			Module<AppRater>.LogDebugWarning("Could not display the App Rater. It is not loaded yet.");
		}
	}
}
