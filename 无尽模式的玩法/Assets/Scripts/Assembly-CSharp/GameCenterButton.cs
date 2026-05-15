using EasyMobile;
using UnityEngine.SocialPlatforms;

public class GameCenterButton : AnimatedButton<GameCenterButton>
{
	private bool wantsLeaderboard;

	private bool hasSuccessfullyLoadedFromLeaderboard;

	protected override void Awake()
	{
		base.Awake();
	}

	private void ShowLeaderboard()
	{
		try
		{
			if (Device.HasInternet())
			{
				if (GameServiceManager.IsInitialized())
				{
					if (!hasSuccessfullyLoadedFromLeaderboard)
					{
						GameServiceManager.LoadLocalUserScore("Score Leaderboard", OnLocalUserScoreLoaded);
						GameServiceManager.LoadLocalUserScore("Night Score Leaderboard", OnLocalUserScoreLoaded);
					}
					GameServiceManager.ShowLeaderboardUI();
				}
				else
				{
					wantsLeaderboard = true;
					GameServiceManager.Init();
				}
			}
			else
			{
				MobileNativeAlert.Alert(Translator.Translate("Server unreachable"), Translator.Translate("Unable to reach the servers. Please check your Internet connection and try again."));
			}
		}
		catch
		{
		}
	}

	protected override void OnGameOver(bool canUseSecondChance)
	{
	}

	private void OnUserLoginSucceeded()
	{
		base.enabled = false;
		if (!hasSuccessfullyLoadedFromLeaderboard)
		{
			GameServiceManager.LoadLocalUserScore("Score Leaderboard", OnLocalUserScoreLoaded);
			GameServiceManager.LoadLocalUserScore("Night Score Leaderboard", OnLocalUserScoreLoaded);
		}
		if (wantsLeaderboard)
		{
			wantsLeaderboard = false;
			GameServiceManager.ShowLeaderboardUI();
		}
	}

	private void OnLocalUserScoreLoaded(string leaderboardName, IScore score)
	{
		hasSuccessfullyLoadedFromLeaderboard = true;
		Stats.Game game = ((!(leaderboardName == "Night Score Leaderboard")) ? Stats.GetNormalTop() : Stats.GetNightTop());
		if (score == null)
		{
			GameServiceManager.ReportScore(game.score, leaderboardName);
			return;
		}
		int num = (int)score.value;
		if (num > game.score)
		{
			game.SetFromLeaderboard(num);
			Singleton<MenuScores>.i.LeaderboardScoreLoaded();
			Singleton<SkinsPage>.i.LoadedFromLeaderboard(num);
		}
		else if (num < game.score)
		{
			GameServiceManager.ReportScore(game.score, leaderboardName);
		}
	}

	private void OnDestroy()
	{
	}
}
