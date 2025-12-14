using UnityEngine;

namespace EasyMobile
{
	internal static class AndroidUtil
	{
		internal static void CallJavaStaticMethod(string className, string method, params object[] args)
		{
			AndroidJavaObject @static;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				@static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			}
			AndroidJavaClass targetClass = new AndroidJavaClass(className);
			@static.Call("runOnUiThread", (AndroidJavaRunnable)delegate
			{
				targetClass.CallStatic(method, args);
				targetClass.Dispose();
			});
		}
	}
}
