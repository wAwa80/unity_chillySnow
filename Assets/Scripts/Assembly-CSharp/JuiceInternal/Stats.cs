using System;
using UnityEngine;

namespace JuiceInternal
{
	internal static class Stats
	{
		private const string DAY_SAVE_KEY = "79587091062497559752";

		private const string DAY_STREAK_SAVE_KEY = "19986770425185350011";

		private static int dayStreak = -1;

		internal static void CheckDayStreak()
		{
			if (dayStreak < 0)
			{
				dayStreak = PlayerPrefs.GetInt("19986770425185350011", 1);
			}
			int num = Mathf.FloorToInt((float)DateTime.Now.ToOADate());
			int num2 = num - PlayerPrefs.GetInt("79587091062497559752", num);
			if (num2 != 0)
			{
				PlayerPrefs.SetInt("79587091062497559752", num);
				if (num2 > 1)
				{
					dayStreak = 1;
					PlayerPrefs.SetInt("19986770425185350011", 1);
				}
				else
				{
					dayStreak++;
					PlayerPrefs.SetInt("19986770425185350011", dayStreak);
					Module<Analytics>.GetInstance().SendDesignEvent("Game:DayStreak", dayStreak);
				}
			}
		}
	}
}
