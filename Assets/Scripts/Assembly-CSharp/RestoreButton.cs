public class RestoreButton : AnimatedButton<RestoreButton>
{
	protected override void Awake()
	{
		base.Awake();
		button.onClick.AddListener(RestorePremium);
	}

	private void RestorePremium()
	{
		//VoodooSauce.RestorePurchases();
	}
}
