using UnityEngine;

namespace EasyMobile
{
	public class EM_Settings : ScriptableObject
	{
		private static EM_Settings _instance;

		[SerializeField]
		private AdSettings _advertisingSettings;

		[SerializeField]
		private GameServiceSettings _gameServiceSettings;

		[SerializeField]
		private IAPSettings _inAppPurchaseSettings;

		[SerializeField]
		private NotificationSettings _notificationSettings;

		[SerializeField]
		private RatingRequestSettings _ratingRequestSettings;

		[SerializeField]
		private bool _isAdModuleEnable;

		[SerializeField]
		private bool _isIAPModuleEnable;

		[SerializeField]
		private bool _isGameServiceModuleEnable;

		[SerializeField]
		private bool _isNotificationModuleEnable;

		public static EM_Settings Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = LoadSettingsAsset();
					if (_instance == null)
					{
						Debug.LogError("Easy Mobile settings not found! Please go to Tools>Easy Mobile>Settings to setup the plugin.");
						_instance = ScriptableObject.CreateInstance<EM_Settings>();
					}
				}
				return _instance;
			}
		}

		public static AdSettings Advertising => Instance._advertisingSettings;

		public static GameServiceSettings GameService => Instance._gameServiceSettings;

		public static IAPSettings InAppPurchasing => Instance._inAppPurchaseSettings;

		public static NotificationSettings Notification => Instance._notificationSettings;

		public static RatingRequestSettings RatingRequest => Instance._ratingRequestSettings;

		public static bool IsAdModuleEnable => Instance._isAdModuleEnable;

		public static bool IsIAPModuleEnable => Instance._isIAPModuleEnable;

		public static bool IsGameServiceModuleEnable => Instance._isGameServiceModuleEnable;

		public static bool IsNotificationModuleEnable => Instance._isNotificationModuleEnable;

		public static EM_Settings LoadSettingsAsset()
		{
			return Resources.Load("EM_Settings") as EM_Settings;
		}
	}
}
