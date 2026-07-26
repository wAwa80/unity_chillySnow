using UnityEngine;

namespace LevelMode
{

	[RequireComponent(typeof(SpriteRenderer), typeof(AudioSource))]
	public sealed class Pine : Recyclable<Pine>
	{
		// 与 EndlessRes/Resources/Pine.prefab 区分，避免 Resources.Load 同名冲突。
		// TODO: [User Action] 请在 Unity 中将 Assets/Resources/Pine.prefab 重命名为 LevelPine.prefab
		static Pine()
		{
			RegisterResourceName("LevelPine");
		}

		private const float CULL_AHEAD = 14f;

		private const float CULL_BEHIND = 8f;

		private SpriteRenderer spriteRenderer;

		private AudioSource source;

		private SpriteRenderer leaves;

		private SpriteRenderer shadow;

		/// <summary>
		/// 月光投影；缺 Prefab 时 null。
		/// // TODO: [User Action] 从 EndlessRes/Resources/Pine.prefab 复制 NightShadow 子树到 LevelPine.prefab。
		/// </summary>
		private SpriteRenderer nightShadow;

		private SpriteRenderer bonusEffect;

		private MeshRenderer bonusPoints;

		private TextMesh bonusPointsText;

		private float x;

		private float y;

		private const int WHOOSH_BASE_POINTS = 2;

		private const int WHOOSH_CHAIN_POINTS = 2;

		private const float MAX_TIME_BETWEEN_WHOOSHES = 1.5f;

		private const float MAX_TIME_BETWEEN_WHOOSHES_WHEN_FEVER = 3f;

		private float passed;

		private static int whooshCombo;

		private static int whooshPoints;

		private static float lastWhooshTime;

		/// <summary>
		/// 无尽续命标志：下一株 Pass 不因时间窗断链（对齐 EndlessRes ContinueWhooshCombo）。
		/// </summary>
		private static bool continueCombo;

		private float size;

		private Color bonusColor = Utility.HexToColor("425f59");

		/// <summary>
		/// Pass 动画期间强制显示，防止裁剪关掉 bonus。
		/// </summary>
		private bool forceVisible;

		private bool renderersVisible;

		private static int cachedPlusPoints = int.MinValue;

		private static string cachedPlusText;

		protected override void Awake()
		{
			base.Awake();
			spriteRenderer = GetComponent<SpriteRenderer>();
			source = GetComponent<AudioSource>();
			// 按名称解析：Endless Prefab 子节点 0 可能是无 SpriteRenderer 的 NightShadow，
			// 误取 GetChild(1) 会 MissingComponentException 并中断整局 Refresh/刷树。
			leaves = ResolveChildSpriteRenderer("Leaves", 0);
			shadow = ResolveChildSpriteRenderer("Shadow", 1);
			nightShadow = ResolveNightShadowRenderer();
			bonusEffect = ResolveChildSpriteRenderer("Bonus", 2);
			Transform bonusTextTransform = ResolveChildTransform("Text", 3);
			if (bonusTextTransform != null)
			{
				bonusPoints = bonusTextTransform.GetComponent<MeshRenderer>();
				bonusPointsText = bonusTextTransform.GetComponent<TextMesh>();
				RemoveConflictingUiComponents(bonusTextTransform);
				if (bonusPointsText == null)
				{
					bonusPointsText = EnsureBonusPointsText(bonusTextTransform);
				}
			}
			base.enabled = false;
		}

		/// <summary>
		/// 优先按子节点名取 SpriteRenderer；否则用索引。禁止把无 Renderer 的 NightShadow 当 shadow。
		/// </summary>
		private SpriteRenderer ResolveChildSpriteRenderer(string childName, int fallbackIndex)
		{
			Transform named = base.transform.Find(childName);
			if (named != null)
			{
				SpriteRenderer onSelf = named.GetComponent<SpriteRenderer>();
				if (onSelf != null)
				{
					return onSelf;
				}
				SpriteRenderer inChildren = named.GetComponentInChildren<SpriteRenderer>(true);
				if (inChildren != null)
				{
					return inChildren;
				}
			}
			if (fallbackIndex >= 0 && fallbackIndex < base.transform.childCount)
			{
				Transform child = base.transform.GetChild(fallbackIndex);
				if (child != null && child.name != "NightShadow")
				{
					SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
					if (sr != null)
					{
						return sr;
					}
					return child.GetComponentInChildren<SpriteRenderer>(true);
				}
			}
			return null;
		}

