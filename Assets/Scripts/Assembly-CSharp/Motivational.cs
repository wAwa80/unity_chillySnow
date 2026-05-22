using System.Collections;
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
			new string[5] { "King of the mountain!", "Unstoppable!", "Mountain is yours!", "滑雪吧兄弟 master!", "You're on fire!" }
		}
	};

	[SerializeField]
	private Text uiText;

	protected override void OnWhoosh()
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

	public void Play(Goodness goodness)
	{
		if (MotivationalButton.motivationalOn)
		{
			string[] array = wordings[goodness];
			uiText.text = Translator.Translate(array[Random.Range(0, array.Length)]);
			StopAllCoroutines();
			StartCoroutine(PlayAnimation());
		}
	}

	private IEnumerator PlayAnimation()
	{
		float timer = 0f;
		Color c = uiText.color;
		c.a = 1f;
		uiText.color = c;
		while (timer < 1f)
		{
			timer += 2f * Time.deltaTime;
			if (timer > 1f)
			{
				timer = 1f;
			}
			float scale2 = 4f * timer - 3f;
			scale2 = (9f - scale2 * scale2) * 0.1f;
			uiText.transform.localScale = new Vector3(scale2, scale2, scale2);
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		while (timer > 0f)
		{
			timer -= 2f * Time.deltaTime;
			if (timer < 0f)
			{
				timer = 0f;
			}
			c.a = timer;
			uiText.color = c;
			yield return null;
		}
	}
}
