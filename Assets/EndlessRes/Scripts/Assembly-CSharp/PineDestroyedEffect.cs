using UnityEngine;


namespace EndlessMode
{
	[RequireComponent(typeof(ParticleSystem))]
	public class PineDestroyedEffect : Recyclable<PineDestroyedEffect>
	{
		private ParticleSystem system;

		protected override void Awake()
		{
			base.Awake();
			system = GetComponent<ParticleSystem>();
		}

		public void SetColor(Color c)
		{
			ParticleSystem.MainModule main = system.main;
			main.startColor = new ParticleSystem.MinMaxGradient(c);
		}

		protected override void OnEnabled()
		{
			system.Play();
			Invoke("Kill", system.main.duration);
		}

		/// <summary>
		/// 入池前必须取消挂起的 Kill Invoke，防止复用后误杀新实例。
		/// </summary>
		protected override void OnDisabled()
		{
			CancelInvoke();
			if (system != null)
			{
				system.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
		}
	}
}
