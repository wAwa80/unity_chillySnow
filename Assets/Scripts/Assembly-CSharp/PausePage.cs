using UnityEngine;
using UnityEngine.EventSystems;

public sealed class PausePage : Page<PausePage>, IPointerDownHandler, IEventSystemHandler
{
	[SerializeField]
	private SoundButton soundButton;

	public void OnPointerDown(PointerEventData data)
	{
		Neuron.Unpause();
	}

	protected override void OnPause()
	{
		Show();
		soundButton.Show(10f);
	}

	protected override void OnUnpause()
	{
		Time.timeScale = 1f;
		Hide();
		soundButton.Hide(10f);
	}

	protected override void Update()
	{
		base.Update();
		if (self.alpha == 1f && Time.timeScale > 0f)
		{
			Time.timeScale = 0f;
		}
	}
}
