using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace EndlessMode
{
	[DefaultExecutionOrder(-100)]
	public abstract class Neuron : MonoBehaviour, IPriority
	{
		public enum Event
		{
			NewGame,
			MeterPlusOne,
			Whoosh,
			Pause,
			Unpause,
			GameOver,
			Continue,
			BackToMenu,
			Purchased,
			NightModeSwitched,
			SkinSelected
		}

		private static readonly Dictionary<string, Event> eventNames;

		private static readonly Dictionary<Event, Chain<Neuron>> neurons;

		static Neuron()
		{
			eventNames = new Dictionary<string, Event>();
			neurons = new Dictionary<Event, Chain<Neuron>>();
			// 预建每个 Event 的空 Chain，避免首次广播 KeyNotFound
			foreach (Event value in Enum.GetValues(typeof(Event)))
			{
				eventNames.Add($"On{value.ToString()}", value);
				neurons.Add(value, new Chain<Neuron>());
			}
		}

		public virtual int GetPriority()
		{
			return 0;
		}

		protected virtual void Awake()
		{
			MethodInfo[] methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
			foreach (MethodInfo methodInfo in methods)
			{
				if (methodInfo.IsVirtual && methodInfo.DeclaringType != typeof(Neuron) && eventNames.ContainsKey(methodInfo.Name))
				{
					Event key = eventNames[methodInfo.Name];
					neurons[key].Add(this);
				}
			}
		}

		/// <summary>
		/// 销毁时从所有事件链退订，防止域不重载/切场景后残留死引用。
		/// </summary>
		protected virtual void OnDestroy()
		{
			foreach (Chain<Neuron> value in neurons.Values)
			{
				value.Remove(this);
			}
		}

		protected virtual void OnNewGame()
		{
		}

		protected virtual void OnMeterPlusOne()
		{
		}

		protected virtual void OnWhoosh()
		{
		}

		protected virtual void OnPause()
		{
		}

		protected virtual void OnUnpause()
		{
		}

		protected virtual void OnGameOver(bool canUseSecondChance)
		{
		}

		protected virtual void OnContinue()
		{
		}

		protected virtual void OnBackToMenu()
		{
		}

		protected virtual void OnPurchased(string ProductID)
		{
		}

		protected virtual void OnNightModeSwitched(bool enabled)
		{
		}

		protected virtual void OnSkinSelected(Skin skin)
		{
		}

		public static void NewGame()
		{
			foreach (Neuron item in neurons[Event.NewGame])
			{
				item.OnNewGame();
			}
		}

		public static void MeterPlusOne()
		{
			foreach (Neuron item in neurons[Event.MeterPlusOne])
			{
				item.OnMeterPlusOne();
			}
		}

		public static void Whoosh()
		{
			foreach (Neuron item in neurons[Event.Whoosh])
			{
				item.OnWhoosh();
			}
		}

		public static void Pause()
		{
			foreach (Neuron item in neurons[Event.Pause])
			{
				item.OnPause();
			}
		}

		public static void Unpause()
		{
			foreach (Neuron item in neurons[Event.Unpause])
			{
				item.OnUnpause();
			}
		}

		public static void GameOver(bool canUseSecondChance)
		{
			foreach (Neuron item in neurons[Event.GameOver])
			{
				item.OnGameOver(canUseSecondChance);
			}
		}

		public static void Continue()
		{
			foreach (Neuron item in neurons[Event.Continue])
			{
				item.OnContinue();
			}
		}

		public static void BackToMenu()
		{
			foreach (Neuron item in neurons[Event.BackToMenu])
			{
				// 已销毁实例跳过；单订阅者异常不得中断后续（含 PineGenerator.Spawn）
				if (item == null)
				{
					continue;
				}
				try
				{
					item.OnBackToMenu();
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
			}
		}

		public static void Purchased(string productID)
		{
			foreach (Neuron item in neurons[Event.Purchased])
			{
				item.OnPurchased(productID);
			}
		}

		public static void NightModeSwitched(bool enabled)
		{
			foreach (Neuron item in neurons[Event.NightModeSwitched])
			{
				item.OnNightModeSwitched(enabled);
			}
		}

		public static void SkinSelected(Skin skin)
		{
			foreach (Neuron item in neurons[Event.SkinSelected])
			{
				item.OnSkinSelected(skin);
			}
		}
	}
}
