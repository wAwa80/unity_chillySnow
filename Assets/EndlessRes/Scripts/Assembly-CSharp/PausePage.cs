using UnityEngine;
using UnityEngine.EventSystems;


namespace EndlessMode
{
	public sealed class PausePage : Page<PausePage>, IPointerDownHandler, IEventSystemHandler
	{
		[SerializeField]
		private SoundButton soundButton;

		[SerializeField]
		private VibrateButton vibrateButton;

		public void OnPointerDown(PointerEventData data)
		{
			Neuron.Unpause();
		}

		protected override void OnPause()
		{
			Show();
			soundButton.Show(10f);
			vibrateButton.Show(10f);
		}

		protected override void OnUnpause()
		{
			Time.timeScale = 1f;
			Hide();
			soundButton.Hide(10f);
			vibrateButton.Hide(10f);
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
}
