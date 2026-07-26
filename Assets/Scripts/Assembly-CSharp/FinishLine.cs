using UnityEngine;

namespace LevelMode
{

	public sealed class FinishLine : Singleton<FinishLine>
	{
		/// <summary>
		/// 无尽哨兵距离：把终点移出玩法区，供进度条防御；禁止当作真实滑行长度驱动 dontFollow。
		/// 【残余风险·FinishLine 哨兵】无尽若误开 GameCamera.dontFollow，镜头会被拉到极远。
		/// </summary>
		private const float ENDLESS_SENTINEL_DISTANCE = 100000f;

		private AudioSource source;

		private static float distance;

		private ParticleSystem[] cheers;

		/// <summary>
		/// 必须早于 Level(-1)：Level.Update 用 GetDistance() 算进度条，若本类尚未 Refresh 则除零。
		/// </summary>
		public override int GetPriority()
		{
			return -2;
		}

		protected override void Awake()
		{
			base.Awake();
			source = GetComponent<AudioSource>();
			cheers = GetComponentsInChildren<ParticleSystem>();
		}

		protected override void OnRefresh()
		{
			// TrySetMode 已先改 Current，此处读到的是目标模式
			if (GameMode.IsEndless)
			{
				distance = ENDLESS_SENTINEL_DISTANCE;
			}
			else
			{
				distance = Parameters.TOTAL_SLIDE_DISTANCE.Sample();
			}
			base.transform.position = new Vector3(0f, 0f - distance, -1f * distance + 1f);
		}

		public static float GetDistance()
		{
			// 防御：未 Refresh 前为 0，调用方须自行防除零
			return distance;
		}

		protected override void OnEndRun()
		{
			if (Neuron.GetCurrentRun().success)
			{
				ParticleSystem[] array = cheers;
				foreach (ParticleSystem particleSystem in array)
				{
					particleSystem.Play();
				}
				source.Play();
			}
		}
	}
}
