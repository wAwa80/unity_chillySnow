using System.Collections.Generic;
using UnityEngine;

public sealed class Device : Essential<Device>
{
	private const int FPS = 60;

	private static bool hasInternet;

	private static float volume;

	private static bool soundOn;

	private static bool vibrationOn;

	private static readonly HashSet<Vibration> activeVibrations = new HashSet<Vibration>();

	protected override void Awake()
	{
		base.Awake();
		InitializeSound();
		InitializeVibrations();
		SetQualitySettings();
	}

	private void Update()
	{
		CheckInternetState();
	}

	private void SetQualitySettings()
	{
		QualitySettings.vSyncCount = 0;
		Time.fixedDeltaTime = 1f / 60f;
		Application.targetFrameRate = 60;
	}

	private static void CheckInternetState()
	{
		bool flag = Application.internetReachability != NetworkReachability.NotReachable;
		if (hasInternet != flag)
		{
			hasInternet = flag;
			Neuron.InternetStateChanged(flag);
		}
	}

	public static bool HasInternet()
	{
		return hasInternet;
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			Neuron.ApplicationPause();
		}
		else
		{
			Neuron.ApplicationResume();
		}
	}

	public static void Log(string message)
	{
		Debug.Log($"JUICE : {message}");
	}

	public static bool IsSoundOn()
	{
		return soundOn;
	}

	private static void InitializeSound()
	{
		soundOn = Data.LoadBool("soundOn", defaultValue: true);
		volume = AudioListener.volume;
		if (!soundOn)
		{
			AudioListener.volume = 0f;
		}
	}

	public static void SwitchSound()
	{
		if (soundOn)
		{
			soundOn = false;
			Data.SaveBool("soundOn", value: false);
			AudioListener.volume = 0f;
		}
		else
		{
			soundOn = true;
			Data.SaveBool("soundOn", value: true);
			AudioListener.volume = volume;
		}
	}

	public static bool IsAudioSupported()
	{
		return SystemInfo.supportsAudio;
	}

	public static bool IsVibrationOn()
	{
		return vibrationOn;
	}

	private static void InitializeVibrations()
	{
		vibrationOn = Data.LoadBool("vibrationOn", defaultValue: true);
	}

	public static void SwitchVibrations()
	{
		vibrationOn = !vibrationOn;
		Data.SaveBool("vibrationOn", vibrationOn);
		if (vibrationOn)
		{
			Vibrate(Vibration.Medium, replaceIfNoHapticFeedbackSupported: true);
		}
	}

	public static void Vibrate(Vibration vibration, bool replaceIfNoHapticFeedbackSupported = false)
	{
		if (vibrationOn)
		{
			if (vibration == Vibration.Full)
			{
				Handheld.Vibrate();
			}
			else if (IsHapticFeedbackSupported())
			{
				TriggerHapticFeedback(vibration);
			}
			else if (replaceIfNoHapticFeedbackSupported)
			{
				Handheld.Vibrate();
			}
		}
	}

	public static bool IsVibrationSupported()
	{
		return SystemInfo.supportsVibration;
	}

	public static bool IsHapticFeedbackSupported()
	{
		return false;
	}

	private static void TriggerHapticFeedback(Vibration vibration)
	{
		if (!activeVibrations.Contains(vibration))
		{
			activeVibrations.Add(vibration);
			InstantiateFeedbackGenerator((int)vibration);
		}
		TriggerFeedbackGenerator((int)vibration);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		foreach (Vibration activeVibration in activeVibrations)
		{
			ReleaseFeedbackGenerator((int)activeVibration);
		}
	}

	private static void _instantiateFeedbackGenerator(int id)
	{
	}

	private static void _triggerFeedbackGenerator(int id, bool advanced)
	{
	}

	private static void _releaseFeedbackGenerator(int id)
	{
	}

	private static void InstantiateFeedbackGenerator(int id)
	{
		_instantiateFeedbackGenerator(id);
	}

	private static void TriggerFeedbackGenerator(int id)
	{
		_triggerFeedbackGenerator(id, advanced: false);
	}

	private static void ReleaseFeedbackGenerator(int id)
	{
		_releaseFeedbackGenerator(id);
	}
}
