namespace EasyMobile
{
	public class BannerAdSize
	{
		public static readonly BannerAdSize Banner = new BannerAdSize(320, 50);

		public static readonly BannerAdSize MediumRectangle = new BannerAdSize(300, 250);

		public static readonly BannerAdSize IABBanner = new BannerAdSize(468, 60);

		public static readonly BannerAdSize Leaderboard = new BannerAdSize(728, 90);

		public static readonly BannerAdSize SmartBanner = new BannerAdSize(isSmartBanner: true);

		public bool IsSmartBanner { get; private set; }

		public int Width { get; private set; }

		public int Height { get; private set; }

		public BannerAdSize(int width, int height)
		{
			IsSmartBanner = false;
			Width = width;
			Height = height;
		}

		private BannerAdSize(bool isSmartBanner)
		{
			IsSmartBanner = isSmartBanner;
			Width = 0;
			Height = 0;
		}
	}
}
