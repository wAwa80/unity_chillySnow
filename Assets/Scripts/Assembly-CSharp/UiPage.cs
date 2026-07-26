using UnityEngine;

namespace LevelMode
{
	/// <summary>
	/// 仿 Endless Page：CanvasGroup 显隐 + 可选淡入淡出（使用 unscaledDeltaTime，暂停时仍可播完）。
	/// </summary>
	[RequireComponent(typeof(CanvasGroup))]
	public abstract class UiPage<T> : Singleton<T> where T : UiPage<T>
	{
		private const float FadeSpeed = 10f;

		protected CanvasGroup self;

		[SerializeField]
		private bool fadeIn = true;

		[SerializeField]
		private bool fadeOut;

		[SerializeField]
		private RectTransform panel;

		private bool shouldFade;

		private bool visible;

		public bool IsVisible()
		{
			return visible;
		}

		protected override void Awake()
		{
			base.Awake();
			self = GetComponent<CanvasGroup>();
			self.alpha = 0f;
			self.interactable = false;
			self.blocksRaycasts = false;
			visible = false;
		}

		public virtual void Show()
		{
			self.interactable = true;
			self.blocksRaycasts = true;
			if (fadeIn)
			{
				shouldFade = true;
			}
			else
			{
				self.alpha = 1f;
				shouldFade = false;
			}
			visible = true;
		}

		public virtual void Hide()
		{
			self.interactable = false;
			self.blocksRaycasts = false;
			if (fadeOut)
			{
				shouldFade = true;
			}
			else
			{
				self.alpha = 0f;
				shouldFade = false;
			}
			visible = false;
		}

		protected virtual void Update()
		{
			if (!shouldFade)
			{
				return;
			}
			float alpha = self.alpha;
			float step = FadeSpeed * Time.unscaledDeltaTime;
			if (visible)
			{
				alpha += step;
				if (alpha >= 1f)
				{
					alpha = 1f;
					shouldFade = false;
				}
			}
			else
			{
				alpha -= step;
				if (alpha <= 0f)
				{
					alpha = 0f;
					shouldFade = false;
				}
			}
			self.alpha = alpha;
			if (panel != null)
			{
				float scale = 0.95f + alpha * 0.05f;
				panel.localScale = new Vector3(scale, scale, scale);
			}
		}
	}
}
