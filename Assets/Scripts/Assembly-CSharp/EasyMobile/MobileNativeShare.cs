using System.IO;
using UnityEngine;

namespace EasyMobile
{
	public static class MobileNativeShare
	{
		public static void ShareText(string text, string subject = "")
		{
			AndroidNativeShare.ShareTextOrURL(text, subject);
		}

		public static void ShareURL(string url, string subject = "")
		{
			AndroidNativeShare.ShareTextOrURL(url, subject);
		}

		public static void ShareImage(string imagePath, string message, string subject = "")
		{
			AndroidNativeShare.ShareImage(imagePath, message, subject);
		}

		public static string ShareScreenshot(string filename, string message, string subject = "")
		{
			return ShareScreenshot(0f, 0f, Screen.width, Screen.height, filename, message, subject);
		}

		public static string ShareScreenshot(float startX, float startY, float width, float height, string filename, string message, string subject = "")
		{
			string text = SaveScreenshot(startX, startY, width, height, filename);
			if (text != null)
			{
				ShareImage(text, message, subject);
			}
			return text;
		}

		public static string ShareTexture2D(Texture2D tt, string filename, string message, string subject = "")
		{
			byte[] bytes = tt.EncodeToPNG();
			string text = Path.Combine(Application.persistentDataPath, filename + ".png");
			File.WriteAllBytes(text, bytes);
			ShareImage(text, message, subject);
			return text;
		}

		public static string SaveScreenshot(string filename)
		{
			return SaveScreenshot(0f, 0f, Screen.width, Screen.height, filename);
		}

		public static string SaveScreenshot(float startX, float startY, float width, float height, string filename)
		{
			string persistentDataPath = Application.persistentDataPath;
			Texture2D texture2D = CaptureScreenshot(startX, startY, width, height);
			byte[] bytes = texture2D.EncodeToPNG();
			string text = Path.Combine(persistentDataPath, filename + ".png");
			File.WriteAllBytes(text, bytes);
			Object.Destroy(texture2D);
			texture2D = null;
			return text;
		}

		public static Texture2D CaptureScreenshot()
		{
			return CaptureScreenshot(0f, 0f, Screen.width, Screen.height);
		}

		public static Texture2D CaptureScreenshot(float startX, float startY, float width, float height)
		{
			Texture2D texture2D = new Texture2D((int)width, (int)height, TextureFormat.RGB24, false);
			texture2D.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
			texture2D.Apply();
			return texture2D;
		}
	}
}
