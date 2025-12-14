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

	private float trailWidthMultiplier;

	private TrailRenderer feverTrail;

	private ParticleSystem feverParticles;

	private AudioSource feverSound;

	private TrailRenderer megaFeverTrail;

	private ParticleSystem megaFeverParticles;

	private AudioSource megaFeverSound;

	private Color baseColor;

	private Color feverColor;

	private Color megaFeverColor;

	private float feverTime;

	private FeverState feverState = FeverState.Fever;

	private const float MIN_SKI_VOLUME = 0.025f;

	private const float MAX_SKI_VOLUME = 0.1f;

	private float glowIntensity = 1f;

	private Color glowBaseColor;

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
		feverTrail = feverParticles.GetComponent<TrailRenderer>();
		megaFeverParticles = base.transform.GetChild(4).GetComponent<ParticleSystem>();
		megaFeverSound = megaFeverParticles.GetComponent<AudioSource>();
		megaFeverTrail = megaFeverParticles.GetComponent<TrailRenderer>();
		glow = base.transform.GetChild(5).GetComponent<SpriteRenderer>();
		glowBaseColor = glow.color;
		trailWidthMultiplier = trail.widthMultiplier;
		baseColor = spriteRenderer.color;
		feverColor = Utility.HexToColor("#ffad00");
		megaFeverColor = Utility.HexToColor("#d7331c");
		if (Analytics.GetCohort() == "BiggerBall")
		{
			base.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
		}
	}

	private void Start()
	{
		OnBackToMenu();
	}

	protected override void OnBackToMenu()
	{
		SetFeverState(FeverState.None);
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
		}
		Device.Vibrate(Device.Vibration.Medium);
	}

	protected override void OnNewGame()
	{
		trail.enabled = true;
		feverTrail.enabled = true;
		megaFeverTrail.enabled = true;
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
		if (App.GetState() == App.State.Menu || App.GetState() == App.State.Playing)
		{
			if (Singleton<MenuPage>.i.IsPressing())
			{
				if (!isPressing)
				{
					if (App.GetState() == App.State.Menu)
					{
						Neuron.NewGame();
					}
					else if (didNotValidateContinue)
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

	public FeverState GetFeverState()
	{
		return feverState;
	}

	public Color GetFeverColor(FeverState state)
	{
		return state switch
		{
			FeverState.None => baseColor, 
			FeverState.Fever => feverColor, 
			_ => megaFeverColor, 
		};
	}

	protected override void OnWhoosh()
	{
		feverTime = Time.time;
		SetFeverState(NextFeverState());
	}

	public FeverState NextFeverState()
	{
		if ((!App.IsRelease() && DebugPage.dontFever) || Analytics.GetCohort() == "NoPerfect")
		{
			return FeverState.None;
		}
		if (Pine.GetWhooshPoints() >= 16 && Analytics.GetCohort() == "MegaFever")
		{
			return FeverState.MegaFever;
		}
		if (Pine.GetWhooshPoints() >= 8 || (Pine.GetWhooshPoints() >= 6 && Analytics.GetCohort() == "SimplerPerfect"))
		{
			return FeverState.Fever;
		}
		return FeverState.None;
	}

	private void SetFeverState(FeverState state)
	{
		if (feverState != state)
		{
			feverState = state;
			if (feverState == FeverState.None)
			{
				spriteRenderer.color = baseColor;
				trail.widthMultiplier = trailWidthMultiplier;
				feverTrail.widthMultiplier = 0f;
				megaFeverTrail.widthMultiplier = 0f;
				feverParticles.Stop();
				megaFeverParticles.Stop();
				Singleton<MenuScores>.i.SetScoreColor(baseColor);
				Color color = baseColor;
				color.a = glow.color.a;
				glow.color = color;
			}
			else if (feverState == FeverState.Fever)
			{
				spriteRenderer.color = feverColor;
				trail.widthMultiplier = 0f;
				feverTrail.widthMultiplier = trailWidthMultiplier;
				megaFeverTrail.widthMultiplier = 0f;
				feverParticles.Play();
				megaFeverParticles.Stop();
				feverSound.Play();
				Singleton<MenuScores>.i.SetScoreColor(feverColor);
				Color color2 = feverColor;
				color2.a = glow.color.a;
				glow.color = color2;
			}
			else
			{
				spriteRenderer.color = megaFeverColor;
				trail.widthMultiplier = 0f;
				feverTrail.widthMultiplier = 0f;
				megaFeverTrail.widthMultiplier = trailWidthMultiplier;
				feverParticles.Stop();
				megaFeverParticles.Play();
				megaFeverSound.Play();
				Singleton<MenuScores>.i.SetScoreColor(megaFeverColor);
				Color color3 = megaFeverColor;
				color3.a = glow.color.a;
				glow.color = color3;
			}
		}
	}

	private void ResetTrail()
	{
		trail.enabled = false;
		feverTrail.enabled = false;
		megaFeverTrail.enabled = false;
		trail.Clear();
		feverTrail.Clear();
		megaFeverTrail.Clear();
	}

	private void UpdateFever()
	{
		if (feverState != 0 && Time.time - feverTime > 3f)
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
			ParticleSystem.ColorOverLifetimeModule colorOverLifetime = powderSpread.colorOverLifetime;
			colorOverLifetime.enabled = true;
		}
		else
		{
			glow.enabled = false;
			shadow.enabled = true;
			ParticleSystem.ColorOverLifetimeModule colorOverLifetime2 = powderSpread.colorOverLifetime;
			colorOverLifetime2.enabled = false;
		}
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
}
