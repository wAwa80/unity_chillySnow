using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

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
		NightModeSwitched
	}

	private static readonly Dictionary<string, Event> eventNames;

	private static readonly Dictionary<Event, Chain<Neuron>> neurons;

	static Neuron()
	{
		eventNames = new Dictionary<string, Event>();
		foreach (Event value in Enum.GetValues(typeof(Event)))
		{
			eventNames.Add($"On{value.ToString()}", value);
		}
		neurons = new Dictionary<Event, Chain<Neuron>>();
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
				if (neurons.ContainsKey(key))
				{
					neurons[key].Add(this);
					continue;
				}
				Chain<Neuron> chain = new Chain<Neuron>();
				chain.Add(this);
				neurons.Add(key, chain);
			}
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
			item.OnBackToMenu();
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
}
