using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LevelMode
{

	[DefaultExecutionOrder(-100)]
	public abstract class Neuron : MonoBehaviour, IPriority
	{
		public enum SystemEvent
		{
			InternetStateChanged,
			ApplicationPause,
			ApplicationResume,
			Purchased
		}

		public enum BaseEvent
		{
			Tap,
			StartRun,
			EndRun,
			Refresh
		}

		public enum GameEvent
		{
			MeterPlusOne,
			Whoosh,
			Continue,
			// 对齐无尽 App.State.Pause / NightModeSwitched；用 canPause 收窄 EndRun 后仍 IsPlaying 的窗口
			Pause,
			Unpause,
			NightModeSwitched
		}

		private static readonly Dictionary<string, SystemEvent> systemEventNames;

		private static readonly Dictionary<SystemEvent, PriorityChain<Neuron>> systemNeurons;

		private static readonly Dictionary<string, BaseEvent> baseEventNames;

		private static readonly Dictionary<BaseEvent, PriorityChain<Neuron>> baseNeurons;

		private static readonly Dictionary<string, GameEvent> gameEventNames;

		private static readonly Dictionary<GameEvent, PriorityChain<Neuron>> gameNeurons;

		private static Run currentRun;

		private static bool isPlaying;

		/// <summary>局内暂停（timeScale / 输入门控）。</summary>
		private static bool isPaused;

		/// <summary>仅 StartRun→EndRun 之间可暂停，避免结算/Continue 期间误暂停。</summary>
		private static bool canPause;

		static Neuron()
		{
			systemEventNames = new Dictionary<string, SystemEvent>();
			systemNeurons = new Dictionary<SystemEvent, PriorityChain<Neuron>>();
			foreach (SystemEvent value in Enum.GetValues(typeof(SystemEvent)))
			{
				systemEventNames.Add($"On{value.ToString()}", value);
				systemNeurons.Add(value, new PriorityChain<Neuron>());
			}
			baseEventNames = new Dictionary<string, BaseEvent>();
			baseNeurons = new Dictionary<BaseEvent, PriorityChain<Neuron>>();
			foreach (BaseEvent value2 in Enum.GetValues(typeof(BaseEvent)))
			{
				baseEventNames.Add($"On{value2.ToString()}", value2);
				baseNeurons.Add(value2, new PriorityChain<Neuron>());
			}
			gameEventNames = new Dictionary<string, GameEvent>();
			gameNeurons = new Dictionary<GameEvent, PriorityChain<Neuron>>();
			foreach (GameEvent value3 in Enum.GetValues(typeof(GameEvent)))
			{
				gameEventNames.Add($"On{value3.ToString()}", value3);
				gameNeurons.Add(value3, new PriorityChain<Neuron>());
			}
		}

		public virtual int GetPriority()
		{
			return 0;
		}

		protected virtual void Awake()
		{
			if (currentRun == null)
			{
				currentRun = Run.GetDefault();
			}
			MethodInfo[] methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
			foreach (MethodInfo methodInfo in methods)
			{
				if (methodInfo.IsVirtual && methodInfo.DeclaringType != typeof(Neuron))
				{
					if (systemEventNames.ContainsKey(methodInfo.Name))
					{
						systemNeurons[systemEventNames[methodInfo.Name]].Add(this);
					}
					else if (baseEventNames.ContainsKey(methodInfo.Name))
					{
						baseNeurons[baseEventNames[methodInfo.Name]].Add(this);
					}
					else if (gameEventNames.ContainsKey(methodInfo.Name))
					{
						gameNeurons[gameEventNames[methodInfo.Name]].Add(this);
					}
				}
			}
		}

		protected virtual void OnDestroy()
		{
			foreach (PriorityChain<Neuron> value in systemNeurons.Values)
			{
				value.Remove(this);
			}
			foreach (PriorityChain<Neuron> value2 in baseNeurons.Values)
			{
				value2.Remove(this);
			}
			foreach (PriorityChain<Neuron> value3 in gameNeurons.Values)
			{
				value3.Remove(this);
			}
		}

		public static Run GetCurrentRun()
		{
			return currentRun;
		}

		public static bool IsPlaying()
		{
			return isPlaying;
		}

		public static bool IsPaused()
		{
			return isPaused;
		}

		public static bool CanPause()
		{
			return canPause;
		}

		protected virtual void OnInternetStateChanged(bool hasInternet)
		{
		}

		public static void InternetStateChanged(bool hasInternet)
		{
			foreach (Neuron item in systemNeurons[SystemEvent.InternetStateChanged])
			{
				item.OnInternetStateChanged(hasInternet);
			}
		}

		protected virtual void OnApplicationPause()
		{
		}

		public static void ApplicationPause()
		{
			foreach (Neuron item in systemNeurons[SystemEvent.ApplicationPause])
			{
				item.OnApplicationPause();
			}
		}

		protected virtual void OnApplicationResume()
		{
		}

		public static void ApplicationResume()
		{
			foreach (Neuron item in systemNeurons[SystemEvent.ApplicationResume])
			{
				item.OnApplicationResume();
			}
		}

		protected virtual void OnPurchased(string productID, bool restored)
		{
		}

		public static void Purchased(string productID, bool restored)
		{
			foreach (Neuron item in systemNeurons[SystemEvent.Purchased])
			{
				item.OnPurchased(productID, restored);
			}
		}

		protected virtual void OnTap()
		{
		}

		protected virtual void OnStartRun(Run run)
		{
		}

		protected virtual void OnEndRun()
		{
		}

		protected virtual void OnRefresh()
		{
		}

		public static void Tap()
		{
			foreach (Neuron item in baseNeurons[BaseEvent.Tap])
			{
				item.OnTap();
			}
		}

		public static void StartRun(Run run)
		{
			currentRun = run;
			isPlaying = true;
			canPause = true;
			foreach (Neuron item in baseNeurons[BaseEvent.StartRun])
			{
				item.OnStartRun(run);
			}
		}

		public static void EndRun()
		{
			// 先解暂停，避免 GameCamera.Invoke 等 scaled 定时器被冻住
			Unpause();
			canPause = false;
			foreach (Neuron item in baseNeurons[BaseEvent.EndRun])
			{
				item.OnEndRun();
			}
		}

		public static void Refresh()
		{
			Unpause();
			isPlaying = false;
			canPause = false;
			foreach (Neuron item in baseNeurons[BaseEvent.Refresh])
			{
				item.OnRefresh();
			}
			// 禁止同步 GC.Collect：与刷树同帧会尖峰；改由菜单/系统时机回收
		}

		protected virtual void OnMeterPlusOne()
		{
		}

		protected virtual void OnWhoosh(int points)
		{
		}

		protected virtual void OnContinue()
		{
		}

		protected virtual void OnPause()
		{
		}

		protected virtual void OnUnpause()
		{
		}

		protected virtual void OnNightModeSwitched(bool enabled)
		{
		}

		/// <summary>
		/// 局内暂停。无 PausePage 时直接 timeScale=0（降级）；有则由 PausePage 淡入后置 0。
		/// </summary>
		public static void Pause()
		{
			if (!canPause || isPaused)
			{
				return;
			}
			isPaused = true;
			foreach (Neuron item in gameNeurons[GameEvent.Pause])
			{
				item.OnPause();
			}
			// 场景未挂 PausePage 时的降级：立即冻结
			if (PausePage.i == null)
			{
				Time.timeScale = 0f;
			}
		}

		/// <summary>
		/// 恢复滑行；可重入（EndRun/Refresh 安全调用）。始终校正 timeScale。
		/// </summary>
		public static void Unpause()
		{
			Time.timeScale = 1f;
			if (!isPaused)
			{
				return;
			}
			isPaused = false;
			foreach (Neuron item in gameNeurons[GameEvent.Unpause])
			{
				item.OnUnpause();
			}
		}

		public static void NightModeSwitched(bool enabled)
		{
			foreach (Neuron item in gameNeurons[GameEvent.NightModeSwitched])
			{
				item.OnNightModeSwitched(enabled);
			}
		}

		public static void MeterPlusOne()
		{
			// 关卡：每米加关卡号倍率；无尽：每米 +1（与关卡号解耦）
			if (GameMode.IsEndless)
			{
				currentRun.score += 1;
			}
			else
			{
				currentRun.score += Level.Get();
			}
			foreach (Neuron item in gameNeurons[GameEvent.MeterPlusOne])
			{
				item.OnMeterPlusOne();
			}
		}

		public static void Whoosh(int points)
		{
			currentRun.score += points;
			foreach (Neuron item in gameNeurons[GameEvent.Whoosh])
			{
				item.OnWhoosh(points);
			}
		}

		public static void Continue()
		{
			currentRun.usedSecondChance = true;
			foreach (Neuron item in gameNeurons[GameEvent.Continue])
			{
				item.OnContinue();
			}
		}
	}
}
