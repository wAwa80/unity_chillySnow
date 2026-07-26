using System.Collections;
using UnityEngine;

namespace LevelMode
{

	[RequireComponent(typeof(SpriteRenderer), typeof(TrailRenderer), typeof(AudioSource))]
	public sealed class Skier : Singleton<Skier>
	{
		private SpriteRenderer spriteRenderer;

		private SpriteRenderer shadow;

		/// <summary>
		/// 夜间光晕；缺 Prefab 节点时为 null（降级）。
		/// // TODO: [User Action] 在 Skier 下增加名为 Glow 的子节点并挂 SpriteRenderer。
		/// </summary>
		private SpriteRenderer glow;

		private Color glowBaseColor = Color.white;

		private float glowIntensity = 1f;

		private TrailRenderer trail;

		private AudioSource source;

		private ParticleSystem speedParticles;

		private ParticleSystem feverParticles;

		private ParticleSystem feverDotParticles;

		private AudioSource feverSound;

		private ParticleSystem powderSpread;

		private ParticleSystem deathParticles;

		private AudioSource deathSound;

		private bool holdOnContinue;

		private static Color color;

		private static float x;

		private static float y;

		private int meters;

		private float yAcceleration;

		private float xSpeed;

		private static float ySpeed;

		private int speedChange;

		private bool isPressing;

		private bool longPress;

		private bool isTurning;

		private bool autoPlay;

		private readonly ParticleSystem.MinMaxCurve powderSpreadNotEmitting = new ParticleSystem.MinMaxCurve(0f);

		private ParticleSystem.MinMaxCurve powderSpreadEmitting;

		private float soundVolume;

		private float soundDecay;

		private float feverTime;

		private static bool inFever;

		private bool pulsing;

		protected override void Awake()
		{
			base.Awake();
			spriteRenderer = GetComponent<SpriteRenderer>();
			shadow = base.transform.GetChild(0).GetComponent<SpriteRenderer>();
			glow = ResolveGlowRenderer();
			if (glow != null)
			{
				glowBaseColor = glow.color;
				glow.enabled = false;
			}
			trail = GetComponent<TrailRenderer>();
			source = GetComponent<AudioSource>();
			speedParticles = base.transform.GetChild(1).GetComponent<ParticleSystem>();
			feverParticles = base.transform.GetChild(2).GetComponent<ParticleSystem>();
			feverDotParticles = feverParticles.transform.GetChild(0).GetComponent<ParticleSystem>();
			feverSound = feverParticles.GetComponent<AudioSource>();
			powderSpread = base.transform.GetChild(3).GetComponent<ParticleSystem>();
			deathParticles = base.transform.GetChild(4).GetComponent<ParticleSystem>();
			deathSound = deathParticles.GetComponent<AudioSource>();
			powderSpreadEmitting = new ParticleSystem.MinMaxCurve(WxPerf.GetPowderRate());
			base.enabled = false;
		}

		/// <summary>
		/// 按名解析 Glow，兼容缺节点（月光树影依赖 GetGlowIntensity）。
		/// </summary>
		private SpriteRenderer ResolveGlowRenderer()
		{
			Transform named = base.transform.Find("Glow");
			if (named != null)
			{
				return named.GetComponent<SpriteRenderer>();
			}
			return null;
		}

		protected override void OnRefresh()
		{
			base.transform.position = new Vector3(0f, 0f, 100f);
			x = 0f;
			y = 0f;
			meters = 0;
			spriteRenderer.enabled = true;
			glowIntensity = 1f;
			speedParticles.Stop();
			ResetPhysics();
			StopPowderSpread();
			ResetPowderSpreadSound();
			SetInFever(fever: false);
			SetSkierColor(Level.GetSkierColor());
			ApplyNightVisuals(NightModeButton.IsOn);
			ParticleSystem.MainModule main = powderSpread.main;
			main.startColor = Level.GetBackgroundColor() * 0.9f;
			autoPlay = false;
			trail.Clear();
			trail.time = 0f;
			trail.enabled = false;
		}

		protected override void OnStartRun(Run slide)
		{
			isPressing = true;
			trail.enabled = true;
			base.enabled = true;
			glowIntensity = 1f;
			soundVolume = 0.025f;
			source.volume = soundVolume;
			ApplyNightVisuals(NightModeButton.IsOn);
		}

