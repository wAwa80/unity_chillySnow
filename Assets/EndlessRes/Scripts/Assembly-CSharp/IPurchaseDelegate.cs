namespace EndlessMode
{
	public interface IPurchaseDelegate
	{
		//void OnInitializeFailure(InitializationFailureReason reason);

		void OnPurchaseComplete(string productId);

		//void OnPurchaseFailure(string productId, PurchaseFailureReason reason);
	}
}
