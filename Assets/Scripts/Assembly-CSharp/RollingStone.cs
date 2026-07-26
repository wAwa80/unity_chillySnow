using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelMode
{

	[RequireComponent(typeof(TrailRenderer))]
	public sealed class RollingStone : Recyclable<RollingStone>
	{
		private static readonly HashSet<RollingStone> stones = new HashSet<RollingStone>();

		/// <summary>
		/// KillAll 快照，避免遍历时 OnDisabled 修改 stones 导致枚举器异常。
		/// </summary>
		private static readonly List<RollingStone> killSnapshot = new List<RollingStone>();

		private TrailRenderer trail;

		private SpriteRenderer stone;

		private SpriteRenderer shadow;

		private ParticleSystem powderSpread;

		private float size;

		private float x;

		private float y;

		private float angularSpeed;

		private float targetY;

		private bool triggered;

		private float speedX;

		private float speedY;

		private float speedSlope;

		public static void KillAll()
		{
			killSnapshot.Clear();
			killSnapshot.AddRange(stones);
			stones.Clear();
			for (int i = 0; i < killSnapshot.Count; i++)
			{
				if (killSnapshot[i] != null)
				{
					killSnapshot[i].Kill();
				}
			}
		}

		public static bool Collides(float x, float y)
		{
			foreach (RollingStone stone in stones)
			{
				float num = x - stone.x;
				float num2 = y - stone.y;
				if (num * num + num2 * num2 < 0.25f * stone.size)
				{
					return true;
				}
			}
			return false;
		}

		protected override void Awake()
		{
			base.Awake();
			trail = GetComponent<TrailRenderer>();
			stone = base.transform.GetChild(2).GetComponent<SpriteRenderer>();
			shadow = base.transform.GetChild(0).GetComponent<SpriteRenderer>();
			powderSpread = base.transform.GetChild(1).GetComponent<ParticleSystem>();
		}

		protected override void OnEnabled()
		{
			powderSpread.Play();
			stone.enabled = true;
			shadow.enabled = true;
			stones.Add(this);
			size = 0.6f + 0.5f * UnityEngine.Random.value;
			base.transform.localScale = new Vector3(size, size, size);
			// 速度改在 SetTargetY 内计算一次，避免 OnEnabled 与注入重载双次 Random。
			Color color = Level.GetBackgroundColor() * 0.9f;
			ParticleSystem.MainModule main = powderSpread.main;
			main.startColor = color;
			trail.startColor = color;
			color.a = 0f;
			trail.endColor = color;
			base.enabled = true;
			triggered = false;
		}

		protected override void OnDisabled()
		{
			// 必须先从活跃集合移除，否则 WarmUp/Kill 后可能留下隐形石碰撞
			stones.Remove(this);
			trail.enabled = false;
			powderSpread.Stop();
			stone.enabled = false;
			shadow.enabled = false;
			base.enabled = false;
		}

		/// <summary>
		/// 契约：Recyclable.Get() 之后必须同帧调用本方法（或带速度重载），否则速度未初始化。
		/// 使用 Parameters 原版速度取样（无尽 / 关卡 Override 关闭）。
		/// </summary>
		public void SetTargetY(float y)
		{
			SetSpeed(Parameters.ROLLING_STONE_MIN_SPEED.Sample(), Parameters.ROLLING_STONE_MAX_SPEED.Sample());
			ApplyTargetPlacement(y);
		}

		/// <summary>
		/// 关卡难度覆盖开启时注入速度范围；内部只 Random 一次。
		/// </summary>
		public void SetTargetY(float y, float minSpeed, float maxSpeed)
		{
			SetSpeed(minSpeed, maxSpeed);
			ApplyTargetPlacement(y);
		}

		private void ApplyTargetPlacement(float y)
		{
			targetY = y;
			if (speedX > 0f)
			{
				x = 0f - GameCamera.GetHorizontalLimit() - 5f;
				this.y = y - speedY * x / speedX;
			}
			else
			{
				x = GameCamera.GetHorizontalLimit() + 5f;
				this.y = y - speedY * x / speedX;
			}
			base.transform.position = new Vector3(x, this.y, 1f * this.y);
			trail.Clear();
			trail.enabled = true;
		}

		private void Update()
		{
			if (!Neuron.IsPlaying())
			{
				return;
			}
			if (!triggered)
			{
				if (!(y + (Skier.GetY() - targetY) * speedY / Skier.GetSpeedY() > targetY - speedSlope * Skier.GetX()))
				{
					return;
				}
				triggered = true;
			}
			x += speedX * Time.deltaTime;
			y -= speedY * Time.deltaTime;
			if (y > GameCamera.GetUpperY())
			{
				Kill();
				return;
			}
			base.transform.position = new Vector3(x, y, 1f * y);
			Vector3 eulerAngles = stone.transform.eulerAngles;
			eulerAngles.z += angularSpeed * Time.deltaTime;
			stone.transform.eulerAngles = eulerAngles;
		}

		private void SetSpeed(float minSpeed, float maxSpeed)
		{
			if (maxSpeed < minSpeed)
			{
				float swap = minSpeed;
				minSpeed = maxSpeed;
				maxSpeed = swap;
			}
			float num = 20f + 40f * UnityEngine.Random.value;
			float num2 = minSpeed + (maxSpeed - minSpeed) * UnityEngine.Random.value;
			speedY = num2 * Mathf.Sin(num * ((float)Math.PI / 180f));
			if (UnityEngine.Random.value > 0.5f)
			{
				speedX = num2 * Mathf.Cos(num * ((float)Math.PI / 180f));
				Vector3 eulerAngles = powderSpread.transform.eulerAngles;
				eulerAngles.z = 90f - num;
				powderSpread.transform.eulerAngles = eulerAngles;
			}
			else
			{
				speedX = (0f - num2) * Mathf.Cos(num * ((float)Math.PI / 180f));
				Vector3 eulerAngles2 = powderSpread.transform.eulerAngles;
				eulerAngles2.z = -90f + num;
				powderSpread.transform.eulerAngles = eulerAngles2;
			}
			angularSpeed = -180f * speedX / size;
			speedSlope = speedY / speedX;
		}
	}
}
