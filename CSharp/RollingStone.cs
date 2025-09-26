using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public sealed class RollingStone : Recyclable<RollingStone>
{
	private static readonly HashSet<RollingStone> stones = new HashSet<RollingStone>();

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
		foreach (RollingStone stone in stones)
		{
			stone.Kill();
		}
		stones.Clear();
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
		SetSpeed();
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
		trail.enabled = false;
		powderSpread.Stop();
		stone.enabled = false;
		shadow.enabled = false;
		base.enabled = false;
	}

	public void SetTargetY(float y)
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

	private void SetSpeed()
	{
		float num = 20f + 40f * UnityEngine.Random.value;
		float num2 = Parameters.ROLLING_STONE_MIN_SPEED.Sample();
		num2 += (Parameters.ROLLING_STONE_MAX_SPEED.Sample() - num2) * UnityEngine.Random.value;
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
