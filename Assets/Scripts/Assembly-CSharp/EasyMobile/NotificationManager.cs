using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace EasyMobile
{
	[AddComponentMenu("")]
	public class NotificationManager : MonoBehaviour
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<string, string, Dictionary<string, object>, bool> m_NotificationOpened;

		public static NotificationManager Instance { get; private set; }

		public static event Action<string, string, Dictionary<string, object>, bool> NotificationOpened
		{
			add
			{
				Action<string, string, Dictionary<string, object>, bool> action = NotificationManager.m_NotificationOpened;
				Action<string, string, Dictionary<string, object>, bool> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref NotificationManager.m_NotificationOpened, (Action<string, string, Dictionary<string, object>, bool>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<string, string, Dictionary<string, object>, bool> action = NotificationManager.m_NotificationOpened;
				Action<string, string, Dictionary<string, object>, bool> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref NotificationManager.m_NotificationOpened, (Action<string, string, Dictionary<string, object>, bool>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		private void Awake()
		{
			if (Instance != null)
			{
				UnityEngine.Object.Destroy(this);
			}
			else
			{
				Instance = this;
			}
		}

		private void Start()
		{
			if (EM_Settings.Notification.IsAutoInit)
			{
				StartCoroutine(CRAutoInit(EM_Settings.Notification.AutoInitDelay));
			}
		}

		private IEnumerator CRAutoInit(float delay)
		{
			yield return new WaitForSeconds(delay);
			Init();
		}

		public static void Init()
		{
			Debug.LogError("SDK missing. Please import OneSignal plugin for Unity.");
		}
	}
}
