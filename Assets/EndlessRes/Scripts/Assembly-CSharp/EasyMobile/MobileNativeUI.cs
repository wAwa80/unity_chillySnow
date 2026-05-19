namespace EasyMobile
{
	public static class MobileNativeUI
	{
		public static MobileNativeAlert ShowThreeButtonAlert(string title, string message, string button1, string button2, string button3)
		{
			return MobileNativeAlert.ShowThreeButtonAlert(title, message, button1, button2, button3);
		}

		public static MobileNativeAlert ShowTwoButtonAlert(string title, string message, string button1, string button2)
		{
			return MobileNativeAlert.ShowTwoButtonAlert(title, message, button1, button2);
		}

		public static MobileNativeAlert Alert(string title, string message, string button)
		{
			return MobileNativeAlert.ShowOneButtonAlert(title, message, button);
		}

		public static MobileNativeAlert Alert(string title, string message)
		{
			return MobileNativeAlert.Alert(title, message);
		}

		public static void ShowToast(string message, bool isLongToast = false)
		{
			MobileNativeAlert.ShowToast(message, isLongToast);
		}
	}
}
