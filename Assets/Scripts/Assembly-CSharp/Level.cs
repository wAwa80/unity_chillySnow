using UnityEngine;
using UnityEngine.UI;

public sealed class Level : Singleton<Level>
{
	private const string SAVE_ID = "uivrhiuvhuhui";

	private static int level = -1;

	private const string BEST_SAVE_ID = "oziadoiez";

	private static int best;

	private Image endLevel;

	private Text endLevelText;

	private Text startLevelText;

	private Image progression;

	private Image bestProgression;

	private readonly Image[] alwaysColoredImages = new Image[3];

	private Color color;

	[SerializeField]
	private Text levelCompleteMessage;

	private RectTransform currentTickBox;

	private Image currentTickBoxImage;

	private Text currentTickBoxText;

	private RectTransform bestTickBox;

	private CanvasGroup bestTickBoxGroup;

	private Image bestTickBoxImage;

	private Text bestTickBoxText;

	private const string PERCENTAGE_FORMAT = "{0}%";

	private int lastProgression = -1;

	private const string COLORS_SAVE_ID = "liorefiuehfh";

	private static Color backgroundColor;

	private static Color skierColor;

	private static Color pineColor;

	private static Color feverColor;

	public override int GetPriority()
	{
		return -1;
	}

	public static int Get()
	{
		return level;
	}

	public static float GetFloat()
	{
		return level;
	}

	protected override void Awake()
	{
		level = Data.LoadInt("uivrhiuvhuhui", 1);
		best = Data.LoadInt("oziadoiez");
		base.Awake();
		alwaysColoredImages[0] = base.transform.GetChild(0).GetComponent<Image>();
		alwaysColoredImages[1] = base.transform.GetChild(1).GetComponent<Image>();
		alwaysColoredImages[2] = base.transform.GetChild(3).GetComponent<Image>();
		endLevel = base.transform.GetChild(4).GetComponent<Image>();
		endLevelText = endLevel.transform.GetChild(0).GetComponent<Text>();
		startLevelText = base.transform.GetChild(3).GetChild(0).GetComponent<Text>();
		progression = base.transform.GetChild(2).GetChild(2).GetComponent<Image>();
		bestProgression = base.transform.GetChild(2).GetChild(0).GetComponent<Image>();
		currentTickBox = base.transform.GetChild(2).GetChild(3).GetComponent<RectTransform>();
		currentTickBoxImage = currentTickBox.GetComponent<Image>();
		currentTickBoxText = currentTickBox.GetChild(0).GetComponent<Text>();
		bestTickBox = base.transform.GetChild(2).GetChild(1).GetComponent<RectTransform>();
		bestTickBoxGroup = bestTickBox.GetComponent<CanvasGroup>();
		bestTickBoxImage = bestTickBox.GetComponent<Image>();
		bestTickBoxText = bestTickBox.GetChild(1).GetComponent<Text>();
		LoadColors();
	}

	protected override void OnRefresh()
	{
		this.color = skierColor;
		Color color = feverColor;
		Image[] array = alwaysColoredImages;
		foreach (Image image in array)
		{
			image.color = this.color;
		}
		endLevel.color = Color.white;
		endLevelText.color = this.color;
		endLevelText.text = (level + 1).ToString();
		startLevelText.text = level.ToString();
		progression.color = this.color;
		currentTickBoxImage.color = this.color;
		color.a = 0.25f;
		bestProgression.color = color;
		color.a = 0.5f;
		bestTickBoxImage.color = color;
		bestTickBoxText.text = $"{best}%";
		bestTickBoxGroup.alpha = ((best != 0) ? 1f : 0f);
		float num = (float)best * 0.01f;
		bestTickBox.anchorMin = new Vector2(num, 0f);
		bestTickBox.anchorMax = bestTickBox.anchorMin;
		bestProgression.fillAmount = num;
		levelCompleteMessage.enabled = false;
		base.enabled = true;
	}

	protected override void OnContinue()
	{
		base.enabled = true;
	}

	protected override void OnEndRun()
	{
		base.enabled = false;
		if (!Neuron.GetCurrentRun().success)
		{
			float num = (0f - Skier.GetY()) / FinishLine.GetDistance();
			int num2 = Mathf.RoundToInt(num * 100f);
			SetProgression(num);
			if (num2 > best)
			{
				best = num2;
				Data.SaveInt("oziadoiez", best);
			}
		}
		else
		{
			SetProgression(1f);
			endLevel.color = color;
			endLevelText.color = Color.white;
			levelCompleteMessage.enabled = true;
			levelCompleteMessage.text = string.Format(Juice.translator.Translate("Level {0} Complete!"), level);
			level++;
			Data.SaveInt("uivrhiuvhuhui", level);
			best = 0;
			Data.SaveInt("oziadoiez", best);
			SetNextLevelColors();
		}
	}

	private void Update()
	{
		SetProgression(progression.fillAmount + ((0f - Skier.GetY()) / FinishLine.GetDistance() - progression.fillAmount) * 5f * Time.deltaTime);
	}

	private void SetProgression(float percentage)
	{
		progression.fillAmount = percentage;
		currentTickBox.anchorMin = new Vector2(percentage, 0f);
		currentTickBox.anchorMax = currentTickBox.anchorMin;
		int num = Mathf.RoundToInt(percentage * 100f);
		if (num != lastProgression)
		{
			lastProgression = num;
			currentTickBoxText.text = $"{num}%";
		}
	}

	public static Color GetBackgroundColor()
	{
		return backgroundColor;
	}

	public static Color GetSkierColor()
	{
		return skierColor;
	}

	public static Color GetPineColor()
	{
		return pineColor;
	}

	public static Color GetFeverColor()
	{
		return feverColor;
	}

	private static void SetNextLevelColors()
	{
		int max = Parameters.COLOR_TEMPLATES.Length / 4;
		string text = Parameters.COLOR_TEMPLATES[Random.Range(0, max) * 4];
		string text2 = Parameters.COLOR_TEMPLATES[Random.Range(0, max) * 4 + 2];
		string text3 = text2;
		while (text3 == text2)
		{
			text3 = Parameters.COLOR_TEMPLATES[Random.Range(0, max) * 4 + 1];
		}
		string text4 = text3;
		while (text4 == text3 || text4 == text2)
		{
			text4 = Parameters.COLOR_TEMPLATES[Random.Range(0, max) * 4 + 3];
		}
		backgroundColor = Utility.HexToColor(text);
		skierColor = Utility.HexToColor(text2);
		pineColor = Utility.HexToColor(text3);
		feverColor = Utility.HexToColor(text4);
		Data.SaveString("liorefiuehfh", $"{text};{text2};{text3};{text4}");
	}

	private static void LoadColors()
	{
		if (Data.HasKey("liorefiuehfh"))
		{
			string[] array = Data.LoadString("liorefiuehfh").Split(';');
			backgroundColor = Utility.HexToColor(array[0]);
			skierColor = Utility.HexToColor(array[1]);
			pineColor = Utility.HexToColor(array[2]);
			feverColor = Utility.HexToColor(array[3]);
		}
		else
		{
			backgroundColor = Utility.HexToColor(Parameters.COLOR_TEMPLATES[0]);
			skierColor = Utility.HexToColor(Parameters.COLOR_TEMPLATES[2]);
			pineColor = Utility.HexToColor(Parameters.COLOR_TEMPLATES[1]);
			feverColor = Utility.HexToColor(Parameters.COLOR_TEMPLATES[3]);
		}
	}
}
