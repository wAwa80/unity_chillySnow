using UnityEngine.UI;

public class NightModeButton : AnimatedButton<NightModeButton>
{
	private Image onIcon;

	private Image offIcon;

	public static bool nightModeOn { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		onIcon = childTransform.GetChild(0).GetComponent<Image>();
		offIcon = childTransform.GetChild(1).GetComponent<Image>();
		button.onClick.AddListener(SwitchMode);
		if (Data.LoadBool("nightModeOn"))
		{
			SwitchMode();
		}
		OnNightModeSwitched(nightModeOn);
		OnBackToMenu();
	}

	private void SwitchMode()
	{
		nightModeOn = !nightModeOn;
		Data.SaveBool("nightModeOn", nightModeOn);
		Neuron.NightModeSwitched(nightModeOn);
		if (nightModeOn)
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

	protected override void OnBackToMenu()
	{
		Show();
	}

	protected override void OnNewGame()
	{
		Hide();
	}

	public override void Show(float appearSpeed = 2f)
	{
		if (Analytics.GetCohort() == "NightMode")
		{
			base.Show(appearSpeed);
		}
	}
}
