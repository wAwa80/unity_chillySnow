using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class ContinuePage : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	private static ContinuePage instance;

	private CanvasGroup canvasGroup;

	private Animation anim;

	[SerializeField]
	private Button continueButton;

	private Action<bool> onFinished;

	public static ContinuePage GetInstance()
	{
		return instance;
	}

	private void Awake()
	{
		instance = this;
		canvasGroup = GetComponent<CanvasGroup>();
		anim = GetComponent<Animation>();
		continueButton.onClick.AddListener(DoContinue);
		base.enabled = false;
	}

	public void Show(Action<bool> onFinished)
	{
		if (!Juice.ads.rewardedVideo.IsReady())
		{
			onFinished?.Invoke(obj: false);
			return;
		}
		this.onFinished = onFinished;
		canvasGroup.alpha = 1f;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
		anim.Play();
		base.enabled = true;
	}

	private void Hide()
	{
		canvasGroup.alpha = 0f;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
		base.enabled = false;
		onFinished = null;
	}

	private void Update()
	{
		if (!anim.isPlaying)
		{
			OnOutcome(outcome: false);
		}
	}

	private void DoContinue()
	{
		Juice.ads.rewardedVideo.Show(OnOutcome);
	}

	private void OnOutcome(bool outcome)
	{
		if (onFinished != null)
		{
			onFinished(outcome);
		}
		Hide();
	}

	public void OnPointerDown(PointerEventData data)
	{
		OnOutcome(outcome: false);
	}
}
