using System;
using System.Threading;
using UnityEngine;
using ThreadPriority = UnityEngine.ThreadPriority;

namespace EasyMobile
{
	internal class GifExportTask
	{
		internal int taskId;

		internal AnimatedClip clip;

		internal Color32[][] imageData;

		internal string filepath;

		internal int loop;

		internal int sampleFac;

		internal bool isExporting;

		internal bool isDone;

		internal float progress;

		internal Action<AnimatedClip, float> exportProgressCallback;

		internal Action<AnimatedClip, string> exportCompletedCallback;

		internal ThreadPriority workerPriority;
	}
}
