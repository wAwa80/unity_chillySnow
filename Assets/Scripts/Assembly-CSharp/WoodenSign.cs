using UnityEngine;
using UnityEngine.UI;

namespace LevelMode
{

	public class WoodenSign : Singleton<WoodenSign>
	{
		private TextMesh textMesh;

		/// <summary>
		/// 编辑器里配置的 Character Size；关卡模式显示关卡号时始终沿用，不再按字数压小。
		/// </summary>
		private float initialCharacterSize;

		/// <summary>
		/// 可选：无尽 HUD 距离 Text。世界木牌 TextMesh 仅用于关卡号，无尽模式不会刷新它。
		/// TODO: [User Action] 请在编辑器 Inspector 中拖拽赋值：HUD_Endless 上的距离 Text（可空）
		/// </summary>
		[SerializeField]
		private Text endlessDistanceHudText;

		private int lastDisplayedDistance = int.MinValue;

		private bool lastWasEndless;

		/// <summary>
		/// 须晚于 PineGenerator(2)：局外无尽文案可回退到生成器游标。
		/// </summary>
		public override int GetPriority()
		{
			return 3;
		}

		protected override void Awake()
		{
			base.Awake();
			textMesh = base.transform.GetChild(1).GetComponent<TextMesh>();
			// 缓存 Inspector 初始字号，避免运行时被旧的 Fit 逻辑改掉
			if (textMesh != null)
			{
				initialCharacterSize = textMesh.characterSize;
			}
		}

		protected override void OnRefresh()
		{
			lastDisplayedDistance = int.MinValue;
			lastWasEndless = GameMode.IsEndless;
			if (GameMode.IsEndless)
			{
				// 无尽：不改世界木牌 Text；仅在绑了 HUD 时刷新距离
				base.enabled = endlessDistanceHudText != null;
				ApplyEndlessDistance(GameMode.GetEndlessSlideDistance());
			}
			else
			{
				base.enabled = false;
				ApplyLevelNumber();
				ClearEndlessHud();
			}
		}

		protected override void OnStartRun(Run slide)
		{
			if (GameMode.IsEndless)
			{
				base.enabled = endlessDistanceHudText != null;
				lastDisplayedDistance = int.MinValue;
			}
		}

		protected override void OnEndRun()
		{
			// 死后仍刷新 HUD 最终距离，直至 Refresh；木牌 Text 不动
			if (GameMode.IsEndless)
			{
				ApplyEndlessDistance(GameMode.GetEndlessSlideDistance());
			}
		}

		private void Update()
		{
			if (!GameMode.IsEndless)
			{
				return;
			}
			ApplyEndlessDistance(GameMode.GetEndlessSlideDistance());
		}

		/// <summary>
		/// 关卡模式：木牌只显示当前关卡号，字号保持编辑器初始值。
		/// </summary>
		private void ApplyLevelNumber()
		{
			if (textMesh == null)
			{
				return;
			}
			textMesh.text = Level.Get().ToString();
			textMesh.characterSize = initialCharacterSize;
		}

		/// <summary>
		/// 无尽模式：只更新可选 HUD 距离，绝不改 WoodSign/Text 的 TextMesh。
		/// </summary>
		private void ApplyEndlessDistance(int distance)
		{
			if (distance == lastDisplayedDistance && lastWasEndless)
			{
				return;
			}
			lastDisplayedDistance = distance;
			lastWasEndless = true;
			if (endlessDistanceHudText != null)
			{
				endlessDistanceHudText.text = distance + "m";
			}
		}

		private void ClearEndlessHud()
		{
			if (endlessDistanceHudText != null)
			{
				endlessDistanceHudText.text = string.Empty;
			}
		}
	}
}
