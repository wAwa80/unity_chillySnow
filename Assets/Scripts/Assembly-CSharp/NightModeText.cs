using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class NightModeText : Neuron
{
	private Text text;

	private Color dayColor;

	private Color nightColor;

	protected override void Awake()
	{
		base.Awake();
		text = GetComponent<Text>();
		dayColor = text.color;
		nightColor = Utility.HexToColor("#f8fff5");
		OnNightModeSwitched(NightModeButton.nightModeOn);
	}

	protected override void OnNightModeSwitched(bool enabled)
	{
		if (enabled)
		{
			text.color = nightColor;
		}
		else
		{
			text.color = dayColor;
		}
	}
}
