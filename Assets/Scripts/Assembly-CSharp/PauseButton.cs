public class PauseButton : AnimatedButton<PauseButton>
{
	protected override void Awake()
	{
		base.Awake();
		button.onClick.AddListener(Neuron.Pause);
	}

	protected override void OnNewGame()
	{
		Invoke("ShowDelay", 0.5f);
	}

	protected override void OnGameOver(bool canUseSecondChance)
	{
		CancelInvoke();
		Hide();
	}

	protected override void OnContinue()
	{
		Show();
	}

	private void ShowDelay()
	{
		Show();
	}

	public override void Show(float appearSpeed = 2f)
	{
		if (App.IsRelease() || !DebugPage.dontShowPause)
		{
			base.Show(appearSpeed);
		}
	}
}
