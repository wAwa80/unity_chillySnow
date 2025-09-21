using UnityEngine;
using mixpanel;
using GameAnalyticsSDK;
using TapjoyUnity.Internal;

namespace VoodooSauceInternal
{
	internal class VoodooSauceBehaviour : MonoBehaviour
	{
		[SerializeField]
		private Mixpanel _mixpanelPrefab;
		[SerializeField]
		private GameAnalytics _gameAnalyticsPrefab;
		[SerializeField]
		private TapjoyComponent _tapjoyPrefab;
	}
}
