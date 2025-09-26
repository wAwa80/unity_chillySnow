using UnityEngine;

public static class Utility
{
	public static Color HexToColor(string hex)
	{
		Color result = new Color(0f, 0f, 0f, 1f);
		hex = hex.ToLower();
		result.r = (float)(HexToInt(hex[0]) * 16 + HexToInt(hex[1])) / 255f;
		result.g = (float)(HexToInt(hex[2]) * 16 + HexToInt(hex[3])) / 255f;
		result.b = (float)(HexToInt(hex[4]) * 16 + HexToInt(hex[5])) / 255f;
		return result;
	}

	private static int HexToInt(char hex)
	{
		return hex switch
		{
			'a' => 10, 
			'b' => 11, 
			'c' => 12, 
			'd' => 13, 
			'e' => 14, 
			'f' => 15, 
			_ => int.Parse(hex.ToString()), 
		};
	}

	public static float GetNote(int i)
	{
		return i switch
		{
			1 => 1.125f, 
			2 => 1.25f, 
			3 => 1.333f, 
			4 => 1.5f, 
			5 => 1.666f, 
			6 => 1.875f, 
			_ => 1f, 
		};
	}
}
