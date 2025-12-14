using System.Collections;
using UnityEngine;

namespace EasyMobile
{
	[AddComponentMenu("Easy Mobile/Clip Player")]
	[RequireComponent(typeof(MeshRenderer))]
	[DisallowMultipleComponent]
	public class ClipPlayer : MonoBehaviour, IClipPlayer
	{
		[SerializeField]
		private ClipPlayerScaleMode _scaleMode = ClipPlayerScaleMode.AutoHeight;

		private Material mat;

		private IEnumerator playCoroutine;

		private bool isPaused;

		public ClipPlayerScaleMode ScaleMode
		{
			get
			{
				return _scaleMode;
			}
			set
			{
				_scaleMode = value;
			}
		}

		private void Awake()
		{
			mat = GetComponent<MeshRenderer>().material;
		}

		public void Play(AnimatedClip clip, float startDelay = 0f, bool loop = true)
		{
			if (clip == null || clip.Frames.Length == 0 || clip.IsDisposed())
			{
				Debug.LogError("Attempted to play an empty or disposed clip.");
				return;
			}
			Stop();
			Resize(clip);
			isPaused = false;
			playCoroutine = CRPlay(clip, startDelay, loop);
			StartCoroutine(playCoroutine);
		}

		public void Pause()
		{
			isPaused = true;
		}

		public void Resume()
		{
			isPaused = false;
		}

		public void Stop()
		{
			if (playCoroutine != null)
			{
				StopCoroutine(playCoroutine);
				playCoroutine = null;
			}
		}

		private void Resize(AnimatedClip clip)
		{
			if (_scaleMode != 0)
			{
				float num = (float)clip.Width / (float)clip.Height;
				Vector3 localScale = base.transform.localScale;
				if (_scaleMode == ClipPlayerScaleMode.AutoHeight)
				{
					localScale.y = localScale.x / num;
				}
				else if (_scaleMode == ClipPlayerScaleMode.AutoWidth)
				{
					localScale.x = localScale.y * num;
				}
				base.transform.localScale = localScale;
			}
		}

		private IEnumerator CRPlay(AnimatedClip clip, float startDelay, bool loop)
		{
			float timePerFrame = 1f / (float)clip.FramePerSecond;
			bool hasDelayed = false;
			do
			{
				for (int i = 0; i < clip.Frames.Length; i++)
				{
					mat.mainTexture = clip.Frames[i];
					yield return new WaitForSeconds(timePerFrame);
					if (startDelay > 0f && !hasDelayed && i == 0)
					{
						hasDelayed = true;
						yield return new WaitForSeconds(startDelay);
					}
					if (isPaused)
					{
						yield return null;
					}
				}
			}
			while (loop);
		}
	}
}
