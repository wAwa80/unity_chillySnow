using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using ThreadPriority = UnityEngine.ThreadPriority;

namespace EasyMobile
{
	[AddComponentMenu("")]
	[DisallowMultipleComponent]
	public class Gif : MonoBehaviour
	{
		private static Gif _instance;

		private static Dictionary<int, GifExportTask> gifExportTasks = new Dictionary<int, GifExportTask>();

		private static int curExportId = 0;

		public static Gif Instance
		{
			get
			{
				if (_instance == null)
				{
					GameObject gameObject = new GameObject("Gif");
					_instance = gameObject.AddComponent<Gif>();
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
				}
				return _instance;
			}
		}

		public static void StartRecording(Recorder recorder)
		{
			if (recorder == null)
			{
				Debug.LogError("StartRecording FAILED: recorder is null.");
			}
			else if (recorder.IsRecording())
			{
				Debug.LogWarning("Attempted to start recording while it is already in progress.");
			}
			else
			{
				recorder.Record();
			}
		}

		public static AnimatedClip StopRecording(Recorder recorder)
		{
			AnimatedClip result = null;
			if (recorder == null)
			{
				Debug.LogError("StopRecording FAILED: recorder is null.");
			}
			else
			{
				result = recorder.Stop();
			}
			return result;
		}

		public static bool IsRecording(Recorder recorder)
		{
			return recorder != null && recorder.IsRecording();
		}

		public static void PlayClip(IClipPlayer player, AnimatedClip clip, float startDelay = 0f, bool loop = true)
		{
			if (player == null)
			{
				Debug.LogError("Player is null.");
			}
			else
			{
				player.Play(clip, startDelay, loop);
			}
		}

		public static void PausePlayer(IClipPlayer player)
		{
			if (player == null)
			{
				Debug.LogError("Player is null.");
			}
			else
			{
				player.Pause();
			}
		}

		public static void ResumePlayer(IClipPlayer player)
		{
			if (player == null)
			{
				Debug.LogError("Player is null.");
			}
			else
			{
				player.Resume();
			}
		}

		public static void StopPlayer(IClipPlayer player)
		{
			if (player == null)
			{
				Debug.LogError("Player is null.");
			}
			else
			{
				player.Stop();
			}
		}

		public static void ExportGif(AnimatedClip clip, string filename, int quality, ThreadPriority threadPriority, Action<AnimatedClip, float> exportProgressCallback, Action<AnimatedClip, string> exportCompletedCallback)
		{
			ExportGif(clip, filename, 0, quality, threadPriority, exportProgressCallback, exportCompletedCallback);
		}

		public static void ExportGif(AnimatedClip clip, string filename, int loop, int quality, ThreadPriority threadPriority, Action<AnimatedClip, float> exportProgressCallback, Action<AnimatedClip, string> exportCompletedCallback)
		{
			if (clip == null || clip.Frames.Length == 0 || clip.IsDisposed())
			{
				Debug.LogError("Attempted to export GIF from an empty or disposed clip.");
			}
			else if (string.IsNullOrEmpty(filename))
			{
				Debug.LogError("Exporting GIF failed: filename is null or empty.");
			}
			else
			{
				Instance.StartCoroutine(CRExportGif(clip, filename, loop, quality, threadPriority, exportProgressCallback, exportCompletedCallback));
			}
		}

		private void Awake()
		{
			if (_instance == null)
			{
				_instance = this;
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		private void OnEnable()
		{
			AndroidNativeGif.GifExportProgress += OnGifExportProgress;
			AndroidNativeGif.GifExportCompleted += OnGifExportCompleted;
		}

		private void OnDisable()
		{
			AndroidNativeGif.GifExportProgress -= OnGifExportProgress;
			AndroidNativeGif.GifExportCompleted -= OnGifExportCompleted;
		}

		private void OnDestroy()
		{
			if (this == _instance)
			{
				_instance = null;
			}
		}

		private void Update()
		{
			List<int> list = new List<int>(gifExportTasks.Keys);
			foreach (int item in list)
			{
				GifExportTask gifExportTask = gifExportTasks[item];
				if (gifExportTask.isExporting && gifExportTask.exportProgressCallback != null)
				{
					gifExportTask.exportProgressCallback(gifExportTask.clip, gifExportTask.progress);
				}
				if (gifExportTask.isDone)
				{
					if (gifExportTask.exportCompletedCallback != null)
					{
						gifExportTask.exportCompletedCallback(gifExportTask.clip, gifExportTask.filepath);
					}
					gifExportTask.clip = null;
					gifExportTask.imageData = null;
					gifExportTasks[item] = null;
					gifExportTasks.Remove(item);
				}
			}
		}

		private static void OnGifPreProcessing(int taskId, float progress)
		{
			if (gifExportTasks.ContainsKey(taskId))
			{
				gifExportTasks[taskId].progress = progress * 0.5f;
			}
		}

		private static void OnGifExportProgress(int taskId, float progress)
		{
			if (gifExportTasks.ContainsKey(taskId))
			{
				gifExportTasks[taskId].progress = 0.5f + progress * 0.5f;
			}
		}

		private static void OnGifExportCompleted(int taskId, string filepath)
		{
			if (gifExportTasks.ContainsKey(taskId))
			{
				gifExportTasks[taskId].isDone = true;
			}
		}

		private static IEnumerator CRExportGif(AnimatedClip clip, string filename, int loop, int quality, ThreadPriority threadPriority, Action<AnimatedClip, float> exportProgressCallback, Action<AnimatedClip, string> exportCompletedCallback)
		{
			if (loop < -1)
			{
				loop = -1;
			}
			int sampleFac = Mathf.RoundToInt(Mathf.Lerp(30f, 1f, (float)Mathf.Clamp(quality, 1, 100) / 100f));
			string folder = Application.persistentDataPath;
			string filepath = Path.Combine(folder, filename + ".gif");
			GifExportTask exportTask = new GifExportTask
			{
				taskId = curExportId++,
				clip = clip,
				imageData = null,
				filepath = filepath,
				loop = loop,
				sampleFac = sampleFac,
				exportProgressCallback = exportProgressCallback,
				exportCompletedCallback = exportCompletedCallback,
				workerPriority = threadPriority,
				isExporting = true,
				isDone = false,
				progress = 0f
			};
			gifExportTasks.Add(exportTask.taskId, exportTask);
			yield return null;
			Texture2D temp = new Texture2D(clip.Width, clip.Height, TextureFormat.RGB24, false)
			{
				hideFlags = HideFlags.HideAndDontSave,
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear,
				anisoLevel = 0
			};
			exportTask.imageData = new Color32[clip.Frames.Length][];
			for (int i = 0; i < clip.Frames.Length; i++)
			{
				RenderTexture source = (RenderTexture.active = clip.Frames[i]);
				temp.ReadPixels(new Rect(0f, 0f, source.width, source.height), 0, 0);
				temp.Apply();
				RenderTexture.active = null;
				exportTask.imageData[i] = temp.GetPixels32();
				OnGifPreProcessing(progress: (float)i / (float)clip.Frames.Length, taskId: exportTask.taskId);
				yield return null;
			}
			AndroidNativeGif.ExportGif(exportTask);
			UnityEngine.Object.Destroy(temp);
		}
	}
}
