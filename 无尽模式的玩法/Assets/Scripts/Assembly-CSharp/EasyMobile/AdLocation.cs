using System.Collections;

namespace EasyMobile
{
	public sealed class AdLocation
	{
		private readonly string name;

		private static Hashtable map = new Hashtable();

		public static readonly AdLocation Default = new AdLocation("Default");

		public static readonly AdLocation Startup = new AdLocation("Startup");

		public static readonly AdLocation HomeScreen = new AdLocation("Home Screen");

		public static readonly AdLocation MainMenu = new AdLocation("Main Menu");

		public static readonly AdLocation GameScreen = new AdLocation("Game Screen");

		public static readonly AdLocation Achievements = new AdLocation("Achievements");

		public static readonly AdLocation Quests = new AdLocation("Quests");

		public static readonly AdLocation Pause = new AdLocation("Pause");

		public static readonly AdLocation LevelStart = new AdLocation("Level Start");

		public static readonly AdLocation LevelComplete = new AdLocation("Level Complete");

		public static readonly AdLocation TurnComplete = new AdLocation("Turn Complete");

		public static readonly AdLocation IAPStore = new AdLocation("IAP Store");

		public static readonly AdLocation ItemStore = new AdLocation("Item Store");

		public static readonly AdLocation GameOver = new AdLocation("Game Over");

		public static readonly AdLocation LeaderBoard = new AdLocation("Leaderboard");

		public static readonly AdLocation Settings = new AdLocation("Settings");

		public static readonly AdLocation Quit = new AdLocation("Quit");

		private AdLocation(string name)
		{
			this.name = name;
			map.Add(name, this);
		}

		public override string ToString()
		{
			return name;
		}

		public static AdLocation LocationFromName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return Default;
			}
			if (map[name] != null)
			{
				return map[name] as AdLocation;
			}
			return new AdLocation(name);
		}
	}
}
