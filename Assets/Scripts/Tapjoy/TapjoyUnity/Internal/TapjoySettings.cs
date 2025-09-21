using UnityEngine;
using TapjoyUnity;

namespace TapjoyUnity.Internal
{
	public class TapjoySettings : ScriptableObject
	{
		[SerializeField]
		private PlatformSettings androidSettings;
		[SerializeField]
		private PlatformSettings iosSettings;
		[SerializeField]
		private bool autoConnectEnabled;
		[SerializeField]
		private bool debugEnabled;
		[SerializeField]
		private TapjoyRuntimeCallbacks tjCallbacks;
		[SerializeField]
		private string hostURL;
		[SerializeField]
		private string eventURL;
	}
}
