using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContinuePage : Page<ContinuePage>, IPointerDownHandler, IEventSystemHandler
{
	private bool clickable;

	private Animation anim;

	[SerializeField]
	private Transform SecondChance;

	[SerializeField]
	private Image fill;

	private float timer;

	private bool freeze;

	private void SetClickable()
	{
		clickable = true;
	}

	protected override void OnGameOver(bool canUseSecondChance)
	{
		if (canUseSecondChance)
		{
			if (VoodooSauce.IsRewardedVideoAvailable())
			{
				Invoke("Show", 1f);
			}
			else
			{
				Invoke("CannotContinue", 2f);
			}
		}
	}

	private void CannotContinue()
	{
		Neuron.GameOver(canUseSecondChance: false);
	}

	public void TryContinue()
	{
		freeze = true;
		VoodooSauce.ShowRewardedVideo(ValidateContinue);
	}

	private void ValidateContinue(bool finishedVideo)
	{
		if (finishedVideo)
		{
			Neuron.Continue();
		}
		else
		{
			Abort();
		}
	}

	protected override void OnContinue()
	{
		Hide();
	}

	public void OnPointerDown(PointerEventData data)
	{
		if (clickable)
		{
			Abort();
		}
	}

	private void Abort()
	{
		Hide();
		Neuron.GameOver(canUseSecondChance: false);
	}

	protected override void Awake()
	{
		base.Awake();
		base.enabled = false;
		anim = GetComponent<Animation>();
	}

	public override void Show()
	{
		base.Show();
		freeze = false;
		anim.Play();
		SecondChance.localScale = new Vector3(0f, 0f, 1f);
		base.enabled = true;
		timer = 4f;
		clickable = true;
	}

	public override void Hide()
	{
		base.Hide();
		anim.Stop();
		base.enabled = false;
	}

	protected override void Update()
	{
		base.Update();
		if (SecondChance.localScale.x < 1f)
		{
			float num = Mathf.Lerp(SecondChance.localScale.x, 1f, 10f * Time.deltaTime);
			SecondChance.localScale = new Vector3(num, num, 1f);
		}
		if (!freeze)
		{
			timer -= Time.deltaTime;
			if (timer <= 0f)
			{
				Abort();
				return;
			}
			float fillAmount = timer * 0.25f;
			fill.fillAmount = fillAmount;
		}
	}
}
