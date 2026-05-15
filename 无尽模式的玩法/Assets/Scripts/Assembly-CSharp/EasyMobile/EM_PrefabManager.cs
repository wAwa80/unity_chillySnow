using System;
using UnityEngine;

namespace EasyMobile
{
	public class EM_PrefabManager : MonoBehaviour
	{
		private const string APP_INSTALLATION_TIMESTAMP_PPKEY = "EM_APP_INSTALLATION_TIMESTAMP";

		public static EM_PrefabManager Instance { get; private set; }

		private void Awake()
		{
			if (Instance != null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			if (Helper.GetTime("EM_APP_INSTALLATION_TIMESTAMP", Helper.UnixEpoch) == Helper.UnixEpoch)
			{
				Helper.StoreTime("EM_APP_INSTALLATION_TIMESTAMP", DateTime.Now);
			}
			Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			//SetLogEnabled(isEnabled: false);
		}

		private void SetLogEnabled(bool isEnabled)
		{
			Debug.unityLogger.logEnabled = isEnabled;
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		public DateTime GetAppInstallationTimestamp()
		{
			return Helper.GetTime("EM_APP_INSTALLATION_TIMESTAMP", Helper.UnixEpoch);
		}
	}
}
