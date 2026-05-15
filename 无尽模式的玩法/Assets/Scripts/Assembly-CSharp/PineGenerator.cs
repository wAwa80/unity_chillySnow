using System.Collections.Generic;
using UnityEngine;

public class PineGenerator : Singleton<PineGenerator>
{
	private const float START_PINES_DISTANCE = 7f;

	private static readonly Queue<Pine> comingPines = new Queue<Pine>();

	private static readonly Queue<Pine> dangerousPines = new Queue<Pine>();

	private static readonly Queue<Pine> whooshablePines = new Queue<Pine>();

	private static readonly Queue<Pine> pendingDelete = new Queue<Pine>();

	[SerializeField]
	private AnimationCurve probabilityCurve;

	private bool didContinue;

	private bool dontResetWhooshCombo;

	private const float VERTICAL_PINE_DENSITY = 0.3f;

	private const float HORIZONTAL_PINE_DENSITY = 0.3f;

	private Vector2 seed;

	private float nextZ;

	private const float SQR_SIZE = 0.0225f;

	private const float WHOOSH_DISTANCE = 1f;

	private const float SQR_WHOOSH_DISTANCE = 1f;

	private float wd;

	private float swd;

	private void Start()
	{
		wd = 1f;
		swd = 1f;
		OnBackToMenu();
		base.enabled = false;
	}

	public override int GetPriority()
	{
		return 2;
	}

	protected override void OnBackToMenu()
	{
		CleanPineQueue(pendingDelete);
		CleanPineQueue(dangerousPines);
		CleanPineQueue(whooshablePines);
		CleanPineQueue(comingPines);
		base.transform.position = new Vector3(0f, -7f, base.transform.position.z);
		seed = new Vector2(10f * Random.value, 10f * Random.value);
		nextZ = 0f;
		SpawnPines();
	}

	protected override void OnGameOver(bool canUseSecondChance)
	{
		Logger.Log($"PineGenerator: OnGameOver called. canUseSecondChance: {canUseSecondChance}");
        base.enabled = false;
	}

	protected override void OnNewGame()
	{
		base.enabled = true;
		didContinue = false;
		if (dontResetWhooshCombo)
		{
			Pine.ContinueWhooshCombo();
		}
		else
		{
			Pine.ResetWhooshCombo();
		}
	}

	private void Update()
	{
		SpawnPines();
		CheckPines();
	}

	protected override void OnContinue()
	{
		Logger.Log("PineGenerator: OnContinue called.");
        CleanClosePines(comingPines);
		CleanClosePines(dangerousPines);
		CleanClosePines(whooshablePines);
		CleanClosePines(pendingDelete);
		dontResetWhooshCombo = true;
		OnNewGame();
		didContinue = true;
		dontResetWhooshCombo = false;
	}

	private void CleanClosePines(Queue<Pine> pines)
	{
		int count = pines.Count;
		int num = 0;
		while (num < count)
		{
			num++;
			Pine pine = pines.Dequeue();
			float num2 = pine.transform.position.x - Singleton<Player>.i.transform.position.x;
			float num3 = pine.transform.position.y - Singleton<Player>.i.transform.position.y;
			float num4 = num2 * num2 + num3 * num3;
			if (num4 < 2.25f)
			{
				pine.Kill();
			}
			else
			{
				pines.Enqueue(pine);
			}
		}
	}

	private void SpawnPines()
	{
		float num = base.transform.position.y;
		float num2 = Singleton<GameCamera>.i.transform.position.y - 12f;
		while (num > num2)
		{
			GeneratePineLine(num);
			num -= 0.3f;
		}
		base.transform.position = new Vector3(0f, num, base.transform.position.z);
	}

