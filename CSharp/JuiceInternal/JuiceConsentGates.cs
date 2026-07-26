namespace JuiceInternal
{
	/// <summary>
	/// 集中管理 GDPR / AppRater / Premium 等可选商业化功能的开关。
	/// 修改下方常量即可一键启用或关闭对应能力；默认全部关闭（不弹窗、不实例化相关 UI）。
	/// </summary>
	public static class JuiceConsentGates
	{
		/// <summary>
		/// 隐私同意弹窗（SYSTEM_OBJECT GDPR）及设置页 GDPR 入口按钮。
		/// 关闭时不创建弹窗，并视为已同意条款，以免 Ads/Analytics 因默认 termsConsent=false 而无法初始化。
		/// </summary>
		public const bool EnableGdpr = false;

		/// <summary>
		/// 应用内评分（PaperPlaneTools RateBox）。关闭时不初始化 RateBox、不弹出评分框。
		/// </summary>
		public const bool EnableAppRater = false;

		/// <summary>
		/// 高级版购买入口（场景 PremiumButton）与 DealPopup 促销弹窗。
		/// 关闭时不实例化 DealPopup、不触发购买与促销逻辑；Premium 模块仍会初始化以供 Ads 查询 IsPremium。
		/// </summary>
		public const bool EnablePremium = false;
	}
}