		private Transform ResolveChildTransform(string childName, int fallbackIndex)
		{
			Transform named = base.transform.Find(childName);
			if (named != null)
			{
				return named;
			}
			if (fallbackIndex >= 0 && fallbackIndex < base.transform.childCount)
			{
				return base.transform.GetChild(fallbackIndex);
			}
			return null;
		}

		/// <summary>
		/// 兼容 Endless「NightShadow 空父 + 子 SR」结构；禁止把无 SR 容器当 Renderer。
		/// </summary>
		private SpriteRenderer ResolveNightShadowRenderer()
		{
			Transform named = base.transform.Find("NightShadow");
			if (named == null)
			{
				return null;
			}
			SpriteRenderer onSelf = named.GetComponent<SpriteRenderer>();
			if (onSelf != null)
			{
				return onSelf;
			}
			return named.GetComponentInChildren<SpriteRenderer>(true);
		}

		/// <summary>
		/// TextMesh 走 MeshRenderer 的 3D 渲染；CanvasRenderer/RectTransform 属于 UI 体系，会阻止世界空间文字显示。
		/// </summary>
		private static void RemoveConflictingUiComponents(Transform bonusTextTransform)
		{
			CanvasRenderer canvasRenderer = bonusTextTransform.GetComponent<CanvasRenderer>();
			if (canvasRenderer != null)
			{
				Object.Destroy(canvasRenderer);
			}
			if (bonusTextTransform is RectTransform)
			{
				Debug.LogWarning("LevelPine 的 Text 使用了 RectTransform，3D TextMesh 可能无法显示。// TODO: [User Action] 请删除 Text 子节点，从 EndlessRes/Resources/Pine.prefab 复制 Text 子树（普通 Transform + MeshRenderer + TextMesh，无 CanvasRenderer）。", bonusTextTransform);
			}
		}

		/// <summary>
		/// LevelPine 的 Text 子节点应挂载 MeshRenderer + TextMesh。
		/// 若 Prefab 被误改为 RectTransform/CanvasRenderer，运行时补回 TextMesh 避免擦边加分崩溃。
		/// </summary>
		private static TextMesh EnsureBonusPointsText(Transform bonusTextTransform)
		{
			TextMesh textMesh = bonusTextTransform.gameObject.AddComponent<TextMesh>();
			textMesh.text = "+0";
			textMesh.anchor = TextAnchor.MiddleCenter;
			textMesh.alignment = TextAlignment.Center;
			textMesh.characterSize = 1f;
			textMesh.fontSize = 40;
			textMesh.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			Debug.LogWarning("LevelPine 的 Text 子节点缺少 TextMesh，已在运行时自动补全。// TODO: [User Action] 请在 LevelPine.prefab 的 Text 上添加 TextMesh 并移除 CanvasRenderer，保存 Prefab 后删除此降级路径。", bonusTextTransform);
			return textMesh;
		}

		protected override void OnEnabled()
		{
			// C1：默认关 Renderer，由 SyncVisible 按相机窗口打开，避免 Refresh 首帧闪全图树
			forceVisible = false;
			renderersVisible = true; // 先置 true，保证下方 SetRenderersVisible(false) 一定会关
			SetRenderersVisible(false);
			if (bonusEffect != null)
			{
				bonusEffect.enabled = false;
			}
			if (bonusPoints != null)
			{
				bonusPoints.enabled = false;
			}
			size = 0.7f + Random.value * 0.59999996f;
			base.transform.localScale = new Vector3(size, size, size);
			passed = -2f;
			if (leaves != null)
			{
				leaves.color = Level.GetPineColor();
			}
			ApplyNightShadowState(NightModeButton.IsOn);
			// 夜态需要 Update 跑月光投影；日间仍关脚本，仅 Pass 时再开
			base.enabled = NightModeButton.IsOn;
		}

		protected override void OnDisabled()
		{
			forceVisible = false;
			renderersVisible = false;
			SetRendererEnabled(spriteRenderer, false);
			SetRendererEnabled(leaves, false);
			SetRendererEnabled(shadow, false);
			SetRendererEnabled(nightShadow, false);
			SetRendererEnabled(bonusEffect, false);
			if (bonusPoints != null)
			{
				bonusPoints.enabled = false;
			}
			if (spriteRenderer != null)
			{
				spriteRenderer.color = Color.white;
			}
			passed = -2f;
			// Kill/回收入池后关闭脚本，避免 Pass 开动画后仍跑 Update（对齐无尽 Pine）
			base.enabled = false;
		}

