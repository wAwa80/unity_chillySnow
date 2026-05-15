using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class SkinsPage : Page<SkinsPage>
{
	[SerializeField]
	private Button closeButton;

	[SerializeField]
	private Text description;

	[SerializeField]
	private Text progress;

	[SerializeField]
	private CanvasGroup RVViewer;

	private Skin currentlyViewed;

	private int currentScore;

	private int currentWhooshPoints;

	private int currentWhooshCount;

	private bool isInSecondChance;

	protected override void Awake()
	{
		base.Awake();
		closeButton.onClick.AddListener(Hide);
		RVViewer.GetComponent<Button>().onClick.AddListener(Watch);
	}

	public override void Show()
	{
		if (currentlyViewed != null)
		{
			ViewSkin(currentlyViewed);
		}
		base.Show();
	}

	public void ViewSkin(Skin skin)
	{
		currentlyViewed = skin;
		string text = skin.GetDescription();
		int num = text.IndexOf('|');
		if (skin.IsUnlocked())
		{
			if (num > 0)
			{
				description.text = Translator.Translate(text.Substring(0, num));
			}
			else
			{
				description.text = Translator.Translate(text);
			}
			progress.enabled = false;
			RVViewer.alpha = 0f;
			RVViewer.blocksRaycasts = false;
			RVViewer.interactable = false;
			return;
		}
		if (num > 0)
		{
			string text2 = text.Substring(num + 1);
			text = text.Substring(0, num);
			progress.enabled = true;
			int num2 = text2.IndexOf('/');
			if (num2 > 0)
			{
				progress.text = $"({GetStat(text2.Substring(0, num2))}/{text2.Substring(num2 + 1)})";
			}
			else
			{
				progress.text = string.Format("({0})", string.Format(Translator.Translate("Best: {0}"), GetBest(text2)));
			}
		}
		else
		{
			progress.enabled = false;
		}
		description.text = Translator.Translate(text);
		if (skin.NeedsRV())
		{
			RVViewer.alpha = 1f;
			RVViewer.blocksRaycasts = true;
			RVViewer.interactable = true;
		}
		else
		{
			RVViewer.alpha = 0f;
			RVViewer.blocksRaycasts = false;
			RVViewer.interactable = false;
		}
	}

	private int GetStat(string name)
	{
		switch (name)
		{
		case "perfects":
			return Stats.GetTop().whooshes;
		case "days":
			return GetDayStreak();
		case "games":
			return Stats.GetGamesPlayed();
		case "secondchances":
			return Stats.GetSecondChancesUsed();
		case "skins":
			return GetUnlockedCount();
		default:
			return 0;
		}
	}

	private int GetBest(string name)
	{
		switch (name)
		{
		case "best":
			return Stats.GetTop().score;
		case "combo":
			return Stats.GetTop().combo;
		case "withSecondChanceScore":
			return Stats.GetTop().withSecondChanceScore;
		case "whooshes":
			return Stats.GetTop().bestWhooshCount;
		default:
			return 0;
		}
	}

	private int GetUnlockedCount()
	{
		int num = 0;
		foreach (Skin item in Multiton<Skin>.Enumerate())
		{
			if (item.IsUnlocked())
			{
				num++;
			}
		}
		return num - 3;
	}

	private void UnlockSkin(string skinName)
	{
		Skin skin = Multiton<Skin>.Get(skinName);
		if (!skin.IsUnlocked())
		{
			skin.Unlock();
			Singleton<SkinTypeSelection>.i.UnlockedSkinOfType(skin.GetSkinType());
			Singleton<SkinsButton>.i.Alert();
			Singleton<SkinUnlockedBanner>.i.Announce(skin);
			if (GetUnlockedCount() == 10)
			{
				UnlockSkin("BackgroundBlue");
			}
			if (Singleton<SkinTypeSelection>.i.GetViewed() == skin.GetSkinType())
			{
				Singleton<SkinTypeSelection>.i.GetScrollViewed().OnSkinViewed(currentlyViewed);
			}
		}
	}

	protected override void OnMeterPlusOne()
	{
		CheckScore();
	}

	protected override void OnWhoosh()
	{
		CheckScore();
		CheckWhooshPoints();
		CheckWhooshCount();
		CheckTotalWhooshCount();
	}

	private void CheckScore()
	{
		if (currentScore >= 2000)
		{
			return;
		}
		int score = Stats.GetCurrent().score;
		if (currentScore < 500 && score >= 500)
		{
			UnlockSkin("BackgroundOrange");
		}
		if (currentScore < 1000 && score >= 1000)
		{
			UnlockSkin("Pink");
			if (isInSecondChance)
			{
				UnlockSkin("Green");
			}
		}
		if (score >= 2000)
		{
			UnlockSkin("PineBlack");
		}
		currentScore = score;
	}

	public void LoadedFromLeaderboard(int score)
	{
		if (score >= 500)
		{
			UnlockSkin("BackgroundOrange");
		}
		if (score >= 1000)
		{
			UnlockSkin("Pink");
		}
		if (score >= 2000)
		{
			UnlockSkin("PineBlack");
		}
	}

	private void CheckWhooshPoints()
	{
		if (currentWhooshPoints < 30)
		{
			int whooshPoints = Pine.GetWhooshPoints();
			if (whooshPoints >= 30)
			{
				UnlockSkin("Black");
			}
			currentWhooshPoints = whooshPoints;
		}
	}

	private void CheckWhooshCount()
	{
		if (currentWhooshCount < 30)
		{
			int whooshes = Stats.GetCurrent().whooshes;
			if (whooshes >= 30)
			{
				UnlockSkin("PineBlue");
			}
			currentWhooshCount = whooshes;
		}
	}

	private void CheckTotalWhooshCount()
	{
		int whooshes = Stats.GetTop().whooshes;
		if (whooshes < 1000)
		{
			whooshes += Stats.GetCurrent().whooshes;
			if (whooshes >= 1000)
			{
				UnlockSkin("Orange");
			}
		}
	}

	protected override void OnBackToMenu()
	{
		isInSecondChance = false;
		currentScore = 0;
		currentWhooshPoints = 0;
		currentWhooshCount = 0;
	}

	protected override void OnContinue()
	{
		isInSecondChance = true;
		if (Stats.GetSecondChancesUsed() == 10)
		{
			UnlockSkin("BackgroundGreen");
		}
		if (Stats.GetCurrent().score >= 1000)
		{
			UnlockSkin("Green");
		}
	}

	protected override void OnNewGame()
	{
		CheckDayStreak();
		CheckGamesPlayed();
	}

	private void CheckGamesPlayed()
	{
		int gamesPlayed = Stats.GetGamesPlayed();
		if (gamesPlayed >= 100)
		{
			UnlockSkin("PineGreen");
		}
		if (gamesPlayed >= 200)
		{
			UnlockSkin("BackgroundPink");
		}
	}

	private void CheckDayStreak()
	{
		int dayOfYear = DateTime.Now.DayOfYear;
		dayOfYear -= Data.LoadInt("dayOfYear");
		if (dayOfYear > 1)
		{
			Data.SaveInt("daysInARowPlayed", 1);
		}
		else if (dayOfYear == 1)
		{
			int num = Data.LoadInt("daysInARowPlayed", 1) + 1;
			Data.SaveInt("daysInARowPlayed", num);
			switch (num)
			{
			case 2:
				UnlockSkin("Blue");
				break;
			case 3:
				UnlockSkin("PinePink");
				break;
			case 7:
				UnlockSkin("BackgroundBlack");
				break;
			}
		}
		Data.SaveInt("dayOfYear", DateTime.Now.DayOfYear);
	}

	private int GetDayStreak()
	{
		return Data.LoadInt("daysInARowPlayed", 1);
	}

	public void Watch()
	{
		//VoodooSauce.ShowRewardedVideo(ValidateRV);
	}

	private void ValidateRV(bool finishedVideo)
	{
		if (finishedVideo)
		{
			UnlockSkin("PineOrange");
		}
	}
}
