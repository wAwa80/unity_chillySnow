using UnityEngine;
using UnityEngine.UI;


namespace EndlessMode
{
	[RequireComponent(typeof(Image))]
	public class NightModeImage : Neuron
	{
		private Image image;

		private Color dayColor;

		[SerializeField]
		private Color nightColor = new Color(1f, 1f, 1f, 0.5f);

		protected override void Awake()
		{
			base.Awake();
			image = GetComponent<Image>();
			dayColor = image.color;
			OnNightModeSwitched(NightModeButton.nightModeOn);
		}

		protected override void OnNightModeSwitched(bool enabled)
		{
			if (enabled)
			{
				image.color = nightColor;
			}
			else
			{
				image.color = dayColor;
			}
		}
	}
}
