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

		protected override void Awake()
		{
			base.Awake();
			bestScore = base.transform.GetChild(0).GetComponent<Text>();
			lastScoreWrapper = base.transform.GetChild(1).GetComponent<RectTransform>();
			lastScore = lastScoreWrapper.GetChild(0).GetComponent<Text>();
			bestScore.text = string.Format(Translator.Translate("Best: <size=50>{0}</size>"), Stats.GetTop().score);
			lastScore.text = string.Format(Translator.Translate("Last: <size=50>{0}</size>"), Stats.GetCurrent().score);
			newBestScoreAnimation = newBestScore.GetComponent<Animation>();
			newBestScoreSound = newBestScore.GetComponent<AudioSource>();
		}

		protected override void OnNewGame()
		{
			bestScoreTargetScale = 0.6f;
			lastScoreTargetScale = 1.1f;
			newGameTimer = 0f;
			if (Stats.GetTop().isNewTopScore)
			{
				dissapearTimer = newBestScore.alpha;
				if (dissapearTimer <= 0f)
				{
					dissapearTimer = 0.001f;
				}
				shineAnimation.clip.wrapMode = WrapMode.Once;
				bestScore.text = string.Format(Translator.Translate("Best: <size=50>{0}</size>"), Stats.GetTop().score);
			}
		}

		protected override void OnNightModeSwitched(bool enabled)
		{
			if (Stats.GetTop().isNewTopScore)
			{
				newBestScoreText.text = string.Format(Translator.Translate("NEW BEST: {0}").ToUpper(), Stats.GetTop().score);
			}
			else
			{
				dissapearTimer = newBestScore.alpha;
				if (dissapearTimer <= 0f)
				{
					dissapearTimer = 0.001f;
				}
				shineAnimation.clip.wrapMode = WrapMode.Once;
			}
			bestScore.text = string.Format(Translator.Translate("Best: <size=50>{0}</size>"), Stats.GetTop().score);
			lastScore.text = string.Format(Translator.Translate("Last: <size=50>{0}</size>"), Stats.GetCurrent().score);
		}

		public void LeaderboardScoreLoaded()
		{
			bestScore.text = string.Format(Translator.Translate("Best: <size=50>{0}</size>"), Stats.GetTop().score);
		}

		protected override void OnBackToMenu()
		{
			bestScoreTargetScale = 1.1f;
			lastScoreTargetScale = 0.6f;
			newGameTimer = 0f;
			if (Stats.GetTop().isNewTopScore)
			{
				newBestScoreAnimation.Play();
				shineAnimation.clip.wrapMode = WrapMode.Loop;
				shineAnimation.Play();
				newBestScoreText.text = string.Format(Translator.Translate("NEW BEST: {0}").ToUpper(), Stats.GetTop().score);
				Invoke("Stamp", 0.16667f);
			}
			else
			{
				bestScore.text = string.Format(Translator.Translate("Best: <size=50>{0}</size>"), Stats.GetTop().score);
			}
		}

		private void Stamp()
		{
			newBestScoreSound.Play();
			Singleton<GameCamera>.i.Shake();
		}

		private void Update()
		{
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
			if (newGameTimer > 0.5f)
			{
				if (App.GetState() == App.State.Menu)
				{
					if (lastScore.text.Length == Stats.GetCurrent().score.ToString().Length)
					{
						lastScore.text = string.Format(Translator.Translate("Last: <size=50>{0}</size>"), Stats.GetCurrent().score.ToString());
					}
				}
				else if (lastScore.text.Length > 1)
				{
					lastScore.text = Stats.GetCurrent().score.ToString();
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
					newBestScoreAnimation.Stop();
					shineAnimation.Stop();
				}
				newBestScore.alpha = dissapearTimer;
			}
		}

		protected override void OnMeterPlusOne()
		{
			lastScore.text = Stats.GetCurrent().score.ToString();
		}

		protected override void OnWhoosh()
		{
			lastScore.text = Stats.GetCurrent().score.ToString();
		}

		public void SetScoreColor(Color c)
		{
			lastScore.color = c;
		}
	}
}
