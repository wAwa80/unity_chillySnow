using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace EasyMobile
{
	[AddComponentMenu("")]
	public class Giphy : MonoBehaviour
	{
		[Serializable]
		private class UploadSuccessResponse
		{
			[Serializable]
			public class UploadSuccessData
			{
				public string id = string.Empty;
			}

			public UploadSuccessData data = new UploadSuccessData();
		}

		private static Giphy _instance;

		public const string GIPHY_PUBLIC_BETA_KEY = "dc6zaTOxFJmzC";

		public const string GIPHY_UPLOAD_PATH = "https://upload.giphy.com/v1/gifs";

		public const string GIPHY_BASE_URL = "http://giphy.com/gifs/";

		private static int _apiUseCount;

		public static Giphy Instance
		{
			get
			{
				if (_instance == null)
				{
					GameObject gameObject = new GameObject("Giphy");
					_instance = gameObject.AddComponent<Giphy>();
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
				}
				return _instance;
			}
		}

		public static bool IsUsingAPI => _apiUseCount > 0;

		public static void Upload(GiphyUploadParams content, Action<float> uploadProgressCallback, Action<string> uploadCompletedCallback, Action<string> uploadFailedCallback)
		{
			Upload(string.Empty, "dc6zaTOxFJmzC", content, uploadProgressCallback, uploadCompletedCallback, uploadFailedCallback);
		}

		public static void Upload(string username, string apiKey, GiphyUploadParams content, Action<float> uploadProgressCallback, Action<string> uploadCompletedCallback, Action<string> uploadFailedCallback)
		{
			if (string.IsNullOrEmpty(content.localImagePath) && string.IsNullOrEmpty(content.sourceImageUrl))
			{
				Debug.LogError("UploadToGiphy FAILED: no image was specified for uploading.");
				return;
			}
			if (!string.IsNullOrEmpty(content.localImagePath) && !File.Exists(content.localImagePath))
			{
				Debug.LogError("UploadToGiphy FAILED: (local) file not found.");
				return;
			}
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("api_key", apiKey);
			wWWForm.AddField("username", username);
			if (!string.IsNullOrEmpty(content.localImagePath) && File.Exists(content.localImagePath))
			{
				wWWForm.AddBinaryData("file", File.ReadAllBytes(content.localImagePath));
			}
			if (!string.IsNullOrEmpty(content.sourceImageUrl))
			{
				wWWForm.AddField("source_image_url", content.sourceImageUrl);
			}
			if (!string.IsNullOrEmpty(content.tags))
			{
				wWWForm.AddField("tags", content.tags);
			}
			if (!string.IsNullOrEmpty(content.sourcePostUrl))
			{
				wWWForm.AddField("source_post_url", content.sourcePostUrl);
			}
			if (content.isHidden)
			{
				wWWForm.AddField("is_hidden", "true");
			}
			Instance.StartCoroutine(CRUpload(wWWForm, uploadProgressCallback, uploadCompletedCallback, uploadFailedCallback));
		}

		private static IEnumerator CRUpload(WWWForm form, Action<float> uploadProgressCB, Action<string> uploadCompletedCB, Action<string> uploadFailedCB)
		{
			WWW www = new WWW("https://upload.giphy.com/v1/gifs", form);
			_apiUseCount++;
			while (!www.isDone)
			{
				uploadProgressCB?.Invoke(www.uploadProgress);
				yield return null;
			}
			if (string.IsNullOrEmpty(www.error))
			{
				if (uploadCompletedCB != null)
				{
					UploadSuccessResponse uploadSuccessResponse = JsonUtility.FromJson<UploadSuccessResponse>(www.text);
					uploadCompletedCB("http://giphy.com/gifs/" + uploadSuccessResponse.data.id);
				}
			}
			else
			{
				uploadFailedCB?.Invoke(www.error);
			}
			_apiUseCount--;
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

		private void OnDestroy()
		{
			if (this == _instance)
			{
				_instance = null;
			}
		}
	}
}
