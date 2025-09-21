using System;
using UnityEngine;

namespace EasyMobile
{
	[Serializable]
	public class RatingRequestSettings
	{
		[SerializeField]
		private RatingDialogContent _defaultRatingDialogContent;
		[SerializeField]
		private uint _minimumAcceptedStars;
		[SerializeField]
		private string _supportEmail;
		[SerializeField]
		private string _iosAppId;
		[SerializeField]
		private uint _annualCap;
		[SerializeField]
		private uint _delayAfterInstallation;
		[SerializeField]
		private uint _coolingOffPeriod;
		[SerializeField]
		private bool _ignoreContraintsInDevelopment;
	}
}
