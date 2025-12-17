public sealed class Stats : Singleton<Stats>
{
	public class Game
	{
		public int score;

		private readonly string scoreSaveID;

		public int whooshes;

		private readonly string whooshesSaveID;

		public int bestWhooshCount;

		private readonly string bestWhooshCountSaveID;

		public int combo;

		private readonly string comboSaveID;

		public int withSecondChanceScore;

		private readonly string withSecondChanceScoreSaveID;

		public bool isNewTopScore { get; private set; }

		public Game(string name)
		{
			score = 0;
			isNewTopScore = false;
			scoreSaveID = $"stats{name}GameScore";
			whooshes = 0;
			whooshesSaveID = $"stats{name}GameWhooshes";
			bestWhooshCount = 0;
			bestWhooshCountSaveID = $"stats{name}GameBestWhooshCount";
			combo = 0;
			comboSaveID = $"stats{name}GameCombo";
			withSecondChanceScore = 0;
			withSecondChanceScoreSaveID = $"stats{name}GameWithSecondChanceScore";
		}

		public void SetFromLeaderboard(int score)
		{
			this.score = score;
			Save();
		}

		public void Load()
		{
			score = Data.LoadInt(scoreSaveID);
			whooshes = Data.LoadInt(whooshesSaveID);
			bestWhooshCount = Data.LoadInt(bestWhooshCountSaveID);
			combo = Data.LoadInt(comboSaveID);
			withSecondChanceScore = Data.LoadInt(withSecondChanceScoreSaveID);
		}

		public void Save()
		{
			Data.SaveInt(scoreSaveID, score);
			Data.SaveInt(whooshesSaveID, whooshes);
			Data.SaveInt(bestWhooshCountSaveID, bestWhooshCount);
			Data.SaveInt(comboSaveID, combo);
			Data.SaveInt(withSecondChanceScoreSaveID, withSecondChanceScore);
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
			whooshes += other.whooshes;
			if (other.whooshes > bestWhooshCount)
			{
				bestWhooshCount = other.whooshes;
			}
			if (other.combo > combo)
			{
				combo = other.combo;
			}
			if (other.withSecondChanceScore > withSecondChanceScore)
			{
				withSecondChanceScore = other.withSecondChanceScore;
			}
		}
	}

	private static int gamesPlayed;

	private static int secondChancesUsed;

	private static int quickPresses;

	private static int longPresses;

	private static Game currentGame;

	private static Game currentGameNight;

	private static Game top;

	private static Game topNight;

	private bool hasContinued;

	public static int GetGamesPlayed()
	{
		return gamesPlayed;
	}

	public static int GetSecondChancesUsed()
	{
		return secondChancesUsed;
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
		secondChancesUsed = Data.LoadInt("statsSecondChancesUsed");
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
		gamesPlayed++;
		Data.SaveInt("statsGamesPlayed", gamesPlayed);
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
		int whooshPoints = Pine.GetWhooshPoints();
		if (NightModeButton.nightModeOn)
		{
			currentGameNight.score += whooshPoints;
			currentGameNight.whooshes++;
			if (currentGameNight.combo < whooshPoints)
			{
				currentGameNight.combo = whooshPoints;
			}
		}
		else
		{
			currentGame.score += whooshPoints;
			currentGame.whooshes++;
			if (currentGame.combo < whooshPoints)
			{
				currentGame.combo = whooshPoints;
			}
		}
	}

	protected override void OnContinue()
	{
		secondChancesUsed++;
		hasContinued = true;
	}

	protected override void OnGameOver(bool canUseSecondCance)
	{
		if (canUseSecondCance)
		{
			return;
		}
		if (hasContinued)
		{
			if (NightModeButton.nightModeOn)
			{
				currentGameNight.withSecondChanceScore = currentGameNight.score;
			}
			else
			{
				currentGame.withSecondChanceScore = currentGame.score;
			}
		}
		Data.SaveInt("statsSecondChancesUsed", secondChancesUsed);
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

	public override int GetPriority()
	{
		return -100;
	}
}
