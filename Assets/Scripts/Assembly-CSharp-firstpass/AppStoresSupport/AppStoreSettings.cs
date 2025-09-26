using System;
using UnityEngine;

namespace AppStoresSupport
{
	[Serializable]
	public class AppStoreSettings : ScriptableObject
	{
		public string UnityClientID;
		public string UnityClientKey;
		public string UnityClientRSAPublicKey;
		public AppStoreSetting XiaomiAppStoreSetting;
	}
}
