using UnityEngine;
using UnityEngine.UI;

public sealed class SelectableButton : Button
{
	private Image borders;

	private Image pin;

	private Text text;

	[SerializeField]
	private bool hasPin;

	[SerializeField]
	private bool isControlled;

	private string saveID;

	private string savePinID;

	protected override void Awake()
	{
		base.Awake();
		saveID = $"selectableButton{base.name}";
		savePinID = $"selectableButtonPin{base.name}";
		borders = base.transform.GetChild(base.transform.childCount - 3).GetComponent<Image>();
		borders.enabled = Data.LoadBool(saveID);
		if (!isControlled)
		{
			base.onClick.AddListener(Toggle);
		}
		if (hasPin)
		{
			pin = base.transform.GetChild(base.transform.childCount - 2).GetComponent<Image>();
			pin.enabled = Data.LoadBool(savePinID);
			text = base.transform.GetChild(base.transform.childCount - 1).GetComponent<Text>();
			text.color = new Color(0f, 0f, 0f, (!borders.enabled) ? 0.5f : 1f);
		}
	}

	public bool IsSelected()
	{
		return borders.enabled;
	}

	private void Toggle()
	{
		if (borders.enabled)
		{
			Unselect();
		}
		else
		{
			SelectButton();
		}
	}

	public void SelectButton()
	{
		if (!borders.enabled)
		{
			borders.enabled = true;
			Data.SaveBool(saveID, value: true);
			text.color = new Color(0f, 0f, 0f, 1f);
			if (!isControlled && hasPin && pin.enabled)
			{
				pin.enabled = false;
				Data.SaveBool(savePinID, value: false);
			}
		}
	}

	public void Unselect()
	{
		if (borders.enabled)
		{
			borders.enabled = false;
			Data.SaveBool(saveID, value: false);
			text.color = new Color(0f, 0f, 0f, 0.5f);
		}
	}

	public void ActivatePin()
	{
		if (hasPin && !pin.enabled)
		{
			pin.enabled = true;
			Data.SaveBool(savePinID, value: true);
		}
	}

	public void DeactivatePin()
	{
		if (hasPin && pin.enabled)
		{
			pin.enabled = false;
			Data.SaveBool(savePinID, value: false);
		}
	}
}
