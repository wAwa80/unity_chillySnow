using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public sealed class Tutorial : Singleton<Tutorial>
{
	private CanvasGroup canvasGroup;

	private const float BLINK_SPEED = 1.5f;

	private bool shouldShow;

	private float timer;

	private float alpha;

	protected override void Awake()
	{
		base.Awake();
		canvasGroup = GetComponent<CanvasGroup>();
	}

	protected override void OnStartRun(Run slide)
	{
		Stop();
	}

	protected override void OnRefresh()
	{
		if (Level.Get() == 1)
		{
			Play();
		}
	}

	private void Play()
	{
		alpha = 0f;
		base.enabled = true;
		shouldShow = true;
	}

	private void Stop()
	{
		shouldShow = false;
		timer = 4.712389f;
	}

	private void Update()
	{
		if (shouldShow)
		{
			timer += 1.5f * Time.deltaTime;
			alpha = Mathf.Sin(timer) * 0.5f + 0.5f;
		}
		else
		{
			alpha -= 5f * Time.deltaTime;
			if (alpha < 0f)
			{
				alpha = 0f;
				base.enabled = false;
			}
		}
		canvasGroup.alpha = alpha;
	}
}
