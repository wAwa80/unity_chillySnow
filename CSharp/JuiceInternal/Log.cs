using UnityEngine;

namespace JuiceInternal
{
	internal static class Log
	{
		private const string FORMATTER = "(Juice - {0}) {1}";

		internal static void Message(string origin, string message)
		{
			if (Settings.Get().debug)
			{
				Debug.Log($"(Juice - {origin}) {message}");
			}
		}

		internal static void DebugWarning(string origin, string message)
		{
			if (Settings.Get().debug)
			{
				Debug.LogWarning($"(Juice - {origin}) {message}");
			}
		}

		internal static void Warning(string origin, string message)
		{
			Debug.LogWarning($"(Juice - {origin}) {message}");
		}
	}
}
