using UnityEngine.UI;

public class MotivationalButton : SingletonButton<MotivationalButton>
{
	private Image onIcon;

	private Image offIcon;

	public static bool motivationalOn { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		onIcon = childTransform.GetChild(0).GetComponent<Image>();
		offIcon = childTransform.GetChild(1).GetComponent<Image>();
		motivationalOn = true;
	}

	private void Start()
	{
		if (!Data.LoadBool("motivationalOn", defaultValue: true))
		{
			OnClick();
		}
	}

	protected override void OnClick()
	{
		motivationalOn = !motivationalOn;
		Data.SaveBool("motivationalOn", motivationalOn);
		if (motivationalOn)
		{
			onIcon.enabled = true;
			offIcon.enabled = false;
		}
		else
		{
			onIcon.enabled = false;
			offIcon.enabled = true;
		}
	}
}
