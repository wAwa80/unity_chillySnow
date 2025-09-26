using UnityEngine.UI;

public sealed class VibrationsButton : SingletonButton<VibrationsButton>
{
	private Image onIcon;

	private Image offIcon;

	protected override void Awake()
	{
		base.Awake();
		onIcon = childTransform.GetChild(0).GetComponent<Image>();
		offIcon = childTransform.GetChild(1).GetComponent<Image>();
		SyncUI();
	}

	protected override void OnClick()
	{
		Device.SwitchVibrations();
		SyncUI();
	}

	private void SyncUI()
	{
		if (Device.IsVibrationOn())
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
