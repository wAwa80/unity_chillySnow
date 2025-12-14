using System;
using System.Threading;
using UnityEngine;

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

		internal UnityEngine.ThreadPriority workerPriority;
	}
}
