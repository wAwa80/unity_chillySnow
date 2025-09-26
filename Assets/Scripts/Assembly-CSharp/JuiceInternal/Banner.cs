using UnityEngine;

namespace JuiceInternal
{
	public sealed class Banner : MonoBehaviour
	{
		private class BannerBackground : MonoBehaviour
		{
			private Material quadMaterial;

			private bool debug;

			private bool use;

			private void Awake()
			{
				quadMaterial = new Material(Shader.Find("Unlit/Color"));
				use = true;
				debug = Settings.Get().debug;
			}

			private void Update()
			{
				if (debug && Input.GetMouseButtonDown(0) && Input.mousePosition.y < 5f / 64f * (float)Screen.height)
				{
					use = !use;
				}
			}

			private void OnPostRender()
			{
				if (use)
				{
					GL.PushMatrix();
					quadMaterial.SetPass(0);
					GL.LoadOrtho();
					GL.Begin(7);
					GL.Color(Color.white);
					float y = 5f / 64f;
					GL.Vertex3(0f, y, 0f);
					GL.Vertex3(1f, y, 0f);
					GL.Vertex3(1f, 0f, 0f);
					GL.Vertex3(0f, 0f, 0f);
					GL.End();
					GL.PopMatrix();
				}
			}
		}

		private const string LOG_PREFIX = "Ads - Banner";

		private const string FORMATTER = "{0} event received";

		private const string FORMATTER_WITH_ERROR = "{0} event received with error \"{1}\" (Code {2} - Error {3})";

		private BannerBackground currentBackground;

		private bool turnedOn;

		private void LogEvent(string eventName)
		{
			Log.Message("Ads - Banner", $"{eventName} event received");
		}

		//private void LogEvent(string eventName, IronSourceError error)
		//{
		//	Log.Message("Ads - Banner", $"{eventName} event received with error \"{error.getDescription()}\" (Code {error.getCode()} - Error {error.getErrorCode()})");
		//}

		private void Awake()
		{
			base.enabled = false;
		}

		public void TurnOn()
		{
			if (!turnedOn && !Ads.forceNoFuckingAds && !Module<Premium>.GetInstance().IsPremium())
			{
				turnedOn = true;
				//IronSourceEvents.onBannerAdLoadedEvent += OnBannerLoaded;
				//IronSourceEvents.onBannerAdLoadFailedEvent += OnBannerFailedToLoad;
				//IronSourceEvents.onBannerAdScreenPresentedEvent += OnBannerShown;
				//IronSourceEvents.onBannerAdClickedEvent += OnBannerClicked;
				//IronSourceEvents.onBannerAdScreenDismissedEvent += OnBannerClosed;
				//IronSourceEvents.onBannerAdLeftApplicationEvent += OnBannerLeftApplication;
				//IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
			}
		}

		public void TurnOff()
		{
			if (turnedOn)
			{
				turnedOn = false;
				//IronSourceEvents.onBannerAdLoadedEvent -= OnBannerLoaded;
				//IronSourceEvents.onBannerAdLoadFailedEvent -= OnBannerFailedToLoad;
				//IronSourceEvents.onBannerAdScreenPresentedEvent -= OnBannerShown;
				//IronSourceEvents.onBannerAdClickedEvent -= OnBannerClicked;
				//IronSourceEvents.onBannerAdScreenDismissedEvent -= OnBannerClosed;
				//IronSourceEvents.onBannerAdLeftApplicationEvent -= OnBannerLeftApplication;
				//base.enabled = false;
				//IronSource.Agent.destroyBanner();
				if (currentBackground != null)
				{
					Object.Destroy(currentBackground);
					currentBackground = null;
				}
			}
		}

		private void Update()
		{
			if (Camera.main != null && Camera.main.GetComponent<BannerBackground>() == null)
			{
				if (currentBackground != null)
				{
					Object.Destroy(currentBackground);
				}
				currentBackground = Camera.main.gameObject.AddComponent<BannerBackground>();
			}
		}

		private void OnBannerLoaded()
		{
			LogEvent("OnBannerLoaded");
			Module<Analytics>.GetInstance().SendDesignEvent("Ads:Banner:Shown");
			base.enabled = true;
		}

		//private void OnBannerFailedToLoad(IronSourceError error)
		//{
		//	LogEvent("OnBannerFailedToLoad", error);
		//}

		private void OnBannerShown()
		{
			LogEvent("OnBannerShown");
		}

		private void OnBannerClicked()
		{
			LogEvent("OnBannerClicked");
			Module<Analytics>.GetInstance().SendDesignEvent("Ads:Banner:Clicked");
		}

		private void OnBannerClosed()
		{
			LogEvent("OnBannerClosed");
		}

		private void OnBannerLeftApplication()
		{
			LogEvent("OnBannerLeftApplication");
		}
	}
}
