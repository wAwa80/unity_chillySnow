using System.Text;
using UnityEngine;

public struct unlimited
{
	public float value;

	public int exposant;

	private static readonly string LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	private static readonly string[] _CURRENCY_SHORTERS = new string[9]
	{
		string.Empty,
		"k",
		"m",
		"b",
		"t",
		"q",
		"Q",
		"u",
		"U"
	};

	public unlimited(float value)
		: this(value, 0)
	{
	}

	public unlimited(float value, int exposant)
	{
		this.value = value;
		if (value == 0f)
		{
			this.exposant = 0;
		}
		else
		{
			this.exposant = exposant;
		}
	}

	public static unlimited operator +(unlimited u1, unlimited u2)
	{
		unlimited result = ((u1.exposant == u2.exposant) ? new unlimited(u1.value + u2.value, u1.exposant) : ((u1.exposant <= u2.exposant) ? new unlimited(u2.value + u1.value * Mathf.Pow(0.001f, u2.exposant - u1.exposant), u2.exposant) : new unlimited(u1.value + u2.value * Mathf.Pow(0.001f, u1.exposant - u2.exposant), u1.exposant)));
		if (result.value >= 1000f)
		{
			result.value *= 0.001f;
			result.exposant++;
		}
		return result;
	}

	public static unlimited operator -(unlimited u1, unlimited u2)
	{
		if (u1.exposant < u2.exposant)
		{
			return new unlimited(0f);
		}
		if (u1.exposant == u2.exposant)
		{
			if (u1.value <= u2.value)
			{
				return new unlimited(0f);
			}
			unlimited result = new unlimited(u1.value - u2.value, u1.exposant);
			if (result.exposant > 0 && result.value < 1f)
			{
				result.value *= 1000f;
				result.exposant--;
			}
			return result;
		}
		unlimited result2 = new unlimited(u1.value - u2.value * Mathf.Pow(0.001f, u1.exposant - u2.exposant), u1.exposant);
		if (result2.exposant > 0 && result2.value < 1f)
		{
			result2.value *= 1000f;
			result2.exposant--;
		}
		return result2;
	}

	public static unlimited operator *(unlimited u, float f)
	{
		u.value *= f;
		while (u.value >= 1000f)
		{
			u.value *= 0.001f;
			u.exposant++;
		}
		while (u.exposant > 0 && u.value < 1f)
		{
			u.value *= 1000f;
			u.exposant--;
		}
		return u;
	}

	public static bool operator <(unlimited u1, unlimited u2)
	{
		if (u1.exposant != u2.exposant)
		{
			return u1.exposant < u2.exposant;
		}
		return u1.value < u2.value;
	}

	public static bool operator >(unlimited u1, unlimited u2)
	{
		if (u1.exposant != u2.exposant)
		{
			return u1.exposant > u2.exposant;
		}
		return u1.value > u2.value;
	}

	public static bool operator <=(unlimited u1, unlimited u2)
	{
		if (u1.exposant != u2.exposant)
		{
			return u1.exposant < u2.exposant;
		}
		return u1.value <= u2.value;
	}

	public static bool operator >=(unlimited u1, unlimited u2)
	{
		if (u1.exposant != u2.exposant)
		{
			return u1.exposant > u2.exposant;
		}
		return u1.value >= u2.value;
	}

	public override string ToString()
	{
		if (value == 0f)
		{
			return "0";
		}
		string arg = ((value < 10f) ? value.ToString("0.00") : ((!(value < 100f)) ? Mathf.RoundToInt(value).ToString() : value.ToString("00.0")));
		return $"{arg}{CURRENCY_SHORTER(exposant)}";
	}

	public static string CURRENCY_SHORTER(int power)
	{
		if (power < _CURRENCY_SHORTERS.Length)
		{
			return _CURRENCY_SHORTERS[power];
		}
		power = power - _CURRENCY_SHORTERS.Length + 27;
		StringBuilder stringBuilder = new StringBuilder();
		while (power > 0)
		{
			power--;
			int num = power % 26;
			stringBuilder.Insert(0, LETTERS[num]);
			power = (power - num) / 26;
		}
		return stringBuilder.ToString();
	}
}
