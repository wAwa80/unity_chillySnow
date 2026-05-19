using System;
using UnityEngine;

namespace EasyMobile
{
	[Serializable]
	public class RatingDialogContent
	{
		public const string PRODUCT_NAME_PLACEHOLDER = "$PRODUCT_NAME";

		public static readonly RatingDialogContent Default = new RatingDialogContent();

		[SerializeField]
		private string _title = "Rate $PRODUCT_NAME";

		[SerializeField]
		private string _message = "How would you rate $PRODUCT_NAME?";

		[SerializeField]
		private string _lowRatingMessage = "That's bad. Would you like to give us some feedback instead?";

		[SerializeField]
		private string _highRatingMessage = "Awesome! Let's do it!";

		[SerializeField]
		private string _postponeButtonText = "Not Now";

		[SerializeField]
		private string _refuseButtonText = "Don't Ask Again";

		[SerializeField]
		private string _rateButtonText = "Rate Now!";

		[SerializeField]
		private string _cancelButtonText = "Cancel";

		[SerializeField]
		private string _feedbackButtonText = "Send Feedback";

		public string Title => _title;

		public string Message => _message;

		public string LowRatingMessage => _lowRatingMessage;

		public string HighRatingMessage => _highRatingMessage;

		public string PostponeButtonText => _postponeButtonText;

		public string RefuseButtonText => _refuseButtonText;

		public string RateButtonText => _rateButtonText;

		public string CancelButtonText => _cancelButtonText;

		public string FeedbackButtonText => _feedbackButtonText;

		private RatingDialogContent()
		{
		}

		public RatingDialogContent(string title, string message, string lowRatingMessage, string highRatingMessage, string postponeButtonText, string refuseButtonText, string rateButtonText, string cancelButtonText, string feedbackButtonText)
		{
			_title = ((title != null) ? title : string.Empty);
			_message = ((message != null) ? message : string.Empty);
			_lowRatingMessage = ((lowRatingMessage != null) ? lowRatingMessage : string.Empty);
			_highRatingMessage = ((highRatingMessage != null) ? highRatingMessage : string.Empty);
			_postponeButtonText = ((postponeButtonText != null) ? postponeButtonText : string.Empty);
			_refuseButtonText = ((refuseButtonText != null) ? refuseButtonText : string.Empty);
			_rateButtonText = ((rateButtonText != null) ? rateButtonText : string.Empty);
			_cancelButtonText = ((cancelButtonText != null) ? cancelButtonText : string.Empty);
			_feedbackButtonText = ((feedbackButtonText != null) ? feedbackButtonText : string.Empty);
		}
	}
}
