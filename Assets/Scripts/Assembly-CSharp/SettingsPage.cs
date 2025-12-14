using UnityEngine;
using UnityEngine.EventSystems;

public sealed class SettingsPage : Page<SettingsPage>, IPointerDownHandler, IEventSystemHandler
{
	[SerializeField]
	private SoundButton soundButton;

	public void OnPointerDown(PointerEventData data)
	{
		Hide();
	}

	public override void Show()
	{
		base.Show();
		Singleton<SettingsButton>.i.Hide(4f);
		soundButton.Show();
		Invoke("ShowGameCenterButton", 0.2f);
		Invoke("ShowPremiumButton", 0.4f);
		Invoke("ShowRestoreButton", 0.6f);
	}

	public override void Hide()
	{
		base.Hide();
		CancelInvoke();
		Singleton<SettingsButton>.i.Show(4f);
		soundButton.Hide();
		Singleton<GameCenterButton>.i.Hide();
		Singleton<PremiumButton>.i.Hide();
		Singleton<RestoreButton>.i.Hide();
	}

	private void ShowVibrateButton()
	{
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
