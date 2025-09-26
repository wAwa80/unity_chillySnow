using UnityEngine;

namespace PaperPlaneTools
{
	public class RateBoxPrefabScript : MonoBehaviour
	{
		public int minSessionCount;
		public int minCustomEventsCount;
		public float delayAfterInstallInHours;
		public float delayAfterLaunchInHours;
		public float postponeCooldownInHours;
		public bool requireInternetConnection;
		public string title;
		public string message;
		public string rateButton;
		public string postponeButton;
		public string rejectButton;
		public string appStoreAppId;
		public string playMarketAppBundleId;
		public bool useIOSReview;
		public GameObject customUIWindow;
		public bool clearOnStart;
		public bool logDebugMessages;
	}
}
