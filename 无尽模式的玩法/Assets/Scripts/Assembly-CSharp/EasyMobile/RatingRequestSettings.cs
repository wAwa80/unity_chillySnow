using System;
using UnityEngine;

namespace EasyMobile
{
	[Serializable]
	public class RatingRequestSettings
	{
		[SerializeField]
		private RatingDialogContent _defaultRatingDialogContent = RatingDialogContent.Default;

		[SerializeField]
		[Range(0f, 5f)]
		private uint _minimumAcceptedStars = 4u;

		[SerializeField]
		private string _supportEmail;

		[SerializeField]
		private string _iosAppId;

		[SerializeField]
		[Range(3f, 100f)]
		private uint _annualCap = 12u;

		[SerializeField]
		[Range(0f, 365f)]
		private uint _delayAfterInstallation = 10u;

		[SerializeField]
		[Range(0f, 365f)]
		private uint _coolingOffPeriod = 10u;

		[SerializeField]
		private bool _ignoreContraintsInDevelopment;

		public RatingDialogContent DefaultRatingDialogContent => _defaultRatingDialogContent;

		public uint MinimumAcceptedStars => _minimumAcceptedStars;

		public string SupportEmail => _supportEmail;

		public string IosAppId => _iosAppId;

		public uint AnnualCap => _annualCap;

		public uint DelayAfterInstallation => _delayAfterInstallation;

		public uint CoolingOffPeriod => _coolingOffPeriod;

		public bool IgnoreConstraintsInDevelopment => _ignoreContraintsInDevelopment;
	}
}
