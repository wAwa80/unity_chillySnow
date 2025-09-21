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
	}
}
