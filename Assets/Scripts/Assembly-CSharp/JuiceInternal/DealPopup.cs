using UnityEngine;
using UnityEngine.UI;

namespace JuiceInternal
{
	internal class DealPopup : MonoBehaviour
	{
		[SerializeField]
		private CanvasGroup background;
		[SerializeField]
		private Transform panel;
		[SerializeField]
		private Button buyButton;
		[SerializeField]
		private Button noThanksButton;
		[SerializeField]
		private Text oldPrice;
		[SerializeField]
		private Text newPrice;
		[SerializeField]
		private float animationSpeed;
	}
}
