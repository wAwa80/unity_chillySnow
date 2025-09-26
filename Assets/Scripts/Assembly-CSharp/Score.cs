using UnityEngine;
using UnityEngine.UI;

public sealed class Score : Singleton<Score>
{
	private const string BEST_SCORE_KEY = "ioruhfiuerhf";

	private const string CURRENT_SCORE_KEY = "fgreuyfghbuybruy";

	private static int bestScore;

	private CanvasGroup scoreAlpha;

	private Text scoreText;

	private CanvasGroup bestScoreAlpha;

	private Text bestScoreText;

	private readonly AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	private bool shouldShow;

	private float timer;

	protected override void Awake()
	{
		base.Awake();
		bestScore = Data.LoadInt("ioruhfiuerhf");
		bestScoreText = base.transform.GetChild(0).GetComponent<Text>();
		bestScoreAlpha = bestScoreText.GetComponent<CanvasGroup>();
		scoreText = base.transform.GetChild(1).GetComponent<Text>();
		scoreAlpha = scoreText.GetComponent<CanvasGroup>();
		OnRefresh();
	}

	protected override void OnRefresh()
	{
		bestScoreText.text = $"\n{bestScore}";
		Hide();
	}

	protected override void OnStartRun(Run slide)
	{
		scoreText.text = slide.score.ToString();
		Show();
	}

	protected override void OnMeterPlusOne()
	{
		scoreText.text = Neuron.GetCurrentRun().score.ToString();
	}

	protected override void OnWhoosh(int points)
	{
		scoreText.text = Neuron.GetCurrentRun().score.ToString();
	}

	protected override void OnEndRun()
	{
		int score = Neuron.GetCurrentRun().score;
		if (score > bestScore)
		{
			bestScore = score;
			Data.SaveInt("ioruhfiuerhf", bestScore);
		}
	}

	public static void SetBestScore(int newBestScore)
	{
		if (newBestScore > bestScore)
		{
			bestScore = newBestScore;
			Data.SaveInt("ioruhfiuerhf", bestScore);
		}
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
