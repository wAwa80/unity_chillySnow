using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Debug = UnityEngine.Debug;

namespace EasyMobile
{
	[AddComponentMenu("")]
	public class GameServiceManager : MonoBehaviour
	{
		private struct LoadScoreRequest
		{
			public bool useLeaderboardDefault;

			public bool loadLocalUserScore;

			public string leaderboardName;

			public string leaderboardId;

			public int fromRank;

			public int scoreCount;

			public TimeScope timeScope;

			public UserScope userScope;

			public Action<string, IScore[]> callback;
		}

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action m_UserLoginSucceeded;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action m_UserLoginFailed;

		private static bool isLoadingScore = false;

		private static List<LoadScoreRequest> loadScoreRequests = new List<LoadScoreRequest>();

		private const string ANDROID_LOGIN_REQUEST_NUMBER_PPKEY = "SGLIB_ANDROID_LOGIN_REQUEST_NUMBER";

		public static GameServiceManager Instance { get; private set; }

		public static ILocalUser LocalUser
		{
			get
			{
				if (IsInitialized())
				{
					return Social.localUser;
				}
				return null;
			}
		}

		public static event Action UserLoginSucceeded
		{
			add
			{
				Action action = GameServiceManager.m_UserLoginSucceeded;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GameServiceManager.m_UserLoginSucceeded, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = GameServiceManager.m_UserLoginSucceeded;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GameServiceManager.m_UserLoginSucceeded, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static event Action UserLoginFailed
		{
			add
			{
				Action action = GameServiceManager.m_UserLoginFailed;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GameServiceManager.m_UserLoginFailed, (Action)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action action = GameServiceManager.m_UserLoginFailed;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref GameServiceManager.m_UserLoginFailed, (Action)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		private void Awake()
		{
			if (Instance != null)
			{
				UnityEngine.Object.Destroy(this);
			}
			else
			{
				Instance = this;
			}
		}

		private void Start()
		{
			if (EM_Settings.GameService.IsAutoInit)
			{
				StartCoroutine(CRAutoInit(EM_Settings.GameService.AutoInitDelay));
			}
		}

		private IEnumerator CRAutoInit(float delay)
		{
			yield return new WaitForSeconds(delay);
			ManagedInit();
		}

		public static void ManagedInit()
		{
			if (!IsInitialized())
			{
				int @int = PlayerPrefs.GetInt("SGLIB_ANDROID_LOGIN_REQUEST_NUMBER", 0);
				if (@int < EM_Settings.GameService.AndroidMaxLoginRequests || EM_Settings.GameService.AndroidMaxLoginRequests <= 0)
				{
					@int++;
					PlayerPrefs.SetInt("SGLIB_ANDROID_LOGIN_REQUEST_NUMBER", @int);
					PlayerPrefs.Save();
					Init();
				}
				else
				{
					Debug.Log("Game Service Init FAILED: AndroidMaxLoginRequests exceeded. Requests attempted: " + @int);
				}
			}
		}

		public static void Init()
		{
			//if (Social.Active != PlayGamesPlatform.Instance)
			//{
			//	PlayGamesPlatform.Activate();
			//}
			//PlayGamesPlatform.DebugLogEnabled = EM_Settings.GameService.IsGPGSDebug;
			Social.localUser.Authenticate(ProcessAuthentication);
		}

		public static bool IsInitialized()
		{
			return Social.localUser.authenticated;
		}

		public static void ShowLeaderboardUI()
		{
			if (IsInitialized())
			{
				Social.ShowLeaderboardUI();
			}
			else
			{
				Debug.Log("ShowLeaderboardUI FAILED: user is not logged in.");
			}
		}

		public static void ShowLeaderboardUI(string leaderboardName)
		{
			ShowLeaderboardUI(leaderboardName, TimeScope.AllTime);
		}

		public static void ShowLeaderboardUI(string leaderboardName, TimeScope timeScope)
		{
			if (!IsInitialized())
			{
				Debug.Log("ShowLeaderboardUI FAILED: user is not logged in.");
				return;
			}
			Leaderboard leaderboardByName = GetLeaderboardByName(leaderboardName);
			if (leaderboardByName == null)
			{
				Debug.Log("ShowLeaderboardUI FAILED: unknown leaderboard name.");
			}
			else
			{
				//PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardByName.Id, AsGPGSLeaderboardTimeSpan(timeScope), null);
			}
		}

		public static void ShowAchievementsUI()
		{
			if (IsInitialized())
			{
				Social.ShowAchievementsUI();
			}
			else
			{
				Debug.Log("ShowAchievementsUI FAILED: user is not logged in.");
			}
		}

		public static void ReportScore(long score, string leaderboardName, Action<bool> callback = null)
		{
			Leaderboard leaderboardByName = GetLeaderboardByName(leaderboardName);
			if (leaderboardByName != null)
			{
				DoReportScore(score, leaderboardByName.Id, callback);
			}
			else
			{
				Debug.Log("ReportScore FAILED: unknown leaderboard name.");
			}
		}

		public static void RevealAchievement(string achievementName, Action<bool> callback = null)
		{
			Achievement achievementByName = GetAchievementByName(achievementName);
			if (achievementByName != null)
			{
				DoReportAchievementProgress(achievementByName.Id, 0.0, callback);
			}
			else
			{
				Debug.Log("RevealAchievement FAILED: unknown achievement name.");
			}
		}

		public static void UnlockAchievement(string achievementName, Action<bool> callback = null)
		{
			Achievement achievementByName = GetAchievementByName(achievementName);
			if (achievementByName != null)
			{
				DoReportAchievementProgress(achievementByName.Id, 100.0, callback);
			}
			else
			{
				Debug.Log("UnlockAchievement FAILED: unknown achievement name.");
			}
		}

		public static void ReportAchievementProgress(string achievementName, double progress, Action<bool> callback = null)
		{
			Achievement achievementByName = GetAchievementByName(achievementName);
			if (achievementByName != null)
			{
				DoReportAchievementProgress(achievementByName.Id, progress, callback);
			}
			else
			{
				Debug.Log("ReportAchievementProgress FAILED: unknown achievement name.");
			}
		}

		public static void LoadFriends(Action<IUserProfile[]> callback)
		{
			if (!IsInitialized())
			{
				Debug.Log("LoadFriends FAILED: user is not logged in.");
				return;
			}
			if (Social.localUser.friends != null && Social.localUser.friends.Length > 0)
			{
				if (callback != null)
				{
					callback(Social.localUser.friends);
				}
				return;
			}
			Social.localUser.LoadFriends(delegate(bool success)
			{
				if (success)
				{
					if (callback != null)
					{
						callback(Social.localUser.friends);
					}
				}
				else
				{
					Debug.Log("LoadFriends FAILED: could not load friends.");
					if (callback != null)
					{
						callback(new IUserProfile[0]);
					}
				}
			});
		}

		public static void LoadUsers(string[] userIds, Action<IUserProfile[]> callback)
		{
			if (!IsInitialized())
			{
				Debug.Log("LoadUsers FAILED: user is not logged in.");
			}
			else
			{
				Social.LoadUsers(userIds, callback);
			}
		}

		public static void LoadScores(string leaderboardName, Action<string, IScore[]> callback)
		{
			if (!IsInitialized())
			{
				Debug.Log("LoadScores FAILED: user is not logged in.");
				return;
			}
			Leaderboard leaderboardByName = GetLeaderboardByName(leaderboardName);
			if (leaderboardByName == null)
			{
				Debug.Log("LoadScores FAILED: unknown leaderboard name.");
				return;
			}
			LoadScoreRequest item = default(LoadScoreRequest);
			item.leaderboardName = leaderboardByName.Name;
			item.leaderboardId = leaderboardByName.Id;
			item.callback = callback;
			item.useLeaderboardDefault = true;
			item.loadLocalUserScore = false;
			loadScoreRequests.Add(item);
			DoNextLoadScoreRequest();
		}

		public static void LoadScores(string leaderboardName, int fromRank, int scoreCount, TimeScope timeScope, UserScope userScope, Action<string, IScore[]> callback)
		{
			if (!IsInitialized())
			{
				Debug.Log("LoadScores FAILED: user is not logged in.");
				return;
			}
			Leaderboard leaderboardByName = GetLeaderboardByName(leaderboardName);
			if (leaderboardByName == null)
			{
				Debug.Log("LoadScores FAILED: unknown leaderboard name.");
				return;
			}
			LoadScoreRequest item = default(LoadScoreRequest);
			item.leaderboardName = leaderboardByName.Name;
			item.leaderboardId = leaderboardByName.Id;
			item.callback = callback;
			item.useLeaderboardDefault = false;
			item.loadLocalUserScore = false;
			item.fromRank = fromRank;
			item.scoreCount = scoreCount;
			item.timeScope = timeScope;
			item.userScope = userScope;
			loadScoreRequests.Add(item);
			DoNextLoadScoreRequest();
		}

		public static void LoadLocalUserScore(string leaderboardName, Action<string, IScore> callback)
		{
			if (!IsInitialized())
			{
				Debug.Log("LoadLocalUserScore FAILED: user is not logged in.");
				return;
			}
			Leaderboard leaderboardByName = GetLeaderboardByName(leaderboardName);
			if (leaderboardByName == null)
			{
				Debug.Log("LoadLocalUserScore FAILED: unknown leaderboard name.");
				return;
			}
			LoadScoreRequest item = default(LoadScoreRequest);
			item.leaderboardName = leaderboardByName.Name;
			item.leaderboardId = leaderboardByName.Id;
			item.callback = delegate(string ldbName, IScore[] scores)
			{
				if (scores != null)
				{
					if (callback != null)
					{
						callback(ldbName, scores[0]);
					}
				}
				else if (callback != null)
				{
					callback(ldbName, null);
				}
			};
			item.useLeaderboardDefault = false;
			item.loadLocalUserScore = true;
			item.fromRank = -1;
			item.scoreCount = -1;
			item.timeScope = TimeScope.AllTime;
			item.userScope = UserScope.FriendsOnly;
			loadScoreRequests.Add(item);
			DoNextLoadScoreRequest();
		}

		public static Leaderboard GetLeaderboardByName(string leaderboardName)
		{
			Leaderboard[] leaderboards = EM_Settings.GameService.Leaderboards;
			foreach (Leaderboard leaderboard in leaderboards)
			{
				if (leaderboard.Name.Equals(leaderboardName))
				{
					return leaderboard;
				}
			}
			return null;
		}

		public static Achievement GetAchievementByName(string achievementName)
		{
			Achievement[] achievements = EM_Settings.GameService.Achievements;
			foreach (Achievement achievement in achievements)
			{
				if (achievement.Name.Equals(achievementName))
				{
					return achievement;
				}
			}
			return null;
		}

		public static void SignOut()
		{
			//PlayGamesPlatform.Instance.SignOut();
		}

		private static void DoReportScore(long score, string leaderboardId, Action<bool> callback)
		{
			if (!IsInitialized())
			{
				Debug.Log("DoReportScore FAILED: user is not logged in.");
				return;
			}
			Debug.Log("Reporting score: " + score);
			Social.ReportScore(score, leaderboardId, delegate(bool success)
			{
				Debug.Log((!success) ? "Failed to report score." : "Score reported successfully.");
				if (callback != null)
				{
					callback(success);
				}
			});
		}

		private static void DoReportAchievementProgress(string achievementId, double progress, Action<bool> callback)
		{
			if (!IsInitialized())
			{
				Debug.Log("DoReportAchievementProgress FAILED: user is not logged in.");
				return;
			}
			Debug.Log("Reporting progress of " + progress + "% for achievement: " + achievementId);
			Social.ReportProgress(achievementId, progress, delegate(bool success)
			{
				Debug.Log((!success) ? ("Failed to report progress for achievement: " + achievementId) : ("Successfully reported progress of " + progress + "% for achievement: " + achievementId));
				if (callback != null)
				{
					callback(success);
				}
			});
		}

		private static void DoNextLoadScoreRequest()
		{
			if (isLoadingScore)
			{
				Debug.Log("DoNextLoadScoreRequest POSTPONED: is loading another request.");
				return;
			}
			if (loadScoreRequests.Count == 0)
			{
				Debug.Log("DoNextLoadScoreRequest DONE: no more requests in queue.");
				return;
			}
			Debug.Log("Performing next score loading request...");
			isLoadingScore = true;
			LoadScoreRequest request = loadScoreRequests[0];
			loadScoreRequests.RemoveAt(0);
			ILeaderboard ldb = Social.CreateLeaderboard();
			ldb.id = request.leaderboardId;
			if (request.useLeaderboardDefault)
			{
				Social.LoadScores(ldb.id, delegate(IScore[] scores)
				{
					Debug.Log("Successfully loaded default set of scores from leaderboard: " + ldb.id + ". Got " + scores.Length + " scores.");
					if (request.callback != null)
					{
						request.callback(request.leaderboardName, scores);
					}
					isLoadingScore = false;
					DoNextLoadScoreRequest();
				});
				return;
			}
			ldb.timeScope = request.timeScope;
			ldb.userScope = request.userScope;
			if (request.fromRank > 0 && request.scoreCount > 0)
			{
				//ldb.range = new Range(request.fromRank, request.scoreCount);
			}
			ldb.LoadScores(delegate
			{
				if (request.loadLocalUserScore)
				{
					Debug.Log("Successfully loaded local user score from leaderboard: " + ldb.id);
					IScore[] arg = new IScore[1] { ldb.localUserScore };
					if (request.callback != null)
					{
						request.callback(request.leaderboardName, arg);
					}
				}
				else
				{
					Debug.Log("Successfully loaded custom set of scores from leaderboard: " + ldb.id + ". Got " + ldb.scores.Length + " scores:");
					if (request.callback != null)
					{
						request.callback(request.leaderboardName, ldb.scores);
					}
				}
				isLoadingScore = false;
				DoNextLoadScoreRequest();
			});
		}

		private static void ProcessAuthentication(bool success)
		{
			//if (success)
			//{
			//	if (GameServiceManager.UserLoginSucceeded != null)
			//	{
			//		GameServiceManager.UserLoginSucceeded();
			//	}
			//	PlayerPrefs.SetInt("SGLIB_ANDROID_LOGIN_REQUEST_NUMBER", 0);
			//	PlayerPrefs.Save();
			//}
			//else if (GameServiceManager.UserLoginFailed != null)
			//{
			//	GameServiceManager.UserLoginFailed();
			//}
		}

		private static void ProcessLoadedAchievements(IAchievement[] achievements)
		{
			if (achievements.Length == 0)
			{
				Debug.Log("No achievements found.");
			}
			else
			{
				Debug.Log("Got " + achievements.Length + " achievements.");
			}
		}

		//private static LeaderboardTimeSpan AsGPGSLeaderboardTimeSpan(TimeScope timeScope)
		//{
		//	switch (timeScope)
		//	{
		//	case TimeScope.AllTime:
		//		return LeaderboardTimeSpan.AllTime;
		//	case TimeScope.Week:
		//		return LeaderboardTimeSpan.Weekly;
		//	case TimeScope.Today:
		//		return LeaderboardTimeSpan.Daily;
		//	default:
		//		return LeaderboardTimeSpan.AllTime;
		//	}
		//}
	}
}
