using UnityEngine;

public static class Utility
{
	public static Color HexToColor(string hex)
	{
		Color result = new Color(0f, 0f, 0f, 1f);
		hex = hex.ToLower();
		while (hex.Length < 7)
		{
			hex += "0";
		}
		result.r = (float)(HexToInt(hex[1]) * 16 + HexToInt(hex[2])) / 255f;
		result.g = (float)(HexToInt(hex[3]) * 16 + HexToInt(hex[4])) / 255f;
		result.b = (float)(HexToInt(hex[5]) * 16 + HexToInt(hex[6])) / 255f;
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
}
