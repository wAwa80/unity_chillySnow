using UnityEngine;
using UnityEngine.UI;

public sealed class SoundButton : SingletonButton<SoundButton>
{
	private AudioSource source;

	private Image onIcon;

	private Image offIcon;

	protected override void Awake()
	{
		base.Awake();
		source = GetComponent<AudioSource>();
		onIcon = childTransform.GetChild(0).GetComponent<Image>();
		offIcon = childTransform.GetChild(1).GetComponent<Image>();
		SyncUI();
	}

	protected override void OnClick()
	{
		Device.SwitchSound();
		SyncUI();
	}

	private void SyncUI()
	{
		if (Device.IsSoundOn())
		{
			source.Play();
			onIcon.enabled = true;
			offIcon.enabled = false;
		}
		else
		{
			source.Stop();
			onIcon.enabled = false;
			offIcon.enabled = true;
		}
	}
}
