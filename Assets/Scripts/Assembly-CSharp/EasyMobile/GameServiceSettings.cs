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
		private bool _autoInit = true;

		[SerializeField]
		private float _autoInitDelay;

		[SerializeField]
		private int _androidMaxLoginRequests = 3;

		[SerializeField]
		private Leaderboard[] _leaderboards;

		[SerializeField]
		private Achievement[] _achievements;

		[SerializeField]
		private string _androidXmlResources = string.Empty;

		public bool IsGPGSDebug
		{
			get
			{
				return _gpgsDebugLog;
			}
			set
			{
				_gpgsDebugLog = value;
			}
		}

		public bool IsAutoInit => _autoInit;

		public float AutoInitDelay => _autoInitDelay;

		public int AndroidMaxLoginRequests => _androidMaxLoginRequests;

		public Leaderboard[] Leaderboards => _leaderboards;

		public Achievement[] Achievements => _achievements;
	}
}
