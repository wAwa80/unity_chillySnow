using UnityEngine.UI;


namespace EndlessMode
{
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
			OnBackToMenu();
		}

		private void Start()
		{
			if (Data.LoadBool("nightModeOn", Analytics.GetCohort() == "DefaultIsNightMode"))
			{
				SwitchMode();
			}
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
	}
}
