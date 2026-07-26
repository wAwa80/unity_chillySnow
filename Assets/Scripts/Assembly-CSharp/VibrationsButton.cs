using UnityEngine.UI;

namespace LevelMode
{

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
			// SettingsBar 展开后靠父 CanvasGroup 控制可点性；按钮本身必须 enabled
			Show();
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
}
