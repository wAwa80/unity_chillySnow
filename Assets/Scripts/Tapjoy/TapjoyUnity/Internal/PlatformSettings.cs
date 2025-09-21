using System;
using UnityEngine;

namespace TapjoyUnity.Internal
{
	[Serializable]
	public class PlatformSettings
	{
		[SerializeField]
		private string sdkKey;
		[SerializeField]
		private string pushKey;
		[SerializeField]
		private bool disableAdvertisingId;
		[SerializeField]
		private bool disablePersistentIds;
		[SerializeField]
		private FyberSettings fyberSettings;
	}
}
