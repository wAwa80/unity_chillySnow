using System.Collections.Generic;
using UnityEngine;


namespace EndlessMode
{
	public static class Tenjin
	{
		public delegate void DeferredDeeplinkDelegate(Dictionary<string, string> deferredLinkData);

		private static Dictionary<string, BaseTenjin> _instances = new Dictionary<string, BaseTenjin>();

		public static BaseTenjin getInstance(string apiKey)
		{
			if (!_instances.ContainsKey(apiKey))
			{
				_instances.Add(apiKey, createTenjin(apiKey));
			}
			return _instances[apiKey];
		}

		private static BaseTenjin createTenjin(string apiKey)
		{
			GameObject gameObject = new GameObject("Tenjin");
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			Object.DontDestroyOnLoad(gameObject);
			BaseTenjin baseTenjin = gameObject.AddComponent<AndroidTenjin>();
			baseTenjin.Init(apiKey);
			return baseTenjin;
		}
	}
}
