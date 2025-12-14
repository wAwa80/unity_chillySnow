using System;
using UnityEngine;

namespace EasyMobile
{
	[AddComponentMenu("")]
	public class MobileNativeRatingRequest : MonoBehaviour
	{
		public enum UserAction
		{
			Refuse,
			Postpone,
			Feedback,
			Rate
		}

		private static readonly string RATING_DIALOG_GAMEOBJECT = "MobileNativeRatingDialog";

		private const int IOS_SYSTEM_DEFAULT_ANNUAL_CAP = 3;

		private const int RATING_REQUEST_ENABLED = 1;

		private const int RATING_REQUEST_DISABLED = -1;

		private const string RATING_REQUEST_DISABLE_PPKEY = "EM_RATING_REQUEST_DISABLE";

		private const string ANNUAL_REQUESTS_MADE_PPKEY_PREFIX = "EM_RATING_REQUESTS_MADE_YEAR_";

		private const string LAST_REQUEST_TIMESTAMP_PPKEY = "EM_RATING_REQUEST_LAST_REQUEST_TIMESTAMP";

		private static Action<UserAction> customBehaviour;

		public static MobileNativeRatingRequest Instance { get; private set; }

		public static void RequestRating()
		{
			RequestRating(null);
		}

		public static void RequestRating(RatingDialogContent dialogContent)
		{
			DoRequestRating(dialogContent, null);
		}

		public static void RequestRating(RatingDialogContent dialogContent, Action<UserAction> callback)
		{
			DoRequestRating(dialogContent, callback);
		}

		public static RatingDialogContent GetDefaultDialogContent()
		{
			return EM_Settings.RatingRequest.DefaultRatingDialogContent;
		}

		public static bool CanRequestRating()
		{
			if (IsDisplayConstraintIgnored())
			{
				return !IsRatingRequestDisabled();
			}
			return !IsRatingRequestDisabled() && GetRemainingDelayAfterInstallation() <= 0 && GetThisYearRemainingRequests() > 0 && GetRemainingCoolingOffDays() <= 0;
		}

		public static bool IsDisplayConstraintIgnored()
		{
			return Helper.IsUnityDevelopmentBuild() && EM_Settings.RatingRequest.IgnoreConstraintsInDevelopment;
		}

		public static bool IsRatingRequestDisabled()
		{
			return PlayerPrefs.GetInt("EM_RATING_REQUEST_DISABLE", 1) == -1;
		}

		public static int GetRemainingDelayAfterInstallation()
		{
			int days = (DateTime.Now - Helper.GetAppInstallationTime()).Days;
			int num = (int)EM_Settings.RatingRequest.DelayAfterInstallation - days;
			return (num >= 0) ? num : 0;
		}

		public static int GetRemainingCoolingOffDays()
		{
			int days = (DateTime.Now - GetLastRequestTimestamp()).Days;
			int num = (int)EM_Settings.RatingRequest.CoolingOffPeriod - days;
			return (num >= 0) ? num : 0;
		}

		public static DateTime GetLastRequestTimestamp()
		{
			return Helper.GetTime("EM_RATING_REQUEST_LAST_REQUEST_TIMESTAMP", Helper.UnixEpoch);
		}

		public static int GetThisYearUsedRequests()
		{
			return GetAnnualUsedRequests(DateTime.Now.Year);
		}

		public static int GetThisYearRemainingRequests()
		{
			return GetAnnualRequestsLimit() - GetThisYearUsedRequests();
		}

		public static int GetAnnualRequestsLimit()
		{
			return (int)EM_Settings.RatingRequest.AnnualCap;
		}

		public static void DisableRatingRequest()
		{
			PlayerPrefs.SetInt("EM_RATING_REQUEST_DISABLE", -1);
			PlayerPrefs.Save();
		}

		private static int GetAnnualUsedRequests(int year)
		{
			string key = "EM_RATING_REQUESTS_MADE_YEAR_" + year;
			return PlayerPrefs.GetInt(key, 0);
		}

		private static void SetAnnualUsedRequests(int year, int requestNumber)
		{
			string key = "EM_RATING_REQUESTS_MADE_YEAR_" + year;
			PlayerPrefs.SetInt(key, requestNumber);
		}

		private static void DoRequestRating(RatingDialogContent content, Action<UserAction> callback)
		{
			if (!CanRequestRating())
			{
				Debug.Log("Could not display the rating request popup because it was disabled, or one or more display constraints are not satisfied.");
				return;
			}
			if (content == null)
			{
				content = EM_Settings.RatingRequest.DefaultRatingDialogContent;
			}
			customBehaviour = callback;
			if (!(Instance != null))
			{
				Instance = new GameObject(RATING_DIALOG_GAMEOBJECT).AddComponent<MobileNativeRatingRequest>();
				RatingDialogContent content2 = new RatingDialogContent(content.Title.Replace("$PRODUCT_NAME", Application.productName), content.Message.Replace("$PRODUCT_NAME", Application.productName), content.LowRatingMessage.Replace("$PRODUCT_NAME", Application.productName), content.HighRatingMessage.Replace("$PRODUCT_NAME", Application.productName), content.PostponeButtonText, content.RefuseButtonText, content.RateButtonText, content.CancelButtonText, content.FeedbackButtonText);
				AndroidNativeUtility.RequestRating(content2, EM_Settings.RatingRequest);
				if (!IsDisplayConstraintIgnored())
				{
					SetAnnualUsedRequests(DateTime.Now.Year, GetAnnualUsedRequests(DateTime.Now.Year) + 1);
					Helper.StoreTime("EM_RATING_REQUEST_LAST_REQUEST_TIMESTAMP", DateTime.Now);
				}
			}
		}

		private static void DefaultCallback(UserAction action)
		{
			if (customBehaviour != null)
			{
				customBehaviour(action);
			}
			else
			{
				PerformDefaultBehaviour(action);
			}
		}

		private static void PerformDefaultBehaviour(UserAction action)
		{
			switch (action)
			{
			case UserAction.Refuse:
				DisableRatingRequest();
				break;
			case UserAction.Postpone:
				break;
			case UserAction.Feedback:
				Application.OpenURL("mailto:" + EM_Settings.RatingRequest.SupportEmail);
				break;
			case UserAction.Rate:
				if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					Application.OpenURL("itms-apps://itunes.apple.com/app/id" + EM_Settings.RatingRequest.IosAppId + "?action=write-review");
				}
				else if (Application.platform == RuntimePlatform.Android)
				{
					Application.OpenURL("market://details?id=" + Application.identifier);
				}
				DisableRatingRequest();
				break;
			}
		}

		private static UserAction ConvertToUserAction(int index)
		{
			return index switch
			{
				0 => UserAction.Refuse, 
				1 => UserAction.Postpone, 
				2 => UserAction.Feedback, 
				3 => UserAction.Rate, 
				_ => UserAction.Postpone, 
			};
		}

		private void OnAndroidRatingDialogCallback(string userAction)
		{
			int index = Convert.ToInt16(userAction);
			DefaultCallback(ConvertToUserAction(index));
			Instance = null;
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
