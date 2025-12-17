using UnityEngine;
using UnityEngine.UI;

public class SkinsButton : AnimatedButton<SkinsButton>
{
	[SerializeField]
	private Image pin;

	protected override void Awake()
	{
		base.Awake();
		button.onClick.AddListener(Clicked);
		OnBackToMenu();
		pin.enabled = Data.LoadBool("skinsButtonPin");
	}

	protected override void OnNewGame()
	{
		Hide();
	}

	protected override void OnBackToMenu()
	{
		Show();
	}

	private void Clicked()
	{
		Singleton<SkinsPage>.i.Show();
		RemoveAlert();
	}

	public void Alert()
	{
		if (!Singleton<SkinsPage>.i.IsVisible())
		{
			pin.enabled = true;
			Data.SaveBool("skinsButtonPin", value: true);
		}
	}

	private void RemoveAlert()
	{
		pin.enabled = false;
		Data.SaveBool("skinsButtonPin", value: false);
	}
}
