using UnityEngine;
using UnityEngine.UI;


namespace EndlessMode
{
	[RequireComponent(typeof(CanvasGroup))]
	public sealed class MenuScores : Singleton<MenuScores>
	{
		[SerializeField]
		private CanvasGroup newBestScore;

		private Animation newBestScoreAnimation;

		private AudioSource newBestScoreSound;

		[SerializeField]
		private Animation shineAnimation;

		[SerializeField]
		private Text newBestScoreText;

		private Text bestScore;

		private RectTransform lastScoreWrapper;

		private Text lastScore;

		private float bestScoreTargetScale = 1.1f;

		private float lastScoreTargetScale = 0.6f;

		private float newGameTimer = 1f;

		private float dissapearTimer;

		/// <summary>
		/// 相同分数不重复写 UI；新局/回菜单时重置。
		/// </summary>
		private int lastDisplayedScore = int.MinValue;

		protected override void Awake()
		{
			base.Awake();
			bestScore = base.transform.GetChild(0).GetComponent<Text>();
			lastScoreWrapper = base.transform.GetChild(1).GetComponent<RectTransform>();
			lastScore = lastScoreWrapper.GetChild(0).GetComponent<Text>();
			// 必须先缓存 Animation/Audio，避免后续 Stats/Translator 异常导致 OnBackToMenu 空引用
			if (newBestScore != null)
			{
				newBestScoreAnimation = newBestScore.GetComponent<Animation>();
				newBestScoreSound = newBestScore.GetComponent<AudioSource>();
			}
			RefreshScoreLabels();
		}

		/// <summary>
		/// Stats 可能尚未 Awake；标签可延后到 Start 再刷一次。
		/// </summary>
		private void Start()
		{
			RefreshScoreLabels();
		}

		private void RefreshScoreLabels()
		{
			Stats.Game top = Stats.GetTop();
			Stats.Game current = Stats.GetCurrent();
			if (bestScore == null || lastScore == null || top == null || current == null)
			{
				return;
			}
			bestScore.text = string.Format(Translator.Translate("Best: <size=50>{0}</size>"), top.score);
			lastScore.text = string.Format(Translator.Translate("Last: <size=50>{0}</size>"), current.score);
		}

		protected override void OnNewGame()
		{
			lastDisplayedScore = int.MinValue;
			bestScoreTargetScale = 0.6f;
			lastScoreTargetScale = 1.1f;
			newGameTimer = 0f;
			Stats.Game top = Stats.GetTop();
			if (top != null && top.isNewTopScore)
			{
				dissapearTimer = newBestScore != null ? newBestScore.alpha : 0f;
				if (dissapearTimer <= 0f)
				{
					dissapearTimer = 0.001f;
				}
				if (shineAnimation != null && shineAnimation.clip != null)
				{
					shineAnimation.clip.wrapMode = WrapMode.Once;
				}
				if (bestScore != null)
				{
					bestScore.text = string.Format(Translator.Translate("Best: <size=50>{0}</size>"), top.score);
				}
			}
		}

		protected override void OnNightModeSwitched(bool enabled)
		{
			Stats.Game top = Stats.GetTop();
			Stats.Game current = Stats.GetCurrent();
			if (top != null && top.isNewTopScore)
			{
				if (newBestScoreText != null)
				{
					newBestScoreText.text = string.Format(Translator.Translate("NEW BEST: {0}").ToUpper(), top.score);
				}
			}
			else
			{
				dissapearTimer = newBestScore != null ? newBestScore.alpha : 0f;
				if (dissapearTimer <= 0f)
				{
					dissapearTimer = 0.001f;
				}
				if (shineAnimation != null && shineAnimation.clip != null)
				{
					shineAnimation.clip.wrapMode = WrapMode.Once;
				}
			}
			if (bestScore != null && top != null)
			{
				bestScore.text = string.Format(Translator.Translate("Best: <size=50>{0}</size>"), top.score);
			}
			if (lastScore != null && current != null)
			{
				lastScore.text = string.Format(Translator.Translate("Last: <size=50>{0}</size>"), current.score);
			}
		}

