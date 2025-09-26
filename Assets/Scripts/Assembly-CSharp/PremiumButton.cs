using JuiceInternal;
using UnityEngine;
using UnityEngine.UI;

public sealed class PremiumButton : MonoBehaviour
{
	private static PremiumButton instance;

	private Button button;

	private Image image;

	private float t;

	[SerializeField]
	private float animationSpeed = 2f;

	public static PremiumButton GetInstance()
	{
		return instance;
	}

	private void Awake()
	{
		if (Juice.premium.IsPremium())
		{
			Module<Ads>.GetInstance().ForceNoFuckingAds();
			Object.DestroyImmediate(base.gameObject);
			return;
		}
		instance = this;
		button = GetComponent<Button>();
		button.onClick.AddListener(OnClick);
		image = base.transform.GetChild(0).GetComponent<Image>();
		//Juice.store.RegisterProductBoughtEvent(OnProductBought);
	}

	private void OnClick()
	{
		Juice.premium.Buy();
	}

	private void OnProductBought(string product, bool newPurchase)
	{
		if (product == Settings.Get().premium || product == Settings.Get().premiumDeal)
		{
			//Juice.store.UnregisterProductBoughtEvent(OnProductBought);
			Module<Ads>.GetInstance().ForceNoFuckingAds();
			Object.DestroyImmediate(base.gameObject);
		}
	}

	public void Show()
	{
		button.enabled = true;
		if (!base.enabled)
		{
			base.enabled = true;
		}
	}

	public void Hide()
	{
		button.enabled = false;
		if (!base.enabled)
		{
			base.enabled = true;
		}
	}

	private void Update()
	{
		if (button.enabled)
		{
			t += animationSpeed * Time.deltaTime;
			if (t >= 1f)
			{
				t = 1f;
				base.enabled = false;
			}
		}
		else
		{
			t -= animationSpeed * Time.deltaTime;
			if (t <= 0f)
			{
				t = 0f;
				base.enabled = false;
			}
		}
		float num = OneMinusSinCardNormalized(t);
		image.transform.localScale = new Vector3(num, num, num);
	}

	private float OneMinusSinCardNormalized(float x)
	{
		if (x == 0f)
		{
			return 0f;
		}
		return 1f - Mathf.Sin(x * 10f) * (0.1f / x - 0.1f);
	}
}
