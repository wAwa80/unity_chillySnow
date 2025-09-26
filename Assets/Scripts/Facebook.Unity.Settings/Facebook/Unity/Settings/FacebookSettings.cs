using UnityEngine;
using System;
using System.Collections.Generic;

namespace Facebook.Unity.Settings
{
	public class FacebookSettings : ScriptableObject
	{
		[Serializable]
		public class UrlSchemes
		{
			[SerializeField]
			private List<string> list;
		}

		[SerializeField]
		private int selectedAppIndex;
		[SerializeField]
		private List<string> clientTokens;
		[SerializeField]
		private List<string> appIds;
		[SerializeField]
		private List<string> appLabels;
		[SerializeField]
		private bool cookie;
		[SerializeField]
		private bool logging;
		[SerializeField]
		private bool status;
		[SerializeField]
		private bool xfbml;
		[SerializeField]
		private bool frictionlessRequests;
		[SerializeField]
		private string iosURLSuffix;
		[SerializeField]
		private List<FacebookSettings.UrlSchemes> appLinkSchemes;
		[SerializeField]
		private string uploadAccessToken;
	}
}
