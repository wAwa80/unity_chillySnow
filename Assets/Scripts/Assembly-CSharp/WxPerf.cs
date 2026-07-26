namespace LevelMode
{
	/// <summary>
	/// 微信小游戏性能档：Trail/粉末降载，Enabled=false 可一键回退原手感。
	/// </summary>
	public static class WxPerf
	{
		public static bool Enabled = true;

		public const float TrailTime = 2.5f;

		public const float PowderRate = 40f;

		public const float DefaultTrailTime = 5f;

		public const float DefaultPowderRate = 100f;

		public static float GetTrailTimeMax()
		{
			return Enabled ? TrailTime : DefaultTrailTime;
		}

		public static float GetPowderRate()
		{
			return Enabled ? PowderRate : DefaultPowderRate;
		}
	}
}
