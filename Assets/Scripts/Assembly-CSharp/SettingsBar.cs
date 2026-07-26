using UnityEngine;
using UnityEngine.UI;

namespace LevelMode
{

	public sealed class SettingsBar : MonoBehaviour
	{
		private static SettingsBar instance;

		private Button settingsButton;

		private Image settingsButtonImage;

		/// <summary>内层 Bar 的 CanvasGroup（直接控制 alpha/interactable/blocksRaycasts）。</summary>
		private CanvasGroup bar;

		private bool shouldShow;

		private float timer;

		[SerializeField]
		private float animationSpeed = 2f;

		private bool shouldExpand;

		/// <summary>防止 Button.onClick 与 Finger 兜底同帧各触发一次，抵消展开。</summary>
		private float lastToggleUnscaledTime = -1f;

		private float expandAlphaSpeed = 8f;

		public static SettingsBar GetInstance()
		{
			return instance;
		}

		/// <summary>供 Finger 兜底：点在齿轮矩形内时调用。</summary>
		public void ToggleExpandFromPointer()
		{
			if (settingsButton == null || !settingsButton.enabled)
			{
				return;
			}
			OnClick();
		}

		/// <summary>齿轮按钮的 Rect，供 Finger 判断是否点在设置上。</summary>
		public RectTransform GetButtonRect()
		{
			return settingsButton != null ? settingsButton.transform as RectTransform : null;
		}

		private void Awake()
		{
			instance = this;
			// 层级：SettingsBar → GetChild(0)=外层面板 → GetChild(0)=Bar(CanvasGroup)
			//        SettingsBar → GetChild(1)=SettingsButton
			bar = base.transform.GetChild(0).GetChild(0).GetComponent<CanvasGroup>();
			settingsButton = base.transform.GetChild(1).GetComponent<Button>();
			settingsButtonImage = settingsButton.GetComponent<Image>();
			settingsButton.onClick.AddListener(OnClick);

			// 初始收起：alpha=0，不可点，不挡射线
			bar.alpha = 0f;
			bar.interactable = false;
			bar.blocksRaycasts = false;
			Show();
		}

		private void OnClick()
		{
			// 防抖：同一次点击若被 Button.onClick 与 Finger 兜底各触发一次，第二次忽略
			if (Time.unscaledTime - lastToggleUnscaledTime < 0.2f)
			{
				Debug.Log("[SettingsBar] OnClick 防抖忽略");
				return;
			}
			lastToggleUnscaledTime = Time.unscaledTime;

			shouldExpand = !shouldExpand;
			if (shouldExpand)
			{
				// 展开：立刻允许射线（alpha 动画跟上）
				bar.interactable = true;
				bar.blocksRaycasts = true;
			}
			else
			{
				// 收起：立刻禁止点击，alpha 动画淡出
				bar.interactable = false;
				bar.blocksRaycasts = false;
			}
			Debug.Log($"[SettingsBar] OnClick → shouldExpand={shouldExpand}");
		}

		public void Show()
		{
			transform.SetAsLastSibling();
			settingsButton.enabled = true;
			if (settingsButtonImage != null)
			{
				settingsButtonImage.enabled = true;
				settingsButtonImage.raycastTarget = true;
			}
			shouldShow = true;
			base.enabled = true;
		}

		public void Hide()
		{
			if (settingsButton.enabled)
			{
				settingsButton.enabled = false;
				if (settingsButtonImage != null)
				{
					settingsButtonImage.raycastTarget = false;
				}
				shouldShow = false;
				// 收起面板
				shouldExpand = false;
				bar.interactable = false;
				bar.blocksRaycasts = false;
			}
		}

		private void Update()
		{
			// 用 alpha 淡入/淡出控制面板显示，不移动位置（避免层级尺寸依赖）
			float targetAlpha = shouldExpand ? 1f : 0f;
			bar.alpha = Mathf.MoveTowards(bar.alpha, targetAlpha, expandAlphaSpeed * Time.deltaTime);

			// 齿轮按钮缩放动画
			if (shouldShow)
			{
				if (timer < 1f)
				{
					timer += animationSpeed * Time.deltaTime;
					if (timer > 1f) timer = 1f;
					float s = OneMinusSinCardNormalized(timer);
					settingsButton.transform.localScale = new Vector3(s, s, s);
				}
			}
			else
			{
				timer -= animationSpeed * Time.deltaTime;
				if (timer < 0f)
				{
					timer = 0f;
					base.enabled = false;
				}
				float s2 = OneMinusSinCardNormalized(timer);
				settingsButton.transform.localScale = new Vector3(s2, s2, s2);
			}
		}

		private float OneMinusSinCardNormalized(float x)
		{
			if (x == 0f) return 0f;
			return 1f - Mathf.Sin(x * 10f) * (0.1f / x - 0.1f);
		}
	}
}
