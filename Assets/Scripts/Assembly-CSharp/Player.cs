using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(TrailRenderer), typeof(AudioSource))]
public sealed class Player : Singleton<Player>
{
	public enum FeverState
	{
		None,
		Fever,
		MegaFever
	}

	private SpriteRenderer spriteRenderer;

	private TrailRenderer trail;

	private AudioSource source;

	private SpriteRenderer shadow;

	private ParticleSystem powderSpread;

	private ParticleSystem deathParticles;

	private AudioSource deathSound;

	private SpriteRenderer glow;

	private bool didNotValidateContinue;

	private bool didContinue;

	private int meters;

	private float yAcceleration;

	private float xSpeed;

	private float ySpeed;

	private int speedChange;

	private bool isPressing;

	private bool longPress;

	private bool isTurning;

	private readonly ParticleSystem.MinMaxCurve powderSpreadNotEmitting = new ParticleSystem.MinMaxCurve(0f);

	private readonly ParticleSystem.MinMaxCurve powderSpreadEmitting = new ParticleSystem.MinMaxCurve(100f);

	private ParticleSystem feverParticles;

	private AudioSource feverSound;

	private ParticleSystem megaFeverParticles;

	private AudioSource megaFeverSound;

	private Color baseColor;

	private Color feverColor;

	private Color megaFeverColor;

	private float feverTime;

	private FeverState feverState;

	private const float MIN_SKI_VOLUME = 0.025f;

	private const float MAX_SKI_VOLUME = 0.1f;

	private Color skinColor;

	private Color backgroundColor;

	private float glowIntensity = 1f;

	private Color glowBaseColor;

	public static bool IsABTestDestroyPines;

	private float destroyFeverTime;

	private bool feverWillStop;

	private float feverWillStopIn;

	private Color lastFeverColor;

	protected override void Awake()
	{
		base.Awake();
		spriteRenderer = GetComponent<SpriteRenderer>();
		trail = GetComponent<TrailRenderer>();
		source = GetComponent<AudioSource>();
		shadow = base.transform.GetChild(0).GetComponent<SpriteRenderer>();
		powderSpread = base.transform.GetChild(1).GetComponent<ParticleSystem>();
		deathParticles = base.transform.GetChild(2).GetComponent<ParticleSystem>();
		deathSound = deathParticles.GetComponent<AudioSource>();
		feverParticles = base.transform.GetChild(3).GetComponent<ParticleSystem>();
		feverSound = feverParticles.GetComponent<AudioSource>();
		megaFeverParticles = base.transform.GetChild(4).GetComponent<ParticleSystem>();
		megaFeverSound = megaFeverParticles.GetComponent<AudioSource>();
		glow = base.transform.GetChild(5).GetComponent<SpriteRenderer>();
		glowBaseColor = glow.color;
		baseColor = spriteRenderer.color;
		feverColor = Utility.HexToColor("#ffad00");
		megaFeverColor = Utility.HexToColor("#d7331c");
		IsABTestDestroyPines = Analytics.GetCohort() == "DestroyPines";
	}

	private void Start()
	{
		OnBackToMenu();
	}

	protected override void OnBackToMenu()
	{
		ResetTrail();
		base.transform.position = new Vector3(0f, 0f, base.transform.position.z);
		meters = 0;
		spriteRenderer.enabled = true;
		if (!NightModeButton.nightModeOn)
		{
			shadow.enabled = true;
		}
		glowIntensity = 1f;
		glow.color = glowBaseColor;
		ResetPhysics();
	}

	protected override void OnGameOver(bool canUseSecondChance)
	{
		spriteRenderer.enabled = false;
		if (!NightModeButton.nightModeOn)
		{
			shadow.enabled = false;
		}
		StopPowderSpread();
		if (isPressing)
		{
			isPressing = false;
			Stats.AddPress(!longPress);
			longPress = false;
		}
		StopTurning();
		SetFeverState(FeverState.None);
		ResetSound();
		if (canUseSecondChance || didContinue)
		{
			deathParticles.Play();
			deathSound.Play();
            //调用一个自定义的封装方法来触发一次移动设备的振动。
            //Device.Vibrate(Device.Vibration.Light);
        }
    }

	protected override void OnNewGame()
	{
		trail.enabled = true;
		didContinue = false;
	}

	protected override void OnContinue()
	{
		didNotValidateContinue = true;
		SetFeverState(FeverState.None);
		ResetTrail();
		base.transform.position = new Vector3(0f, base.transform.position.y, base.transform.position.z);
		meters = 0;
		spriteRenderer.enabled = true;
		if (!NightModeButton.nightModeOn)
		{
			shadow.enabled = true;
		}
		glowIntensity = 1f;
		glow.color = glowBaseColor;
		ResetPhysics();
		OnNewGame();
		didContinue = true;
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
		speedChange = -1;
	}

