using UnityEngine;
using UnityEngine.UI;

public class RatePage : Page<RatePage>
{
	[SerializeField]
	private CanvasGroup starPanel;

	[SerializeField]
	private CanvasGroup badPanel;

	[SerializeField]
	private Button validateRatingButton;

	[SerializeField]
	private Button[] stars;

	[SerializeField]
	private Text starPanelDescription;

	[SerializeField]
	private Text badPanelDescription;

	private Image[] starIcons;

	private int currentRating;

	private const string STAR_PANEL_DESCRIPTION = "您如何评价<滑雪吧兄弟>?"; //How would you rate Chilly Snow?
	private const string BAD_PANEL_DESCRIPTION = "感谢您的宝贵反馈! 我们会尽力改进<滑雪吧兄弟>!"; //Thanks for your precious feedback! We will do our best to improve Chilly Snow!

	protected override void Awake()
	{
		base.Awake();
		currentRating = 0;
		starIcons = new Image[stars.Length];
		for (int i = 0; i < stars.Length; i++)
		{
			starIcons[i] = stars[i].transform.GetChild(0).GetComponent<Image>();
		}
		starPanelDescription.text = Translator.Translate(STAR_PANEL_DESCRIPTION);
		badPanelDescription.text = Translator.Translate(BAD_PANEL_DESCRIPTION);
	}

	protected override void OnBackToMenu()
	{
		if (Data.LoadString("version", "0") != Application.version)
		{
			Data.SaveString("version", Application.version);
			Data.SaveInt("versionEvaluation", 1);
			return;
		}
		int num = Data.LoadInt("versionEvaluation");
		num++;
		if (num <= 15)
		{
			Data.SaveInt("versionEvaluation", num);
			if (num == 15)
			{
				InitiateRate();
			}
		}
	}

	private void InitiateRate()
	{
		validateRatingButton.interactable = false;
		SwitchPanel(starPanel, visible: true);
		SwitchPanel(badPanel, visible: false);
		currentRating = 0;
		Show();
	}

	public void Rate(int stars)
	{
		validateRatingButton.interactable = true;
		currentRating = stars;
		for (int i = 0; i < starIcons.Length; i++)
		{
			if (i < stars)
			{
				starIcons[i].color = Color.yellow;
			}
			else
			{
				starIcons[i].color = Color.white;
			}
		}
	}

	public void ValidateRating()
	{
		if (currentRating != 0)
		{
			Singleton<Analytics>.i.Rated(currentRating);
			SwitchPanel(starPanel, visible: false);
			if (currentRating > 3)
			{
				GoToAppStore();
			}
			else
			{
				SwitchPanel(badPanel, visible: true);
			}
		}
	}

	public void GoToAppStore()
	{
		Hide();
		Application.OpenURL(App.GetStoreLink());
	}

	public void CallSupport()
	{
		Hide();
		string text = "";
		string text2 = MyEscapeURL("滑雪吧兄弟 Issue");
		string empty = string.Empty;
		Application.OpenURL("mailto:" + text + "?subject=" + text2 + "&body=" + empty);
	}

	private string MyEscapeURL(string url)
	{
		return WWW.EscapeURL(url).Replace("+", "%20");
	}

	private void SwitchPanel(CanvasGroup panel, bool visible)
	{
		if (visible)
		{
			panel.alpha = 1f;
			panel.blocksRaycasts = true;
			panel.interactable = true;
		}
		else
		{
			panel.alpha = 0f;
			panel.blocksRaycasts = false;
			panel.interactable = false;
		}
	}
}