		protected override void OnNightModeSwitched(bool enabled)
		{
			if (!IsAlive)
			{
				return;
			}
			ApplyNightShadowState(enabled);
			if (!enabled && spriteRenderer != null)
			{
				spriteRenderer.color = Color.white;
			}
			// 存活树：夜态开 Update 投影；日间且无 Pass 动画则关
			if (enabled)
			{
				base.enabled = true;
			}
			else if (passed < 0f)
			{
				base.enabled = false;
			}
		}

		/// <summary>
		/// 夜：nightShadow 开、日间 shadow 关；日相反。尊重当前可见性。
		/// </summary>
		private void ApplyNightShadowState(bool night)
		{
			if (!renderersVisible && !forceVisible)
			{
				SetRendererEnabled(nightShadow, false);
				SetRendererEnabled(shadow, false);
				return;
			}
			if (night)
			{
				SetRendererEnabled(nightShadow, true);
				SetRendererEnabled(shadow, false);
			}
			else
			{
				SetRendererEnabled(nightShadow, false);
				SetRendererEnabled(shadow, true);
			}
		}

		public float GetX()
		{
			return x;
		}

		public float GetY()
		{
			return y;
		}

		/// <summary>
		/// 关卡 Place：第三参为深度偏移 zBias，公式 1f*y+zBias（与无尽 worldZ 不同）。
		/// </summary>
		public void Place(float x, float y, float zBias)
		{
			this.x = x;
			this.y = y;
			base.transform.position = new Vector3(x, y, 1f * y + zBias);
			SyncVisible();
		}

		/// <summary>
		/// 按相机窗口开关 Renderer；forceVisible 时始终显示。
		/// </summary>
		public void SyncVisible()
		{
			bool shouldShow = forceVisible;
			if (!shouldShow)
			{
				float camY = GameCamera.GetY();
				shouldShow = y > camY - CULL_AHEAD && y < camY + CULL_BEHIND;
			}
			SetRenderersVisible(shouldShow);
		}

		/// <summary>
		/// 进入 dangerous 队列时强制显示（近距离必见）。
		/// </summary>
		public void ForceShowForDangerous()
		{
			SetRenderersVisible(true);
		}

		private void SetRenderersVisible(bool visible)
		{
			if (renderersVisible == visible)
			{
				return;
			}
			renderersVisible = visible;
			SetRendererEnabled(spriteRenderer, visible);
			SetRendererEnabled(leaves, visible);
			if (!visible)
			{
				SetRendererEnabled(shadow, false);
				SetRendererEnabled(nightShadow, false);
			}
			else
			{
				ApplyNightShadowState(NightModeButton.IsOn);
			}
		}

		private static void SetRendererEnabled(SpriteRenderer renderer, bool enabled)
		{
			if (renderer != null)
			{
				renderer.enabled = enabled;
			}
		}

		public static int GetWhooshCombo()
		{
			return whooshCombo;
		}

		public static int GetWhooshPoints()
		{
			return whooshPoints;
		}

		/// <summary>
		/// 整局 Refresh 必调；与当前 GameMode 无关，防止无尽→关卡连击泄漏。
		/// </summary>
		public static void ResetState()
		{
			whooshCombo = 1;
			whooshPoints = WHOOSH_BASE_POINTS;
			lastWhooshTime = 0f;
			continueCombo = false;
		}

		/// <summary>
		/// 无尽开局重置连击表（续命路径改走 ContinueWhooshCombo）。
		/// </summary>
		public static void ResetWhooshCombo()
		{
			whooshPoints = WHOOSH_BASE_POINTS;
			whooshCombo = 1;
		}

		/// <summary>
		/// 无尽续命：下一株 Pass 继承连击，不因时间窗断链。
		/// </summary>
		public static void ContinueWhooshCombo()
		{
			continueCombo = true;
		}

		public bool IsPassed()
		{
			return passed > -1.5f;
		}

		public void Pass()
		{
			int points;
			if (GameMode.IsEndless)
			{
				points = PassEndless();
			}
			else
			{
				points = PassLevel();
			}

			lastWhooshTime = Time.time;
			if (bonusPointsText != null)
			{
				bonusPointsText.text = FormatPlusText(points);
				bonusPointsText.color = Skier.GetColor();
			}
			passed = 0f;
			forceVisible = true;
			base.enabled = true;
			SetRenderersVisible(true);
			if (bonusEffect != null)
			{
				bonusEffect.enabled = true;
			}
			if (bonusPoints != null)
			{
				bonusPoints.enabled = true;
			}
			SyncBonusEffect(0f);
			if (source != null)
			{
				source.Play();
			}

			// 加分与振动一致：跳过计分时不震
			if (points > 0)
			{
				Neuron.Whoosh(points);
				if (GameMode.IsEndless)
				{
					if (whooshCombo == 3 || whooshCombo == 6)
					{
						Device.Vibrate(Vibration.Medium);
					}
				}
			}
		}

