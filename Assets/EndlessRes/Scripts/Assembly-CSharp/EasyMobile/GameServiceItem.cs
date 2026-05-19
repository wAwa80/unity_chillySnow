using System;
using UnityEngine;

namespace EasyMobile
{
	[Serializable]
	public class GameServiceItem
	{
		[SerializeField]
		private string _name;

		[SerializeField]
		private string _iosId;

		[SerializeField]
		private string _androidId;

		public string Name => _name;

		public string IOSId => _iosId;

		public string AndroidId => _androidId;

		public string Id => _androidId;

		public GameServiceItem(string name, string iosId, string androidId)
		{
			_name = name;
			_iosId = iosId;
			_androidId = androidId;
		}
	}
}
