using System.Collections.Generic;
using System.Xml;
using UnityEngine;


namespace EndlessMode
{
	public static class Translator
	{
		private static readonly Dictionary<string, string> traductions = new Dictionary<string, string>();

		private static bool loaded = false;

		public static string Translate(string baseText)
		{
			if (baseText.Length == 0)
			{
				return baseText;
			}
			try
			{
				return traductions[baseText.ToLower()];
			}
			catch
			{
				return baseText;
			}
		}

		public static void Load()
		{
			if (!loaded)
			{
				TextAsset textAsset = (TextAsset)Resources.Load("language");
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(textAsset.text);
				// 强制默认简体中文，忽略设备系统语言
				LoadLanguage(xmlDocument.FirstChild, "chinesesimplified");
				loaded = true;
			}
		}

		private static void LoadLanguage(XmlNode document, string language)
		{
			foreach (XmlNode childNode in document.ChildNodes)
			{
				if (childNode.Name != "text")
				{
					continue;
				}
				foreach (XmlNode childNode2 in childNode.ChildNodes)
				{
					if (childNode2.Name == language)
					{
						traductions.Add(childNode.Attributes["value"].Value.ToLower(), childNode2.InnerText);
						break;
					}
				}
			}
		}
	}
}
