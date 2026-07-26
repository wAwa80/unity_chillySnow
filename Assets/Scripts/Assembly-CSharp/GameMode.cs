using System;
using UnityEngine;

namespace LevelMode
{
	/// <summary>
	/// 双模式单一真相源：Kind 持久化、切换 API、变更事件。
	/// 玩法侧在 OnRefresh 读 IsLevel/Current；仅 HUD（UIHudTransition）订阅 Changed。
	/// </summary>
	/// <remarks>
	/// TrySetMode 成功路径顺序写死：先 Current+Data → 再 Neuron.Refresh → 最后 Changed。
	/// Changed 回调内禁止再调用 TrySetMode / Neuron.Refresh（避免重入与坐标读旧模式）。
	/// 【残余风险·IsPlaying】EndRun 后至 Refresh 完成前 IsPlaying 仍为 true，此间拒绝切模式。
	/// 【残余风险·同模式】target==Current 时短路返回 true，不 Refresh、不 Changed。
	/// </remarks>
	public static class GameMode
	{
		public enum Kind
		{
			Level = 0,
			Endless = 1
		}

		public const string MODE_KEY = "gamemode.kind.v1";

		public const string ENDLESS_BEST_KEY = "gamemode.endless.best.v1";

		/// <summary>
		/// 关卡模式历史最高分（与无尽 ENDLESS_BEST_KEY 隔离，沿用原版 PlayerPrefs key）。
		/// </summary>
		public const string LEVEL_BEST_KEY = "ioruhfiuerhf";

		private static Kind current;

		private static bool loaded;

		/// <summary>
		/// 冷启动从 Data 恢复；非法值回落 Level。
		/// </summary>
		private static void EnsureLoaded()
		{
			if (loaded)
			{
				return;
			}
			int raw = Data.LoadInt(MODE_KEY, (int)Kind.Level);
			if (raw != (int)Kind.Level && raw != (int)Kind.Endless)
			{
				raw = (int)Kind.Level;
			}
			current = (Kind)raw;
			loaded = true;
		}

		public static Kind Current
		{
			get
			{
				EnsureLoaded();
				return current;
			}
		}

		public static bool IsEndless => Current == Kind.Endless;

		public static bool IsLevel => Current == Kind.Level;

		/// <summary>
		/// 在 Current+Data 与 Refresh 之后触发；仅供 HUD 过渡，禁止玩法侧依赖此事件还原状态。
		/// </summary>
		public static event Action<Kind> Changed;

		/// <summary>
		/// 局外切换模式。成功时严格按：Current+持久化 → Refresh → Changed。
		/// </summary>
		/// <returns>对局中拒绝为 false；同模式短路与成功切换均为 true。</returns>
		public static bool TrySetMode(Kind target)
		{
			EnsureLoaded();

			// 1) 对局中（含 Continue 弹出 / 续命中）拒绝
			if (Neuron.IsPlaying())
			{
				Debug.Log("[GameMode] 对局中无法切换模式");
				return false;
			}

			// 2) 同模式短路：不 Refresh、不 Changed（避免树闪与无意义动画）
			if (target == current)
			{
				return true;
			}

			// 3) 先改真相源并持久化，保证后续 OnRefresh 读到新模式
			current = target;
			Data.SaveInt(MODE_KEY, (int)target);

			// 4) 再整局 Refresh：清四队列/坐标等；禁止此处 WarmUp（见合并计划 §7.1）
			Neuron.Refresh();

			// 5) 最后通知 HUD；回调内禁止再 TrySetMode / Refresh
			Changed?.Invoke(current);

			return true;
		}

		public static int GetEndlessBest()
		{
			return Data.LoadInt(ENDLESS_BEST_KEY, 0);
		}

		public static int GetLevelBest()
		{
			return Data.LoadInt(LEVEL_BEST_KEY, 0);
		}

		/// <summary>
		/// 无尽结算：仅当更高分时写入独立 best key（与关卡 Score key 隔离）。
		/// </summary>
		public static bool SaveEndlessBestIfHigher(int score)
		{
			int best = GetEndlessBest();
			if (score <= best)
			{
				return false;
			}
			Data.SaveInt(ENDLESS_BEST_KEY, score);
			// 微信小游戏等环境可能等不到 Data.LateUpdate，结算后立即落盘
			Data.Save();
			return true;
		}

		/// <summary>
		/// 关卡结算：写入 LEVEL_BEST_KEY；与无尽 best 分离。
		/// </summary>
		public static bool SaveLevelBestIfHigher(int score)
		{
			int best = GetLevelBest();
			if (score <= best)
			{
				return false;
			}
			Data.SaveInt(LEVEL_BEST_KEY, score);
			Data.Save();
			return true;
		}

		/// <summary>
		/// 无尽真实滑行距离（世界单位取整），供木牌 / HUD 展示。
		/// 禁止用 FinishLine 哨兵（约 1e5）；关卡号与进度条百分比也不是本值。
		/// </summary>
		/// <remarks>
		/// 局内或已下滑：⌊-Skier.Y⌋；局外未开滑：回退 PineGenerator.GetDistance()（刷树游标）。
		/// 与 MeterPlusOne 所用 meters（×0.7）刻度不同，勿混用。
		/// </remarks>
		public static int GetEndlessSlideDistance()
		{
			float skierY = Skier.GetY();
			// 已离开起点：以滑雪者真实 Y 为准
			if (skierY < -0.01f)
			{
				return Mathf.Max(0, Mathf.FloorToInt(0f - skierY));
			}
			// 局外 / 尚未下滑：用生成器游标，避免显示 0 与哨兵
			return Mathf.Max(0, Mathf.FloorToInt(PineGenerator.GetDistance()));
		}

#if UNITY_EDITOR
		/// <summary>
		/// 编辑器误开旧无尽场景时告警（M4）；不阻止 Play，仅提示改用 MainLevelMode。
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void WarnIfMisenteredEndlessScene()
		{
			string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
			if (sceneName == "MainEndlessMode")
			{
				Debug.LogError("[GameMode] 请用 MainLevelMode 作为唯一运行场景，勿以 MainEndlessMode 为入口（会触发 Endless App DDOL）");
			}
		}
#endif
	}
}
