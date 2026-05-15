using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public sealed class SoundButton : MonoBehaviour
{
	private static HashSet<SoundButton> soundButtons;

	private static bool audioOn;

	private Image image;

	private Button button;

	private RectTransform bothIcons;

	private Image onIcon;

	private Image offIcon;

	private bool shouldShow;

	private float timer;

	private float appearSpeed;

	private float dissappearSpeed;

	static SoundButton()
	{
		soundButtons = new HashSet<SoundButton>();
	}

	private static void TurnAll()
	{
		if (audioOn)
		{
			AudioListener.volume = 0f;
			audioOn = false;
			Data.SaveBool("audioOn", value: false);
			{
				foreach (SoundButton soundButton in soundButtons)
				{
					soundButton.onIcon.enabled = false;
					soundButton.offIcon.enabled = true;
				}
				return;
			}
		}
		AudioListener.volume = 1f;
		audioOn = true;
		Data.SaveBool("audioOn", value: true);
		foreach (SoundButton soundButton2 in soundButtons)
		{
			soundButton2.onIcon.enabled = true;
			soundButton2.offIcon.enabled = false;
		}
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
			if (Data.LoadBool("audioOn"))
			{
				audioOn = true;
				AudioListener.volume = 1f;
				onIcon.enabled = true;
				offIcon.enabled = false;
			}
			else
			{
				audioOn = false;
				AudioListener.volume = 0f;
				onIcon.enabled = false;
				offIcon.enabled = true;
			}
		}
		else if (audioOn)
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
