using System;
using System.Collections;
using UnityEngine;

namespace EasyMobile
{
	[AddComponentMenu("")]
	internal class Helper : MonoBehaviour
	{
		public static readonly DateTime UnixEpoch = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc);

		private static Helper _instance;

		public static Helper Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new GameObject("EM_Helper").AddComponent<Helper>();
					UnityEngine.Object.DontDestroyOnLoad(_instance.gameObject);
				}
				return _instance;
			}
		}

		private void OnDisable()
		{
			if (_instance == this)
			{
				_instance = null;
			}
		}

		private static void DestroyProxy()
		{
			if (_instance != null)
			{
				UnityEngine.Object.Destroy(_instance.gameObject);
				_instance = null;
			}
		}

		public static DateTime GetAppInstallationTime()
		{
			if (EM_PrefabManager.Instance != null)
			{
				return EM_PrefabManager.Instance.GetAppInstallationTimestamp();
			}
			return UnixEpoch;
		}

		public static bool IsUnityDevelopmentBuild()
		{
			return false;
		}

		public static void RunCoroutine(IEnumerator routine)
		{
			if (routine != null)
			{
				Instance.StartCoroutine(routine);
			}
		}

		public static void EndCoroutine(IEnumerator routine)
		{
			if (routine != null)
			{
				Instance.StopCoroutine(routine);
			}
		}

		public static T NullArgumentTest<T>(T value)
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			return value;
		}

		public static T NullArgumentTest<T>(T value, string paramName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(paramName);
			}
			return value;
		}

		public static DateTime FromMillisSinceUnixEpoch(long millisSinceEpoch)
		{
			return UnixEpoch.Add(TimeSpan.FromMilliseconds(millisSinceEpoch));
		}

		public static long ToMilliseconds(TimeSpan span)
		{
			double totalMilliseconds = span.TotalMilliseconds;
			if (totalMilliseconds > 9.223372036854776E+18)
			{
				return long.MaxValue;
			}
			if (totalMilliseconds < -9.223372036854776E+18)
			{
				return long.MinValue;
			}
			return Convert.ToInt64(totalMilliseconds);
		}

		public static void StoreTime(string ppkey, DateTime time)
		{
			PlayerPrefs.SetString(ppkey, time.ToBinary().ToString());
			PlayerPrefs.Save();
		}

		public static DateTime GetTime(string ppkey, DateTime defaultTime)
		{
			string @string = PlayerPrefs.GetString(ppkey, string.Empty);
			if (!string.IsNullOrEmpty(@string))
			{
				return DateTime.FromBinary(Convert.ToInt64(@string));
			}
			return defaultTime;
		}
	}
}
