using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class VibrateButton : MonoBehaviour
{
	private static HashSet<VibrateButton> soundButtons;

	public static bool vibrateOn;

	private Image image;

	private Button button;

	private RectTransform bothIcons;

	private Image onIcon;

	private Image offIcon;

	private bool shouldShow;

	private float timer;

	private float appearSpeed;

	private float dissappearSpeed;

	static VibrateButton()
	{
		soundButtons = new HashSet<VibrateButton>();
	}

	private static void TurnAll()
	{
		if (vibrateOn)
		{
			vibrateOn = false;
			Data.SaveBool("vibrateOn", value: false);
			{
				foreach (VibrateButton soundButton in soundButtons)
				{
					soundButton.onIcon.enabled = false;
					soundButton.offIcon.enabled = true;
				}
				return;
			}
		}
		vibrateOn = true;
		Data.SaveBool("vibrateOn", value: true);
		foreach (VibrateButton soundButton2 in soundButtons)
		{
			soundButton2.onIcon.enabled = true;
			soundButton2.offIcon.enabled = false;
		}
		Device.Vibrate(Device.Vibration.Light);
	}

	private void Awake()
	{
		image = GetComponent<Image>();
		button = GetComponent<Button>();
		bothIcons = base.transform.GetChild(0).GetComponent<RectTransform>();
		onIcon = bothIcons.transform.GetChild(0).GetComponent<Image>();
		offIcon = bothIcons.transform.GetChild(1).GetComponent<Image>();
		button.onClick.AddListener(TurnAll);
		if (soundButtons.Count == 0)
		{
			if (Data.LoadBool("vibrateOn", defaultValue: true))
			{
				vibrateOn = true;
				onIcon.enabled = true;
				offIcon.enabled = false;
			}
			else
			{
				vibrateOn = false;
				onIcon.enabled = false;
				offIcon.enabled = true;
			}
		}
		else if (vibrateOn)
		{
			onIcon.enabled = true;
			offIcon.enabled = false;
		}
		else
		{
			onIcon.enabled = false;
			offIcon.enabled = true;
		}
		soundButtons.Add(this);
	}

	private void OnDestroy()
	{
		soundButtons.Remove(this);
	}

	public void Show(float appearSpeed = 2f)
	{
		this.appearSpeed = appearSpeed;
		image.enabled = true;
		button.enabled = true;
		shouldShow = true;
		base.enabled = true;
	}

	public void Hide(float dissappearSpeed = 2f)
	{
		this.dissappearSpeed = dissappearSpeed;
		button.enabled = false;
		shouldShow = false;
		base.enabled = true;
	}

	private void Update()
	{
		if (shouldShow)
		{
			timer += appearSpeed * Time.deltaTime;
			if (timer > 1f)
			{
				timer = 1f;
				base.enabled = false;
			}
			float num = 4f * timer - 3f;
			num = (9f - num * num) * 0.125f;
			bothIcons.transform.localScale = new Vector3(num, num, num);
			return;
		}
		timer -= dissappearSpeed * Time.deltaTime;
		if (timer < 0f)
		{
			timer = 0f;
			base.enabled = false;
			image.enabled = false;
		}
		float num2 = 4f * timer - 3f;
		num2 = (9f - num2 * num2) * 0.125f;
		bothIcons.transform.localScale = new Vector3(num2, num2, num2);
	}
}
