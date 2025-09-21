using UnityEngine;

namespace EasyMobile
{
	public class EM_Settings : ScriptableObject
	{
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
	}
}
