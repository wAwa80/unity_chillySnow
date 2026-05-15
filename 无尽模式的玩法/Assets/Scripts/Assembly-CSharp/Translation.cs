using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Translation : MonoBehaviour
{
	private Text text;

	[SerializeField]
	private bool caps;

	private void Awake()
	{
		text = GetComponent<Text>();
		text.text = Translator.Translate(text.text);
		if (caps)
		{
			text.text = text.text.ToUpper();
		}
	}
}
