using UnityEngine;
using UnityEngine.UI;

public sealed class GameCenterButton : MonoBehaviour
{
	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(OnClick);
	}

	private void OnClick()
	{
		Juice.gameCenter.Show();
	}
}
