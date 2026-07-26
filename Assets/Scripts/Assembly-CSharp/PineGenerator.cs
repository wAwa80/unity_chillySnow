using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelMode
{

	public class PineGenerator : Singleton<PineGenerator>
	{
		private const float START_PINES_DISTANCE = 7f;

		private const float SPAWN_AHEAD = 12f;

		private const int MAX_LINES_PER_FRAME = 4;

		private const int BURST_MAX_LINES = 12;

		[SerializeField]
		private FastNoiseUnity fastNoise;

		/// <summary>
		/// 无尽刷树概率曲线；空则运行时填默认，不覆盖已调好的 Inspector 曲线。
		/// TODO: [User Action] 可选：在 Inspector 中指定无尽 probabilityCurve；留空则用代码默认
		/// </summary>
		[SerializeField]
		private AnimationCurve probabilityCurve;

		private static readonly Queue<Pine> comingPines = new Queue<Pine>();

		private static readonly Queue<Pine> dangerousPines = new Queue<Pine>();

		private static readonly Queue<Pine> whooshablePines = new Queue<Pine>();

		private static readonly Queue<Pine> pendingDelete = new Queue<Pine>();

		private static float spawnY;

		/// <summary>
		/// Awake 缓存的世界坐标；无尽会下移 transform，切回关卡时必须还原。
		/// </summary>
		private Vector3 cachedAwakeWorldPos;

		private Vector2 seed;

		private static int pineSerial;

		/// <summary>
		/// 续命后下一次 StartRun 勿重置 Whoosh（Continue 与 Tap→StartRun 之间会隔一帧）。
		/// </summary>
		private bool preserveWhooshOnNextStart;

		public override int GetPriority()
		{
			return 2;
		}

		protected override void Awake()
		{
			base.Awake();
			cachedAwakeWorldPos = base.transform.position;
			EnsureEndlessCurve();
		}

		private void Start()
		{
			base.enabled = false;
			StartCoroutine(InitLevelPines());
		}

		/// <summary>
		/// Clean → WarmUp → Refresh，保证域不重载时无脏引用，且刷树前池已热。
		/// 切模式禁止再走 WarmUp（见合并计划 §7.1）。
		/// </summary>
		private IEnumerator InitLevelPines()
		{
			CleanPineQueue(pendingDelete);
			CleanPineQueue(dangerousPines);
			CleanPineQueue(whooshablePines);
			CleanPineQueue(comingPines);
			if (Recyclable<Pine>.PoolCount < 300)
			{
				yield return StartCoroutine(Recyclable<Pine>.WarmUpInactiveCoroutine(300, 30));
			}
			if (Recyclable<RollingStone>.PoolCount < 8)
			{
				yield return StartCoroutine(Recyclable<RollingStone>.WarmUpInactiveCoroutine(8, 4));
			}
			Neuron.Refresh();
		}

		/// <summary>
		/// 仅在曲线为空时写入默认；禁止覆盖 User 已调好的 SerializeField。
		/// </summary>
		private void EnsureEndlessCurve()
		{
			if (probabilityCurve != null && probabilityCurve.length > 0)
			{
				return;
			}
			probabilityCurve = AnimationCurve.Linear(0f, 0.15f, 1f, 0.55f);
		}

		protected override void OnRefresh()
		{
			CleanPineQueue(pendingDelete);
			CleanPineQueue(dangerousPines);
			CleanPineQueue(whooshablePines);
			CleanPineQueue(comingPines);
			// 模式无关：整局必清连击静态状态
			Pine.ResetState();
			RollingStone.KillAll();
			pineSerial = 0;
			preserveWhooshOnNextStart = false;

			// TrySetMode 已先改 Current，此处读新模式决定坐标策略
			if (GameMode.IsLevel)
			{
				base.transform.position = cachedAwakeWorldPos;
				spawnY = -START_PINES_DISTANCE;
				SpawnPinesLevel();
			}
			else
			{
				base.transform.position = new Vector3(0f, -START_PINES_DISTANCE, cachedAwakeWorldPos.z);
				seed = new Vector2(10f * UnityEngine.Random.value, 10f * UnityEngine.Random.value);
				spawnY = -START_PINES_DISTANCE;
				SpawnPinesEndless();
			}
		}

		protected override void OnStartRun(Run slide)
		{
			base.enabled = true;
			if (GameMode.IsEndless)
			{
				// 续命路径：OnContinue 已 ContinueWhooshCombo，此处禁止 Reset 冲掉连击
				if (preserveWhooshOnNextStart)
				{
					preserveWhooshOnNextStart = false;
				}
				else
				{
					Pine.ResetWhooshCombo();
				}
			}
		}

		protected override void OnEndRun()
		{
			base.enabled = false;
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

		private void Update()
		{
			if (GameMode.IsEndless)
			{
				SpawnPinesEndless();
			}
			CheckPines();
		}

		/// <summary>
		/// 关卡：刷到 FinishLine 附近一次性铺满。
		/// </summary>
		private void SpawnPinesLevel()
		{
			float num = 0f - FinishLine.GetDistance() + 3f;
			while (spawnY > num)
			{
				GeneratePineLineLevel(spawnY);
				spawnY -= 0.3f;
			}
			foreach (Pine pine in comingPines)
			{
				if (pine != null)
				{
					pine.SyncVisible();
				}
			}
		}

		/// <summary>
		/// 无尽：相机下界持续刷树；每帧限流，落后过多时 burst。
		/// </summary>
		private void SpawnPinesEndless()
		{
			float cursor = base.transform.position.y;
			float targetY = GameCamera.GetY() - SPAWN_AHEAD;
			float deficit = (cursor - targetY) / 0.3f;
			int maxLines = deficit > 20f ? BURST_MAX_LINES : MAX_LINES_PER_FRAME;
			int lines = 0;
			while (cursor > targetY && lines < maxLines)
			{
				GeneratePineLineEndless(cursor);
				cursor -= 0.3f;
				lines++;
			}
			base.transform.position = new Vector3(0f, cursor, base.transform.position.z);
			spawnY = cursor;
		}

		private float InverseSigmoid(float value)
		{
			return Mathf.Pow(0.5f * (ATanh(2f * value - 1f) * 10f + 1f), 1f);
		}

		private float ATanh(float x)
		{
			return (float)(Math.Log(1f + x) - Math.Log(1f - x)) * 0.5f;
		}

		private void GeneratePineLineLevel(float y)
		{
			// 关卡树概率经 DifficultyGates（总开关关闭时透传原 Sample 公式）。
			float num = DifficultyGates.GetPineLineChance();
			float num2 = Mathf.Floor((GameCamera.GetHorizontalLimit() + 1.3f) * 3.33333f) * 0.3f;
			float num3 = 0f - num2;
			float num4 = 0f;
			for (float num5 = num3; num5 <= num2; num5 += 0.3f)
			{
				if (UnityEngine.Random.value < num)
				{
					Pine pine = Recyclable<Pine>.Get();
					if (pine == null)
					{
						continue;
					}
					pine.Place(num5, y, num4);
					comingPines.Enqueue(pine);
					num4 += 0.01f;
				}
			}
			// 策略 A：两模式均保留滚石概率生成，以维持难度节奏（否决策略 B 静默关闭）。
			TrySpawnRollingStoneLevel(y);
		}

		private void GeneratePineLineEndless(float y)
		{
			EnsureEndlessCurve();
			float step = 0.3f;
			float num2 = Mathf.Floor((GameCamera.GetHorizontalLimit() + 2f) / step) * step;
			float num3 = 0f - num2;
			num2 += step * 0.5f;
			Vector2 vector = new Vector2(seed.x + num3, seed.y + y);
			// 用本行 y 算距离，避免同帧 burst 内 spawnY 尚未推进导致难度标量滞后
			float dist = Mathf.Min(0f - y, 400f);
			float scale = 2f + dist * 0.005f;
			float distChanceMul = dist < 5f ? dist * 0.008f : dist * 0.0002f + 0.04f;
			for (float num4 = num3; num4 < num2; num4 += step)
			{
				// 无尽难度用距离标量，禁止热路径读 Level.GetFloat()
				float chance = probabilityCurve.Evaluate(Mathf.PerlinNoise(vector.x * scale, vector.y * scale));
				chance *= distChanceMul;
				if (UnityEngine.Random.value < chance)
				{
					Pine pine = Recyclable<Pine>.Get();
					if (pine == null)
					{
						vector.x += step;
						continue;
					}
					pineSerial++;
					float zBias = (pineSerial % 1000) * 0.001f;
					pine.Place(num4, y, zBias);
					comingPines.Enqueue(pine);
				}
				vector.x += step;
			}
			// 策略 A：无尽仍按概率生成滚石（零引用 DifficultyGates，避免污染）。
			TrySpawnRollingStoneEndless(y);
		}

		/// <summary>
		/// 关卡刷石：概率走 Gates；仅 Override 开启时注入速度范围。
		/// </summary>
		private static void TrySpawnRollingStoneLevel(float y)
		{
			if (UnityEngine.Random.value < DifficultyGates.GetRollingStoneChance())
			{
				RollingStone stone = Recyclable<RollingStone>.Get();
				if (stone != null)
				{
					DifficultyGates.BindRollingStoneTarget(stone, y);
				}
			}
		}

		/// <summary>
		/// 无尽刷石：保持原版 Sample + 单参 SetTargetY。
		/// </summary>
		private static void TrySpawnRollingStoneEndless(float y)
		{
			if (UnityEngine.Random.value < Parameters.ROLLING_STONE_PROBABILITY.Sample())
			{
				RollingStone stone = Recyclable<RollingStone>.Get();
				if (stone != null)
				{
					stone.SetTargetY(y);
				}
			}
		}

		/// <summary>
		/// 生成器已推进距离（无尽游标 / 关卡 spawnY）。
		/// </summary>
		public static float GetDistance()
		{
			return 0f - spawnY;
		}

		private void CheckPines()
		{
			float num = Skier.GetY() - 0.3f;
			while (comingPines.Count > 0)
			{
				Pine peek = comingPines.Peek();
				if (peek == null)
				{
					comingPines.Dequeue();
					continue;
				}
				if (peek.GetY() > num)
				{
					Pine pine = comingPines.Dequeue();
					pine.ForceShowForDangerous();
					dangerousPines.Enqueue(pine);
					continue;
				}
				break;
			}
			foreach (Pine coming in comingPines)
			{
				if (coming != null)
				{
					coming.SyncVisible();
				}
			}
			float y = Skier.GetY();
			while (dangerousPines.Count > 0)
			{
				Pine peek = dangerousPines.Peek();
				if (peek == null)
				{
					dangerousPines.Dequeue();
					continue;
				}
				if (peek.GetY() > y)
				{
					whooshablePines.Enqueue(dangerousPines.Dequeue());
					continue;
				}
				break;
			}
			float x = Skier.GetX();
			foreach (Pine dangerousPine in dangerousPines)
			{
				if (dangerousPine == null)
				{
					continue;
				}
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
			while (whooshablePines.Count > 0)
			{
				Pine peek = whooshablePines.Peek();
				if (peek == null)
				{
					whooshablePines.Dequeue();
					continue;
				}
				if (peek.GetY() > num)
				{
					pendingDelete.Enqueue(whooshablePines.Dequeue());
					continue;
				}
				break;
			}
			foreach (Pine whooshablePine in whooshablePines)
			{
				if (whooshablePine == null)
				{
					continue;
				}
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
			while (pendingDelete.Count > 0)
			{
				Pine peek = pendingDelete.Peek();
				if (peek == null)
				{
					pendingDelete.Dequeue();
					continue;
				}
				if (peek.GetY() > num)
				{
					pendingDelete.Dequeue().Kill();
					continue;
				}
				break;
			}
		}

		protected override void OnContinue()
		{
			CleanClosePines(comingPines);
			CleanClosePines(dangerousPines);
			CleanClosePines(whooshablePines);
			CleanClosePines(pendingDelete);
			// 无尽续命：继承 Whoosh 连击；并标记下一次 StartRun 勿 Reset
			if (GameMode.IsEndless)
			{
				preserveWhooshOnNextStart = true;
				Pine.ContinueWhooshCombo();
			}
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
				if (pine == null)
				{
					continue;
				}
				float num2 = pine.GetX() - Skier.GetX();
				float num3 = pine.GetY() - Skier.GetY();
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
}
