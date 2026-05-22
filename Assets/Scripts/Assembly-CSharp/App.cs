using System;
using System.Collections;
using EasyMobile;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_WEIXINMINIGAME && !UNITY_EDITOR
using WeChatWASM;
#endif
public sealed class App : Singleton<App>
{
	public enum State
	{
		Menu,
		Playing,
		Pause,
		GameOver
	}

	public const string NAME = "滑雪吧兄弟";

	private const string IOS_APP_LINK = "https://itunes.apple.com/fr/app/chilly-snow/id1297701531";

	private const string ANDROID_APP_LINK = "https://play.google.com/store/apps/details?id=com.acidcousins.chilly";

	[SerializeField]
	private bool resetOnStart;

	private State state;

	private bool lastSecondChance;

	public static string GetStoreLink()
	{
		return "https://play.google.com/store/apps/details?id=com.acidcousins.chilly";
	}

	protected override void Awake()
	{
        Debug.Log("App.Awake ��ʼ");
        base.Awake();
		InitializeVersion();
		if (i == this)
		{
			UnityEngine.Object.DontDestroyOnLoad(this);
			base.name = "SYSTEM_OBJECT App";
			Translator.Load();
			UnityEngine.Object.DontDestroyOnLoad(new GameObject("SYSTEM_OBJECT Easy Mobile", typeof(EM_PrefabManager), typeof(GameServiceManager)));
			UnityEngine.Object.DontDestroyOnLoad(new GameObject("SYSTEM_OBJECT Data Auto Saver", typeof(Data)));
			UnityEngine.Object.DontDestroyOnLoad(new GameObject("SYSTEM_OBJECT Stats Saver", typeof(Stats)));
			UnityEngine.Object.DontDestroyOnLoad(new GameObject("SYSTEM_OBJECT Analytics", typeof(Analytics)));
			//VoodooSauce.RegisterPurchaseDelegate(this);
        	StartCoroutine(DelayStart());
			if (IsRelease())
			{
				StartCoroutine(LaunchGameDelay());
				return;
			}
			CanvasGroup component = GameObject.Find("Buttons").GetComponent<CanvasGroup>();
			component.alpha = 1f;
			component.interactable = true;
			component.blocksRaycasts = true;
		}
        else
        {
            Debug.Log("App: �ظ�ʵ������ִ�г�ʼ��");
        }
    }

	 IEnumerator DelayStart()
    {
        // 等待3帧确保引擎初始化
        for (int i = 0; i < 10; i++)
        {
            yield return null;
        }

        // 强制初始化SDK
#if UNITY_WEIXINMINIGAME && !UNITY_EDITOR
        // 1. 初始化微信SDK
        WX.InitSDK(OnWXInitialized);


        // 添加SDK调用防止裁剪
        WX.GetSystemInfo(new GetSystemInfoOption());
#endif
    }

	// 初始化完成回调
    private void OnWXInitialized(int code)
    {
        Debug.Log("微信SDK初始化完成 : " + code);
        // 这里执行需要SDK的代码
        FetchSystemInfo();
    }
    // 获取系统信息
    private void FetchSystemInfo()
    {
#if UNITY_WEIXINMINIGAME && !UNITY_EDITOR
        WX.GetSystemInfo(new GetSystemInfoOption());
#endif
    }

	public void LaunchGame(string forcedABTest)
	{
		Analytics.ForceABTest(forcedABTest);
		StartCoroutine(LaunchGameDelay());
	}

	private IEnumerator LaunchGameDelay()
	{
		yield return new WaitForEndOfFrame();
		SceneManager.LoadScene(1);
	}

	public static bool IsRelease()
	{
#if !UNITY_EDITOR
		return true;
#endif
		return true;//!Debug.isDebugBuild!;
	}

	private void InitializeVersion()
	{
		if (!IsRelease() && resetOnStart)
		{
			Data.Reset();
		}
	}

	public override int GetPriority()
	{
		return -1000;
	}

	public static State GetState()
	{
		return Singleton<App>.i.state;
	}

	protected override void OnNewGame()
	{
		state = State.Playing;
	}

	protected override void OnPause()
	{
		state = State.Pause;
	}

	protected override void OnUnpause()
	{
		state = State.Playing;
	}

	protected override void OnContinue()
	{
		state = State.Playing;
	}

	protected override void OnGameOver(bool canUseSecondChance)
	{
		state = State.GameOver;
		lastSecondChance = canUseSecondChance;
	}

	protected override void OnBackToMenu()
	{
		state = State.Menu;
		GC.Collect();
	}

	//public void OnInitializeFailure(InitializationFailureReason reason)
	//{
	//}

	public void OnPurchaseComplete(string productID)
	{
		Neuron.Purchased(productID);
	}

	//public void OnPurchaseFailure(string productID, PurchaseFailureReason reason)
	//{
	//}
}
