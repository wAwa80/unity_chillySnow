using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LevelMode
{
	/// <summary>
	/// 局外昼夜切换：存档 + 广播 NightModeSwitched。子层级 Both/On/Off（对齐无尽）。
	/// On=太阳（当前为夜），Off=月亮（当前为日，可切夜）。
	///
	/// 自动挂载：场景加载后若节点 "NightMode" 上没有本脚本，自动挂上并清除 EndlessMode 同名脚本。
	/// // TODO: [User Action] 确保场景中 NightMode 节点存在；非法 .meta 请删后由 Unity 重建（32位十六进制 GUID）。
	/// </summary>
	[DefaultExecutionOrder(-150)]
	public sealed class NightModeButton : SingletonButton<NightModeButton>
	{
		private const string NightModeKey = "nightModeOn";

		private Image onIcon;

		private Image offIcon;

		public static bool IsOn { get; private set; }

		protected override void Awake()
		{
			// 先于 NightModeImage / GameCamera 等订阅者 Awake：从存档恢复静态 IsOn
			IsOn = Data.LoadBool(NightModeKey, false);
			base.Awake();
			// childTransform = Both；On/Off 为其子节点（与 Endless 一致：0=On 太阳，1=Off 月亮）
			if (childTransform != null && childTransform.childCount >= 2)
			{
				onIcon = childTransform.GetChild(0).GetComponent<Image>();
				offIcon = childTransform.GetChild(1).GetComponent<Image>();
				// 子 Icon 不参与射线，由根 Button Image 拦截（避免穿透 FingerPage）
				if (onIcon != null) onIcon.raycastTarget = false;
				if (offIcon != null) offIcon.raycastTarget = false;
			}
			else
			{
				Debug.LogWarning("[NightModeButton] Both/On/Off 子层级不完整，childCount=" +
					(childTransform != null ? childTransform.childCount : -1));
			}
			SyncIcons();
			// 先打开 Menu 射线，再 Show 按钮（否则父 CanvasGroup 会吞掉点击）
			EnsureMenuHudClickable();
			Show();
			Debug.Log($"[NightModeButton] Awake 完成 IsOn={IsOn} image.enabled={(image != null && image.enabled)}");
		}

		private void Start()
		{
			EnsureMenuHudClickable();
			// 初始广播一次（含 false 白天状态），确保所有订阅者与存档值同步
			Neuron.NightModeSwitched(IsOn);
			Show();
			Debug.Log($"[NightModeButton] Start 广播 NightModeSwitched({IsOn})");
		}

		/// <summary>
		/// 打开 Menu 的 HUD 点击（卸掉 Endless MenuPage 后 CanvasGroup 会卡在不可点）。
		/// 逻辑内联于此，避免独立脚本因非法 .meta 被 Unity 排除编译。
		/// </summary>
		public static void EnsureMenuHudClickable()
		{
			GameObject menu = GameObject.Find("Menu");
			if (menu == null)
			{
				return;
			}
			CanvasGroup group = menu.GetComponent<CanvasGroup>();
			if (group != null)
			{
				group.blocksRaycasts = true;
				group.interactable = true;
			}
			Image menuImage = menu.GetComponent<Image>();
			if (menuImage != null)
			{
				menuImage.raycastTarget = false;
			}
			SettingsBar settings = SettingsBar.GetInstance();
			if (settings != null)
			{
				settings.transform.SetAsLastSibling();
				settings.Show();
			}
		}

		protected override void OnClick()
		{
			Debug.Log($"[NightModeButton] OnClick 当前 IsOn={IsOn} → 切换");
			SwitchMode();
		}

		private void SwitchMode()
		{
			IsOn = !IsOn;
			Data.SaveBool(NightModeKey, IsOn);
			Data.Save();
			SyncIcons();
			Neuron.NightModeSwitched(IsOn);
			Debug.Log($"[NightModeButton] SwitchMode 完成 IsOn={IsOn}");
		}

		private void SyncIcons()
		{
			if (onIcon != null) onIcon.enabled = IsOn;
			if (offIcon != null) offIcon.enabled = !IsOn;
		}

		protected override void OnRefresh()
		{
			EnsureMenuHudClickable();
			Show();
		}

		protected override void OnStartRun(Run run)
		{
			Debug.Log("[NightModeButton] OnStartRun → Hide");
			Hide();
		}

		// ─────────────────────────────────────────────────────────────────
		// 自动挂载：解决 .meta GUID 非法导致 Inspector 无法拖拽的问题。
		// ─────────────────────────────────────────────────────────────────
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void RegisterAutoAttach()
		{
			Debug.Log("[NightModeButton] RegisterAutoAttach（BeforeSceneLoad）");
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			Debug.Log($"[NightModeButton] OnSceneLoaded scene={scene.name}");
			if (!scene.name.Contains("Level"))
			{
				Debug.Log("[NightModeButton] 非 Level 场景，跳过自动挂载");
				return;
			}
			if (i != null)
			{
				Debug.Log("[NightModeButton] 已有实例，跳过自动挂载");
				return;
			}

			GameObject go = GameObject.Find("NightMode");
			if (go == null)
			{
				Debug.LogError("[NightModeButton] 找不到名为 NightMode 的节点，无法自动挂载！");
				return;
			}

			try
			{
				RemoveEndlessComponents(go);
				go.AddComponent<NightModeButton>();
				Debug.Log("[NightModeButton] 自动挂载成功 → " + go.name);
			}
			catch (System.Exception e)
			{
				Debug.LogError("[NightModeButton] 自动挂载异常: " + e);
			}
		}

		/// <summary>立即移除 EndlessMode 组件（DestroyImmediate，避免同帧双脚本并存）。</summary>
		private static void RemoveEndlessComponents(GameObject go)
		{
			MonoBehaviour[] comps = go.GetComponents<MonoBehaviour>();
			for (int idx = 0; idx < comps.Length; idx++)
			{
				MonoBehaviour comp = comps[idx];
				if (comp != null && comp.GetType().FullName != null &&
					comp.GetType().FullName.StartsWith("EndlessMode."))
				{
					Debug.Log("[NightModeButton] DestroyImmediate Endless 组件: " + comp.GetType().FullName);
					Object.DestroyImmediate(comp);
				}
			}
		}
	}
}
