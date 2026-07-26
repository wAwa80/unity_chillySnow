using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EndlessMode
{
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

		/// <summary>
		/// 替换 ResetZ：每棵树递增序号，用 %1000 微偏移保证叠画顺序。
		/// </summary>
		private static int pineSerial;

		private const float SQR_SIZE = 0.0225f;

		private const float WHOOSH_DISTANCE = 1f;

		private const float SQR_WHOOSH_DISTANCE = 1f;

		private const float SPAWN_AHEAD = 12f;

		private const int MAX_LINES_PER_FRAME = 4;

		private const int BURST_MAX_LINES = 12;

		private float wd;

		private float swd;

		private void Start()
		{
			wd = 1f;
			swd = 1f;
			base.enabled = false;
			StartCoroutine(InitEndlessPines());
		}

		/// <summary>
		/// 必须先 WarmUp 再 OnBackToMenu（内含 Spawn），禁止先刷树再预热。
		/// </summary>
		private IEnumerator InitEndlessPines()
		{
			if (Recyclable<Pine>.PoolCount < 80)
			{
				yield return StartCoroutine(Recyclable<Pine>.WarmUpInactiveCoroutine(80, 20));
			}
			OnBackToMenu();
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
			pineSerial = 0;
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
				if (pine == null)
				{
					continue;
				}
				float num2 = pine.GetX() - Singleton<Player>.i.transform.position.x;
				float num3 = pine.GetY() - Singleton<Player>.i.transform.position.y;
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
			float cursor = base.transform.position.y;
			float targetY = Singleton<GameCamera>.i.transform.position.y - SPAWN_AHEAD;
			float deficit = (cursor - targetY) / 0.3f;
			int maxLines = deficit > 20f ? BURST_MAX_LINES : MAX_LINES_PER_FRAME;
			int lines = 0;
			while (cursor > targetY && lines < maxLines)
			{
				GeneratePineLine(cursor);
				cursor -= 0.3f;
				lines++;
			}
			base.transform.position = new Vector3(0f, cursor, base.transform.position.z);
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
					if (pine == null)
					{
						vector.x += num;
						continue;
					}
					pineSerial++;
					float worldZ = base.transform.position.z - (pineSerial % 1000) * 0.001f;
					pine.Place(num4, y, worldZ);
					comingPines.Enqueue(pine);
				}
				vector.x += num;
			}
		}

		public float GetDistance()
		{
			return 0f - base.transform.position.y;
		}

		private void CheckPines()
		{
			while (comingPines.Count > 0)
			{
				Pine peek = comingPines.Peek();
				if (peek == null)
				{
					comingPines.Dequeue();
					continue;
				}
				if (peek.GetY() > Singleton<Player>.i.transform.position.y - 0.3f)
				{
					dangerousPines.Enqueue(comingPines.Dequeue());
					continue;
				}
				break;
			}
			while (dangerousPines.Count > 0)
			{
				Pine peek = dangerousPines.Peek();
				if (peek == null)
				{
					dangerousPines.Dequeue();
					continue;
				}
				if (peek.GetY() > Singleton<Player>.i.transform.position.y)
				{
					whooshablePines.Enqueue(dangerousPines.Dequeue());
					continue;
				}
				break;
			}
			Vector3 position = Singleton<Player>.i.transform.position;
			foreach (Pine dangerousPine in dangerousPines)
			{
				if (dangerousPine == null || dangerousPine.IsDestroyed())
				{
					continue;
				}
				float num = dangerousPine.GetX() - position.x;
				float num2 = dangerousPine.GetY() - position.y;
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
			while (whooshablePines.Count > 0)
			{
				Pine peek = whooshablePines.Peek();
				if (peek == null)
				{
					whooshablePines.Dequeue();
					continue;
				}
				if (peek.GetY() > Singleton<Player>.i.transform.position.y + wd)
				{
					pendingDelete.Enqueue(whooshablePines.Dequeue());
					continue;
				}
				break;
			}
			foreach (Pine whooshablePine in whooshablePines)
			{
				// Fever DestroyPine 后不得再 Pass/Whoosh
				if (whooshablePine == null || whooshablePine.IsDestroyed())
				{
					continue;
				}
				if (!whooshablePine.IsPassed())
				{
					float num4 = whooshablePine.GetX() - position.x;
					float num5 = whooshablePine.GetY() - position.y;
					float num6 = num4 * num4 + num5 * num5;
					if (num6 < swd)
					{
						whooshablePine.Pass();
						Neuron.Whoosh();
					}
				}
			}
			while (pendingDelete.Count > 0)
			{
				Pine peek = pendingDelete.Peek();
				if (peek == null)
				{
					pendingDelete.Dequeue();
					continue;
				}
				if (peek.GetY() > Singleton<GameCamera>.i.transform.position.y + 10f)
				{
					pendingDelete.Dequeue().Kill();
					continue;
				}
				break;
			}
		}

		private void CleanPineQueue(Queue<Pine> queue)
		{
			foreach (Pine item in queue)
			{
				if (item == null)
				{
					continue;
				}
				item.Kill();
			}
			queue.Clear();
		}
	}
}