		/// <summary>
		/// 关卡 Whoosh：关卡号 × 连击。
		/// </summary>
		private int PassLevel()
		{
			float window = Skier.IsInFever() ? MAX_TIME_BETWEEN_WHOOSHES_WHEN_FEVER : MAX_TIME_BETWEEN_WHOOSHES;
			if (Time.time - lastWhooshTime > window)
			{
				whooshCombo = 1;
			}
			else
			{
				whooshCombo++;
			}
			return Level.Get() * whooshCombo;
		}

		/// <summary>
		/// 无尽 Whoosh：基数 2、链上 +2；续命首株用 continueCombo 防断链（仍计一次分）。
		/// </summary>
		private int PassEndless()
		{
			float window = Skier.IsInFever() ? MAX_TIME_BETWEEN_WHOOSHES_WHEN_FEVER : MAX_TIME_BETWEEN_WHOOSHES;
			if (!continueCombo && Time.time - lastWhooshTime > window)
			{
				ResetWhooshCombo();
			}
			else
			{
				if (continueCombo)
				{
					continueCombo = false;
				}
				whooshPoints += WHOOSH_CHAIN_POINTS;
				whooshCombo++;
			}
			return whooshPoints;
		}

		private static string FormatPlusText(int points)
		{
			if (points != cachedPlusPoints)
			{
				cachedPlusPoints = points;
				cachedPlusText = $"+{points.ToString()}";
			}
			return cachedPlusText;
		}

		private void Update()
		{
			// Pass 动画仅在 passed>=0 时推进，避免夜态常开 Update 误触 IsPassed
			if (passed >= 0f)
			{
				passed += Time.deltaTime;
				float z;
				if (passed > 1f)
				{
					z = size;
					SetRendererEnabled(bonusEffect, false);
					if (bonusPoints != null)
					{
						bonusPoints.enabled = false;
					}
					forceVisible = false;
					SyncVisible();
					passed = -1f;
					// 日间可关脚本；夜态继续跑月光投影
					if (!NightModeButton.IsOn)
					{
						base.enabled = false;
					}
				}
				else
				{
					z = Mathf.Min(passed * 4f, 1f) * 2f - 1f;
					z = ((0f - z) * z + 1f) * 0.3f + size;
					SyncBonusEffect(passed);
				}
				base.transform.localScale = new Vector3(z, z, z);
			}
			UpdateMoonlitShadow();
		}

		/// <summary>
		/// 无尽同款月光树影：按滑雪者相对位置旋转投影，并用光强×距离衰减染树干。
		/// </summary>
		private void UpdateMoonlitShadow()
		{
			if (!NightModeButton.IsOn || !IsAlive)
			{
				return;
			}
			Skier skier = Skier.i;
			if (skier == null || spriteRenderer == null)
			{
				return;
			}
			float dx = skier.transform.position.x - base.transform.position.x;
			float dy = skier.transform.position.y - base.transform.position.y;
			if (nightShadow != null)
			{
				nightShadow.transform.localEulerAngles = new Vector3(90f, (0f - Mathf.Atan2(dy, dx)) * 57.29578f - 90f, 0f);
			}
			float intensity = skier.GetGlowIntensity() * Mathf.Min(5f / Mathf.Max(dx * dx + dy * dy, 1f), 1f);
			Color c = Skier.IsInFever() ? Level.GetFeverColor() : Skier.GetColor();
			spriteRenderer.color = new Color(c.r * intensity, c.g * intensity, c.b * intensity, 1f);
		}

		private void SyncBonusEffect(float time)
		{
			if (bonusEffect == null)
			{
				return;
			}
			float num = time * 4f;
			bonusColor.a = Mathf.Max(1f - num, 0f);
			bonusEffect.color = bonusColor;
			bonusEffect.transform.localScale = new Vector3(num, num * 0.6f, num);
			if (bonusPointsText == null)
			{
				return;
			}
			Color color = bonusPointsText.color;
			color.a = 1f - time * time;
			bonusPointsText.color = color;
			bonusPointsText.transform.localPosition = new Vector3(0f, 2f - bonusPointsText.color.a * 0.5f, 0f);
		}
	}
}
