using UnityEngine;


namespace EndlessMode
{
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
			switch (hex)
			{
			case 'a':
				return 10;
			case 'b':
				return 11;
			case 'c':
				return 12;
			case 'd':
				return 13;
			case 'e':
				return 14;
			case 'f':
				return 15;
			default:
				return int.Parse(hex.ToString());
			}
		}
	}
}