		protected override void OnEndRun()
		{
			if (Neuron.GetCurrentRun().success)
			{
				autoPlay = true;
				soundDecay = 1f;
				return;
			}
			spriteRenderer.enabled = false;
			if (shadow != null)
			{
				shadow.enabled = false;
			}
			if (glow != null)
			{
				glow.enabled = false;
			}
			glowIntensity = 0f;
			speedParticles.Stop();
			StopPowderSpread();
			ResetPowderSpreadSound();
			deathParticles.Play();
			deathSound.Play();
			StopTurning();
			base.enabled = false;
			SetInFever(fever: false);
			Device.Vibrate(Vibration.Failure);
		}

		protected override void OnContinue()
		{
			holdOnContinue = true;
			x = 0f;
			base.transform.position = new Vector3(x, y, base.transform.position.z);
			speedParticles.Stop();
			ResetPhysics();
			StopPowderSpread();
			ResetPowderSpreadSound();
			SetInFever(fever: false);
			SetSkierColor(Level.GetSkierColor());
			glowIntensity = 1f;
			ApplyNightVisuals(NightModeButton.IsOn);
			ParticleSystem.MainModule main = powderSpread.main;
			main.startColor = Level.GetBackgroundColor() * 0.9f;
			autoPlay = false;
			base.transform.position = new Vector3(0f, base.transform.position.y, base.transform.position.z);
			// 契约 3A（写死）：续命同步 meters 为当前深度，禁止 meters=0。
			// 否则恢复 Update 后会按深度一次性补发 MeterPlusOne（无尽每米+1 / 关卡×关卡号刷分）。
			meters = Mathf.Max(0, Mathf.FloorToInt((0f - y) * 0.7f));
			trail.Clear();
			trail.enabled = false;
			spriteRenderer.enabled = true;
		}

		protected override void OnNightModeSwitched(bool enabled)
		{
			ApplyNightVisuals(enabled);
		}

		protected override void OnPause()
		{
			if (source != null && source.isPlaying)
			{
				source.Pause();
			}
		}

		protected override void OnUnpause()
		{
			if (source != null && Neuron.IsPlaying() && base.enabled)
			{
				source.UnPause();
			}
		}

		/// <summary>
		/// 夜间：开 glow、关地面影；日间相反。缺 Glow 时仅切 shadow。
		/// </summary>
		private void ApplyNightVisuals(bool night)
		{
			if (night)
			{
				if (shadow != null)
				{
					shadow.enabled = false;
				}
				if (glow != null)
				{
					SyncGlowColor();
					glow.enabled = spriteRenderer != null && spriteRenderer.enabled;
				}
			}
			else
			{
				if (glow != null)
				{
					glow.enabled = false;
				}
				if (shadow != null && spriteRenderer != null && spriteRenderer.enabled)
				{
					shadow.enabled = true;
				}
			}
		}

		private void SyncGlowColor()
		{
			if (glow == null)
			{
				return;
			}
			Color c = color;
			c.a = glowBaseColor.a;
			glowBaseColor = c;
			if (glowBaseColor.r < 0.2f)
			{
				glowBaseColor.r = 0.2f;
			}
			if (glowBaseColor.g < 0.2f)
			{
				glowBaseColor.g = 0.2f;
			}
			if (glowBaseColor.b < 0.2f)
			{
				glowBaseColor.b = 0.2f;
			}
			float max = Mathf.Max(glowBaseColor.r, glowBaseColor.g, glowBaseColor.b);
			if (max < 1f && max > 0f)
			{
				float inv = 1f / max;
				glowBaseColor.r *= inv;
				glowBaseColor.g *= inv;
				glowBaseColor.b *= inv;
			}
			glow.color = glowBaseColor;
		}

		/// <summary>
		/// 月光树影光强（对齐无尽 Player.GetGlowIntensity）。
		/// </summary>
		public float GetGlowIntensity()
		{
			return glowIntensity;
		}

		protected override void OnTap()
		{
			if (holdOnContinue)
			{
				holdOnContinue = false;
				OnStartRun(Neuron.GetCurrentRun());
			}
		}

		public static Color GetColor()
		{
			return color;
		}

