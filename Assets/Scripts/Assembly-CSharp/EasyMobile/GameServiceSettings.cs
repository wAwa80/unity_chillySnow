using System;
using UnityEngine;

namespace EasyMobile
{
	[Serializable]
	public class GameServiceSettings
	{
		[SerializeField]
		private bool _gpgsDebugLog;
		[SerializeField]
		private bool _autoInit;
		[SerializeField]
		private float _autoInitDelay;
		[SerializeField]
		private int _androidMaxLoginRequests;
		[SerializeField]
		private Leaderboard[] _leaderboards;
		[SerializeField]
		private Achievement[] _achievements;
		[SerializeField]
		private string _androidXmlResources;
	}
}
