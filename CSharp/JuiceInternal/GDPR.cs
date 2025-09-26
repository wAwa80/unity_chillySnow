using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JuiceInternal
{
	internal sealed class GDPR : MonoBehaviour
	{
		[Serializable]
		internal class ConsentChangeEvent : UnityEvent<bool>
		{
		}

		private static bool m_TermsConsent;

		private const string TERMS_CONSENT_KEY = "94653784192657574819";

		private static bool m_PersonalizedAdsConsent;

		private const string PERSONALIZED_ADS_CONSENT_KEY = "22943788165955326078";

		private static bool m_AnalyticsConsent;

		private const string ANALYTICS_CONSENT_KEY = "05640878948078746958";

		private static bool loaded;

		private static GDPR instance;

		private const string PERSONALIZED_ADS_ON_TEXT = "See ads tailored by your interests";

		private const string PERSONALIZED_ADS_OFF_TEXT = "Don't see ads personalised by your interests";

		private const string PRIVACY_POLICY_URL = "http://www.acidcousins.com/";

		[SerializeField]
		private CanvasGroup background;

		[SerializeField]
		private Transform pages;

		[SerializeField]
		private CanvasGroup introductionPage;

		[SerializeField]
		private CanvasGroup explanationPage;

		[SerializeField]
		private CanvasGroup settingsPage;

		[SerializeField]
		private CanvasGroup confirmationPage;

		[SerializeField]
		private AnimatedToggle termsToggle;

		[SerializeField]
		private TextMeshProUGUI introductionPageTitle;

		[SerializeField]
		private TextMeshProUGUI introductionPageParagraph1;

		private ClickableLink introductionLink;

		[SerializeField]
		private Button introductionPagePlayButton;

		[SerializeField]
		private Button introductionPageMoreInformationButton;

		[SerializeField]
		private Button explanationPageBackButton;

		[SerializeField]
		private Button explanationPageMoreInformationButton;

		[SerializeField]
		private AnimatedToggle adsToggle;

		[SerializeField]
		private TextMeshProUGUI adsText;

		[SerializeField]
		private AnimatedToggle analyticsToggle;

		[SerializeField]
		private Button settingsPagePlayButton;

		[SerializeField]
		private Button confirmationPageFixSettingsButton;

		[SerializeField]
		private Button confirmationPageIUnderstandButton;

		private bool termsConsentChanged;

		internal static readonly ConsentChangeEvent onTermsConsentChanged = new ConsentChangeEvent();

		private bool personalizedAdsConsentChanged;

		internal static readonly ConsentChangeEvent onPersonalizedAdsConsentChanged = new ConsentChangeEvent();

		private bool analyticsConsentChanged;

		internal static readonly ConsentChangeEvent onAnalyticsConsentChanged = new ConsentChangeEvent();

		private CanvasGroup lastPage;

		private float t;

		[SerializeField]
		private float animationSpeed = 2f;

		internal static bool termsConsent
		{
			get
			{
				if (!loaded)
				{
					m_TermsConsent = LoadKey("94653784192657574819", defaultValue: false);
				}
				return m_TermsConsent;
			}
		}

		internal static bool perosnalizedAdsConsent
		{
			get
			{
				if (!loaded)
				{
					m_PersonalizedAdsConsent = LoadKey("22943788165955326078", defaultValue: true);
				}
				return m_PersonalizedAdsConsent;
			}
		}

		internal static bool analyticsConsent
		{
			get
			{
				if (!loaded)
				{
					m_AnalyticsConsent = LoadKey("05640878948078746958", defaultValue: true);
				}
				return m_AnalyticsConsent;
			}
		}

		private static void SaveKey(string key, bool value)
		{
			PlayerPrefs.SetInt(key, value ? 1 : 0);
		}

		private static bool LoadKey(string key, bool defaultValue)
		{
			if (PlayerPrefs.HasKey(key))
			{
				return PlayerPrefs.GetInt(key) == 1;
			}
			return defaultValue;
		}

		private static void Setup()
		{
			if (!loaded)
			{
				m_TermsConsent = LoadKey("94653784192657574819", defaultValue: false);
				m_PersonalizedAdsConsent = LoadKey("22943788165955326078", defaultValue: true);
				m_AnalyticsConsent = LoadKey("05640878948078746958", defaultValue: true);
				Log.Message("GDPR", "Player consents are Terms (" + ((!m_TermsConsent) ? "OFF" : "ON") + "), Personalized Ads (" + ((!m_PersonalizedAdsConsent) ? "OFF" : "ON") + ") and Analytics (" + ((!m_AnalyticsConsent) ? "OFF" : "ON") + ")");
				if (UnityEngine.Object.FindObjectOfType<EventSystem>() == null)
				{
					UnityEngine.Object.DontDestroyOnLoad(new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule)));
				}
				GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("GDPR"));
				gameObject.name = "SYSTEM_OBJECT GDPR";
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				instance = gameObject.GetComponent<GDPR>();
			}
		}

		private void Awake()
		{
			termsToggle.isOn = m_TermsConsent;
			termsToggle.onToggle.AddListener(OnTermsConsentToggle);
			introductionPageTitle.text = string.Format(introductionPageTitle.text, Application.productName);
			introductionPageParagraph1.text = string.Format(introductionPageParagraph1.text, Application.productName, "http://www.acidcousins.com/");
			introductionLink = introductionPageParagraph1.GetComponent<ClickableLink>();
			introductionLink.onClick.AddListener(ShowPrivacyPolicy);
			introductionPagePlayButton.interactable = m_TermsConsent;
			introductionPagePlayButton.onClick.AddListener(TryClose);
			introductionPageMoreInformationButton.onClick.AddListener(ShowExplanationPage);
			explanationPageBackButton.onClick.AddListener(ShowIntroductionPage);
			explanationPageMoreInformationButton.onClick.AddListener(ShowSettingsPage);
			adsToggle.isOn = m_PersonalizedAdsConsent;
			adsToggle.onToggle.AddListener(OnAdsConsentToggle);
			if (m_PersonalizedAdsConsent)
			{
				adsText.text = "See ads tailored by your interests";
			}
			else
			{
				adsText.text = "Don't see ads personalised by your interests";
			}
			analyticsToggle.isOn = m_AnalyticsConsent;
			analyticsToggle.onToggle.AddListener(OnAnalyticsConsentToggle);
			settingsPagePlayButton.onClick.AddListener(TryClose);
			confirmationPageFixSettingsButton.onClick.AddListener(ShowSettingsPage);
			confirmationPageIUnderstandButton.onClick.AddListener(Close);
			loaded = true;
			if (m_TermsConsent)
			{
				base.gameObject.SetActive(value: false);
				return;
			}
			termsConsentChanged = true;
			personalizedAdsConsentChanged = true;
			analyticsConsentChanged = true;
			_Open();
		}

		internal static void Open()
		{
			instance._Open();
		}

		private void _Open()
		{
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(value: true);
			}
			PlayShowAnimation();
			if (m_TermsConsent)
			{
				ShowSettingsPage();
			}
			else
			{
				ShowIntroductionPage();
			}
		}

		private void TryClose()
		{
			if (!m_AnalyticsConsent)
			{
				ShowConfirmationPage();
			}
			else if (!m_TermsConsent)
			{
				ShowIntroductionPage();
			}
			else
			{
				Close();
			}
		}

		private void Close()
		{
			if (!m_TermsConsent)
			{
				ShowIntroductionPage();
				return;
			}
			PlayHideAnimation();
			if (termsConsentChanged || personalizedAdsConsentChanged || analyticsConsentChanged)
			{
				if (termsConsentChanged)
				{
					Log.Message("GDPR", "Player consent for Terms changed to " + ((!m_TermsConsent) ? "OFF" : "ON"));
					SaveKey("94653784192657574819", m_TermsConsent);
					onTermsConsentChanged.Invoke(m_TermsConsent);
				}
				termsConsentChanged = false;
				if (analyticsConsentChanged)
				{
					Log.Message("GDPR", "Player consent for Analytics changed to " + ((!m_AnalyticsConsent) ? "OFF" : "ON"));
					SaveKey("05640878948078746958", m_AnalyticsConsent);
					onAnalyticsConsentChanged.Invoke(m_AnalyticsConsent);
				}
				analyticsConsentChanged = false;
				if (personalizedAdsConsentChanged)
				{
					Log.Message("GDPR", "Player consent for Personalized Ads changed to " + ((!m_PersonalizedAdsConsent) ? "OFF" : "ON"));
					SaveKey("22943788165955326078", m_PersonalizedAdsConsent);
					onPersonalizedAdsConsentChanged.Invoke(m_PersonalizedAdsConsent);
				}
				personalizedAdsConsentChanged = false;
				PlayerPrefs.Save();
			}
		}

		private void ShowPage(CanvasGroup page)
		{
			if (!(lastPage == page))
			{
				if (lastPage != null)
				{
					lastPage.alpha = 0f;
					lastPage.interactable = false;
					lastPage.blocksRaycasts = false;
				}
				lastPage = page;
				lastPage.alpha = 1f;
				lastPage.interactable = true;
				lastPage.blocksRaycasts = true;
			}
		}

		private void ShowIntroductionPage()
		{
			ShowPage(introductionPage);
		}

		private void ShowExplanationPage()
		{
			ShowPage(explanationPage);
		}

		private void ShowSettingsPage()
		{
			ShowPage(settingsPage);
		}

		private void ShowConfirmationPage()
		{
			ShowPage(confirmationPage);
		}

		public void ShowPrivacyPolicy()
		{
			Application.OpenURL("http://www.acidcousins.com/");
		}

		private void OnTermsConsentToggle(bool value)
		{
			if (value != m_TermsConsent)
			{
				m_TermsConsent = value;
				termsConsentChanged = true;
				introductionPagePlayButton.interactable = m_TermsConsent;
			}
		}

		private void OnAdsConsentToggle(bool value)
		{
			if (value != m_PersonalizedAdsConsent)
			{
				m_PersonalizedAdsConsent = value;
				personalizedAdsConsentChanged = true;
				if (m_PersonalizedAdsConsent)
				{
					adsText.text = "See ads tailored by your interests";
				}
				else
				{
					adsText.text = "Don't see ads personalised by your interests";
				}
			}
		}

		private void OnAnalyticsConsentToggle(bool value)
		{
			if (value != m_AnalyticsConsent)
			{
				m_AnalyticsConsent = value;
				analyticsConsentChanged = true;
			}
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
				}
				background.alpha = t * t;
			}
			float num = OneMinusSinCardNormalized(t);
			pages.transform.localScale = new Vector3(num, num, num);
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
