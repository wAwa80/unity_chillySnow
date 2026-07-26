using UnityEngine;
using UnityEngine.UI;

namespace LevelMode
{

	/// <summary>
	/// 对齐无尽 AnimatedButton：Show 时必须启用根 Image 以拦截射线，否则 FingerPage 全屏层会吃掉点击。
	/// </summary>
	[RequireComponent(typeof(Button), typeof(Image))]
	public abstract class SingletonButton<T> : Singleton<T> where T : SingletonButton<T>
	{
		private Button button;

		protected Image image;

		protected RectTransform childTransform;

		private bool shouldShow;

		private float timer;

		protected override void Awake()
		{
			base.Awake();
			button = GetComponent<Button>();
			image = GetComponent<Image>();
			button.onClick.AddListener(OnClick);
			if (base.transform.childCount > 0)
			{
				childTransform = base.transform.GetChild(0).GetComponent<RectTransform>();
			}
			// 根 Image 负责按钮热区射线拦截（子 Icon 通常 raycastTarget=false）
			if (image != null)
			{
				image.raycastTarget = true;
			}
			ApplyHiddenVisualState();
			base.enabled = false;
		}

		public virtual void Show()
		{
			if (image != null)
			{
				image.enabled = true;
				image.raycastTarget = true;
			}
			button.enabled = true;
			button.interactable = true;
			shouldShow = true;
			base.enabled = true;
		}

		public virtual void Hide()
		{
			button.interactable = false;
			button.enabled = false;
			shouldShow = false;
			base.enabled = true;
		}

		/// <summary>Instant hide without scale animation (e.g. scene unload).</summary>
		protected void ApplyHiddenVisualState()
		{
			button.interactable = false;
			button.enabled = false;
			if (image != null)
			{
				image.enabled = false;
			}
			shouldShow = false;
			timer = 0f;
			if (childTransform != null)
			{
				childTransform.localScale = Vector3.zero;
			}
		}

		private void Update()
		{
			// 暂停 timeScale=0 时仍要播完显隐缩放
			float dt = Time.unscaledDeltaTime;
			if (shouldShow)
			{
				timer += 4f * dt;
				if (timer > 1f)
				{
					timer = 1f;
					base.enabled = false;
				}
				float num = 4f * timer - 3f;
				num = (9f - num * num) * 0.125f;
				if (childTransform != null)
				{
					childTransform.localScale = new Vector3(num, num, num);
				}
			}
			else
			{
				timer -= 4f * dt;
				if (timer < 0f)
				{
					timer = 0f;
					base.enabled = false;
					// 缩放动画结束后关闭根 Image，释放射线给 FingerPage
					if (image != null)
					{
						image.enabled = false;
					}
				}
				float num2 = 4f * timer - 3f;
				num2 = (9f - num2 * num2) * 0.125f;
				if (childTransform != null)
				{
					childTransform.localScale = new Vector3(num2, num2, num2);
				}
			}
		}

		protected abstract void OnClick();
	}
}
