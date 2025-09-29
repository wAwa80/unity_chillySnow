using UnityEngine;

/*
 * ϵͳ��������
1. ��ʼ���׶Σ�
? Premium��ȡSettings�еĲ�ƷID����
? ͨ��Store����û��Ƿ��ѹ���
? ������Ǹ߼����û���������������
2. ���н׶Σ�
? ÿ����Ϸ����ʱ��ticks����������
? �ﵽ�趨����ֵʱ����ʾ������ʾ����
? �û��������ʱ����Store�Ĺ�����
3. ����ɹ���
? Store�����ѹ����Ʒ�б��־û�
? Premium��⵽����״̬�仯��ֹͣ��ʾ����
 */
namespace JuiceInternal
{
	/// <summary>
	/// �߼���������
	/// ��Ҫ���ܣ�
	/// ��Ա״̬��������û��Ƿ��ѹ���߼���
	/// ����չʾ�߼������ƹ�����ʾ��������ʾʱ��
	/// ����������ƣ�������Ϸ���д���������ʱ��ʾ������ʾ
	/// </summary>

	public sealed class Premium : Module<Premium>
	{
		private const string TICK_COUNT_KEY = "0612790575264219705";

		private static int ticks;

		private DealPopup dealPopup;

		private bool isPremium;

		private bool isNotFirstTryShow;

		protected override void OnSetup()
		{
			// ����û��Ƿ�ӵ�и߼���
			isPremium = Store.IsOwned(Settings.Get().premium) || Store.IsOwned(Settings.Get().premiumDeal);
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
					// ������ʾ�߼����״����в���ʾ��֮���ض������ʾ
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
