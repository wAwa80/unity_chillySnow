using System;
using System.Collections;
using EasyMobile;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


namespace EndlessMode
{
	/// <summary>
	/// 必须早于其它 Neuron（如 MenuScores/Tutorial）Awake：
	/// Data/Stats 在本类 Awake 里动态创建，过晚会导致 GetTop() 空引用。
	/// </summary>
	[DefaultExecutionOrder(-1000)]
	public sealed class App : Singleton<App>
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

		/// <summary>
		/// 防重入：连续回菜单只预约一次下一帧 GC。
		/// </summary>
		private bool gcScheduled;

		public static string GetStoreLink()
		{
			return "https://play.google.com/store/apps/details?id=com.acidcousins.chilly";
		}

		protected override void Awake()
		{
	        Debug.Log("App.Awake 开始");
	        base.Awake();
			// 与 Singleton 配合：重复 App 不会成为 i，直接销毁，避免再 Init / LoadScene
			if (i != null && i != this)
			{
				Debug.Log("App: 重复实例，销毁并跳过初始化");
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
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

				StartCoroutine(LaunchGameDelay());
					return;
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
	            Debug.Log("App: 重复实例，跳过初始化");
	        }
	    }

		public void LaunchGame(string forcedABTest)
		{
			Analytics.ForceABTest(forcedABTest);
			StartCoroutine(LaunchGameDelay());
		}

		private IEnumerator LaunchGameDelay()
		{
			yield return new WaitForEndOfFrame();
			// 已在游戏场景时禁止再 LoadScene：否则会反复重载同场景，
			// 新 App 在 Singleton 构造里覆盖 i 并再次 LaunchGameDelay，形成死循环。
			// （原先 LoadScene(1) 因 Build Settings 无 index 1 而失败，反而“碰巧”不重载。）
			Scene active = SceneManager.GetActiveScene();
			if (active.IsValid() && active.name == "MainEndlessMode")
			{
				EnsureSingleEventSystem();
				yield break;
			}
			SceneManager.LoadScene("MainEndlessMode");
			yield return null;
			EnsureSingleEventSystem();
		}

		private void OnEnable()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			EnsureSingleEventSystem();
		}

		/// <summary>
		/// 日志刷屏「There are 2 event systems」：销毁多余 EventSystem，保留第一个。
		/// GDPR DontDestroy 的 EventSystem 与场景内 EventSystem 常会叠两份。
		/// </summary>
		private static void EnsureSingleEventSystem()
		{
			EventSystem[] systems = UnityEngine.Object.FindObjectsOfType<EventSystem>();
			if (systems == null || systems.Length <= 1)
			{
				return;
			}
			for (int i = 1; i < systems.Length; i++)
			{
				if (systems[i] != null)
				{
					UnityEngine.Object.Destroy(systems[i].gameObject);
				}
			}
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
			// 延后一帧 GC，保证同帧 PineGenerator.Spawn 等订阅者先完成
			if (gcScheduled)
			{
				return;
			}
			gcScheduled = true;
			StartCoroutine(DeferredCollect());
		}

		private IEnumerator DeferredCollect()
		{
			yield return null;
			gcScheduled = false;
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
}
