using System;
using UnityEngine;

namespace EasyMobile
{
	[Serializable]
	public class NotificationSettings
	{
		[SerializeField]
		private bool _autoInit = true;

		[SerializeField]
		private float _autoInitDelay;

		[SerializeField]
		private string _oneSignalAppId;

		public bool IsAutoInit => _autoInit;

		public float AutoInitDelay => _autoInitDelay;

		public string OneSignalAppId => _oneSignalAppId;
	}
}
