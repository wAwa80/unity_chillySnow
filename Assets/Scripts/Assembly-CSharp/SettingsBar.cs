using UnityEngine;
using UnityEngine.UI;

public sealed class SettingsBar : MonoBehaviour
{
	private static SettingsBar instance;

	private Button settingsButton;

	private CanvasGroup bar;

	private RectTransform barTransform;

	private bool shouldShow;

	private float timer;

	[SerializeField]
	private float animationSpeed = 2f;

	private bool shouldExpand;

	private float expandSpeed;

	public static SettingsBar GetInstance()
	{
		return instance;
	}

	private void Awake()
	{
		instance = this;
		settingsButton = base.transform.GetChild(1).GetComponent<Button>();
		settingsButton.onClick.AddListener(OnClick);
		bar = base.transform.GetChild(0).GetChild(0).GetComponent<CanvasGroup>();
		barTransform = bar.GetComponent<RectTransform>();
		Show();
	}

	private void OnClick()
	{
		shouldExpand = !shouldExpand;
		bar.interactable = shouldExpand;
		bar.blocksRaycasts = shouldExpand;
	}

	public void Show()
	{
		settingsButton.enabled = true;
		shouldShow = true;
		base.enabled = true;
		bar.alpha = 1f;
		expandSpeed = 10f;
	}

	public void Hide()
	{
		if (settingsButton.enabled)
		{
			settingsButton.enabled = false;
			shouldShow = false;
			if (shouldExpand)
			{
				shouldExpand = false;
				bar.blocksRaycasts = false;
				bar.interactable = false;
			}
			expandSpeed = 20f;
		}
	}

	private void Update()
	{
		if (shouldExpand)
		{
			Vector2 anchoredPosition = barTransform.anchoredPosition;
			anchoredPosition.y += (-15f - barTransform.sizeDelta.y - anchoredPosition.y) * expandSpeed * Time.deltaTime;
			barTransform.anchoredPosition = anchoredPosition;
		}
		else
		{
			Vector2 anchoredPosition2 = barTransform.anchoredPosition;
			anchoredPosition2.y += (-15f - anchoredPosition2.y) * expandSpeed * Time.deltaTime;
			barTransform.anchoredPosition = anchoredPosition2;
		}
		if (shouldShow)
		{
			if (timer < 1f)
			{
				timer += animationSpeed * Time.deltaTime;
				if (timer > 1f)
				{
					timer = 1f;
				}
				float num = OneMinusSinCardNormalized(timer);
				settingsButton.transform.localScale = new Vector3(num, num, num);
			}
		}
		else
		{
			timer -= animationSpeed * Time.deltaTime;
			if (timer < 0f)
			{
				timer = 0f;
				bar.alpha = 0f;
				base.enabled = false;
			}
			float num2 = OneMinusSinCardNormalized(timer);
			settingsButton.transform.localScale = new Vector3(num2, num2, num2);
		}
	}

	private float OneMinusSinCardNormalized(float x)
	{
		if (x == 0f)
		{
			return 0f;
		}
		return 1f - Mathf.Sin(x * 10f) * (0.1f / x - 0.1f);
	}
}