		private void SetSkierColor(Color c)
		{
			color = c;
			spriteRenderer.color = c;
			ParticleSystem.MainModule main = speedParticles.main;
			main.startColor = c;
			main = feverParticles.main;
			main.startColor = color;
			main = feverDotParticles.main;
			main.startColor = color;
			main = deathParticles.main;
			main.startColor = color;
			c.a = 0.39f;
			trail.startColor = c;
			c.a = 0f;
			trail.endColor = c;
			if (NightModeButton.IsOn)
			{
				SyncGlowColor();
			}
		}

		public static float GetX()
		{
			return x;
		}

		public static float GetY()
		{
			return y;
		}

		public static float GetDistance()
		{
			return 0f - y;
		}

		public static float GetSpeedY()
		{
			return ySpeed;
		}

		public bool IsTurning()
		{
			return isTurning;
		}

		private void StopTurning()
		{
			isTurning = false;
		}

		private void ResetPhysics()
		{
			yAcceleration = -3f;
			xSpeed = 0f;
			ySpeed = -3f;
			speedChange = 1;
		}

		private void Update()
		{
			if (Neuron.IsPaused())
			{
				return;
			}
			if (trail.time < WxPerf.GetTrailTimeMax())
			{
				trail.time += 0.9f * Time.deltaTime;
				if (trail.time > WxPerf.GetTrailTimeMax())
				{
					trail.time = WxPerf.GetTrailTimeMax();
				}
			}
			float num = Parameters.SKIER_MANIABILITY_RELATIVE_TO_OLD_CHILLY_SNOW.Sample() * 400f;
			if (Neuron.IsPlaying())
			{
				if ((!autoPlay) ? Finger.IsPressing() : (!isTurning))
				{
					if (!isPressing)
					{
						isPressing = true;
						if (speedChange > 0)
						{
							speedChange = -1;
							powderSpread.transform.eulerAngles = new Vector3(0f, 0f, -50f);
						}
						else
						{
							speedChange = 1;
							powderSpread.transform.eulerAngles = new Vector3(0f, 0f, 50f);
						}
						isTurning = true;
						PlayPowderSpread();
						yAcceleration = 0.65f - num * 0.0005f;
					}
				}
				else if (isPressing)
				{
					if (longPress)
					{
						StopTurning();
					}
					longPress = false;
					isPressing = false;
					yAcceleration = -3f - num * 0.0075f;
					StopPowderSpread();
				}
			}
			if (Neuron.IsPlaying())
			{
				ySpeed += yAcceleration * Time.deltaTime;
				float num2 = -3f - num * 0.005f;
				if (ySpeed > num2)
				{
					ySpeed = num2;
				}
				float num3 = -5f - num * 0.005f;
				if (ySpeed < num3)
				{
					ySpeed = num3;
				}
				if (speedChange > 0)
				{
					float num4 = 3f + num * 0.005f;
					float num5 = 6f + num * 0.005f;
					if (xSpeed < num4)
					{
						xSpeed += (12f + 0.03f * num) * Time.deltaTime;
						if (xSpeed > num4)
						{
							if (isPressing)
							{
								longPress = true;
							}
							else
							{
								StopTurning();
								xSpeed = num4;
							}
						}
					}
					else if (isPressing && xSpeed < num5)
					{
						xSpeed += (12f + 0.03f * num) * Time.deltaTime;
						if (xSpeed > num5)
						{
							StopTurning();
							xSpeed = num5;
						}
					}
				}
				else
				{
					float num6 = -3f - num * 0.005f;
					float num7 = -6f - num * 0.005f;
					if (xSpeed > num6)
					{
						xSpeed -= (12f + 0.03f * num) * Time.deltaTime;
						if (xSpeed < num6)
						{
							if (isPressing)
							{
								longPress = true;
							}
							else
							{
								xSpeed = num6;
								StopTurning();
							}
						}
					}
					else if (isPressing && xSpeed > num7)
					{
						xSpeed -= (12f + 0.03f * num) * Time.deltaTime;
						if (xSpeed < num7)
						{
							xSpeed = num7;
							StopTurning();
						}
					}
				}
				float num8 = ySpeed * Time.deltaTime;
				float num9 = xSpeed * Time.deltaTime;
				base.transform.position = new Vector3(base.transform.position.x + num9, base.transform.position.y + num8, 1f * y);
				if (!autoPlay)
				{
					if (base.transform.position.x < 0f - GameCamera.GetHorizontalLimit() || base.transform.position.x > GameCamera.GetHorizontalLimit())
					{
						Neuron.EndRun();
					}
					else if (RollingStone.Collides(x, y))
					{
						Neuron.EndRun();
					}
					else
					{
						int num10 = Mathf.FloorToInt((0f - base.transform.position.y) * 0.7f);
						while (num10 > meters)
						{
							meters++;
							Neuron.MeterPlusOne();
						}
					}
				}
				if (!autoPlay)
				{
					UpdateFever();
				}
				UpdatePowderSpreadSound();
			}
			x = base.transform.position.x;
			y = base.transform.position.y;
			// 仅局内 + 关卡模式可触达终点；distance==0（未 Refresh）时不判定，避免菜单态空引用刷屏
			Run run = Neuron.GetCurrentRun();
			float finishDistance = FinishLine.GetDistance();
			if (Neuron.IsPlaying() && !autoPlay && GameMode.IsLevel && run != null &&
				finishDistance > 0.01f && y <= 0f - finishDistance)
			{
				run.success = true;
				Neuron.EndRun();
				Device.Vibrate(Vibration.Success);
			}
		}

