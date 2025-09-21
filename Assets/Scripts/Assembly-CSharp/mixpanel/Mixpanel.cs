using UnityEngine;
using mixpanel.detail;

namespace mixpanel
{
	public class Mixpanel : MonoBehaviour
	{
		public string token;
		public string debugToken;
		public bool trackInEditor;
		public Mixpanel.LogEntry.Level minLogLevel;
		public int flushInterval;
	}
}
