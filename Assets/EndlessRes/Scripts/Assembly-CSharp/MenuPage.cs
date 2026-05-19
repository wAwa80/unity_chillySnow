using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace EndlessMode
{
	[RequireComponent(typeof(Image))]
	public sealed class MenuPage : Page<MenuPage>, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		private Image image;

		private bool didContinue;

		private bool pressing;

		private float directionChangeThreshold;

		private float sensitivity;

		private float direction;

		private float timer;

		private float touchPosition;

		protected override void Awake()
		{
			base.Awake();
			image = GetComponent<Image>();
		}

		private void Start()
		{
			Show();
			StartCoroutine(GoToMenu());
			//VoodooSauce.ShowBanner(OnBannerDisplayed);
		}

		private void OnBannerDisplayed(float height)
		{
		}

		protected override void OnContinue()
		{
			didContinue = true;
			image.raycastTarget = true;
		}

		protected override void OnNewGame()
		{
			didContinue = false;
		}

		protected override void OnGameOver(bool canUseSecondChance)
		{
			image.raycastTarget = false;
			if (!canUseSecondChance)
			{
				StartCoroutine(InitiateGoToMenu(!didContinue));
			}
		}

		private IEnumerator InitiateGoToMenu(bool immediately)
		{
			if (!immediately)
			{
				yield return new WaitForSeconds(1.5f);
			}
			float alpha = 0f;
			Singleton<GameCamera>.i.Transit();
			while (alpha < 1f)
			{
				alpha += Time.deltaTime * 2f;
				if (alpha > 1f)
				{
					alpha = 1f;
				}
				Color c = Singleton<GameCamera>.i.GetCamera().backgroundColor;
				c.a = alpha;
				image.color = c;
				yield return null;
			}
			//VoodooSauce.ShowInterstitial(ValidateTransition);
			ValidateTransition();

	        Neuron.BackToMenu();
			StartCoroutine(GoToMenu());
		}

		private void ValidateTransition()
		{
		}

		private IEnumerator GoToMenu()
		{
			float alpha = 1f;
			image.raycastTarget = true;
			while (alpha > 0f)
			{
				alpha -= Time.deltaTime * 2f;
				if (alpha < 0f)
				{
					alpha = 0f;
				}
				Color c = Singleton<GameCamera>.i.GetCamera().backgroundColor;
				c.a = alpha;
				image.color = c;
				yield return null;
			}
		}

		public bool IsPressing()
		{
			return pressing;
		}

		public bool GetDirection()
		{
			return direction > 0f;
		}

		public void OnPointerDown(PointerEventData data)
		{
			if (App.GetState() == App.State.Menu)
			{
				Neuron.NewGame();
			}
			pressing = true;
			touchPosition = data.position.x / (float)Screen.width;
			direction = 0f;
			timer = 0f;
		}

		public void OnPointerUp(PointerEventData data)
		{
			pressing = false;
		}

		protected override void Update()
		{
			base.Update();
			if (!pressing)
			{
				return;
			}
			float num = Input.mousePosition.x / (float)Screen.width;
			float num2 = num - touchPosition;
			if (direction == 0f)
			{
				if (num2 > directionChangeThreshold || num2 < 0f - directionChangeThreshold)
				{
					direction = num2;
					touchPosition = num;
				}
			}
			else if (direction < 0f)
			{
				if (num2 < 0f)
				{
					direction += num2;
					touchPosition = num;
				}
				else if (num2 > directionChangeThreshold)
				{
					direction = num2;
					timer = direction;
					touchPosition = num;
				}
			}
			else if (num2 > 0f)
			{
				direction += num2;
				touchPosition = num;
			}
			else if (num2 < 0f - directionChangeThreshold)
			{
				direction = num2;
				timer = direction;
				touchPosition = num;
			}
			if (timer < direction)
			{
				timer += sensitivity * Time.deltaTime;
				if (timer > direction)
				{
					timer = direction;
				}
			}
			else if (timer > direction)
			{
				timer -= sensitivity * Time.deltaTime;
				if (timer < direction)
				{
					timer = direction;
				}
			}
		}
	}
}
