using UnityEngine;
using UnityEngine.UI;

namespace LevelMode
{

	public sealed class GDPRButton : MonoBehaviour
	{
		private void Awake()
		{
			// 功能开关关闭：隐藏设置页 GDPR 入口（通常为 Settings 下第一个子物体）
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
}
