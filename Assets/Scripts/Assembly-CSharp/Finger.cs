using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LevelMode
{

	public sealed class Finger : Essential<Finger>
	{
		private class FingerPage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
		{
			public void OnPointerDown(PointerEventData data)
			{
				// 暂停中忽略，避免与 PausePage / 开局 Tap 冲突
				if (Neuron.IsPaused())
				{
					Debug.Log("[Finger] OnPointerDown 忽略：已暂停");
					return;
				}
				// 点在 NightMode / Pause 矩形内：不开滑；若 Button 因父级 CanvasGroup 未收到事件则兜底 Invoke
				if (TryHandleHudButtonFallback(data))
				{
					Debug.Log("[Finger] OnPointerDown 拦截：HUD 按钮区域（已尝试兜底点击），不 Tap");
					return;
				}
				if (IsPointerOverBlockingUi(data))
				{
					Debug.Log("[Finger] OnPointerDown 拦截：Raycast 命中阻挡 UI，不 Tap");
					return;
				}
				Debug.Log("[Finger] OnPointerDown → Neuron.Tap()（将开滑）");
				pressing = true;
				Neuron.Tap();
			}

			public void OnPointerUp(PointerEventData data)
			{
				pressing = false;
			}
		}

		private static bool pressing;

		private bool cannotLaunch;

		private bool isInGame;

		protected override void Awake()
		{
			base.Awake();
			SceneManager.sceneLoaded += OnSceneLoaded;
			EnsureSingleEventSystem();
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			EnsureSingleEventSystem();
			if (scene.name.Contains("Level"))
			{
				NightModeButton.EnsureMenuHudClickable();
			}
			GameObject gameObject = GameObject.Find("Canvas");
			if (gameObject != null)
			{
				FingerPage component = new GameObject("FingerPage", typeof(FingerPage), typeof(RectTransform), typeof(Image)).GetComponent<FingerPage>();
				component.transform.SetParent(gameObject.transform);
				component.transform.SetAsFirstSibling();
				RectTransform component2 = component.GetComponent<RectTransform>();
				component2.anchorMin = Vector2.zero;
				component2.anchorMax = Vector2.one;
				component2.sizeDelta = Vector2.one;
				component2.anchoredPosition = Vector2.zero;
				component2.localPosition = Vector3.zero;
				component2.localScale = Vector3.one;
				component2.localRotation = Quaternion.identity;
				component.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
				// FingerPage 仅作兜底接收「空白区」点击；热区由上层 UI 拦截
				Image fingerImage = component.GetComponent<Image>();
				if (fingerImage != null)
				{
					fingerImage.raycastTarget = true;
				}
				Debug.Log($"[Finger] FingerPage 已创建，场景={scene.name}");
			}
		}

		/// <summary>
		/// 关卡场景与 GDPR DontDestroy 常叠两个 EventSystem，日志每帧刷警告。
		/// </summary>
		private static void EnsureSingleEventSystem()
		{
			EventSystem[] systems = Object.FindObjectsOfType<EventSystem>();
			if (systems == null || systems.Length <= 1)
			{
				return;
			}
			for (int i = 1; i < systems.Length; i++)
			{
				if (systems[i] != null)
				{
					Object.Destroy(systems[i].gameObject);
				}
			}
		}

		public static bool IsPressing()
		{
			if (Neuron.IsPaused())
			{
				return false;
			}
			return pressing;
		}

		protected override void OnRefresh()
		{
			cannotLaunch = false;
			isInGame = false;
		}

		protected override void OnContinue()
		{
			cannotLaunch = false;
		}

		protected override void OnTap()
		{
			if (!cannotLaunch)
			{
				cannotLaunch = true;
				Debug.Log($"[Finger] OnTap → StartRun，isInGame={isInGame}");
				Neuron.StartRun((!isInGame) ? Run.GetDefault() : Neuron.GetCurrentRun());
				isInGame = true;
			}
		}

		/// <summary>
		/// 点在 NightMode/Pause/Settings 矩形内则拦截开滑，并兜底触发对应按钮。
		/// </summary>
		private static bool TryHandleHudButtonFallback(PointerEventData data)
		{
			if (TryInvokeIfOver(NightModeButton.i, data))
			{
				return true;
			}
			if (TryInvokeIfOver(PauseButton.i, data))
			{
				return true;
			}
			// SettingsBar：挡住开滑；若与 Button.onClick 同帧双触发，由 SettingsBar 内防抖抵消一次
			SettingsBar settings = SettingsBar.GetInstance();
			if (settings != null)
			{
				RectTransform settingsRect = settings.GetButtonRect();
				if (settingsRect != null &&
					RectTransformUtility.RectangleContainsScreenPoint(settingsRect, data.position, GetUiCamera(settingsRect)))
				{
					Debug.Log("[Finger] HUD 兜底 SettingsBar.ToggleExpand");
					settings.ToggleExpandFromPointer();
					return true;
				}
			}
			return false;
		}

		private static Camera GetUiCamera(RectTransform rt)
		{
			Canvas canvas = rt.GetComponentInParent<Canvas>();
			if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
			{
				return canvas.worldCamera;
			}
			return null;
		}

		private static bool TryInvokeIfOver(Component hud, PointerEventData data)
		{
			if (!IsScreenPointOverComponent(hud, data))
			{
				return false;
			}
			// 先确保 Menu 可点，再兜底触发（避免只拦 Finger、按钮自己永远收不到）
			NightModeButton.EnsureMenuHudClickable();
			Button button = hud.GetComponent<Button>();
			if (button != null && button.enabled)
			{
				Debug.Log("[Finger] HUD 兜底 Invoke: " + hud.GetType().Name);
				button.onClick.Invoke();
			}
			return true;
		}

		private static bool IsScreenPointOverComponent(Component comp, PointerEventData data)
		{
			if (comp == null)
			{
				return false;
			}
			RectTransform rt = comp.transform as RectTransform;
			if (rt == null)
			{
				return false;
			}
			Canvas canvas = comp.GetComponentInParent<Canvas>();
			Camera cam = null;
			if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
			{
				cam = canvas.worldCamera;
			}
			return RectTransformUtility.RectangleContainsScreenPoint(rt, data.position, cam);
		}

		/// <summary>
		/// GraphicRaycaster 命中非 FingerPage 的可交互 UI 时，不视为「点屏开滑」。
		/// </summary>
		private static bool IsPointerOverBlockingUi(PointerEventData data)
		{
			EventSystem eventSystem = EventSystem.current;
			if (eventSystem == null)
			{
				Debug.LogWarning("[Finger] EventSystem.current == null");
				return false;
			}
			List<RaycastResult> results = new List<RaycastResult>();
			eventSystem.RaycastAll(data, results);
			StringBuilder sb = new StringBuilder();
			sb.Append("[Finger] RaycastAll 命中数=").Append(results.Count);
			for (int i = 0; i < results.Count; i++)
			{
				GameObject hit = results[i].gameObject;
				sb.Append(" | ").Append(hit != null ? GetHierarchyPath(hit.transform) : "null");
				if (hit == null || hit.name == "FingerPage")
				{
					continue;
				}
				// 按名字硬拦：NightMode / Pause 整棵子树
				if (IsUnderNamedAncestor(hit.transform, "NightMode") || IsUnderNamedAncestor(hit.transform, "Pause"))
				{
					Debug.Log(sb.ToString() + " → 按名称拦截");
					return true;
				}
				// Button / Toggle 等可交互控件
				Selectable selectable = hit.GetComponentInParent<Selectable>();
				if (selectable != null && selectable.enabled && selectable.IsInteractable())
				{
					Debug.Log(sb.ToString() + " → Selectable 拦截: " + selectable.name);
					return true;
				}
				// 其它显式挡射线的 Graphic（PausePage 全屏遮罩等）
				Graphic graphic = hit.GetComponent<Graphic>();
				if (graphic != null && graphic.enabled && graphic.raycastTarget && IsGraphicBlockingRaycast(graphic))
				{
					Debug.Log(sb.ToString() + " → Graphic 拦截: " + graphic.name);
					return true;
				}
			}
			Debug.Log(sb.ToString() + " → 无阻挡，允许 Tap");
			return false;
		}

		private static bool IsUnderNamedAncestor(Transform t, string ancestorName)
		{
			while (t != null)
			{
				if (t.name == ancestorName)
				{
					return true;
				}
				t = t.parent;
			}
			return false;
		}

		private static string GetHierarchyPath(Transform t)
		{
			string path = t.name;
			while (t.parent != null)
			{
				t = t.parent;
				path = t.name + "/" + path;
			}
			return path;
		}

		/// <summary>沿父链检查 CanvasGroup 是否允许挡射线。</summary>
		private static bool IsGraphicBlockingRaycast(Graphic graphic)
		{
			Transform cursor = graphic.transform;
			while (cursor != null)
			{
				if (cursor.name == "FingerPage")
				{
					return false;
				}
				CanvasGroup group = cursor.GetComponent<CanvasGroup>();
				if (group != null && (!group.blocksRaycasts || group.alpha <= 0.01f))
				{
					return false;
				}
				cursor = cursor.parent;
			}
			return true;
		}
	}
}
