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
		private bool _validateAppleReceipt;
		[SerializeField]
		private bool _validateGooglePlayReceipt;
		[SerializeField]
		private IAPProduct[] _products;
	}
}
