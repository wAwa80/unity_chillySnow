using System;
using UnityEngine;

namespace EasyMobile
{
	[Serializable]
	public class NotificationSettings
	{
		[SerializeField]
		private bool _autoInit;
		[SerializeField]
		private float _autoInitDelay;
		[SerializeField]
		private string _oneSignalAppId;
	}
}
