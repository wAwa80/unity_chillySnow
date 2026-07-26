using JuiceInternal;
using UnityEngine;
using UnityEngine.UI;

namespace LevelMode
{
	/// <summary>
	/// UI 文本多语言（对齐无尽 EndlessMode.Translation）。
	/// 以 Text 组件里的英文源文案（如 "TAP TO CONTINUE"）作为 key，
	/// 通过统一的 Juice.translator（JuiceInternal.Translator）查表替换为当前语言文案。
	/// 注：JuiceInternal.Translator 自带 RuntimeInitializeOnLoadMethod(AfterSceneLoad) 会在场景加载后
	/// 自动扫描并翻译场景内全部 Text；本组件用于保证该 Text 在被无尽脚本卸载后仍显式具备多语言能力，
	/// 且可搭配 caps 还原"全大写"展示效果（与无尽 Translation 行为一致）。
	/// // TODO: [User Action] 卸掉该 Text 上挂的 EndlessMode.Translation，改挂本脚本；
	/// // "TAP TO CONTINUE" 对应场景勾选 caps=true（当前语言表 key："Tap to continue"，中文："点按以继续"）。
	/// </summary>
	[RequireComponent(typeof(Text))]
	public sealed class Translation : MonoBehaviour
	{
		private Text text;

		[SerializeField]
		private bool caps;

		private void Awake()
		{
			text = GetComponent<Text>();
			// 防御性判空：极端情况下 JuiceInternal.Translator 尚未完成初始化时，保留原文不崩溃，
			// 场景加载后 Translator 自身的全局扫描仍会补上翻译
			Translator translator = Juice.translator;
			if (translator != null)
			{
				text.text = translator.Translate(text.text);
			}
			if (caps)
			{
				text.text = text.text.ToUpper();
			}
		}
	}
}
