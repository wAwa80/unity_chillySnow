using System;
using System.Collections;
using EasyMobile;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class App : Singleton<App>, IPurchaseDelegate
{
	public enum State
	{
		Menu,
		Playing,
		Pause,
		GameOver
	}

	public const string NAME = "Chilly Snow";

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
		base.Awake();
		InitializeVersion();
		UnityEngine.Object.DontDestroyOnLoad(this);
		base.name = "SYSTEM_OBJECT App";
		Translator.Load();
		UnityEngine.Object.DontDestroyOnLoad(new GameObject("SYSTEM_OBJECT Easy Mobile", typeof(EM_PrefabManager), typeof(GameServiceManager)));
		UnityEngine.Object.DontDestroyOnLoad(new GameObject("SYSTEM_OBJECT Data Auto Saver", typeof(Data)));
		UnityEngine.Object.DontDestroyOnLoad(new GameObject("SYSTEM_OBJECT Stats Saver", typeof(Stats)));
		UnityEngine.Object.DontDestroyOnLoad(new GameObject("SYSTEM_OBJECT Analytics", typeof(Analytics)));
		VoodooSauce.RegisterPurchaseDelegate(this);
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
		return !Debug.isDebugBuild;
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
