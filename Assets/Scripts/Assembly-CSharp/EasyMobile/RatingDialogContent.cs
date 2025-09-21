using System;
using UnityEngine;

namespace EasyMobile
{
	[Serializable]
	public class RatingDialogContent
	{
		[SerializeField]
		private string _title;
		[SerializeField]
		private string _message;
		[SerializeField]
		private string _lowRatingMessage;
		[SerializeField]
		private string _highRatingMessage;
		[SerializeField]
		private string _postponeButtonText;
		[SerializeField]
		private string _refuseButtonText;
		[SerializeField]
		private string _rateButtonText;
		[SerializeField]
		private string _cancelButtonText;
		[SerializeField]
		private string _feedbackButtonText;
	}
}
