using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SkinSprite : Neuron
{
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private Skin.Type type;

	protected override void Awake()
	{
		base.Awake();
		spriteRenderer = GetComponent<SpriteRenderer>();
		Skin selected = Skin.GetSelected(type);
		if (selected != null)
		{
			OnSkinSelected(selected);
		}
	}

	protected override void OnSkinSelected(Skin skin)
	{
		if (skin.GetSkinType() == type)
		{
			spriteRenderer.sprite = skin.GetSprite();
			if (skin.GetSkinType() != Skin.Type.Pine)
			{
				spriteRenderer.color = skin.GetColor();
			}
		}
	}
}
