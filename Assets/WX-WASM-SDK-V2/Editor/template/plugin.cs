using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using WeChatWASM;

/// <summary>
/// 微信小游戏构建模板钩子：每次「生成并转换」后关掉 loading 页底部 Unity icon。
/// 官方约定路径：Assets/WX-WASM-SDK-V2/Editor/template/plugin.cs
/// 注意：这与 Player Settings → WebGL Template（Default/Minimal/PWA）无关，无需在 Resolution 里选择。
/// </summary>
public class WxMinigameTemplatePlugin : LifeCycleBase
{
	/// <summary>
	/// 标准模板拷贝到导出目录之后、自定义 template 覆盖之前：改写产物 game.js。
	/// （官方文档样例钩子；此后 template 只覆盖 images，不会把 game.js 改回去。）
	/// </summary>
	public override void afterCopyDefault()
	{
		string gameJsPath = Path.Combine(BuildTemplateHelper.DstMinigameDir, "game.js");
		if (!File.Exists(gameJsPath))
		{
			Debug.LogWarning("[WxMinigameTemplate] 未找到导出 game.js，跳过关闭 icon：" + gameJsPath);
			return;
		}

		string content = File.ReadAllText(gameJsPath);
		string original = content;

		// iconConfig.visible: true → false（关掉底部 Unity logo）
		content = Regex.Replace(
			content,
			@"(iconConfig:\s*\{\s*visible:\s*)true",
			"${1}false",
			RegexOptions.Multiline);

		// 同步注释 materialConfig.iconImage（与手动改 Build 的意图一致）
		content = Regex.Replace(
			content,
			@"^(\s*)iconImage:\s*'images/unity_logo\.png',\s*//.*$",
			"${1}//iconImage: 'images/unity_logo.png', // icon图片，一般不更换",
			RegexOptions.Multiline);

		if (content == original)
		{
			Debug.LogWarning("[WxMinigameTemplate] game.js 未匹配到 iconConfig/iconImage，请检查 SDK 模板是否变更。");
			return;
		}

		File.WriteAllText(gameJsPath, content);
		Debug.Log("[WxMinigameTemplate] 已关闭 loading icon（iconConfig.visible=false）。");
	}
}
