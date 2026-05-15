using System;
using UnityEngine;

namespace EasyMobile
{
	[Serializable]
	public class IAPProduct
	{
		[Serializable]
		public class StoreSpecificId
		{
			public IAPStore store;

			public string id;
		}

		[SerializeField]
		private string _name;

		[SerializeField]
		private IAPProductType _type;

		[SerializeField]
		private string _id;

		[SerializeField]
		private string _price;

		[SerializeField]
		private string _description;

		[SerializeField]
		private StoreSpecificId[] _storeSpecificIds;

		public string Name => _name;

		public string Id => _id;

		public IAPProductType Type => _type;

		public string Price => _price;

		public string Description => _description;

		public StoreSpecificId[] StoreSpecificIds => _storeSpecificIds;
	}
}
