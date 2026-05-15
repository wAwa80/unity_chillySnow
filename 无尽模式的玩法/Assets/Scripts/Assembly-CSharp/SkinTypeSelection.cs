using UnityEngine;

public sealed class SkinTypeSelection : Singleton<SkinTypeSelection>
{
	private SelectableButton ballButton;

	private SelectableButton pineButton;

	private SelectableButton backgroundButton;

	[SerializeField]
	private SkinScroll balls;

	[SerializeField]
	private SkinScroll pines;

	[SerializeField]
	private SkinScroll backgrounds;

	private Skin.Type viewed;

	public Skin.Type GetViewed()
	{
		return viewed;
	}

	public SkinScroll GetScrollViewed()
	{
		switch (viewed)
		{
		case Skin.Type.Ball:
			return balls;
		case Skin.Type.Pine:
			return pines;
		case Skin.Type.Background:
			return backgrounds;
		default:
			return null;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		ballButton = base.transform.GetChild(0).GetComponent<SelectableButton>();
		ballButton.onClick.AddListener(delegate
		{
			Select(Skin.Type.Ball);
		});
		pineButton = base.transform.GetChild(1).GetComponent<SelectableButton>();
		pineButton.onClick.AddListener(delegate
		{
			Select(Skin.Type.Pine);
		});
		backgroundButton = base.transform.GetChild(2).GetComponent<SelectableButton>();
		backgroundButton.onClick.AddListener(delegate
		{
			Select(Skin.Type.Background);
		});
	}

	private void Start()
	{
		if (ballButton.IsSelected())
		{
			ApplySelected(Skin.Type.Ball);
		}
		else if (pineButton.IsSelected())
		{
			ApplySelected(Skin.Type.Pine);
		}
		else if (backgroundButton.IsSelected())
		{
			ApplySelected(Skin.Type.Background);
		}
		else
		{
			Select(Skin.Type.Ball);
		}
	}

	private void Select(Skin.Type type)
	{
		switch (type)
		{
		case Skin.Type.Ball:
			ballButton.SelectButton();
			pineButton.Unselect();
			backgroundButton.Unselect();
			break;
		case Skin.Type.Pine:
			ballButton.Unselect();
			pineButton.SelectButton();
			backgroundButton.Unselect();
			break;
		case Skin.Type.Background:
			ballButton.Unselect();
			pineButton.Unselect();
			backgroundButton.SelectButton();
			break;
		}
		ApplySelected(type);
	}

	private void ApplySelected(Skin.Type type)
	{
		viewed = type;
		switch (type)
		{
		case Skin.Type.Ball:
			balls.BecomeActive();
			pines.BecomeInactive();
			backgrounds.BecomeInactive();
			break;
		case Skin.Type.Pine:
			balls.BecomeInactive();
			pines.BecomeActive();
			backgrounds.BecomeInactive();
			break;
		case Skin.Type.Background:
			balls.BecomeInactive();
			pines.BecomeInactive();
			backgrounds.BecomeActive();
			break;
		}
	}

	public void UnlockedSkinOfType(Skin.Type type)
	{
		switch (type)
		{
		case Skin.Type.Ball:
			ballButton.ActivatePin();
			break;
		case Skin.Type.Pine:
			pineButton.ActivatePin();
			break;
		case Skin.Type.Background:
			backgroundButton.ActivatePin();
			break;
		}
	}

	public void RemovePin(Skin.Type type)
	{
		switch (type)
		{
		case Skin.Type.Ball:
			ballButton.DeactivatePin();
			break;
		case Skin.Type.Pine:
			pineButton.DeactivatePin();
			break;
		case Skin.Type.Background:
			backgroundButton.DeactivatePin();
			break;
		}
	}
}
