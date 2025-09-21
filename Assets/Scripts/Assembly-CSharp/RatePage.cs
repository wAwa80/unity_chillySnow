using UnityEngine;
using UnityEngine.UI;

public class RatePage : Page<RatePage>
{
	[SerializeField]
	private CanvasGroup starPanel;
	[SerializeField]
	private CanvasGroup badPanel;
	[SerializeField]
	private Button validateRatingButton;
	[SerializeField]
	private Button[] stars;
}
