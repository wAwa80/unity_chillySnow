using System;
using System.Reflection;
using UnityEngine;

public static class JuiceInitializer
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Setup()
	{
		if (Application.isPlaying)
		{
			if (ShouldResetOnStart())
			{
				Data.Reset();
			}
			Type[] types = Assembly.GetExecutingAssembly().GetTypes();
			foreach (Type type in types)
			{
				IsEssential(type)?.GetMethod("Setup").Invoke(null, null);
			}
		}
	}

	private static Type IsEssential(Type type)
	{
		if (type.ContainsGenericParameters)
		{
			return null;
		}
		for (type = type.BaseType; type != null; type = type.BaseType)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Essential<>))
			{
				return type;
			}
		}
		return null;
	}

	private static bool ShouldResetOnStart()
	{
		return false;
	}
}
