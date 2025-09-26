using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

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
		Continue
	}

	private static readonly Dictionary<string, SystemEvent> systemEventNames;

	private static readonly Dictionary<SystemEvent, PriorityChain<Neuron>> systemNeurons;

	private static readonly Dictionary<string, BaseEvent> baseEventNames;

	private static readonly Dictionary<BaseEvent, PriorityChain<Neuron>> baseNeurons;

	private static readonly Dictionary<string, GameEvent> gameEventNames;

	private static readonly Dictionary<GameEvent, PriorityChain<Neuron>> gameNeurons;

	private static Run currentRun;

	private static bool isPlaying;

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
		foreach (Neuron item in baseNeurons[BaseEvent.StartRun])
		{
			item.OnStartRun(run);
		}
	}

	public static void EndRun()
	{
		foreach (Neuron item in baseNeurons[BaseEvent.EndRun])
		{
			item.OnEndRun();
		}
	}

	public static void Refresh()
	{
		isPlaying = false;
		foreach (Neuron item in baseNeurons[BaseEvent.Refresh])
		{
			item.OnRefresh();
		}
		GC.Collect();
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

	public static void MeterPlusOne()
	{
		currentRun.score += Level.Get();
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
