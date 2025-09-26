using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(AudioSource))]
public sealed class Pine : Recyclable<Pine>
{
	private SpriteRenderer spriteRenderer;

	private AudioSource source;

	private SpriteRenderer leaves;

	private SpriteRenderer shadow;

	private SpriteRenderer bonusEffect;

	private MeshRenderer bonusPoints;

	private TextMesh bonusPointsText;

	private float x;

	private float y;

	private float passed;

	private static int whooshCombo;

	private static float lastWhooshTime;

	private static bool continueCombo;

	private float size;

	private Color bonusColor = Utility.HexToColor("425f59");

	protected override void Awake()
	{
		base.Awake();
		spriteRenderer = GetComponent<SpriteRenderer>();
		source = GetComponent<AudioSource>();
		leaves = base.transform.GetChild(0).GetComponent<SpriteRenderer>();
		shadow = base.transform.GetChild(1).GetComponent<SpriteRenderer>();
		bonusEffect = base.transform.GetChild(2).GetComponent<SpriteRenderer>();
		bonusPoints = base.transform.GetChild(3).GetComponent<MeshRenderer>();
		bonusPointsText = base.transform.GetChild(3).GetComponent<TextMesh>();
		base.enabled = false;
	}

	protected override void OnEnabled()
	{
		spriteRenderer.enabled = true;
		leaves.enabled = true;
		shadow.enabled = true;
		bonusEffect.enabled = false;
		bonusPoints.enabled = false;
		size = 0.7f + Random.value * 0.59999996f;
		base.transform.localScale = new Vector3(size, size, size);
		passed = -2f;
		leaves.color = Level.GetPineColor();
	}

	protected override void OnDisabled()
	{
		spriteRenderer.enabled = false;
		leaves.enabled = false;
		shadow.enabled = false;
		bonusEffect.enabled = false;
		bonusPoints.enabled = false;
		passed = -2f;
	}

	public float GetX()
	{
		return x;
	}

	public float GetY()
	{
		return y;
	}

	public void Place(float x, float y, float z)
	{
		this.x = x;
		this.y = y;
		base.transform.position = new Vector3(x, y, 1f * y + z);
	}

	public static int GetWhooshCombo()
	{
		return whooshCombo;
	}

	public static void ResetState()
	{
		whooshCombo = 1;
	}

	public bool IsPassed()
	{
		return passed > -1.5f;
	}

	public void Pass()
	{
		float num = ((!Skier.IsInFever()) ? 1.5f : 3f);
		if (Time.time - lastWhooshTime > num)
		{
			whooshCombo = 1;
		}
		else
		{
			whooshCombo++;
		}
		lastWhooshTime = Time.time;
		int points = Level.Get() * whooshCombo;
		Neuron.Whoosh(points);
		bonusPointsText.text = $"+{points.ToString()}";
		bonusPointsText.color = Skier.GetColor();
		passed = 0f;
		base.enabled = true;
		bonusEffect.enabled = true;
		bonusPoints.enabled = true;
		SyncBonusEffect(0f);
		source.Play();
	}

	private void Update()
	{
		passed += Time.deltaTime;
		float z;
		if (passed > 1f)
		{
			base.enabled = false;
			z = size;
			bonusEffect.enabled = false;
			bonusPoints.enabled = false;
		}
		else
		{
			z = Mathf.Min(passed * 4f, 1f) * 2f - 1f;
			z = ((0f - z) * z + 1f) * 0.3f + size;
			SyncBonusEffect(passed);
		}
		base.transform.localScale = new Vector3(z, z, z);
	}

	private void SyncBonusEffect(float time)
	{
		float num = time * 4f;
		bonusColor.a = Mathf.Max(1f - num, 0f);
		bonusEffect.color = bonusColor;
		bonusEffect.transform.localScale = new Vector3(num, num * 0.6f, num);
		Color color = bonusPointsText.color;
		color.a = 1f - time * time;
		bonusPointsText.color = color;
		bonusPointsText.transform.localPosition = new Vector3(0f, 2f - bonusPointsText.color.a * 0.5f, 0f);
	}
}
