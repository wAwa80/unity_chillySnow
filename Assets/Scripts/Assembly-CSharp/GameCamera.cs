using UnityEngine;

[RequireComponent(typeof(Camera))]
public sealed class GameCamera : Singleton<GameCamera>
{
	private Camera thisCamera;

	private float horizontalLimit;

	private float amplitude;

	private float fadeSpeed;

	private float frequencyX;

	private float frequencyY;

	private const float OFFSET = 3f;

	private bool refusedSecondChance;

	private float transitionTimer = 0.5f;

	private readonly AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	private Color dayColor;

	private Color nightColor;

	protected override void Awake()
	{
		base.Awake();
		thisCamera = GetComponent<Camera>();
		OnBackToMenu();
		horizontalLimit = base.transform.position.x - thisCamera.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x;
		dayColor = thisCamera.backgroundColor;
		nightColor = Utility.HexToColor("#0A0226");
	}

	public Camera GetCamera()
	{
		return thisCamera;
	}

	public float GetHorizontalLimit()
	{
		return horizontalLimit;
	}

	public override int GetPriority()
	{
		return 1;
	}

	public void Shake(float amplitude = 0.01f, float frequency = 40f, float fadeSpeed = 0.02f)
	{
		if (App.IsRelease() || !DebugPage.dontShake)
		{
			this.amplitude = amplitude;
			this.fadeSpeed = fadeSpeed;
			frequencyX = 1.52664f * frequency;
			frequencyY = 1.79246f * frequency;
		}
	}

	private void OnPreCull()
	{
		if (amplitude > 0.001f)
		{
			amplitude -= Time.deltaTime * fadeSpeed;
			if (amplitude < 0.001f)
			{
				amplitude = 0f;
				thisCamera.rect = new Rect(0f, 0f, 1f, 1f);
			}
			else
			{
				thisCamera.rect = new Rect(amplitude * Mathf.Cos(Time.time * frequencyX), amplitude * Mathf.Sin(Time.time * frequencyY), 1f, 1f);
			}
		}
	}

	protected override void OnBackToMenu()
	{
		base.transform.position = new Vector3(base.transform.position.x, Singleton<Player>.i.transform.position.y - 3f, base.transform.position.z);
		refusedSecondChance = false;
	}

	protected override void OnGameOver(bool canUseSecondChance)
	{
		if (canUseSecondChance || !refusedSecondChance)
		{
			refusedSecondChance = true;
			Shake(0.02f, 40f, 0.05f);
			base.transform.position = new Vector3(base.transform.position.x, Singleton<Player>.i.transform.position.y - 3f, base.transform.position.z);
		}
	}

	protected override void OnContinue()
	{
		refusedSecondChance = false;
	}

	private void LateUpdate()
	{
		if (App.GetState() == App.State.Playing)
		{
			base.transform.position = new Vector3(base.transform.position.x, Singleton<Player>.i.transform.position.y - 3f, base.transform.position.z);
		}
		else
		{
			if (!(transitionTimer >= 0f))
			{
				return;
			}
			if (transitionTimer < 0.5f)
			{
				transitionTimer += Time.deltaTime;
				if (transitionTimer > 0.5f)
				{
					transitionTimer = 0.5f;
				}
				base.transform.position = new Vector3(base.transform.position.x, Singleton<Player>.i.transform.position.y - 3f + transitionCurve.Evaluate(transitionTimer) * 2f, base.transform.position.z);
			}
			else if (transitionTimer < 1f)
			{
				transitionTimer += Time.deltaTime;
				if (transitionTimer > 1f)
				{
					transitionTimer = -1f;
					base.transform.position = new Vector3(base.transform.position.x, Singleton<Player>.i.transform.position.y - 3f, base.transform.position.z);
				}
				else
				{
					base.transform.position = new Vector3(base.transform.position.x, Singleton<Player>.i.transform.position.y - 3f - (1f - transitionCurve.Evaluate(transitionTimer)) * 2f, base.transform.position.z);
				}
			}
		}
	}

	public void Transit()
	{
		transitionTimer = 0f;
	}

	protected override void OnNightModeSwitched(bool enabled)
	{
		if (enabled)
		{
			thisCamera.backgroundColor = nightColor;
		}
		else
		{
			thisCamera.backgroundColor = dayColor;
		}
	}

	protected override void OnSkinSelected(Skin skin)
	{
		if (skin.GetSkinType() == Skin.Type.Background)
		{
			dayColor = skin.GetColor();
			if (!NightModeButton.nightModeOn)
			{
				thisCamera.backgroundColor = dayColor;
			}
		}
	}
}
