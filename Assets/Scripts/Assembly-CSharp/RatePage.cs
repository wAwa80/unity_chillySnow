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

	private Image[] starIcons;

	private int currentRating;

	protected override void Awake()
	{
		base.Awake();
		currentRating = 0;
		starIcons = new Image[stars.Length];
		for (int i = 0; i < stars.Length; i++)
		{
			starIcons[i] = stars[i].transform.GetChild(0).GetComponent<Image>();
		}
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
