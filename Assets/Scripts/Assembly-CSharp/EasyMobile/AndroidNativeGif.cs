using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

namespace EasyMobile
{
	internal class AndroidNativeGif
	{
		private delegate void GifExportProgressDelegate(int taskId, float progress);

		private delegate void GifExportCompletedDelegate(int taskId, string filepath);

		private GifExportTask myExportTask;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<int, float> m_GifExportProgress;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Action<int, string> m_GifExportCompleted;

		private static Dictionary<int, GCHandle[]> gcHandles;

		internal static event Action<int, float> GifExportProgress
		{
			add
			{
				Action<int, float> action = AndroidNativeGif.m_GifExportProgress;
				Action<int, float> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref AndroidNativeGif.m_GifExportProgress, (Action<int, float>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<int, float> action = AndroidNativeGif.m_GifExportProgress;
				Action<int, float> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref AndroidNativeGif.m_GifExportProgress, (Action<int, float>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		internal static event Action<int, string> GifExportCompleted
		{
			add
			{
				Action<int, string> action = AndroidNativeGif.m_GifExportCompleted;
				Action<int, string> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref AndroidNativeGif.m_GifExportCompleted, (Action<int, string>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<int, string> action = AndroidNativeGif.m_GifExportCompleted;
				Action<int, string> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref AndroidNativeGif.m_GifExportCompleted, (Action<int, string>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		internal AndroidNativeGif(GifExportTask exportTask)
		{
			myExportTask = exportTask;
		}

		//[MonoPInvokeCallback(typeof(GifExportProgressDelegate))]
		//private static void GifExportProgressCallback(int taskId, float progress)
		//{
		//	if (AndroidNativeGif.GifExportProgress != null)
		//	{
		//		AndroidNativeGif.GifExportProgress(taskId, progress);
		//	}
		//}

		//[MonoPInvokeCallback(typeof(GifExportCompletedDelegate))]
		//private static void GifExportCompletedCallback(int taskId, string filepath)
		//{
		//	if (AndroidNativeGif.GifExportCompleted != null)
		//	{
		//		AndroidNativeGif.GifExportCompleted(taskId, filepath);
		//	}
		//	GCHandle[] array = gcHandles[taskId];
		//	foreach (GCHandle gCHandle in array)
		//	{
		//		gCHandle.Free();
		//	}
		//	gcHandles.Remove(taskId);
		//}

		[DllImport("easymobile")]
		private static extern void _ExportGif(int taskId, string filepath, int width, int height, int loop, int fps, int sampleFac, int frameCount, IntPtr[] imageData, GifExportProgressDelegate exportingCallback, GifExportCompletedDelegate exportCompletedCallback);

		internal static void ExportGif(GifExportTask exportTask)
		{
			AndroidNativeGif @object = new AndroidNativeGif(exportTask);
			Thread thread = new Thread(@object.DoExportGif);
			thread.Priority = (System.Threading.ThreadPriority)exportTask.workerPriority;
			thread.Start();
		}

		private void DoExportGif()
		{
			int taskId = myExportTask.taskId;
			string filepath = myExportTask.filepath;
			int width = myExportTask.clip.Width;
			int height = myExportTask.clip.Height;
			int loop = myExportTask.loop;
			int framePerSecond = myExportTask.clip.FramePerSecond;
			int sampleFac = myExportTask.sampleFac;
			int frameCount = myExportTask.clip.Frames.Length;
			Color32[][] imageData = myExportTask.imageData;
			GCHandle[] array = new GCHandle[imageData.Length];
			IntPtr[] array2 = new IntPtr[imageData.Length];
			for (int i = 0; i < imageData.Length; i++)
			{
				ref GCHandle reference = ref array[i];
				reference = GCHandle.Alloc(imageData[i], GCHandleType.Pinned);
				ref IntPtr reference2 = ref array2[i];
				reference2 = array[i].AddrOfPinnedObject();
			}
			if (gcHandles == null)
			{
				gcHandles = new Dictionary<int, GCHandle[]>();
			}
			gcHandles.Add(taskId, array);
			//_ExportGif(taskId, filepath, width, height, loop, framePerSecond, sampleFac, frameCount, array2, GifExportProgressCallback, GifExportCompletedCallback);
		}
	}
}
