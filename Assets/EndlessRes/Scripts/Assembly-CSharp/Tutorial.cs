using UnityEngine;


namespace EndlessMode
{
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
			OnBackToMenu();
		}

		protected override void OnNewGame()
		{
			Stop();
		}

		protected override void OnBackToMenu()
		{
			if (NeedsToShow())
			{
				Play();
			}
		}

		private bool NeedsToShow()
		{
			// Stats 未就绪时暂不显示，避免 Awake 时序空引用；回菜单时会再判定
			Stats.Game top = Stats.GetTop();
			if (top == null)
			{
				return false;
			}
			return top.score < 30 || Stats.GetQuickPresses() < 3 || Stats.GetLongPresses() < 3;
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
}
