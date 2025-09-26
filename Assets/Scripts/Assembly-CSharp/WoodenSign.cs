using UnityEngine;

public class WoodenSign : Singleton<WoodenSign>
{
	private TextMesh textMesh;

	protected override void Awake()
	{
		base.Awake();
		textMesh = base.transform.GetChild(1).GetComponent<TextMesh>();
	}

	protected override void OnRefresh()
	{
		textMesh.text = Level.Get().ToString();
		textMesh.characterSize = Mathf.Min(0.1f, 0.24f / (float)textMesh.text.Length);
	}
}
