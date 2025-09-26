using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

namespace JuiceInternal
{
	public sealed class Translator : Module<Translator>
	{
		private readonly Dictionary<string, string> translations = new Dictionary<string, string>();

		private bool loaded;

		private bool hasTranslations;

		private const string TEXT = "text";

		private const string VALUE = "value";

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void TranslateGame()
		{
			if (!(Module<Translator>.GetInstance() == null))
			{
				Text[] array = Object.FindObjectsOfType<Text>();
				Module<Translator>.LogMessage($"Translating {array.Length} texts");
				Text[] array2 = array;
				foreach (Text text in array2)
				{
					text.text = Module<Translator>.GetInstance().Translate(text.text);
				}
			}
		}

		public string Translate(string baseText)
		{
			if (!hasTranslations)
			{
				return baseText;
			}
			if (string.IsNullOrEmpty(baseText))
			{
				return baseText;
			}
			string key = baseText.ToLower();
			if (translations.ContainsKey(key))
			{
				if (IsAllUpper(baseText))
				{
					return translations[key].ToUpper();
				}
				return translations[key];
			}
			Module<Translator>.LogDebugWarning($"Missing translation : \"{baseText}\"");
			return baseText;
		}

		private static bool IsAllUpper(string input)
		{
			foreach (char c in input)
			{
				if (char.IsLetter(c) && !char.IsUpper(c))
				{
					return false;
				}
			}
			return true;
		}

		protected override void OnSetup()
		{
			if (!loaded)
			{
				loaded = true;
				TextAsset textAsset = (TextAsset)Resources.Load("language");
				if (textAsset == null)
				{
					Module<Translator>.LogDebugWarning("No translation has been added to the game !");
					hasTranslations = false;
					return;
				}
				hasTranslations = true;
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(textAsset.text);
				LoadLanguage(xmlDocument.FirstChild, Application.systemLanguage.ToString().ToLower());
			}
		}

		private void LoadLanguage(XmlNode document, string language)
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
						translations.Add(childNode.Attributes["value"].Value.ToLower(), childNode2.InnerText);
						break;
					}
				}
			}
		}
	}
}
