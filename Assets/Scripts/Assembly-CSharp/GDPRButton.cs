using UnityEngine;
using UnityEngine.UI;

namespace LevelMode
{

	public sealed class GDPRButton : MonoBehaviour
	{
		private void Awake()
		{
			GetComponent<Button>().onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			Juice.ShowGDPR();
			Juice.analytics.SendDesignEvent("Player:ClickedOnGDPRPopup");
		}
	}
}
