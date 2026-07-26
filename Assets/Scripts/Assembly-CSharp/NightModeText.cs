using UnityEngine;
using UnityEngine.UI;

namespace LevelMode
{
	/// <summary>
	/// UI 文本昼夜着色（对齐无尽 EndlessMode.NightModeText）。
	/// 用于 Pause 遮罩下 "TAP TO CONTINUE" 等需要跟随昼夜模式变色的文案。
	/// 订阅 LevelMode.Neuron 的 NightModeSwitched 事件，与 Level 自己的 NightModeButton 联动。
	/// // TODO: [User Action] 卸掉该 Text 上挂的 EndlessMode.NightModeText，改挂本脚本（无需额外字段配置）。
	/// </summary>
	[RequireComponent(typeof(Text))]
	public sealed class NightModeText : Neuron
	{
		private Text text;

		private Color dayColor;

		private Color nightColor;

		protected override void Awake()
		{
			base.Awake();
			text = GetComponent<Text>();
			dayColor = text.color;
			// 注意：LevelMode.Utility.HexToColor 入参不带 '#'，与 Endless 版本参数格式不同
			nightColor = Utility.HexToColor("f8fff5");
			// 开局按当前存档的昼夜状态直接着色，避免先显示白天色再跳变
			OnNightModeSwitched(NightModeButton.IsOn);
		}

		protected override void OnNightModeSwitched(bool enabled)
		{
			// 保留原有透明度（淡入淡出由 UiPage/CanvasGroup 控制），只替换 RGB
			Color color = enabled ? nightColor : dayColor;
			color.a = text.color.a;
			text.color = color;
		}
	}
}
