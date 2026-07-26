using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LevelMode
{
	/// <summary>
	/// UI Image 昼夜着色（Menu/Fade 全屏遮罩；Pause Banner/Fill 等跟夜色变色）。
	/// dayColor 在 Awake 时从 Image.color 读取；nightColor 在 Inspector 设置或使用默认值。
	/// 
	/// 自动挂载：在 "Fade" 节点上自动附加（遮罩主节点）。
	/// Banner/Fill 等可手动挂或由 Inspector 拖拽。
	/// // TODO: [User Action] 若需要 Banner/Fill 也跟夜色，在 Inspector 手动挂 NightModeImage。
	/// </summary>
	[RequireComponent(typeof(Image))]
	public sealed class NightModeImage : Neuron
	{
		private Image image;

		private Color dayColor;

		[SerializeField]
		private Color nightColor = new Color(0.024f, 0.004f, 0.039f, 0.5f);

		protected override void Awake()
		{
			base.Awake();
			image = GetComponent<Image>();
			dayColor = image.color;
			// 初始读取存档状态，确保开场颜色正确（NightModeButton [DefaultExecutionOrder(-150)] 先于本脚本 Awake）
			Apply(NightModeButton.IsOn);
		}

		protected override void OnNightModeSwitched(bool enabled)
		{
			Apply(enabled);
		}

		private void Apply(bool night)
		{
			if (image == null) return;
			image.color = night ? nightColor : dayColor;
		}

		// ─────────────────────────────────────────────────────────────────
		// 自动挂载：在 "Fade" 节点上自动挂 NightModeImage（主全屏夜色遮罩）。
		// Banner/Fill 等有需要可手动挂，代码不强制。
		// ─────────────────────────────────────────────────────────────────
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void RegisterAutoAttach()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (!scene.name.Contains("Level")) return;
			// 在名为 "Fade" 的节点上自动挂（每个节点独立一个 NightModeImage 实例）
			AutoAttachToNode("Fade");
		}

		private static void AutoAttachToNode(string nodeName)
		{
			GameObject go = GameObject.Find(nodeName);
			if (go == null) return;

			// 已有 LevelMode.NightModeImage 则跳过（避免重复）
			if (go.GetComponent<NightModeImage>() != null) return;

			// 需要有 Image 组件（[RequireComponent] 会自动补）
			RemoveEndlessComponents(go);
			go.AddComponent<NightModeImage>();
			Debug.Log($"[LevelMode] NightModeImage 自动挂载到节点: {nodeName}");
		}

		private static void RemoveEndlessComponents(GameObject go)
		{
			foreach (MonoBehaviour comp in go.GetComponents<MonoBehaviour>())
			{
				if (comp != null && comp.GetType().FullName != null &&
					comp.GetType().FullName.StartsWith("EndlessMode."))
				{
					Object.Destroy(comp);
				}
			}
		}
	}
}
