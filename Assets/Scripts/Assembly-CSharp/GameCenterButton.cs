using UnityEngine;
using UnityEngine.UI;

namespace LevelMode
{

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
}