		public void LeaderboardScoreLoaded()
		{
			Stats.Game top = Stats.GetTop();
			if (bestScore == null || top == null)
			{
				return;
			}
			bestScore.text = string.Format(Translator.Translate("Best: <size=50>{0}</size>"), top.score);
		}

		protected override void OnBackToMenu()
		{
			lastDisplayedScore = int.MinValue;
			bestScoreTargetScale = 1.1f;
			lastScoreTargetScale = 0.6f;
			newGameTimer = 0f;
			Stats.Game top = Stats.GetTop();
			if (top == null)
			{
				return;
			}
			if (top.isNewTopScore)
			{
				// TODO: [User Action] 请确认 Inspector 中 newBestScore / shineAnimation 已拖拽赋值且带 Animation 组件
				if (newBestScoreAnimation != null)
				{
					newBestScoreAnimation.Play();
				}
				if (shineAnimation != null && shineAnimation.clip != null)
				{
					shineAnimation.clip.wrapMode = WrapMode.Loop;
					shineAnimation.Play();
				}
				if (newBestScoreText != null)
				{
					newBestScoreText.text = string.Format(Translator.Translate("NEW BEST: {0}").ToUpper(), top.score);
				}
				Invoke("Stamp", 0.16667f);
			}
			else if (bestScore != null)
			{
				bestScore.text = string.Format(Translator.Translate("Best: <size=50>{0}</size>"), top.score);
			}
		}

		private void Stamp()
		{
			if (newBestScoreSound != null)
			{
				newBestScoreSound.Play();
			}
			if (Singleton<GameCamera>.i != null)
			{
				Singleton<GameCamera>.i.Shake();
			}
		}

		private void Update()
		{
			if (bestScore == null || lastScoreWrapper == null || lastScore == null)
			{
				return;
			}
			float x = bestScore.transform.localScale.x;
			if (Mathf.Abs(x - bestScoreTargetScale) > 0.01f)
			{
				x += (bestScoreTargetScale - x) * 5f * Time.deltaTime;
				bestScore.transform.localScale = new Vector3(x, x, x);
				x = lastScoreWrapper.localScale.x;
				x += (lastScoreTargetScale - x) * 5f * Time.deltaTime;
				lastScoreWrapper.localScale = new Vector3(x, x, x);
			}
			if (!(newGameTimer < 1f))
			{
				return;
			}
			newGameTimer += 2f * Time.deltaTime;
			if (newGameTimer > 1f)
			{
				newGameTimer = 1f;
			}
			Stats.Game current = Stats.GetCurrent();
			if (newGameTimer > 0.5f && current != null)
			{
				if (App.GetState() == App.State.Menu)
				{
					if (lastScore.text.Length == current.score.ToString().Length)
					{
						lastScore.text = string.Format(Translator.Translate("Last: <size=50>{0}</size>"), current.score.ToString());
					}
				}
				else if (lastScore.text.Length > 1)
				{
					lastScore.text = current.score.ToString();
				}
			}
			x = Mathf.Abs((newGameTimer - 0.5f) * 2f);
			lastScore.transform.localScale = new Vector3(x, x, x);
		}

		private void LateUpdate()
		{
			if (dissapearTimer != 0f)
			{
				dissapearTimer -= 2f * Time.deltaTime;
				if (dissapearTimer < 0f)
				{
					dissapearTimer = 0f;
					if (newBestScoreAnimation != null)
					{
						newBestScoreAnimation.Stop();
					}
					if (shineAnimation != null)
					{
						shineAnimation.Stop();
					}
				}
				if (newBestScore != null)
				{
					newBestScore.alpha = dissapearTimer;
				}
			}
		}

		protected override void OnMeterPlusOne()
		{
			SetLastScoreText(Stats.GetCurrent().score);
		}

		protected override void OnWhoosh()
		{
			SetLastScoreText(Stats.GetCurrent().score);
		}

		private void SetLastScoreText(int score)
		{
			if (lastScore == null || score == lastDisplayedScore)
			{
				return;
			}
			lastDisplayedScore = score;
			lastScore.text = score.ToString();
		}

		public void SetScoreColor(Color c)
		{
			if (lastScore != null)
			{
				lastScore.color = c;
			}
		}
	}
}
