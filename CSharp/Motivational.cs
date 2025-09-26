using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Motivational : Singleton<Motivational>
{
	public enum Goodness
	{
		Good,
		Awesome,
		Exquisite,
		Perfect
	}

	private readonly Dictionary<Goodness, string[]> wordings = new Dictionary<Goodness, string[]>
	{
		{
			Goodness.Good,
			new string[3] { "Good job!", "Yeah!", "Smooth!" }
		},
		{
			Goodness.Awesome,
			new string[3] { "Awesome!", "Wow!", "Chilly!" }
		},
		{
			Goodness.Exquisite,
			new string[4] { "What a style!", "Incredible!", "Amazing!", "Exquisite!" }
		},
		{
			Goodness.Perfect,
			new string[5] { "King of the mountain!", "Unstoppable!", "Mountain is yours!", "Chilly Snow master!", "You're on fire!" }
		}
	};

	[SerializeField]
	private Text uiText;

	private float timer;

	protected override void Awake()
	{
		base.Awake();
		base.enabled = false;
	}

	protected override void OnWhoosh(int points)
	{
		if (MotivationalButton.motivationalOn)
		{
			int whooshCombo = Pine.GetWhooshCombo();
			if (whooshCombo > 6)
			{
				Play(Goodness.Perfect);
			}
			else if (whooshCombo > 3)
			{
				Play(Goodness.Exquisite);
			}
			else if (whooshCombo > 2)
			{
				Play(Goodness.Awesome);
			}
			else if (whooshCombo > 1)
			{
				Play(Goodness.Good);
			}
		}
	}

	public void Play(Goodness goodness)
	{
		string[] array = wordings[goodness];
		uiText.text = Juice.translator.Translate(array[Random.Range(0, array.Length)]);
		timer = 0f;
		base.enabled = true;
		Color color = uiText.color;
		color.a = 1f;
		uiText.color = color;
	}

	private void Update()
	{
		if (timer < 1f)
		{
			timer += 2f * Time.deltaTime;
			if (timer > 1f)
			{
				timer = 1f;
			}
			float num = 4f * timer - 3f;
			num = (9f - num * num) * 0.1f;
			uiText.transform.localScale = new Vector3(num, num, num);
			return;
		}
		if (timer < 2f)
		{
			timer += Time.deltaTime;
			return;
		}
		timer += 2f * Time.deltaTime;
		if (timer > 3f)
		{
			timer = 3f;
			base.enabled = false;
		}
		Color color = uiText.color;
		color.a = 3f - timer;
		uiText.color = color;
	}
}
