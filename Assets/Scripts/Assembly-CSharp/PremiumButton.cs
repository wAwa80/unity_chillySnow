public class PremiumButton : AnimatedButton<PremiumButton>
{
	protected override void Awake()
	{
		base.Awake();
		button.onClick.AddListener(PurchasePremium);
	}

	private void PurchasePremium()
	{
		VoodooSauce.Purchase("chilly_noads");
	}

	protected override void OnPurchased(string product)
	{
		if (product == "chilly_noads")
		{
			VoodooSauce.EnablePremium();
		}
	}
}
