using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LevelMode
{
	/// <summary>
	/// 模式 HUD 过渡：订阅 GameMode.Changed，先退后进。
	/// 单按钮循环切换 Level ↔ Endless（无面板、无双 BtnLevel/BtnEndless）。
	/// 本类禁止再次调用 Neuron.Refresh（Refresh 仅由 GameMode.TrySetMode 触发）。
	/// </summary>
	/// <remarks>
	/// 按钮视觉：子节点 Level / Endless 两个 Image 互斥显示。
	/// 局内：开滑时 Tween 移出屏外；Continue/死亡期间保持屏外；Refresh 后再滑回布局位。
	/// 缺 SerializeField 时降级瞬时改 alpha，不抛异常。
	/// </remarks>
	public sealed class UIHudTransition : Neuron
	{
		// TODO: [User Action] 请在编辑器 Inspector 中拖拽赋值下列引用
		[SerializeField]
		private CanvasGroup hudLevel;

		// TODO: [User Action] 请在编辑器 Inspector 中拖拽赋值下列引用
		[SerializeField]
		private CanvasGroup hudEndless;

		// TODO: [User Action] 请在编辑器 Inspector 中拖拽赋值下列引用
		[SerializeField]
		private RectTransform hudLevelRect;

		// TODO: [User Action] 请在编辑器 Inspector 中拖拽赋值下列引用
		[SerializeField]
		private RectTransform hudEndlessRect;

		/// <summary>
		/// 唯一模式切换按钮（如 SwitchModeBtn）；OnClick 绑 UserToggleMode。
		/// </summary>
		// TODO: [User Action] 请在编辑器 Inspector 中拖拽赋值下列引用
		[SerializeField]
		private Button modeSwitchButton;

		/// <summary>
		/// 可选：滑出/滑回移动的根（如 SwitchModeRoot）。不填则自动向上找 SwitchModeRoot 或按钮父节点。
		/// TODO: [User Action] 建议在 Inspector 显式拖拽 SwitchModeRoot，并挂 CanvasGroup
		/// </summary>
		[SerializeField]
		private RectTransform modeSwitchMotionRoot;

		/// <summary>
		/// 切换钮淡出/淡入。不填则在 motionRoot 上 GetComponent，仍无则运行时 AddComponent（仅兜底）。
		/// TODO: [User Action] 请在 SwitchModeRoot 上添加 CanvasGroup 并拖入此字段
		/// </summary>
		[SerializeField]
		private CanvasGroup modeSwitchCanvasGroup;

		/// <summary>
		/// 按钮子节点：关卡图标 Image。Current==Level 时显示，Endless 时隐藏。
		/// </summary>
		// TODO: [User Action] 请在编辑器 Inspector 中拖拽赋值：按钮下 Level 的 Image
		[SerializeField]
		private Image levelIconImage;

		/// <summary>
		/// 按钮子节点：无尽图标 Image。Current==Endless 时显示，Level 时隐藏。
		/// </summary>
		// TODO: [User Action] 请在编辑器 Inspector 中拖拽赋值：按钮下 Endless 的 Image
		[SerializeField]
		private Image endlessIconImage;

		[SerializeField]
		private float outDuration = 0.18f;

		[SerializeField]
		private float inDuration = 0.22f;

		[SerializeField]
		private AnimationCurve transitionCurve;

		[SerializeField]
		private float outOffsetY = 40f;

		[SerializeField]
		private float inOffsetY = -40f;

		/// <summary>
		/// 局内藏钮：相对布局位的偏移。左下角按钮默认左移出屏（勿用正 Y，会移进屏幕中心）。
		/// TODO: [User Action] 可选：按实际锚点微调，确保完全离开可视区
		/// </summary>
		[SerializeField]
		private Vector2 modeSwitchHideOffset = new Vector2(-500f, 0f);

		/// <summary>
		/// 切换钮滑出/滑回时长。
		/// </summary>
		[SerializeField]
		private float modeSwitchSlideDuration = 0.22f;

		private bool isTransitioning;

		private Coroutine running;

		private Coroutine modeSwitchSlide;

		private Vector2 levelLayoutPos;

		private Vector2 endlessLayoutPos;

		private bool layoutCached;

		private RectTransform modeSwitchRect;

		private Vector2 modeSwitchLayoutPos;

		private bool modeSwitchLayoutCached;

		/// <summary>true=在布局位（局外可见）；false=在屏外藏位。</summary>
		private bool modeSwitchOnScreen = true;

		protected override void Awake()
		{
			base.Awake();

			if (transitionCurve == null || transitionCurve.length == 0)
			{
				transitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
			}

			CacheLayoutPositions();
			CacheModeSwitchLayout();
			WarnMissingHudRefs();
		}

		private void OnEnable()
		{
			GameMode.Changed += OnGameModeChanged;
			ApplyHudInstant(GameMode.Current);
			ApplyModeSwitchVisual(GameMode.Current);
			// 冷启动：局外钮在布局位；若异常处于对局中则瞬时藏出
			bool showBtn = !Neuron.IsPlaying();
			SnapModeSwitch(showBtn);
			SetButtonInteractable(modeSwitchButton, showBtn && !isTransitioning);
		}

		private void OnDisable()
		{
			GameMode.Changed -= OnGameModeChanged;
			StopRunningCoroutineAndRestoreLayout();
			StopModeSwitchSlide();
			isTransitioning = false;
		}

		/// <summary>
		/// 单按钮 OnClick：Level ↔ Endless 循环切换。
		/// </summary>
		public void UserToggleMode()
		{
			if (isTransitioning)
			{
				Debug.Log("[UIHudTransition] 模式切换动画进行中，忽略");
				return;
			}

			if (Neuron.IsPlaying())
			{
				Debug.Log("[UIHudTransition] 对局中无法切换模式");
				TryVibrateLight();
				return;
			}

			GameMode.Kind target = GameMode.IsLevel ? GameMode.Kind.Endless : GameMode.Kind.Level;
			GameMode.Kind before = GameMode.Current;
			bool ok = GameMode.TrySetMode(target);
			if (!ok)
			{
				TryVibrateLight();
				return;
			}

			if (before == target)
			{
				ApplyModeSwitchVisual(GameMode.Current);
			}
		}

		/// <summary>
		/// 局外：钮可点且在屏内；局内/过渡：不可点（位置由滑出协程管）。
		/// </summary>
		public void SyncInteractableFromPlaying()
		{
			bool locked = Neuron.IsPlaying() || isTransitioning;
			SetButtonInteractable(modeSwitchButton, !locked);
		}

		protected override void OnStartRun(Run run)
		{
			// 开滑：锁点击并 Tween 移出屏幕
			SetButtonInteractable(modeSwitchButton, false);
			StartModeSwitchSlide(visibleOnScreen: false, animate: true);
		}

		protected override void OnEndRun()
		{
			// Continue 弹出期间 IsPlaying 仍 true：保持屏外，禁止滑回
			SetButtonInteractable(modeSwitchButton, false);
			StartModeSwitchSlide(visibleOnScreen: false, animate: false);
		}

		protected override void OnRefresh()
		{
			ApplyModeSwitchVisual(GameMode.Current);
			// 重置回局外：再滑回布局位后解锁
			StartModeSwitchSlide(visibleOnScreen: true, animate: true);
		}

		private void OnGameModeChanged(GameMode.Kind kind)
		{
			StopRunningCoroutineAndRestoreLayout();
			running = StartCoroutine(TransitionRoutine(kind));
		}

		private IEnumerator TransitionRoutine(GameMode.Kind kind)
		{
			isTransitioning = true;
			SyncInteractableFromPlaying();
			SetHudRaycasts(false);

			bool hasAnim = hudLevel != null && hudEndless != null && hudLevelRect != null && hudEndlessRect != null;
			if (!hasAnim)
			{
				Debug.LogWarning("[UIHudTransition] 缺少 hudLevel/hudEndless 引用，降级为瞬时切换");
				ApplyHudInstant(kind);
				ApplyModeSwitchVisual(kind);
				isTransitioning = false;
				SetHudRaycastsForMode(kind);
				SyncInteractableFromPlaying();
				running = null;
				yield break;
			}

			CanvasGroup outgoing = kind == GameMode.Kind.Endless ? hudLevel : hudEndless;
			RectTransform outgoingRect = kind == GameMode.Kind.Endless ? hudLevelRect : hudEndlessRect;
			Vector2 outgoingBase = kind == GameMode.Kind.Endless ? levelLayoutPos : endlessLayoutPos;

			CanvasGroup incoming = kind == GameMode.Kind.Endless ? hudEndless : hudLevel;
			RectTransform incomingRect = kind == GameMode.Kind.Endless ? hudEndlessRect : hudLevelRect;
			Vector2 incomingBase = kind == GameMode.Kind.Endless ? endlessLayoutPos : levelLayoutPos;

			yield return AnimateHud(outgoing, outgoingRect, outgoingBase, outgoingBase + new Vector2(0f, outOffsetY), 1f, 0f, outDuration);

			outgoing.interactable = false;
			outgoing.blocksRaycasts = false;
			outgoingRect.anchoredPosition = outgoingBase;

			incomingRect.anchoredPosition = incomingBase + new Vector2(0f, inOffsetY);
			incoming.alpha = 0f;
			yield return AnimateHud(incoming, incomingRect, incomingBase + new Vector2(0f, inOffsetY), incomingBase, 0f, 1f, inDuration);

			ApplyModeSwitchVisual(kind);
			SetHudRaycastsForMode(kind);

			isTransitioning = false;
			SyncInteractableFromPlaying();
			running = null;
		}

		private IEnumerator AnimateHud(CanvasGroup group, RectTransform rect, Vector2 fromPos, Vector2 toPos, float fromAlpha, float toAlpha, float duration)
		{
			if (group == null || rect == null)
			{
				yield break;
			}
			float safeDuration = Mathf.Max(0.01f, duration);
			float t = 0f;
			while (t < 1f)
			{
				t += Time.unscaledDeltaTime / safeDuration;
				float u = transitionCurve.Evaluate(Mathf.Clamp01(t));
				rect.anchoredPosition = Vector2.LerpUnclamped(fromPos, toPos, u);
				group.alpha = Mathf.Lerp(fromAlpha, toAlpha, u);
				yield return null;
			}
			rect.anchoredPosition = toPos;
			group.alpha = toAlpha;
		}

		private void ApplyHudInstant(GameMode.Kind kind)
		{
			bool levelOn = kind == GameMode.Kind.Level;
			if (hudLevel != null)
			{
				hudLevel.alpha = levelOn ? 1f : 0f;
				hudLevel.interactable = levelOn;
				hudLevel.blocksRaycasts = levelOn;
			}
			if (hudEndless != null)
			{
				hudEndless.alpha = levelOn ? 0f : 1f;
				hudEndless.interactable = !levelOn;
				hudEndless.blocksRaycasts = !levelOn;
			}
			if (layoutCached)
			{
				if (hudLevelRect != null)
				{
					hudLevelRect.anchoredPosition = levelLayoutPos;
				}
				if (hudEndlessRect != null)
				{
					hudEndlessRect.anchoredPosition = endlessLayoutPos;
				}
			}
		}

		private void ApplyModeSwitchVisual(GameMode.Kind kind)
		{
			bool levelOn = kind == GameMode.Kind.Level;
			if (levelIconImage != null)
			{
				levelIconImage.enabled = levelOn;
				if (levelIconImage.gameObject != null)
				{
					levelIconImage.gameObject.SetActive(levelOn);
				}
			}
			if (endlessIconImage != null)
			{
				endlessIconImage.enabled = !levelOn;
				if (endlessIconImage.gameObject != null)
				{
					endlessIconImage.gameObject.SetActive(!levelOn);
				}
			}
		}

		/// <summary>
		/// 滑出屏外 / 滑回布局位（无 DOTween：协程 + 曲线）。
		/// </summary>
		private void StartModeSwitchSlide(bool visibleOnScreen, bool animate)
		{
			if (!modeSwitchLayoutCached || modeSwitchRect == null)
			{
				return;
			}

			StopModeSwitchSlide();

			if (!animate)
			{
				SnapModeSwitch(visibleOnScreen);
				if (visibleOnScreen && !Neuron.IsPlaying() && !isTransitioning)
				{
					SetButtonInteractable(modeSwitchButton, true);
				}
				return;
			}

			modeSwitchSlide = StartCoroutine(ModeSwitchSlideRoutine(visibleOnScreen));
		}

		private IEnumerator ModeSwitchSlideRoutine(bool visibleOnScreen)
		{
			Vector2 hideOffset = ResolveModeSwitchHideOffset();
			Vector2 from = modeSwitchRect.anchoredPosition;
			Vector2 to = visibleOnScreen ? modeSwitchLayoutPos : modeSwitchLayoutPos + hideOffset;
			CanvasGroup fadeGroup = ResolveModeSwitchCanvasGroup();
			float fromAlpha = fadeGroup != null ? fadeGroup.alpha : 1f;
			float toAlpha = visibleOnScreen ? 1f : 0f;
			float safeDuration = Mathf.Max(0.01f, modeSwitchSlideDuration);
			float t = 0f;
			while (t < 1f)
			{
				t += Time.unscaledDeltaTime / safeDuration;
				float u = transitionCurve.Evaluate(Mathf.Clamp01(t));
				modeSwitchRect.anchoredPosition = Vector2.LerpUnclamped(from, to, u);
				if (fadeGroup != null)
				{
					fadeGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, u);
				}
				yield return null;
			}
			modeSwitchRect.anchoredPosition = to;
			if (fadeGroup != null)
			{
				fadeGroup.alpha = toAlpha;
			}
			modeSwitchOnScreen = visibleOnScreen;
			modeSwitchSlide = null;

			// 仅滑回局外后解锁；滑出保持不可点
			if (visibleOnScreen && !Neuron.IsPlaying() && !isTransitioning)
			{
				SetButtonInteractable(modeSwitchButton, true);
			}
			else
			{
				SetButtonInteractable(modeSwitchButton, false);
			}
		}

		private void SnapModeSwitch(bool visibleOnScreen)
		{
			if (!modeSwitchLayoutCached || modeSwitchRect == null)
			{
				return;
			}
			Vector2 hideOffset = ResolveModeSwitchHideOffset();
			modeSwitchRect.anchoredPosition = visibleOnScreen
				? modeSwitchLayoutPos
				: modeSwitchLayoutPos + hideOffset;
			CanvasGroup fadeGroup = ResolveModeSwitchCanvasGroup();
			if (fadeGroup != null)
			{
				fadeGroup.alpha = visibleOnScreen ? 1f : 0f;
			}
			modeSwitchOnScreen = visibleOnScreen;
		}

		private void StopModeSwitchSlide()
		{
			if (modeSwitchSlide != null)
			{
				StopCoroutine(modeSwitchSlide);
				modeSwitchSlide = null;
			}
		}

		private void CacheModeSwitchLayout()
		{
			modeSwitchRect = null;
			modeSwitchLayoutCached = false;
			modeSwitchRect = ResolveModeSwitchMotionRoot();
			if (modeSwitchRect == null)
			{
				return;
			}
			modeSwitchLayoutPos = modeSwitchRect.anchoredPosition;
			modeSwitchLayoutCached = true;
		}

		/// <summary>
		/// 运动根优先级：Inspector 绑定 → 祖先 SwitchModeRoot → 按钮父 Rect → 按钮自身。
		/// </summary>
		private RectTransform ResolveModeSwitchMotionRoot()
		{
			if (modeSwitchMotionRoot != null)
			{
				return modeSwitchMotionRoot;
			}
			if (modeSwitchButton == null)
			{
				return null;
			}
			Transform cursor = modeSwitchButton.transform.parent;
			while (cursor != null)
			{
				if (cursor.name.IndexOf("SwitchModeRoot", System.StringComparison.OrdinalIgnoreCase) >= 0)
				{
					RectTransform namedRoot = cursor as RectTransform;
					if (namedRoot != null)
					{
						return namedRoot;
					}
				}
				cursor = cursor.parent;
			}
			RectTransform parentRect = modeSwitchButton.transform.parent as RectTransform;
			if (parentRect != null)
			{
				return parentRect;
			}
			return modeSwitchButton.transform as RectTransform;
		}

		/// <summary>
		/// 左锚点且 Inspector 偏移几乎无水平分量时，自动左移出屏（覆盖场景里误填的 y:280）。
		/// </summary>
		private Vector2 ResolveModeSwitchHideOffset()
		{
			if (modeSwitchRect == null)
			{
				return modeSwitchHideOffset;
			}
			bool leftAnchored = modeSwitchRect.anchorMin.x < 0.01f && modeSwitchRect.anchorMax.x < 0.01f;
			if (leftAnchored && Mathf.Abs(modeSwitchHideOffset.x) < 50f)
			{
				float scaleX = Mathf.Abs(modeSwitchRect.lossyScale.x);
				float width = modeSwitchRect.rect.width * scaleX;
				float offX = -(width + 120f);
				return new Vector2(offX, modeSwitchHideOffset.y);
			}
			return modeSwitchHideOffset;
		}

		private CanvasGroup ResolveModeSwitchCanvasGroup()
		{
			if (modeSwitchCanvasGroup != null)
			{
				return modeSwitchCanvasGroup;
			}
			if (modeSwitchRect == null)
			{
				return null;
			}
			modeSwitchCanvasGroup = modeSwitchRect.GetComponent<CanvasGroup>();
			if (modeSwitchCanvasGroup == null)
			{
				modeSwitchCanvasGroup = modeSwitchRect.gameObject.AddComponent<CanvasGroup>();
				Debug.LogWarning("[UIHudTransition] SwitchModeRoot 缺少 CanvasGroup，已运行时 AddComponent；建议在 Inspector 预先挂载并拖入 modeSwitchCanvasGroup");
			}
			return modeSwitchCanvasGroup;
		}

		private void SetHudRaycasts(bool enabled)
		{
			if (hudLevel != null)
			{
				hudLevel.blocksRaycasts = enabled;
				hudLevel.interactable = enabled;
			}
			if (hudEndless != null)
			{
				hudEndless.blocksRaycasts = enabled;
				hudEndless.interactable = enabled;
			}
		}

		private void SetHudRaycastsForMode(GameMode.Kind kind)
		{
			bool levelOn = kind == GameMode.Kind.Level;
			if (hudLevel != null)
			{
				hudLevel.blocksRaycasts = levelOn;
				hudLevel.interactable = levelOn;
			}
			if (hudEndless != null)
			{
				hudEndless.blocksRaycasts = !levelOn;
				hudEndless.interactable = !levelOn;
			}
		}

		private void CacheLayoutPositions()
		{
			if (hudLevelRect != null)
			{
				levelLayoutPos = hudLevelRect.anchoredPosition;
			}
			if (hudEndlessRect != null)
			{
				endlessLayoutPos = hudEndlessRect.anchoredPosition;
			}
			layoutCached = hudLevelRect != null || hudEndlessRect != null;
		}

		private void WarnMissingHudRefs()
		{
			if (hudLevel == null || hudEndless == null || hudLevelRect == null || hudEndlessRect == null)
			{
				Debug.LogWarning("[UIHudTransition] 缺少 hudLevel/hudEndless 引用");
			}
			if (modeSwitchButton == null)
			{
				Debug.LogWarning("[UIHudTransition] 缺少 modeSwitchButton 引用");
			}
		}

		private void StopRunningCoroutineAndRestoreLayout()
		{
			if (running != null)
			{
				StopCoroutine(running);
				running = null;
			}
			RestoreLayoutPositions();
		}

		private void RestoreLayoutPositions()
		{
			if (!layoutCached)
			{
				return;
			}
			if (hudLevelRect != null)
			{
				hudLevelRect.anchoredPosition = levelLayoutPos;
			}
			if (hudEndlessRect != null)
			{
				hudEndlessRect.anchoredPosition = endlessLayoutPos;
			}
		}

		private static void SetButtonInteractable(Button button, bool interactable)
		{
			if (button != null)
			{
				button.interactable = interactable;
			}
		}

		private static void TryVibrateLight()
		{
			try
			{
#if UNITY_ANDROID || UNITY_IOS
				Handheld.Vibrate();
#endif
			}
			catch
			{
				// 忽略：部分小游戏基础库无震动权限
			}
		}
	}
}
