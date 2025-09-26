using System;
using UnityEngine;

namespace JuiceInternal
{
	public abstract class Module<T> : ModuleBase where T : Module<T>
	{
		protected static string LOG_PREFIX;

		private static T instance;

		internal static T GetInstance()
		{
			return instance;
		}

		private static T internal_Setup()
		{
			Type typeFromHandle = typeof(T);
			LOG_PREFIX = typeFromHandle.ToString();
			try
			{
				if ((UnityEngine.Object)instance != (UnityEngine.Object)null)
				{
					return instance;
				}
				Log.Message(LOG_PREFIX, "Performing instantiation...");
				instance = new GameObject($"JUICE MODULE {LOG_PREFIX}").AddComponent<T>();
				UnityEngine.Object.DontDestroyOnLoad(instance.gameObject);
				try
				{
					Log.Message(LOG_PREFIX, "Performing setup...");
					instance.OnSetup();
					Log.Message(LOG_PREFIX, "Setup complete !");
				}
				catch (Exception ex)
				{
					Log.Warning(LOG_PREFIX, $"Could not perform setup successfully. Exception was thrown : \"{ex.Message}\" ({ex.StackTrace})");
				}
				return instance;
			}
			catch (Exception ex2)
			{
				Log.Warning(LOG_PREFIX, $"Could not perform instantiation successfully. Exception was thrown : \"{ex2.Message}\" ({ex2.StackTrace})");
				return (T)null;
			}
		}

		protected static void LogMessage(string message)
		{
			Log.Message(LOG_PREFIX, message);
		}

		protected static void LogDebugWarning(string message)
		{
			Log.DebugWarning(LOG_PREFIX, message);
		}

		protected static void LogWarning(string message)
		{
			Log.Warning(LOG_PREFIX, message);
		}
	}
}
