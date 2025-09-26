using UnityEngine;

public sealed class FinishLine : Singleton<FinishLine>
{
	private AudioSource source;

	private static float distance;

	private ParticleSystem[] cheers;

	protected override void Awake()
	{
		base.Awake();
		source = GetComponent<AudioSource>();
		cheers = GetComponentsInChildren<ParticleSystem>();
	}

	protected override void OnRefresh()
	{
		distance = Parameters.TOTAL_SLIDE_DISTANCE.Sample();
		base.transform.position = new Vector3(0f, 0f - distance, -1f * distance + 1f);
	}

	public static float GetDistance()
	{
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
