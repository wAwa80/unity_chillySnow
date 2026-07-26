using UnityEngine;
using UnityEngine.UI;

public sealed class GDPRButton : MonoBehaviour
{
	private void Awake()
	{
		if (!JuiceInternal.JuiceConsentGates.EnableGdpr)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		GetComponent<Button>().onClick.AddListener(OnClick);
	}

	private void OnClick()
	{
		Juice.ShowGDPR();
		Juice.analytics.SendDesignEvent("Player:ClickedOnGDPRPopup");
	}
}
