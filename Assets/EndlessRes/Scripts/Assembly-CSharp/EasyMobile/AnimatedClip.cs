using System;
using UnityEngine;

namespace EasyMobile
{
	public sealed class AnimatedClip : IDisposable
	{
		private bool isDisposed;

		public int Width { get; private set; }

		public int Height { get; private set; }

		public int FramePerSecond { get; private set; }

		public float Length { get; private set; }

		public RenderTexture[] Frames { get; private set; }

		public AnimatedClip(int width, int height, int fps, RenderTexture[] frames)
		{
			Width = width;
			Height = height;
			FramePerSecond = fps;
			Frames = frames;
			Length = (float)frames.Length / (float)fps;
		}

		public bool IsDisposed()
		{
			return isDisposed;
		}

		public void Dispose()
		{
			Cleanup();
		}

		private void Cleanup()
		{
			if (isDisposed)
			{
				return;
			}
			if (Frames != null)
			{
				RenderTexture[] frames = Frames;
				foreach (RenderTexture renderTexture in frames)
				{
					renderTexture.Release();
					UnityEngine.Object.Destroy(renderTexture);
				}
				Frames = null;
			}
			isDisposed = true;
		}
	}
}
