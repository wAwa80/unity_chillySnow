using UnityEngine;

public class Page<T> : Singleton<T>
{
	[SerializeField]
	private bool fadeIn;
	[SerializeField]
	private bool fadeOut;
	[SerializeField]
	private RectTransform panel;
}
