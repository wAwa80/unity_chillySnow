namespace EasyMobile
{
	public interface IClipPlayer
	{
		ClipPlayerScaleMode ScaleMode { get; set; }

		void Play(AnimatedClip clip, float startDelay = 0f, bool loop = true);

		void Pause();

		void Resume();

		void Stop();
	}
}
