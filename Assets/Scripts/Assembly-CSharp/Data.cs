using UnityEngine;

public sealed class Data : Singleton<Data>
{
	private static bool needsSave;

	private void LateUpdate()
	{
		if (needsSave)
		{
			PlayerPrefs.Save();
			needsSave = false;
		}
	}

	public static void Reset()
	{
		PlayerPrefs.DeleteAll();
	}

	public static bool HasKey(string key)
	{
		return PlayerPrefs.HasKey(key);
	}

	public static void SaveBool(string key, bool value)
	{
		PlayerPrefs.SetInt(key, value ? 1 : 0);
		needsSave = true;
	}

	public static void SaveInt(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
		needsSave = true;
	}

	public static void SaveFloat(string key, float value)
	{
		PlayerPrefs.SetFloat(key, value);
		needsSave = true;
	}

	public static void SaveString(string key, string value)
	{
		PlayerPrefs.SetString(key, value);
		needsSave = true;
	}

	public static bool LoadBool(string key, bool defaultValue = false)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefs.GetInt(key) == 1;
		}
		return defaultValue;
	}

	public static int LoadInt(string key, int defaultValue = 0)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefs.GetInt(key);
		}
		return defaultValue;
	}

	public static float LoadFloat(string key, float defaultValue = 0f)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefs.GetFloat(key);
		}
		return defaultValue;
	}

	public static string LoadString(string key, string defaultValue = null)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefs.GetString(key);
		}
		return defaultValue;
	}
}
