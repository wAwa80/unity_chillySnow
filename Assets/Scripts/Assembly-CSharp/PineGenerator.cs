using System;
using System.Collections.Generic;
using UnityEngine;

public class PineGenerator : Singleton<PineGenerator>
{
	[SerializeField]
	private FastNoiseUnity fastNoise;

	private static readonly Queue<Pine> comingPines = new Queue<Pine>();

	private static readonly Queue<Pine> dangerousPines = new Queue<Pine>();

	private static readonly Queue<Pine> whooshablePines = new Queue<Pine>();

	private static readonly Queue<Pine> pendingDelete = new Queue<Pine>();

	private static float spawnY;

	public override int GetPriority()
	{
		return 2;
	}

	private void Start()
	{
		base.enabled = false;
		Neuron.Refresh();
	}

	protected override void OnRefresh()
	{
		CleanPineQueue(pendingDelete);
		CleanPineQueue(dangerousPines);
		CleanPineQueue(whooshablePines);
		CleanPineQueue(comingPines);
		Pine.ResetState();
		spawnY = -7f;
		RollingStone.KillAll();
		SpawnPines();
	}

	protected override void OnStartRun(Run slide)
	{
		base.enabled = true;
	}

	protected override void OnEndRun()
	{
		base.enabled = false;
	}

	private void CleanPineQueue(Queue<Pine> queue)
	{
		foreach (Pine item in queue)
		{
			item.Kill();
		}
		queue.Clear();
	}

	private void Update()
	{
		CheckPines();
	}

	private void SpawnPines()
	{
		float num = 0f - FinishLine.GetDistance() + 3f;
		while (spawnY > num)
		{
			GeneratePineLine(spawnY);
			spawnY -= 0.3f;
		}
	}

	private float InverseSigmoid(float value)
	{
		return Mathf.Pow(0.5f * (ATanh(2f * value - 1f) * 10f + 1f), 1f);
	}

	private float ATanh(float x)
	{
		return (float)(Math.Log(1f + x) - Math.Log(1f - x)) * 0.5f;
	}

	private void GeneratePineLine(float y)
	{
		float num = Parameters.PINE_PROBABILITY.Sample() * Parameters.PINE_PROBABILITY_MULTIPLIER.Sample();
		float num2 = Mathf.Floor((GameCamera.GetHorizontalLimit() + 1.3f) * 3.33333f) * 0.3f;
		float num3 = 0f - num2;
		float num4 = 0f;
		for (float num5 = num3; num5 <= num2; num5 += 0.3f)
		{
			if (UnityEngine.Random.value < num)
			{
				Pine pine = Recyclable<Pine>.Get();
				pine.Place(num5, y, num4);
				comingPines.Enqueue(pine);
				num4 += 0.01f;
			}
		}
		if (UnityEngine.Random.value < Parameters.ROLLING_STONE_PROBABILITY.Sample())
		{
			Recyclable<RollingStone>.Get().SetTargetY(y);
		}
	}

	public static float GetDistance()
	{
		return 0f - spawnY;
	}

	private void CheckPines()
	{
		float num = Skier.GetY() - 0.3f;
		while (comingPines.Count > 0 && comingPines.Peek().GetY() > num)
		{
			dangerousPines.Enqueue(comingPines.Dequeue());
		}
		float y = Skier.GetY();
		while (dangerousPines.Count > 0 && dangerousPines.Peek().GetY() > y)
		{
			whooshablePines.Enqueue(dangerousPines.Dequeue());
		}
		float x = Skier.GetX();
		foreach (Pine dangerousPine in dangerousPines)
		{
			float num2 = dangerousPine.GetX() - x;
			float num3 = dangerousPine.GetY() - y;
			float num4 = num2 * num2 + num3 * num3;
			if (num4 < 0.0225f)
			{
				Neuron.EndRun();
				break;
			}
			if (!dangerousPine.IsPassed() && num4 < 1f)
			{
				dangerousPine.Pass();
			}
		}
		num = Skier.GetY() + 1f;
		while (whooshablePines.Count > 0 && whooshablePines.Peek().GetY() > num)
		{
			pendingDelete.Enqueue(whooshablePines.Dequeue());
		}
		foreach (Pine whooshablePine in whooshablePines)
		{
			if (!whooshablePine.IsPassed())
			{
				float num5 = whooshablePine.GetX() - x;
				float num6 = whooshablePine.GetY() - y;
				float num7 = num5 * num5 + num6 * num6;
				if (num7 < 1f)
				{
					whooshablePine.Pass();
				}
			}
		}
		num = GameCamera.GetY() + 10f;
		while (pendingDelete.Count > 0 && pendingDelete.Peek().GetY() > num)
		{
			pendingDelete.Dequeue().Kill();
		}
	}

	protected override void OnContinue()
	{
		CleanClosePines(comingPines);
		CleanClosePines(dangerousPines);
		CleanClosePines(whooshablePines);
		CleanClosePines(pendingDelete);
		base.enabled = true;
	}

	private void CleanClosePines(Queue<Pine> pines)
	{
		int count = pines.Count;
		int num = 0;
		while (num < count)
		{
			num++;
			Pine pine = pines.Dequeue();
			float num2 = pine.transform.position.x - Skier.GetX();
			float num3 = pine.transform.position.y - Skier.GetY();
			float num4 = num2 * num2 + num3 * num3;
			if (num4 < 1.5f)
			{
				pine.Kill();
			}
			else
			{
				pines.Enqueue(pine);
			}
		}
	}
}