		private void PlayPowderSpread()
		{
			powderSpreadEmitting = new ParticleSystem.MinMaxCurve(WxPerf.GetPowderRate());
			ParticleSystem.EmissionModule emission = powderSpread.emission;
			emission.rateOverTime = powderSpreadEmitting;
		}

		private void StopPowderSpread()
		{
			ParticleSystem.EmissionModule emission = powderSpread.emission;
			emission.rateOverTime = powderSpreadNotEmitting;
		}

		private void ResetPowderSpreadSound()
		{
			soundVolume = 0f;
			source.volume = 0f;
		}

		private void UpdatePowderSpreadSound()
		{
			if (!Device.IsSoundOn())
			{
				if (source.isPlaying)
				{
					source.Stop();
				}
			}
			else if (!source.isPlaying)
			{
				source.Play();
			}
			if (isTurning)
			{
				if (soundVolume < 0.1f)
				{
					soundVolume += 2f * Time.deltaTime;
					if (soundVolume > 0.1f)
					{
						soundVolume = 0.1f;
					}
				}
			}
			else if (soundVolume > 0.025f)
			{
				soundVolume -= 0.5f * Time.deltaTime;
				if (soundVolume < 0.025f)
				{
					soundVolume = 0.025f;
				}
			}
			if (autoPlay)
			{
				if (soundDecay > 0f)
				{
					soundDecay -= 0.5f * Time.deltaTime;
					if (soundDecay < 0f)
					{
						soundDecay = 0f;
					}
				}
				source.volume = soundDecay * soundVolume;
			}
			else
			{
				source.volume = soundVolume;
			}
		}

		public static bool IsInFever()
		{
			return inFever;
		}

		protected override void OnWhoosh(int points)
		{
			feverTime = Time.time;
			if (!inFever && Pine.GetWhooshCombo() >= 4)
			{
				SetInFever(fever: true);
			}
		}

		private void SetInFever(bool fever)
		{
			if (inFever == fever)
			{
				return;
			}
			inFever = fever;
			if (fever)
			{
				SetSkierColor(Level.GetFeverColor());
				feverParticles.Play();
				feverSound.Play();
				// 陪玩语音：仅在进入 Fever 边沿通知一次（退出不播）
				VoiceCompanion.NotifyFeverEntered();
				if (!pulsing)
				{
					StartCoroutine(Pulse());
				}
			}
			else
			{
				SetSkierColor(Level.GetSkierColor());
				feverParticles.Stop();
			}
		}

		private void UpdateFever()
		{
			if (inFever && Time.time - feverTime > 3f)
			{
				SetInFever(fever: false);
			}
		}

		private IEnumerator Pulse()
		{
			pulsing = true;
			while (inFever)
			{
				Device.Vibrate(Vibration.Micro);
				yield return new WaitForSeconds(0.33f);
				if (!inFever)
				{
					break;
				}
				Device.Vibrate(Vibration.Micro);
				yield return new WaitForSeconds(0.67f);
			}
			pulsing = false;
		}
	}
}