	private void Update()
	{
		float num = Mathf.Min(Singleton<PineGenerator>.i.GetDistance(), 400f);
		if (App.GetState() == App.State.Playing)
		{
			if (Singleton<MenuPage>.i.IsPressing())
			{
				if (!isPressing)
				{
					if (didNotValidateContinue)
					{
						didNotValidateContinue = false;
					}
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
				Stats.AddPress(!longPress);
				longPress = false;
				isPressing = false;
				yAcceleration = -3f - num * 0.0075f;
				StopPowderSpread();
			}
		}
		if (App.GetState() == App.State.Playing && !didNotValidateContinue)
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
			base.transform.position = new Vector3(base.transform.position.x + num9, base.transform.position.y + num8, base.transform.position.z);
			if (base.transform.position.x < 0f - Singleton<GameCamera>.i.GetHorizontalLimit() || base.transform.position.x > Singleton<GameCamera>.i.GetHorizontalLimit())
			{
				Neuron.GameOver(!didContinue);
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
			UpdateFever();
			UpdateSound();
			if (IsABTestDestroyPines)
			{
				UpdateFeverWillStop();
			}
		}
		else
		{
			UpdateGlow();
		}
	}

	private void PlayPowderSpread()
	{
		if (App.IsRelease() || !DebugPage.dontThrowPowder)
		{
			ParticleSystem.EmissionModule emission = powderSpread.emission;
			emission.rateOverTime = powderSpreadEmitting;
		}
	}

	private void StopPowderSpread()
	{
		ParticleSystem.EmissionModule emission = powderSpread.emission;
		emission.rateOverTime = powderSpreadNotEmitting;
	}

	public Color GetSkinColor()
	{
		return skinColor;
	}

	public FeverState GetFeverState()
	{
		return feverState;
	}

	public Color GetFeverColor(FeverState state)
	{
		switch (state)
		{
		case FeverState.None:
			return baseColor;
		case FeverState.Fever:
			return feverColor;
		default:
			return megaFeverColor;
		}
	}

	protected override void OnWhoosh()
	{
		if (!IsABTestDestroyPines)
		{
			feverTime = Time.time;
		}
		if (!IsABTestDestroyPines || !feverWillStop)
		{
			SetFeverState(NextFeverState());
		}
	}

	public FeverState NextFeverState()
	{
		if (!App.IsRelease() && DebugPage.dontFever)
		{
			return FeverState.None;
		}
		if (Pine.GetWhooshPoints() >= 12 || (Pine.GetWhooshPoints() >= 8 && IsABTestDestroyPines))
		{
			return FeverState.MegaFever;
		}
		if (Pine.GetWhooshPoints() >= 6 && !IsABTestDestroyPines)
		{
			return FeverState.Fever;
		}
		return FeverState.None;
	}

	private void SetFeverState(FeverState state)
	{
		if (feverState == state)
		{
			return;
		}
		feverState = state;
		if (feverState == FeverState.None)
		{
			if (IsABTestDestroyPines)
			{
				Pine.ResetWhooshCombo();
			}
			SetColor(skinColor);
			feverParticles.Stop();
			megaFeverParticles.Stop();
			Singleton<MenuScores>.i.SetScoreColor(baseColor);
			return;
		}
		if (feverState == FeverState.Fever)
		{
			SetColor(feverColor);
			feverParticles.Play();
			megaFeverParticles.Stop();
			feverSound.Play();
			Singleton<MenuScores>.i.SetScoreColor(feverColor);
			return;
		}
		if (IsABTestDestroyPines)
		{
			feverTime = Time.time;
			destroyFeverTime = Time.time;
		}
		SetColor(megaFeverColor, lightenUp: true);
		feverParticles.Stop();
		megaFeverParticles.Play();
		megaFeverSound.Play();
		Singleton<MenuScores>.i.SetScoreColor(megaFeverColor);
	}

	private void ResetTrail()
	{
		trail.enabled = false;
		trail.Clear();
	}

	private void UpdateFever()
	{
		if (IsABTestDestroyPines)
		{
			if (feverState != 0 && !feverWillStop && (Time.time - feverTime > 8f || Time.time - destroyFeverTime > 3f))
			{
				FeverWillStop();
			}
		}
		else if (feverState != 0 && Time.time - feverTime > 3f)
		{
			SetFeverState(FeverState.None);
		}
	}

	private void ResetSound()
	{
		source.volume = 0f;
	}

	private void UpdateSound()
	{
		if (isTurning)
		{
			if (source.volume < 0.1f)
			{
				source.volume += 2f * Time.deltaTime;
				if (source.volume > 0.1f)
				{
					source.volume = 0.1f;
				}
			}
		}
		else if (source.volume > 0.025f)
		{
			source.volume -= 0.5f * Time.deltaTime;
			if (source.volume < 0.025f)
			{
				source.volume = 0.025f;
			}
		}
	}

	protected override void OnPause()
	{
		source.Stop();
	}

	protected override void OnUnpause()
	{
		source.Play();
	}

	protected override void OnNightModeSwitched(bool enabled)
	{
		if (enabled)
		{
			glow.enabled = true;
			shadow.enabled = false;
			SyncGlowColor();
		}
		else
		{
			glow.enabled = false;
			shadow.enabled = true;
		}
		SyncPowderSpreadColor(skinColor);
	}

	protected override void OnSkinSelected(Skin s)
	{
		if (s.GetSkinType() == Skin.Type.Ball)
		{
			skinColor = s.GetColor();
			if (NightModeButton.nightModeOn)
			{
				SyncGlowColor();
				SyncPowderSpreadColor(skinColor);
			}
			ParticleSystem.MainModule main = deathParticles.main;
			main.startColor = new ParticleSystem.MinMaxGradient(skinColor);
			SyncTrailColor(skinColor);
		}
		else if (s.GetSkinType() == Skin.Type.Background)
		{
			backgroundColor = s.GetColor();
			SyncPowderSpreadColor(skinColor);
		}
	}

	private void SyncGlowColor()
	{
		Color color = skinColor;
		color.a = glowBaseColor.a;
		glowBaseColor = color;
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
		float num = Mathf.Max(glowBaseColor.r, glowBaseColor.g, glowBaseColor.b);
		if (num < 1f)
		{
			num = 1f / num;
			glowBaseColor.r *= num;
			glowBaseColor.g *= num;
			glowBaseColor.b *= num;
		}
		glow.color = glowBaseColor;
	}

	private void SyncPowderSpreadColor(Color color)
	{
		ParticleSystem.MainModule main = powderSpread.main;
		ParticleSystem.ColorOverLifetimeModule colorOverLifetime = powderSpread.colorOverLifetime;
		if (NightModeButton.nightModeOn)
		{
			main.startColor = new ParticleSystem.MinMaxGradient(color);
			colorOverLifetime.enabled = true;
			return;
		}
		Color color2 = backgroundColor;
		color2.r *= 0.9f;
		color2.g *= 0.9f;
		color2.b *= 0.9f;
		main.startColor = new ParticleSystem.MinMaxGradient(color2);
		colorOverLifetime.enabled = false;
	}

	private void SyncTrailColor(Color c)
	{
		GradientColorKey[] colorKeys = new GradientColorKey[2]
		{
			new GradientColorKey(c, 0f),
			new GradientColorKey(c, 1f)
		};
		GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2]
		{
			new GradientAlphaKey(0.39f, 0f),
			new GradientAlphaKey(0f, 1f)
		};
		trail.colorGradient = new Gradient
		{
			mode = GradientMode.Blend,
			colorKeys = colorKeys,
			alphaKeys = alphaKeys
		};
	}

