using UnityEngine;

public static class Parameters
{
	public const float Z_PER_Y = 1f;

	public const float CAMERA_OFFSET = -3f;

	public const float LOSE_CAMERA_SHAKE_AMPLITUDE = 0.01f;

	public const float LOSE_CAMERA_SHAKE_FREQUENCY = 40f;

	public const float LOSE_CAMERA_SHAKE_FADE_SPEED = 0.02f;

	public const float END_GAME_TRANSITION_SPEED = 1f;

	public const float END_GAME_TRANSITION_OFFSET = 2f;

	public static readonly AnimationCurve END_GAME_TRANSITION_CURVE = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	public const float DELAY_BETWEEN_LOSE_AND_RESTART = 1f;

	public const float DELAY_BETWEEN_SUCCESS_AND_RESTART = 3f;

	public const float DELAY_BETWEEN_LOSE_AND_CONTINUE_POPUP = 1f;

	public const float MIN_PINE_SIZE = 0.7f;

	public const float MAX_PINE_SIZE = 1.3f;

	public const float MAX_TIME_BETWEEN_WHOOSHES = 1.5f;

	public const float MAX_TIME_BETWEEN_WHOOSHES_WHEN_FEVER = 3f;

	public const float PINE_COLLISION_SIZE = 0.15f;

	public const float WHOOSH_DISTANCE = 1f;

	public const float START_PINES_DISTANCE = 7f;

	public const float VERTICAL_PINE_DENSITY = 0.3f;

	public const float HORIZONTAL_PINE_DENSITY = 0.3f;

	public const float INVERSE_HORIZONTAL_PINE_DENSITY = 3.33333f;

	public const float HALF_HORIZONTAL_PINE_DENSITY = 0.15f;

	public const float PINES_STOP_AT_FINISH_LINE_DISTANCE = 3f;

	public const float PINES_SPARSITY = 10f;

	public const float PINES_INTER_GROUP_DISTANCE = 1f;

	public const float PINES_GROUP_COUNT = 10f;

	public const float PINES_RARITY = 1f;

	public static readonly Sampler PINE_PROBABILITY = new Sampler(Level.GetFloat, 1f, 0.005f, 3f, 0.006f, 4f, 0.007f, 20f, 0.0115f, 100f, 0.0135f, 1000f, 0.014f);

	public static readonly Sampler PINE_PROBABILITY_MULTIPLIER = new Sampler(PineGenerator.GetDistance, 0f, 1f, 1f, 1.1f);

	public const float ROLLING_STONE_MIN_SIZE = 0.6f;

	public const float ROLLING_STONE_MAX_SIZE = 1.1f;

	public const float ROLLING_STONE_MIN_ANGLE = 20f;

	public const float ROLLING_STONE_MAX_ANGLE = 60f;

	public static readonly Sampler ROLLING_STONE_MIN_SPEED = new Sampler(Level.GetFloat, 1f, 2f, 1000f, 3.5f);

	public static readonly Sampler ROLLING_STONE_MAX_SPEED = new Sampler(Level.GetFloat, 1f, 3.5f, 1000f, 6f);

	public static readonly Sampler ROLLING_STONE_PROBABILITY = new Sampler(Level.GetFloat, 1f, 0f, 2f, 0f, 3f, 0.005f, 4f, 0.006f, 20f, 0.012f, 100f, 0.018f, 1000f, 0.021f);

	public static readonly Sampler ROLLING_STONE_PROBABILITY_MULTIPLIER = new Sampler(PineGenerator.GetDistance, 0f, 1f, 1f, 1.4f);

	public const int TRIGGER_FEVER = 4;

	public const float MIN_SKI_VOLUME = 0.025f;

	public const float MAX_SKI_VOLUME = 0.1f;

	public static readonly Sampler SKIER_MANIABILITY_RELATIVE_TO_OLD_CHILLY_SNOW = new Sampler(Skier.GetDistance, 0f, 0f, 180f, 0.8f);

	public static readonly string[] COLOR_TEMPLATES = new string[40]
	{
		"fff8f5", "415f59", "41abad", "d7341c", "fff8f5", "e72d12", "e72d12", "e72d12", "fff8f5", "d67715",
		"d67715", "d67715", "fff8f5", "eCe91b", "eCe91b", "eCe91b", "fff8f5", "22c01c", "22c01c", "22c01c",
		"fff8f5", "1aecd4", "1aecd4", "1aecd4", "fff8f5", "1a64ec", "1aecd4", "1aecd4", "fff8f5", "963fc9",
		"963fc9", "963fc9", "fff8f5", "d61fbd", "d61fbd", "d61fbd", "fff8f5", "373737", "373737", "373737"
	};

	public static readonly Sampler TOTAL_SLIDE_DISTANCE = new Sampler(Level.GetFloat, 1f, 90f, 4f, 120f, 100f, 180f, 1000f, 230f);

	public const Vibration VIBRATION_WHEN_LOSE = Vibration.Failure;

	public const Vibration VIBRATION_WHEN_FEVER = Vibration.Micro;

	public const Vibration VIBRATION_WHEN_SWITCH_SETTINGS = Vibration.Medium;

	public const Vibration VIBRATION_WHEN_LEVEL_COMPLETE = Vibration.Success;
}
