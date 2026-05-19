using UnityEngine.UI;


namespace EndlessMode
{
	public class MotivationalButton : AnimatedButton<MotivationalButton>
	{
		private Image onIcon;

		private Image offIcon;

		public static bool motivationalOn { get; private set; }

		protected override void Awake()
		{
			base.Awake();
			onIcon = childTransform.GetChild(0).GetComponent<Image>();
			offIcon = childTransform.GetChild(1).GetComponent<Image>();
			motivationalOn = true;
			button.onClick.AddListener(SwitchMode);
		}

		private void Start()
		{
			if (!Data.LoadBool("motivationalOn", defaultValue: true))
			{
				SwitchMode();
			}
		}

		private void SwitchMode()
		{
			motivationalOn = !motivationalOn;
			Data.SaveBool("motivationalOn", motivationalOn);
			if (motivationalOn)
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
