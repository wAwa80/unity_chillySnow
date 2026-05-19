using UnityEngine;


namespace EndlessMode
{
	public static class Device
	{
		public enum Vibration
		{
			Light,
			Medium,
			Heavy
		}

		public static bool HasInternet()
		{
			return Application.internetReachability != NetworkReachability.NotReachable;
		}

		public static void Vibrate(Vibration vibration)
		{
			if (VibrateButton.vibrateOn)
			{
				//Handheld.Vibrate();
			}
		}
	}
}
