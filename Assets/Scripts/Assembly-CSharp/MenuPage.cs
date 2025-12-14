using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public sealed class MenuPage : Page<MenuPage>, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
	private Image image;

	private bool didContinue;

	private bool pressing;

	protected override void Awake()
	{
		base.Awake();
		image = GetComponent<Image>();
	}

	private void Start()
	{
		Show();
		StartCoroutine(GoToMenu(start: true, immediately: false));
		VoodooSauce.ShowBanner(OnBannerDisplayed);
	}

	private void OnBannerDisplayed(float height)
	{
	}

	protected override void OnContinue()
	{
		didContinue = true;
	}

	protected override void OnNewGame()
	{
		didContinue = false;
	}

	protected override void OnGameOver(bool canUseSecondChance)
	{
		if (!canUseSecondChance)
		{
			VoodooSauce.ShowInterstitial(delegate
			{
				StartCoroutine(GoToMenu(start: false, !didContinue));
			});
		}
	}

	private IEnumerator GoToMenu(bool start, bool immediately)
	{
		Color c = Singleton<GameCamera>.i.GetCamera().backgroundColor;
		image.enabled = true;
		if (!start)
		{
			c.a = 0f;
			image.color = c;
			if (!immediately)
			{
				yield return new WaitForSeconds(1.5f);
			}
			Singleton<GameCamera>.i.Transit();
			while (c.a < 1f)
			{
				c.a += Time.deltaTime * 2f;
				if (c.a > 1f)
				{
					c.a = 1f;
				}
				image.color = c;
				yield return null;
			}
			Neuron.BackToMenu();
		}
		c.a = 1f;
		image.color = c;
		while (c.a > 0f)
		{
			c.a -= Time.deltaTime * 2f;
			if (c.a < 0f)
			{
				c.a = 0f;
			}
			image.color = c;
			yield return null;
		}
	}

	public bool IsPressing()
	{
		return pressing;
	}

	public void OnPointerDown(PointerEventData data)
	{
		pressing = true;
	}

	public void OnPointerUp(PointerEventData data)
	{
		pressing = false;
	}
}
