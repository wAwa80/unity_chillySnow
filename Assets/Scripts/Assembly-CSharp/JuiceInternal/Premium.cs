using UnityEngine;

/*
 * 系统工作流程
1. 初始化阶段：
? Premium读取Settings中的产品ID配置
? 通过Store检查用户是否已购买
? 如果不是高级版用户，创建弹窗对象
2. 运行阶段：
? 每次游戏运行时，ticks计数器增加
? 达到设定的阈值时，显示购买提示弹窗
? 用户点击购买时调用Store的购买功能
3. 购买成功：
? Store更新已购买产品列表并持久化
? Premium检测到购买状态变化，停止显示弹窗
 */
namespace JuiceInternal
{
	/// <summary>
	/// 高级版管理核心
	/// 主要功能：
	/// 会员状态管理：检测用户是否已购买高级版
	/// 弹窗展示逻辑：控制购买提示弹窗的显示时机
	/// 购买计数机制：基于游戏运行次数决定何时显示购买提示
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
			// 检查用户是否拥有高级版
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
					// 弹窗显示逻辑：首次运行不显示，之后按特定间隔显示
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
