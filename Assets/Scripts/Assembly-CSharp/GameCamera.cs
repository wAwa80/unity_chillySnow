using UnityEngine;

[RequireComponent(typeof(Camera))]
public sealed class GameCamera : Singleton<GameCamera>
{
	private const string LEADERBOARD_ID = "";

	private Camera thisCamera;

	private SpriteRenderer transitionCover;

	private static float horizontalLimit;

	private float z;

	private float amplitude;

	private float fadeSpeed;

	private float frequencyX;

	private float frequencyY;

	private static float y;

	private static float upperY;

	private bool transiting;

	private float transitionTimer;

	private float transitionOffset;

	private bool dontFollow;

	private bool wasSuccess;

	private bool continued;

	protected override void Awake()
	{
		base.Awake();
		thisCamera = GetComponent<Camera>();
		transitionCover = base.transform.GetChild(0).GetComponent<SpriteRenderer>();
		transitionCover.color = Utility.HexToColor("fff8f5");
		horizontalLimit = base.transform.position.x - thisCamera.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x;
		z = base.transform.position.z;
	}

	public Camera GetCamera()
	{
		return thisCamera;
	}

	public static float GetHorizontalLimit()
	{
		return horizontalLimit;
	}

	public override int GetPriority()
	{
		return 1;
	}

	public static void Shake(float amplitude, float frequency, float fadeSpeed)
	{
		Singleton<GameCamera>.i.ShakeCamera(amplitude, frequency, fadeSpeed);
	}

	private void ShakeCamera(float amplitude, float frequency, float fadeSpeed)
	{
		this.amplitude = amplitude;
		this.fadeSpeed = fadeSpeed;
		frequencyX = 1.52664f * frequency;
		frequencyY = 1.79246f * frequency;
	}

	private void OnPreCull()
	{
		if (!(amplitude < 0.001f))
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

	public static float GetY()
	{
		return y;
	}

	public static float GetUpperY()
	{
		return upperY;
	}

	private void LateUpdate()
	{
		if (transiting)
		{
			if (transitionTimer < 0.5f)
			{
				transitionTimer += Time.deltaTime * 1f;
				if (transitionTimer >= 0.5f)
				{
					transitionTimer = 0.5f;
					transiting = false;
					TryShowInterstitial();
				}
				if (Neuron.GetCurrentRun().success)
				{
					transitionOffset = 0f - Parameters.END_GAME_TRANSITION_CURVE.Evaluate(transitionTimer);
				}
				else
				{
					transitionOffset = Parameters.END_GAME_TRANSITION_CURVE.Evaluate(transitionTimer);
				}
			}
			else
			{
				transitionTimer += Time.deltaTime * 1f;
				if (transitionTimer > 1f)
				{
					transiting = false;
					transitionOffset = 0f;
					wasSuccess = false;
				}
				else if (wasSuccess)
				{
					transitionOffset = 1f - Parameters.END_GAME_TRANSITION_CURVE.Evaluate(transitionTimer);
				}
				else
				{
					transitionOffset = 0f - (1f - Parameters.END_GAME_TRANSITION_CURVE.Evaluate(transitionTimer));
				}
			}
		}
		if (dontFollow)
		{
			y = 0f - FinishLine.GetDistance() + transitionOffset * 2f + -3f;
		}
		else
		{
			y = Skier.GetY() + transitionOffset * 2f + -3f;
		}
		base.transform.position = new Vector3(0f, y, z + 1f * y);
		Color color = transitionCover.color;
		color.a = 2f * Mathf.Abs(transitionOffset);
		transitionCover.color = color;
		upperY = y + thisCamera.orthographicSize;
	}

	protected override void OnRefresh()
	{
		transiting = true;
		transitionTimer = 0.5f;
		y = Skier.GetY() + -3f;
		thisCamera.backgroundColor = Level.GetBackgroundColor();
		transitionCover.color = Level.GetBackgroundColor();
		dontFollow = false;
		continued = false;
		Juice.LevelBreak();
	}

	protected override void OnStartRun(Run run)
	{
		Juice.LevelStarts(run.level.ToString());
	}

	protected override void OnEndRun()
	{
		if (Neuron.GetCurrentRun().success)
		{
			transitionCover.color = Level.GetBackgroundColor();
			dontFollow = true;
			wasSuccess = true;
			Invoke("InitiateRefresh", 3f);
		}
		else if (continued)
		{
			Invoke("InitiateRefresh", 1f);
		}
		else
		{
			Invoke("InitiateContinue", 1f);
		}
		Juice.gameCenter.SendScore(string.Empty, Neuron.GetCurrentRun().score);
	}

	private void InitiateContinue()
	{
		ContinuePage.GetInstance().Show(OnContinueChosen);
	}

	private void OnContinueChosen(bool yes)
	{
		if (yes)
		{
			continued = true;
			Neuron.Continue();
		}
		else
		{
			InitiateRefresh();
		}
	}

	public void InitiateRefresh()
	{
		Singleton<GameCamera>.i.transiting = true;
		Singleton<GameCamera>.i.transitionTimer = 0f;
		Juice.LevelEnds(Neuron.GetCurrentRun().success);
	}

	private void TryShowInterstitial()
	{
		Juice.ads.interstitial.Show(Neuron.Refresh);
	}
}
