using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(Button))]
public class Skin : Multiton<Skin>
{
	public enum Type
	{
		Ball,
		Pine,
		Background
	}

	public enum State
	{
		Locked = 1,
		Unlocked,
		New,
		Selected
	}

	private static Dictionary<Type, Skin> selected;

	private State currentState;

	private string saveID;

	private Button button;

	private Image skin;

	[SerializeField]
	private Type type;

	[SerializeField]
	private State defaultState;

	[SerializeField]
	private Color skinColor = Color.white;

	[SerializeField]
	private Image check;

	[SerializeField]
	private Image newIcon;

	[SerializeField]
	private string description;

	[SerializeField]
	private bool needsRV;

	private bool seen;

	public override string Name => base.name;

	public static Skin GetSelected(Type type)
	{
		if (selected != null && selected.ContainsKey(type))
		{
			return selected[type];
		}
		return null;
	}

	private static void SetSelected(Skin skin)
	{
		if (selected == null)
		{
			selected = new Dictionary<Type, Skin>();
		}
		if (selected.ContainsKey(skin.type) && selected[skin.type] != skin)
		{
			selected[skin.type].SetState(State.Unlocked);
		}
		selected[skin.type] = skin;
		Neuron.SkinSelected(skin);
	}

	protected override void Awake()
	{
		base.Awake();
		saveID = $"skin{Name}";
		skin = GetComponent<Image>();
		button = GetComponent<Button>();
		button.onClick.AddListener(Select);
	}

	private void Start()
	{
		State state = (State)Data.LoadInt(saveID, (int)defaultState);
		SetState(state, initialization: true);
	}

	private void SetState(State state, bool initialization = false)
	{
		if (!initialization)
		{
			if (state == currentState)
			{
				return;
			}
			Data.SaveInt(saveID, (int)state);
		}
		if (state == State.Selected)
		{
			SetSelected(this);
		}
		OnStateChanged(state);
	}

	public State GetState()
	{
		return currentState;
	}

	public bool IsUnlocked()
	{
		return currentState != State.Locked;
	}

	public Type GetSkinType()
	{
		return type;
	}

	public Sprite GetSprite()
	{
		return skin.sprite;
	}

	public Color GetColor()
	{
		return skinColor;
	}

	public string GetDescription()
	{
		return description;
	}

	public bool NeedsRV()
	{
		return needsRV;
	}

	private void OnStateChanged(State newState)
	{
		currentState = newState;
		switch (newState)
		{
		case State.Locked:
			skin.color = new Color(skin.color.r, skin.color.g, skin.color.b, 0.5f);
			newIcon.enabled = false;
			check.enabled = false;
			seen = false;
			break;
		case State.Unlocked:
			skin.color = new Color(skin.color.r, skin.color.g, skin.color.b, 1f);
			newIcon.enabled = false;
			check.enabled = false;
			seen = true;
			break;
		case State.New:
			skin.color = new Color(skin.color.r, skin.color.g, skin.color.b, 1f);
			newIcon.enabled = true;
			check.enabled = false;
			seen = false;
			break;
		case State.Selected:
			skin.color = new Color(skin.color.r, skin.color.g, skin.color.b, 1f);
			newIcon.enabled = false;
			check.enabled = true;
			seen = true;
			break;
		}
	}

	public void Select()
	{
		if (currentState > State.Locked)
		{
			SetState(State.Selected);
		}
	}

	public void Unlock()
	{
		if (currentState <= State.Locked)
		{
			SetState(State.New);
		}
	}

	public bool HasBeenSeen()
	{
		return seen;
	}

	public bool Seen()
	{
		if (seen || currentState == State.Locked)
		{
			return false;
		}
		seen = true;
		return true;
	}
}
