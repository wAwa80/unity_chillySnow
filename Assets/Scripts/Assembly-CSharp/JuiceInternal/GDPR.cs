using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace JuiceInternal
{
	internal class GDPR : MonoBehaviour
	{
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
		[SerializeField]
		private float animationSpeed;
	}
}
