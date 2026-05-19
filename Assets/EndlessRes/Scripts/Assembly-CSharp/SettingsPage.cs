using UnityEngine;
using UnityEngine.EventSystems;


namespace EndlessMode
{
	public sealed class SettingsPage : Page<SettingsPage>, IPointerDownHandler, IEventSystemHandler
	{
		[SerializeField]
		private SoundButton soundButton;

		[SerializeField]
		private VibrateButton vibrateButton;

		public void OnPointerDown(PointerEventData data)
		{
			Hide();
		}

		public override void Show()
		{
			base.Show();
			Singleton<SettingsButton>.i.Hide(4f);
			soundButton.Show();
			Invoke("ShowVibrateButton", 0.2f);
			Invoke("ShowMotivationalButton", 0.4f);
			Invoke("ShowGameCenterButton", 0.6f);
			Invoke("ShowPremiumButton", 0.8f);
			Invoke("ShowRestoreButton", 1f);
		}

		public override void Hide()
		{
			base.Hide();
			CancelInvoke();
			Singleton<SettingsButton>.i.Show(4f);
			soundButton.Hide();
			vibrateButton.Hide();
			Singleton<MotivationalButton>.i.Hide();
			Singleton<GameCenterButton>.i.Hide();
			Singleton<PremiumButton>.i.Hide();
			Singleton<RestoreButton>.i.Hide();
		}

		private void ShowVibrateButton()
		{
			vibrateButton.Show();
		}

		private void ShowMotivationalButton()
		{
			Singleton<MotivationalButton>.i.Show();
		}

		private void ShowGameCenterButton()
		{
			Singleton<GameCenterButton>.i.Show();
		}

		private void ShowPremiumButton()
		{
			Singleton<PremiumButton>.i.Show();
		}

		private void ShowRestoreButton()
		{
			Singleton<RestoreButton>.i.Show();
		}
	}
}
