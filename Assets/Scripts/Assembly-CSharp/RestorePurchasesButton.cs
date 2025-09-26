using UnityEngine;

public sealed class RestorePurchasesButton : MonoBehaviour
{
	private void Awake()
	{
		Object.DestroyImmediate(base.gameObject);
	}

	private void OnClick()
	{
		//Juice.store.RestorePurchases();
	}
}
