public sealed class Stats : Singleton<Stats>
{
	public class Game
	{
		private readonly string name;

		public int score;

		public bool isNewTopScore { get; private set; }

		public Game(string name)
		{
			this.name = name;
			score = 0;
			isNewTopScore = false;
		}

		public void SetFromLeaderboard(int score)
		{
			this.score = score;
			Save();
		}

		public void Load()
		{
			score = Data.LoadInt($"stats{name}GameScore");
		}

		public void Save()
		{
			Data.SaveInt($"stats{name}GameScore", score);
		}

		public void Update(Game other)
		{
			if (other.score > score)
			{
				score = other.score;
				isNewTopScore = true;
			}
			else
			{
				isNewTopScore = false;
			}
		}
	}

	private static int gamesPlayed;

	private static int quickPresses;

	private static int longPresses;

	private static Game currentGame;

	private static Game currentGameNight;

	private static Game top;

	private static Game topNight;

	public static int GetGamesPlayed()
	{
		return gamesPlayed;
	}

	public static int GetQuickPresses()
	{
		return quickPresses;
	}

	public static int GetLongPresses()
	{
		return longPresses;
	}

	public static void AddPress(bool quick)
	{
		if (quick)
		{
			quickPresses++;
		}
		else
		{
			longPresses++;
		}
	}

	public static Game GetTop()
	{
		if (NightModeButton.nightModeOn)
		{
			return topNight;
		}
		return top;
	}

	public static Game GetNormalTop()
	{
		return top;
	}

	public static Game GetNightTop()
	{
		return topNight;
	}

	public static Game GetCurrent()
	{
		if (NightModeButton.nightModeOn)
		{
			return currentGameNight;
		}
		return currentGame;
	}

	protected override void Awake()
	{
		base.Awake();
		gamesPlayed = Data.LoadInt("statsGamesPlayed");
		quickPresses = Data.LoadInt("statsQuickPresses");
		longPresses = Data.LoadInt("statsLongPresses");
		top = new Game("Stats");
		top.Load();
		topNight = new Game("StatsNight");
		topNight.Load();
		currentGame = new Game("Last");
		currentGame.Load();
		currentGameNight = new Game("LastNight");
		currentGameNight.Load();
	}

	protected override void OnNewGame()
	{
		if (NightModeButton.nightModeOn)
		{
			currentGameNight = new Game("LastNight");
		}
		else
		{
			currentGame = new Game("Last");
		}
	}

	protected override void OnMeterPlusOne()
	{
		if (NightModeButton.nightModeOn)
		{
			currentGameNight.score++;
		}
		else
		{
			currentGame.score++;
		}
	}

	protected override void OnWhoosh()
	{
		if (NightModeButton.nightModeOn)
		{
			currentGameNight.score += Pine.GetWhooshPoints();
		}
		else
		{
			currentGame.score += Pine.GetWhooshPoints();
		}
	}

	protected override void OnGameOver(bool canUseSecondCance)
	{
		if (!canUseSecondCance)
		{
			gamesPlayed++;
			Data.SaveInt("statsGamesPlayed", gamesPlayed);
			Data.SaveInt("statsQuickPresses", quickPresses);
			Data.SaveInt("statsLongPresses", longPresses);
			if (NightModeButton.nightModeOn)
			{
				currentGameNight.Save();
			}
			else
			{
				currentGame.Save();
			}
			if (NightModeButton.nightModeOn)
			{
				topNight.Update(currentGameNight);
				topNight.Save();
			}
			else
			{
				top.Update(currentGame);
				top.Save();
			}
		}
	}

	public override int GetPriority()
	{
		return -100;
	}
}
