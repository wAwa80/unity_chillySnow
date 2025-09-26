using UnityEngine;

namespace JuiceInternal
{
	public sealed class Premium : Module<Premium>
	{
		private const string TICK_COUNT_KEY = "0612790575264219705";

		private static int ticks;

		private DealPopup dealPopup;

		private bool isPremium;

		private bool isNotFirstTryShow;

		protected override void OnSetup()
		{
            //isPremium = Store.IsOwned(Settings.Get().premium) || Store.IsOwned(Settings.Get().premiumDeal);
            ticks = PlayerPrefs.GetInt("0612790575264219705", 0);
		}

		protected override void OnGameStarted()
		{
			if (!isPremium)
			{
				GameObject gameObject = Object.Instantiate(Resources.Load<GameObject>("DealPopup"));
				gameObject.name = "JUICE MODULE Deal Popup";
				Object.DontDestroyOnLoad(gameObject);
				dealPopup = gameObject.GetComponent<DealPopup>();
			}
		}

		public bool IsPremium()
		{
			return isPremium;
		}

		public void Buy()
		{
			//Module<Store>.GetInstance().Buy(Settings.Get().premium);
		}

		public void TryShowDeal()
		{
			if (!isNotFirstTryShow)
			{
				isNotFirstTryShow = true;
			}
			else
			{
				if (!Module<Store>.GetInstance().IsInitialized() || dealPopup == null)
				{
					return;
				}
				ticks++;
				int num = ticks - Settings.Get().dealPopFirstAtTick;
				if (num >= 0)
				{
					if (num % Settings.Get().dealPopThenEveryTick == 0)
					{
						dealPopup.Show();
					}
					PlayerPrefs.SetInt("0612790575264219705", ticks);
					PlayerPrefs.Save();
				}
			}
		}
	}
}
