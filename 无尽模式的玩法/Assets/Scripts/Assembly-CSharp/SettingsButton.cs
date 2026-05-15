public sealed class SettingsButton : AnimatedButton<SettingsButton>
{
	protected override void Awake()
	{
		base.Awake();
		button.onClick.AddListener(delegate
		{
			Singleton<SettingsPage>.i.Show();
		});
		OnBackToMenu();
	}

	protected override void OnNewGame()
	{
		Hide();
	}

	protected override void OnBackToMenu()
	{
		Show();
	}
}
