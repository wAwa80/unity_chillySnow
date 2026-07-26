using UnityEngine;
using UnityEngine.UI;

namespace LevelMode
{

	public sealed class Score : Singleton<Score>
	{
		private static int bestScore;

		private CanvasGroup scoreAlpha;

		private Text scoreText;

		private CanvasGroup bestScoreAlpha;

		private Text bestScoreText;

		private readonly AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		private bool shouldShow;

		private float timer;

		/// <summary>
		/// 相同分数不重复写 UI 文本，开局必须重置。
		/// </summary>
		private int lastDisplayedScore = int.MinValue;

		protected override void Awake()
		{
			base.Awake();
			ReloadLevelBestFromDisk();
			bestScoreText = base.transform.GetChild(0).GetComponent<Text>();
			bestScoreAlpha = bestScoreText.GetComponent<CanvasGroup>();
			scoreText = base.transform.GetChild(1).GetComponent<Text>();
			scoreAlpha = scoreText.GetComponent<CanvasGroup>();
			OnRefresh();
		}

		protected override void OnRefresh()
		{
			lastDisplayedScore = int.MinValue;
			// 无尽 / 关卡各读各的 best key；Refresh 时从盘重载，避免静态缓存与 PlayerPrefs 不一致
			int displayBest = GameMode.IsEndless ? GameMode.GetEndlessBest() : ReloadLevelBestFromDisk();
			bestScoreText.text = $"\n{displayBest}";
			Hide();
		}

		protected override void OnStartRun(Run slide)
		{
			lastDisplayedScore = int.MinValue;
			SetScoreText(slide.score);
			Show();
		}

		protected override void OnMeterPlusOne()
		{
			SetScoreText(Neuron.GetCurrentRun().score);
		}

		protected override void OnWhoosh(int points)
		{
			SetScoreText(Neuron.GetCurrentRun().score);
		}

		private void SetScoreText(int score)
		{
			if (score == lastDisplayedScore)
			{
				return;
			}
			lastDisplayedScore = score;
			scoreText.text = score.ToString();
		}

		protected override void OnEndRun()
		{
			int score = Neuron.GetCurrentRun().score;
			if (GameMode.IsEndless)
			{
				// 无尽结算写独立 key，并立刻刷新文案（避免只写盘界面仍旧值）
				GameMode.SaveEndlessBestIfHigher(score);
				bestScoreText.text = $"\n{GameMode.GetEndlessBest()}";
				return;
			}
			// 关卡：单关分数结算进 LEVEL_BEST_KEY；SaveLevelBestIfHigher 内已 Data.Save()
			GameMode.SaveLevelBestIfHigher(score);
			bestScoreText.text = $"\n{ReloadLevelBestFromDisk()}";
		}

		/// <summary>
		/// 从 PlayerPrefs 重载关卡 best 到静态缓存，并返回最新值（供 HUD 展示）。
		/// </summary>
		private static int ReloadLevelBestFromDisk()
		{
			bestScore = GameMode.GetLevelBest();
			return bestScore;
		}

		public static void SetBestScore(int newBestScore)
		{
			GameMode.SaveLevelBestIfHigher(newBestScore);
			ReloadLevelBestFromDisk();
		}

		private void Show()
		{
			shouldShow = true;
			base.enabled = true;
		}

		private void Hide()
		{
			shouldShow = false;
			base.enabled = true;
		}

		private void Update()
		{
			if (shouldShow)
			{
				timer += Time.deltaTime;
				if (timer >= 1f)
				{
					timer = 1f;
					base.enabled = false;
				}
			}
			else
			{
				timer -= Time.deltaTime;
				if (timer <= 0f)
				{
					timer = 0f;
					base.enabled = false;
				}
			}
			if (timer > 0.5f)
			{
				scoreAlpha.alpha = curve.Evaluate((timer - 0.5f) * 2f);
				bestScoreAlpha.alpha = 0f;
			}
			else
			{
				scoreAlpha.alpha = 0f;
				bestScoreAlpha.alpha = 1f - curve.Evaluate(timer * 2f);
			}
		}
	}
}
