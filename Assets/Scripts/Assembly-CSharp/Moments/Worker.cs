using System.Threading;
using System.Collections.Generic;
using Moments.Encoder;
using System;

namespace Moments
{
	internal class Worker
	{
		internal Worker(int taskId, ThreadPriority priority, List<GifFrame> frames, GifEncoder encoder, string filepath, Action<int, float> onFileSaveProgress, Action<int, string> onFileSaved)
		{
		}

	}
}
