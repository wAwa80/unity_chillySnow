using System;
using UnityEngine;

public abstract class Essential<T> : Singleton<T> where T : Essential<T>
{
	public static void Setup()
	{
		if (!((UnityEngine.Object)Singleton<T>.i != (UnityEngine.Object)null))
		{
			Type typeFromHandle = typeof(T);
			UnityEngine.Object.DontDestroyOnLoad(new GameObject($"SYSTEM_OBJECT {typeFromHandle.ToString()}", typeFromHandle));
		}
	}
}