	private void GeneratePineLine(float y)
	{
		float num = 0.3f;
		float num2 = Mathf.Floor((Singleton<GameCamera>.i.GetHorizontalLimit() + 2f) / num) * num;
		float num3 = 0f - num2;
		num2 += num * 0.5f;
		Vector2 vector = new Vector2(seed.x + num3, seed.y + y);
		for (float num4 = num3; num4 < num2; num4 += num)
		{
			float num5 = Mathf.Min(Singleton<PineGenerator>.i.GetDistance(), 400f);
			float num6 = 2f + num5 * 0.005f;
			num6 = probabilityCurve.Evaluate(Mathf.PerlinNoise(vector.x * num6, vector.y * num6));
			num6 = ((!(num5 < 5f)) ? (num6 * (num5 * 0.0002f + 0.04f)) : (num6 * (num5 * 0.008f)));
			if (Random.value < num6)
			{
				Pine pine = Recyclable<Pine>.Get();
				pine.transform.position = new Vector3(num4, y, base.transform.position.z - nextZ);
				comingPines.Enqueue(pine);
				nextZ += 0.001f;
				if (nextZ >= 1f)
				{
					ResetZ();
				}
			}
			vector.x += num;
		}
	}

	private void ResetZ()
	{
		foreach (Pine comingPine in comingPines)
		{
			comingPine.transform.position = new Vector3(comingPine.transform.position.x, comingPine.transform.position.y, comingPine.transform.position.z + nextZ);
		}
		foreach (Pine dangerousPine in dangerousPines)
		{
			dangerousPine.transform.position = new Vector3(dangerousPine.transform.position.x, dangerousPine.transform.position.y, dangerousPine.transform.position.z + nextZ);
		}
		foreach (Pine whooshablePine in whooshablePines)
		{
			whooshablePine.transform.position = new Vector3(whooshablePine.transform.position.x, whooshablePine.transform.position.y, whooshablePine.transform.position.z + nextZ);
		}
		foreach (Pine item in pendingDelete)
		{
			item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, item.transform.position.z + nextZ);
		}
		nextZ = 0f;
	}

	public float GetDistance()
	{
		return 0f - base.transform.position.y;
	}

	private void CheckPines()
	{
		while (comingPines.Count > 0 && comingPines.Peek().transform.position.y > Singleton<Player>.i.transform.position.y - 0.3f)
		{
			dangerousPines.Enqueue(comingPines.Dequeue());
		}
		while (dangerousPines.Count > 0 && dangerousPines.Peek().transform.position.y > Singleton<Player>.i.transform.position.y)
		{
			whooshablePines.Enqueue(dangerousPines.Dequeue());
		}
		Vector3 position = Singleton<Player>.i.transform.position;
		foreach (Pine dangerousPine in dangerousPines)
		{
			if (dangerousPine.IsDestroyed())
			{
				continue;
			}
			float num = dangerousPine.transform.position.x - position.x;
			float num2 = dangerousPine.transform.position.y - position.y;
			float num3 = num * num + num2 * num2;
			if (num3 < 0.0225f)
			{
				if (Player.IsABTestDestroyPines && Singleton<Player>.i.GetFeverState() != 0)
				{
					dangerousPine.DestroyPine();
				}
				else
				{
					Neuron.GameOver(!didContinue);
				}
				break;
			}
			if (!dangerousPine.IsPassed() && num3 < swd)
			{
				dangerousPine.Pass();
				Neuron.Whoosh();
			}
		}
		while (whooshablePines.Count > 0 && whooshablePines.Peek().transform.position.y > Singleton<Player>.i.transform.position.y + wd)
		{
			pendingDelete.Enqueue(whooshablePines.Dequeue());
		}
		foreach (Pine whooshablePine in whooshablePines)
		{
			if (!whooshablePine.IsPassed())
			{
				float num4 = whooshablePine.transform.position.x - position.x;
				float num5 = whooshablePine.transform.position.y - position.y;
				float num6 = num4 * num4 + num5 * num5;
				if (num6 < swd)
				{
					whooshablePine.Pass();
					Neuron.Whoosh();
				}
			}
		}
		while (pendingDelete.Count > 0 && pendingDelete.Peek().transform.position.y > Singleton<GameCamera>.i.transform.position.y + 10f)
		{
			pendingDelete.Dequeue().Kill();
		}
	}

	private void CleanPineQueue(Queue<Pine> queue)
	{
		foreach (Pine item in queue)
		{
			item.Kill();
		}
		queue.Clear();
	}
}
