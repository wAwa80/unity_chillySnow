using UnityEngine;


namespace EndlessMode
{
	[RequireComponent(typeof(CanvasGroup))]
	public abstract class Page<T> : Singleton<T> where T : Page<T>
	{
		private const float FADE_SPEED = 10f;

		protected CanvasGroup self;

		protected RectTransform rectTransform;

		[SerializeField]
		private bool fadeIn;

		[SerializeField]
		private bool fadeOut;

		private bool shouldFade;

		[SerializeField]
		private RectTransform panel;

		private bool visible;

		public bool IsVisible()
		{
			return visible;
		}

		protected override void Awake()
		{
			base.Awake();
			self = GetComponent<CanvasGroup>();
			rectTransform = GetComponent<RectTransform>();
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
			if (visible)
			{
				alpha += 10f * Time.deltaTime;
				if (alpha > 1f)
				{
					alpha = 1f;
					shouldFade = false;
				}
			}
			else
			{
				alpha -= 10f * Time.deltaTime;
				if (alpha < 0f)
				{
					alpha = 0f;
					shouldFade = false;
				}
			}
			self.alpha = alpha;
			if (panel != null)
			{
				float num = 0.95f + alpha * 0.05f;
				panel.transform.localScale = new Vector3(num, num, num);
			}
		}

		public static void ShowPanel(CanvasGroup panel)
		{
			panel.alpha = 1f;
			panel.interactable = true;
			panel.blocksRaycasts = true;
		}

		public static void HidePanel(CanvasGroup panel)
		{
			panel.alpha = 0f;
			panel.interactable = false;
			panel.blocksRaycasts = false;
		}
	}
}
