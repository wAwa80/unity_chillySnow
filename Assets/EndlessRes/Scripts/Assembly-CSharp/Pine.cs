using UnityEngine;


namespace EndlessMode
{
	[RequireComponent(typeof(SpriteRenderer), typeof(AudioSource))]
	public sealed class Pine : Recyclable<Pine>
	{
		private const float MIN_PINE_SIZE = 0.7f;

		private const float MAX_PINE_SIZE = 1.3f;

		private SpriteRenderer spriteRenderer;

		private AudioSource source;

		private SpriteRenderer nightShadow;

		private SpriteRenderer shadow;

		private SpriteRenderer bonusEffect;

		private MeshRenderer bonusPoints;

		private TextMesh bonusPointsText;

		private const int WHOOSH_BASE_POINTS = 2;

		private const int WHOOSH_CHAIN_POINTS = 2;

		private const float MAX_TIME_BETWEEN_WHOOSHES = 1.5f;

		public const float MAX_TIME_BETWEEN_WHOOSHES_WHEN_FEVER = 3f;

		private float passed;

		private static int whooshPoints;

		private static int whooshCombo;

		private static float lastWhooshTime;

		private static bool continueCombo;

		private float size;

		private Color bonusColor = Utility.HexToColor("#425f59");

		private const float MAX_TIME_BETWEEN_WHOOSHES_AB_TEST = 1f;

		private bool destroyed;

		protected override void Awake()
		{
			base.Awake();
			spriteRenderer = GetComponent<SpriteRenderer>();
			source = GetComponent<AudioSource>();
			nightShadow = base.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
			shadow = base.transform.GetChild(1).GetComponent<SpriteRenderer>();
			bonusEffect = base.transform.GetChild(2).GetComponent<SpriteRenderer>();
			bonusPoints = base.transform.GetChild(3).GetComponent<MeshRenderer>();
			bonusPointsText = base.transform.GetChild(3).GetComponent<TextMesh>();
			nightShadow.enabled = false;
			shadow.enabled = false;
		}

		protected override void OnEnabled()
		{
			spriteRenderer.enabled = true;
			if (NightModeButton.nightModeOn)
			{
				nightShadow.enabled = true;
			}
			else
			{
				shadow.enabled = true;
			}
			bonusEffect.enabled = false;
			bonusPoints.enabled = false;
			size = 0.7f;
			size += Random.value * (1.3f - size);
			base.transform.localScale = new Vector3(size, size, size);
			passed = -2f;
			destroyed = false;
		}

		protected override void OnDisabled()
		{
			spriteRenderer.enabled = false;
			nightShadow.enabled = false;
			shadow.enabled = false;
			bonusEffect.enabled = false;
			bonusPoints.enabled = false;
			passed = -2f;
		}

		public static int GetWhooshPoints()
		{
			return whooshPoints;
		}

		public static int GetWhooshCombo()
		{
			return whooshCombo;
		}

		public static void ResetWhooshCombo()
		{
			whooshPoints = 2;
			whooshCombo = 1;
		}

		public static void ContinueWhooshCombo()
		{
			continueCombo = true;
		}

		public bool IsPassed()
		{
			return passed > -1.5f;
		}

		public void Pass()
		{
			float num = ((Singleton<Player>.i.GetFeverState() != 0) ? 3f : ((!Player.IsABTestDestroyPines) ? 1.5f : 1f));
			if (!continueCombo && Time.time - lastWhooshTime > num)
			{
				if (!Player.IsABTestDestroyPines || num < 3f)
				{
					ResetWhooshCombo();
				}
			}
			else
			{
				if (continueCombo)
				{
					continueCombo = false;
				}
				whooshPoints += 2;
				whooshCombo++;
			}
			lastWhooshTime = Time.time;
			bonusPointsText.text = $"+{whooshPoints.ToString()}";
			bonusPointsText.color = Singleton<Player>.i.GetFeverColor(Singleton<Player>.i.NextFeverState());
			passed = 0f;
			if (App.IsRelease() || !DebugPage.dontAnimatePine)
			{
				bonusEffect.enabled = true;
			}
			if (App.IsRelease() || !DebugPage.dontBonusText)
			{
				bonusPoints.enabled = true;
			}
			SyncBonusEffect(0f);
			source.Play();
			if (whooshCombo == 3)
			{
				Device.Vibrate(Device.Vibration.Medium);
			}
			else if (whooshCombo == 6)
			{
				Device.Vibrate(Device.Vibration.Medium);
			}
		}

		private void Update()
		{
			if (passed >= 0f)
			{
				passed += Time.deltaTime;
				float num;
				if (passed > 1f)
				{
					passed = -1f;
					num = size;
					bonusEffect.enabled = false;
					bonusPoints.enabled = false;
				}
				else
				{
					num = Mathf.Min(passed * 4f, 1f) * 2f - 1f;
					num = ((0f - num) * num + 1f) * 0.3f + size;
					SyncBonusEffect(passed);
				}
				if (App.IsRelease() || !DebugPage.dontAnimatePine)
				{
					base.transform.localScale = new Vector3(num, num, num);
				}
			}
			if (NightModeButton.nightModeOn)
			{
				float num2 = Singleton<Player>.i.transform.position.x - base.transform.position.x;
				float num3 = Singleton<Player>.i.transform.position.y - base.transform.position.y;
				nightShadow.transform.localEulerAngles = new Vector3(90f, (0f - Mathf.Atan2(num3, num2)) * 57.29578f - 90f, 0f);
				num2 = Singleton<Player>.i.GetGlowIntensity() * Mathf.Min(5f / Mathf.Max(num2 * num2 + num3 * num3, 1f), 1f);
				Color color = ((Singleton<Player>.i.GetFeverState() != 0) ? Singleton<Player>.i.GetFeverColor(Singleton<Player>.i.GetFeverState()) : Singleton<Player>.i.GetSkinColor());
				spriteRenderer.color = new Color(color.r * num2, color.g * num2, color.b * num2, 1f);
			}
		}

		private void SyncBonusEffect(float time)
		{
			if (App.IsRelease() || !DebugPage.dontAnimatePine)
			{
				float num = time * 4f;
				bonusColor.a = Mathf.Max(1f - num, 0f);
				bonusEffect.color = bonusColor;
				bonusEffect.transform.localScale = new Vector3(num, num * 0.6f, num);
			}
			if (App.IsRelease() || !DebugPage.dontBonusText)
			{
				Color color = bonusPointsText.color;
				color.a = 1f - time * time;
				bonusPointsText.color = color;
				bonusPointsText.transform.localPosition = new Vector3(0f, 2f - bonusPointsText.color.a * 0.5f, 0f);
			}
		}

		protected override void OnNightModeSwitched(bool enabled)
		{
			if (!enabled)
			{
				spriteRenderer.color = Color.white;
			}
			if (spriteRenderer.enabled)
			{
				if (enabled)
				{
					nightShadow.enabled = true;
					shadow.enabled = false;
				}
				else
				{
					nightShadow.enabled = false;
					shadow.enabled = true;
				}
			}
		}

		public bool IsDestroyed()
		{
			return destroyed;
		}

		public void DestroyPine()
		{
			OnDisabled();
			destroyed = true;
			Singleton<Player>.i.PineDestroyed();
			PineDestroyedEffect pineDestroyedEffect = Recyclable<PineDestroyedEffect>.Get();
			pineDestroyedEffect.SetColor(Skin.GetSelected(Skin.Type.Pine).GetColor());
			pineDestroyedEffect.transform.position = base.transform.position;
			pineDestroyedEffect.transform.localScale = base.transform.localScale;
		}
	}
}
