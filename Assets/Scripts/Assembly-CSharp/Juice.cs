using GameAnalyticsSDK;
using JuiceInternal;

public static class Juice
{
	private static string levelMetadata;

	public static Analytics analytics => Module<Analytics>.GetInstance();

	public static Store store => Module<Store>.GetInstance();

	public static Premium premium => Module<Premium>.GetInstance();

	public static Ads ads => Module<Ads>.GetInstance();

	public static GameCenter gameCenter => Module<GameCenter>.GetInstance();

	public static AppRater rater => Module<AppRater>.GetInstance();

	public static Translator translator => Module<Translator>.GetInstance();

	public static void ShowGDPR()
	{
		GDPR.Open();
	}

	public static void LevelStarts(string metadata)
	{
		if (levelMetadata != null)
		{
			Log.DebugWarning("Juice", $"LevelStarts has already been called. Please don't call it twice without calling LevelEnds inbetween. Last call was made with metadata \"{metadata}\"");
			return;
		}
		levelMetadata = metadata;
		analytics.SendProgressionEvent(GAProgressionStatus.Start, metadata);
		analytics.SendDesignEvent("Game:Played:" + metadata);
		if (PremiumButton.GetInstance() != null)
		{
			PremiumButton.GetInstance().Hide();
		}
		SettingsBar.GetInstance().Hide();
	}

	public static void LevelEnds(bool win)
	{
		if (levelMetadata == null)
		{
			Log.DebugWarning("Juice", "LevelEnds has already been called. Please don't call it twice without calling LevelStarts inbetween.");
			return;
		}
		if (win)
		{
			analytics.SendProgressionEvent(GAProgressionStatus.Complete, levelMetadata);
		}
		else
		{
			analytics.SendProgressionEvent(GAProgressionStatus.Fail, levelMetadata);
		}
		levelMetadata = null;
	}

	public static void LevelBreak()
	{
		Module<AppRater>.GetInstance().TryShow();
		Module<Premium>.GetInstance().TryShowDeal();
		Stats.CheckDayStreak();
		if (PremiumButton.GetInstance() != null)
		{
			PremiumButton.GetInstance().Show();
		}
		SettingsBar.GetInstance().Show();
	}
}
