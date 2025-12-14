using System;
using UnityEngine;

namespace EasyMobile
{
	[Serializable]
	public class IAPSettings
	{
		[SerializeField]
		private IAPAndroidStore _targetAndroidStore;

		[SerializeField]
		private bool _validateAppleReceipt = true;

		[SerializeField]
		private bool _validateGooglePlayReceipt = true;

		[SerializeField]
		private IAPProduct[] _products;

		public IAPAndroidStore TargetAndroidStore => _targetAndroidStore;

		public bool IsValidateAppleReceipt => _validateAppleReceipt;

		public bool IsValidateGooglePlayReceipt => _validateGooglePlayReceipt;

		public IAPProduct[] Products => _products;
	}
}
