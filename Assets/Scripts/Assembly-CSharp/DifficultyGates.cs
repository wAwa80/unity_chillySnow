using UnityEngine;

namespace LevelMode
{
	/// <summary>
	/// 关卡难度覆盖开关（风格对齐 JuiceConsentGates）。
	/// 仅允许被关卡刷树/刷石路径调用；无尽路径禁止引用本类。
	/// 总开关关闭时 helpers 透传原版 Parameters.Sample 公式，与改前一致。
	///
	/// 用法示例——只加滚石、树保持曲线原样：
	///   EnableDifficultyOverride = true（必须打开，否则下方 Scale 不生效）
	///   ForceHellDifficulty = false（按需；true 则树/石都按 HellVirtualLevel 取样曲线）
	///   DifficultyScale = 1f（树倍率不变）
	///   RollingStoneScale = 2f~3f（只拉高滚石概率）
	/// </summary>
	public static class DifficultyGates
	{
		/// <summary>
		/// 总开关：false=完全原样；true=才应用下方 Hell / 树 Scale / 石 Scale。
		/// </summary>
		public const bool EnableDifficultyOverride = true;

		/// <summary>
		/// 仅当总开关为 true 时生效：按地狱虚拟等级取样（树与石的 SampleAt 共用）。
		/// </summary>
		public const bool ForceHellDifficulty = false;

		/// <summary>
		/// 仅当总开关为 true 时生效：只乘在树（GetPineLineChance）上，建议 0~2。
		/// 调石请改 RollingStoneScale，勿用本常量。
		/// </summary>
		public const float DifficultyScale = 1f;

		/// <summary>
		/// 仅当总开关为 true 时生效：只乘在滚石（GetRollingStoneChance）上，建议 0~3。
		/// 默认 1f 与现网一致；想「石多、树不变」时保持 DifficultyScale=1、本值调高。
		/// </summary>
		public const float RollingStoneScale = 5f;

		/// <summary>
		/// Hell 时喂给 Sampler 的虚拟关卡号（对齐曲线右端）。
		/// </summary>
		public const float HellVirtualLevel = 1000f;

		/// <summary>
		/// 关卡一行树的生成概率（含距离倍率；Override 时再乘 DifficultyScale 并 Clamp01）。
		/// </summary>
		public static float GetPineLineChance()
		{
			if (!EnableDifficultyOverride)
			{
				return Parameters.PINE_PROBABILITY.Sample() * Parameters.PINE_PROBABILITY_MULTIPLIER.Sample();
			}

			float baseChance = Parameters.PINE_PROBABILITY.SampleAt(GetEffectiveLevel());
			float distMul = Parameters.PINE_PROBABILITY_MULTIPLIER.Sample();
			return Mathf.Clamp01(baseChance * distMul * Mathf.Max(0f, DifficultyScale));
		}

		/// <summary>
		/// 关卡滚石生成概率（不接未使用的 ROLLING_STONE_PROBABILITY_MULTIPLIER）。
		/// Override 时乘 RollingStoneScale（与树的 DifficultyScale 独立）。
		/// </summary>
		public static float GetRollingStoneChance()
		{
			if (!EnableDifficultyOverride)
			{
				return Parameters.ROLLING_STONE_PROBABILITY.Sample();
			}

			return Mathf.Clamp01(Parameters.ROLLING_STONE_PROBABILITY.SampleAt(GetEffectiveLevel()) * Mathf.Max(0f, RollingStoneScale));
		}

		/// <summary>
		/// 关卡刷石摆位：Override 关走原版单参；开则注入 Hell/等级速度（不乘任何 Scale）。
		/// </summary>
		public static void BindRollingStoneTarget(RollingStone stone, float y)
		{
			if (!EnableDifficultyOverride)
			{
				stone.SetTargetY(y);
				return;
			}

			float minSpeed = Parameters.ROLLING_STONE_MIN_SPEED.SampleAt(GetEffectiveLevel());
			float maxSpeed = Parameters.ROLLING_STONE_MAX_SPEED.SampleAt(GetEffectiveLevel());
			stone.SetTargetY(y, minSpeed, maxSpeed);
		}

		/// <summary>
		/// 仅在 Override==true 时使用；ForceHell 才抬到虚拟高等级。
		/// </summary>
		private static float GetEffectiveLevel()
		{
			return ForceHellDifficulty ? HellVirtualLevel : Level.GetFloat();
		}
	}
}
