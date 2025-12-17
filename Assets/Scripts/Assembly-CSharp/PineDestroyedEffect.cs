using UnityEngine;

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
}
