using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SkinImage : Neuron
{
	private Image image;

	[SerializeField]
	private Skin.Type type;

	protected override void Awake()
	{
		base.Awake();
		image = GetComponent<Image>();
	}

	protected override void OnSkinSelected(Skin skin)
	{
		if (skin.GetSkinType() == type)
		{
			image.sprite = skin.GetSprite();
			image.color = skin.GetColor();
		}
	}
}
