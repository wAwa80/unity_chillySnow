using System;
using UnityEngine;
using UnityEngine.Purchasing;

namespace VoodooSauceInternal
{
	[Serializable]
	internal class ProductDescriptor
	{
		[SerializeField]
		private string _productId;
		[SerializeField]
		private ProductType _type;
		[SerializeField]
		private float _priceInUSD;
	}
}
