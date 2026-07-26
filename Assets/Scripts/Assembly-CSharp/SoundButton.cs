using UnityEngine;
using UnityEngine.UI;

namespace LevelMode
{

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
			// SettingsBar 展开后靠父 CanvasGroup 控制可点性；按钮本身必须 enabled
			Show();
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
}
