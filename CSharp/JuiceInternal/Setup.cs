using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace JuiceInternal
{
	internal static class Setup
	{
		private static bool setupDone;

		private static bool startupDone;

		private static readonly HashSet<ModuleBase> modules = new HashSet<ModuleBase>();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void SetupAll()
		{
			if (setupDone)
			{
				return;
			}
			if (Settings.Get().debug && Settings.Get().resetOnStart)
			{
				PlayerPrefs.DeleteAll();
			}
			typeof(GDPR).GetMethod("Setup", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
			Type[] types = Assembly.GetExecutingAssembly().GetTypes();
			foreach (Type type in types)
			{
				Type moduleType = GetModuleType(type);
				if (moduleType != null)
				{
					ModuleBase moduleBase = (ModuleBase)moduleType.GetMethod("internal_Setup", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
					if (moduleBase != null)
					{
						modules.Add(moduleBase);
					}
				}
			}
			setupDone = true;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void GameStarted()
		{
			if (startupDone)
			{
				return;
			}
			foreach (ModuleBase module in modules)
			{
				try
				{
					module.GetType().GetMethod("OnGameStarted", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(module, null);
				}
				catch (Exception ex)
				{
					Log.Warning(module.GetType().ToString(), $"Could not perform game startup successfully. Exception was thrown : \"{ex.Message}\" ({ex.StackTrace})");
				}
			}
			startupDone = true;
		}

		private static Type GetModuleType(Type type)
		{
			if (type.ContainsGenericParameters)
			{
				return null;
			}
			for (type = type.BaseType; type != null; type = type.BaseType)
			{
				if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Module<>))
				{
					return type;
				}
			}
			return null;
		}
	}
}
