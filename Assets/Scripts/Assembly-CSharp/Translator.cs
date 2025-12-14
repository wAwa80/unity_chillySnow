using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public static class Translator
{
	private static readonly Dictionary<string, string> traductions = new Dictionary<string, string>();

	private static bool loaded = false;

	public static string Translate(string baseText)
	{
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
			LoadLanguage(xmlDocument.FirstChild, Application.systemLanguage.ToString().ToLower());
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
