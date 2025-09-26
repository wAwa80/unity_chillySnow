using UnityEngine;

public sealed class Data : Essential<Data>
{
	private static bool needsSave;

	private void LateUpdate()
	{
		if (needsSave)
		{
			Save();
		}
	}

	public static void Save()
	{
		PlayerPrefs.Save();
		needsSave = false;
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

	public static void SaveLong(string key, long value)
	{
		PlayerPrefs.SetString(key, value.ToString());
		needsSave = true;
	}

	public static void SaveFloat(string key, float value)
	{
		PlayerPrefs.SetFloat(key, value);
		needsSave = true;
	}

	public static void SaveDouble(string key, double value)
	{
		PlayerPrefs.SetString(key, value.ToString("R"));
		needsSave = true;
	}

	public static void SaveString(string key, string value)
	{
		PlayerPrefs.SetString(key, value);
		needsSave = true;
	}

	public static void SaveUnlimited(string key, unlimited value)
	{
		PlayerPrefs.SetFloat(key, value.value);
		PlayerPrefs.SetInt($"{key}_e", value.exposant);
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

	public static long LoadLong(string key, long defaultValue = 0L)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return long.Parse(PlayerPrefs.GetString(key));
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

	public static double LoadDouble(string key, double defaultValue = 0.0)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return double.Parse(PlayerPrefs.GetString(key));
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

	public static unlimited LoadUnlimited(string key, unlimited defaultValue = default(unlimited))
	{
		string key2 = $"{key}_e";
		if (PlayerPrefs.HasKey(key) && PlayerPrefs.HasKey(key2))
		{
			return new unlimited(PlayerPrefs.GetFloat(key), PlayerPrefs.GetInt(key2));
		}
		return defaultValue;
	}
}
