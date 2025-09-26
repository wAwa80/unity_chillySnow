using GooglePlayGames;
using UnityEngine;

namespace JuiceInternal
{
	public sealed class GameCenter : Module<GameCenter>
	{
		private bool wantsShow;

		protected override void OnSetup()
		{
			Module<GameCenter>.LogMessage("Activating Play Games Platform");
			PlayGamesPlatform.Activate();
			Module<GameCenter>.LogMessage("Play Games Platform activated");
		}

		protected override void OnGameStarted()
		{
		}

		private void OnAuthenticated(bool success, string error)
		{
			if (success)
			{
				Module<GameCenter>.LogMessage("Authenticated successfully");
				if (wantsShow)
				{
					Social.ShowLeaderboardUI();
					wantsShow = false;
				}
			}
			else
			{
				Module<GameCenter>.LogWarning($"Authentication failed for reason \"{error}\"");
			}
		}

		public void Show()
		{
			if (!Social.localUser.authenticated)
			{
				wantsShow = true;
				Module<GameCenter>.LogDebugWarning("User is not authenticated, proceeding...");
				Social.localUser.Authenticate(OnAuthenticated);
			}
			else
			{
				Module<GameCenter>.LogMessage("Displaying leaderboard");
				Social.ShowLeaderboardUI();
			}
		}

		public void SendScore(string leaderboard, int score)
		{
		}

		public void OnScoreSent(bool success)
		{
			if (success)
			{
				Module<GameCenter>.LogMessage("Score sent successfully");
			}
			else
			{
				Module<GameCenter>.LogDebugWarning("Score failed to send");
			}
		}
	}
}
