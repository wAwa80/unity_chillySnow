using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public abstract class SingletonButton<T> : Singleton<T> where T : SingletonButton<T>
{
	private Button button;

	protected RectTransform childTransform;

	private bool shouldShow;

	private float timer;

	protected override void Awake()
	{
		base.Awake();
		button = GetComponent<Button>();
		button.onClick.AddListener(OnClick);
		childTransform = base.transform.GetChild(0).GetComponent<RectTransform>();
		base.enabled = false;
	}

	public virtual void Show()
	{
		button.enabled = true;
		shouldShow = true;
		base.enabled = true;
	}

	public virtual void Hide()
	{
		button.enabled = false;
		shouldShow = false;
		base.enabled = true;
	}

	private void Update()
	{
		if (shouldShow)
		{
			timer += 4f * Time.deltaTime;
			if (timer > 1f)
			{
				timer = 1f;
				base.enabled = false;
			}
			float num = 4f * timer - 3f;
			num = (9f - num * num) * 0.125f;
			childTransform.localScale = new Vector3(num, num, num);
		}
		else
		{
			timer -= 4f * Time.deltaTime;
			if (timer < 0f)
			{
				timer = 0f;
				base.enabled = false;
			}
			float num2 = 4f * timer - 3f;
			num2 = (9f - num2 * num2) * 0.125f;
			childTransform.localScale = new Vector3(num2, num2, num2);
		}
	}

	protected abstract void OnClick();
}
