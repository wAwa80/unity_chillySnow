using UnityEngine;
using UnityEngine.UI;

namespace JuiceInternal
{
	internal sealed class DealPopup : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup background;

		[SerializeField]
		private Transform panel;

		[SerializeField]
		private Button buyButton;

		private Animation buyButtonAnimation;

		[SerializeField]
		private Button noThanksButton;

		[SerializeField]
		private Text oldPrice;

		[SerializeField]
		private Text newPrice;

		private float t;

		[SerializeField]
		private float animationSpeed = 2f;

		private void Awake()
		{
			background.alpha = 0f;
			background.interactable = false;
			background.blocksRaycasts = false;
			panel.transform.localScale = Vector3.zero;
			buyButton.onClick.AddListener(Buy);
			buyButtonAnimation = buyButton.GetComponent<Animation>();
			noThanksButton.onClick.AddListener(_Hide);
			base.gameObject.SetActive(value: false);
		}

		private void Buy()
		{
			Module<Store>.GetInstance().Buy(Settings.Get().premiumDeal);
		}

		public void Show()
		{
			oldPrice.text = Module<Store>.GetInstance().GetLocalizedProductPrice(Settings.Get().premium);
			newPrice.text = string.Format(Module<Translator>.GetInstance().Translate(newPrice.text), Module<Store>.GetInstance().GetLocalizedProductPrice(Settings.Get().premiumDeal));
			base.gameObject.SetActive(value: true);
			background.alpha = 1f;
			background.interactable = true;
			background.blocksRaycasts = true;
			buyButtonAnimation.Play();
			PlayShowAnimation();
		}

		public void Hide()
		{
			buyButtonAnimation.Stop();
			PlayHideAnimation();
		}

		private void _Hide()
		{
			Module<Analytics>.GetInstance().SendDesignEvent("UI:DealPopup:Closed");
			Hide();
		}

		private void PlayShowAnimation()
		{
			background.blocksRaycasts = true;
			background.interactable = true;
			if (!base.enabled)
			{
				base.enabled = true;
			}
		}

		private void PlayHideAnimation()
		{
			background.blocksRaycasts = false;
			background.interactable = false;
			if (!base.enabled)
			{
				base.enabled = true;
			}
		}

		private void Update()
		{
			if (background.blocksRaycasts)
			{
				t += animationSpeed * Time.deltaTime;
				if (t >= 1f)
				{
					t = 1f;
					base.enabled = false;
				}
				background.alpha = Mathf.Sqrt(t);
			}
			else
			{
				t -= animationSpeed * Time.deltaTime;
				if (t <= 0f)
				{
					t = 0f;
					base.enabled = false;
					base.gameObject.SetActive(value: false);
				}
				background.alpha = t * t;
			}
			float num = OneMinusSinCardNormalized(t);
			panel.transform.localScale = new Vector3(num, num, num);
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
}
