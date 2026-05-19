using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace EndlessMode
{
	[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
	public sealed class SkinUnlockedBanner : Singleton<SkinUnlockedBanner>
	{
		private readonly Queue<Skin> announcements = new Queue<Skin>();

		private RectTransform rectTransform;

		private CanvasGroup canvasGroup;

		[SerializeField]
		private Image skinPreview;

		private float maxPivot = 1.2f;

		private bool shouldShow;

		private float showTime;

		protected override void Awake()
		{
			base.Awake();
			rectTransform = GetComponent<RectTransform>();
			canvasGroup = GetComponent<CanvasGroup>();
			if (Screen.height > 2 * Screen.width)
			{
				maxPivot = 1.5f;
			}
		}

		public void Announce(Skin skin)
		{
			if (base.enabled)
			{
				announcements.Enqueue(skin);
				return;
			}
			base.enabled = true;
			canvasGroup.alpha = 1f;
			Show(skin);
		}

		private void Show(Skin skin)
		{
			showTime = 2f;
			shouldShow = true;
			skinPreview.sprite = skin.GetSprite();
			skinPreview.color = skin.GetColor();
		}

		private void Update()
		{
			float y = rectTransform.pivot.y;
			if (shouldShow)
			{
				y += (maxPivot - y) * 5f * Time.deltaTime;
				showTime -= Time.deltaTime;
				if (showTime <= 0f)
				{
					shouldShow = false;
				}
			}
			else
			{
				y -= (0.2f + y) * 5f * Time.deltaTime;
				if (y <= 0.001f)
				{
					if (announcements.Count > 0)
					{
						Show(announcements.Dequeue());
					}
					else
					{
						base.enabled = false;
						canvasGroup.alpha = 0f;
					}
				}
			}
			rectTransform.pivot = new Vector2(rectTransform.pivot.x, y);
			rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 0f);
		}
	}
}
