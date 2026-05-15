using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace EasyMobile
{
	[AddComponentMenu("")]
	public class MobileNativeAlert : MonoBehaviour
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<int> m_OnComplete;

		private static readonly string ALERT_GAMEOBJECT = "MobileNativeAlert";

		public static MobileNativeAlert Instance { get; private set; }

		public event Action<int> OnComplete
		{
			add
			{
				Action<int> action = this.m_OnComplete;
				Action<int> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref this.m_OnComplete, (Action<int>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<int> action = this.m_OnComplete;
				Action<int> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref this.m_OnComplete, (Action<int>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public MobileNativeAlert()
		{
			//this.OnComplete = delegate
			//{
			//};
			//base._002Ector();
		}

		internal static MobileNativeAlert ShowThreeButtonAlert(string title, string message, string button1, string button2, string button3)
		{
			if (Instance != null)
			{
				return null;
			}
			Instance = new GameObject(ALERT_GAMEOBJECT).AddComponent<MobileNativeAlert>();
			AndroidNativeAlert.ShowThreeButtonsAlert(title, message, button1, button2, button3);
			return Instance;
		}

		internal static MobileNativeAlert ShowTwoButtonAlert(string title, string message, string button1, string button2)
		{
			if (Instance != null)
			{
				return null;
			}
			Instance = new GameObject(ALERT_GAMEOBJECT).AddComponent<MobileNativeAlert>();
			AndroidNativeAlert.ShowTwoButtonsAlert(title, message, button1, button2);
			return Instance;
		}

		internal static MobileNativeAlert ShowOneButtonAlert(string title, string message, string button)
		{
			if (Instance != null)
			{
				return null;
			}
			Instance = new GameObject(ALERT_GAMEOBJECT).AddComponent<MobileNativeAlert>();
			AndroidNativeAlert.ShowOneButtonAlert(title, message, button);
			return Instance;
		}

		internal static MobileNativeAlert Alert(string title, string message)
		{
			return ShowOneButtonAlert(title, message, "OK");
		}

		internal static void ShowToast(string message, bool isLongToast = false)
		{
			AndroidNativeAlert.ShowToast(message, isLongToast);
		}

		private void OnNativeAlertCallback(string buttonIndex)
		{
			int obj = Convert.ToInt16(buttonIndex);
			//this.OnComplete(obj);
			Instance = null;
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