	public float GetGlowIntensity()
	{
		return glowIntensity;
	}

	private void UpdateGlow()
	{
		if (App.GetState() == App.State.GameOver && glowIntensity > 0f)
		{
			glowIntensity -= Time.deltaTime;
			if (glowIntensity < 0f)
			{
				glowIntensity = 0f;
			}
			Color color = glow.color;
			color.a = glowIntensity * glowBaseColor.a;
			glow.color = color;
		}
	}

	private void SetColor(Color c, bool lightenUp = false)
	{
		spriteRenderer.color = c;
		SyncPowderSpreadColor(c);
		SyncTrailColor(c);
		if (lightenUp)
		{
			glow.color = new Color(c.r + 0.2f, c.g + 0.2f, c.b + 0.2f, glow.color.a);
			return;
		}
		c.a = glow.color.a;
		glow.color = c;
	}

	public void PineDestroyed()
	{
		destroyFeverTime = Time.time;
		Singleton<GameCamera>.i.Shake();
		Device.Vibrate(Device.Vibration.Light);
	}

	private void FeverWillStop()
	{
		if (!feverWillStop)
		{
			lastFeverColor = spriteRenderer.color;
			feverWillStop = true;
			feverWillStopIn = Time.time;
			SetColor(skinColor);
			feverParticles.Stop();
			megaFeverParticles.Stop();
			Singleton<MenuScores>.i.SetScoreColor(baseColor);
		}
	}

	private void UpdateFeverWillStop()
	{
		float num = Time.time - feverWillStopIn;
		if (!feverWillStop)
		{
			if (feverState != 0)
			{
				SetColor(Color.Lerp(feverColor, megaFeverColor, Mathf.Cos(num * 5f * (float)Math.PI) * 0.5f + 0.5f));
			}
		}
		else if (num >= 0.5f)
		{
			feverWillStop = false;
			SetFeverState(FeverState.None);
		}
	}
}
