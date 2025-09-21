using UnityEngine;
using UnityEngine.UI;

public class MenuScores : Singleton<MenuScores>
{
	[SerializeField]
	private CanvasGroup newBestScore;
	[SerializeField]
	private Animation shineAnimation;
	[SerializeField]
	private Text newBestScoreText;
}
