using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect), typeof(CanvasGroup))]
public sealed class SkinScroll : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IEventSystemHandler
{
	private ScrollRect scrollRect;

	private CanvasGroup canvasGroup;

	private int skinCount;

	private Skin[] skins;

	private int currentSkin;

	private float currentSnapSpeed;

	[SerializeField]
	private float snapSpeed = 4f;

	[SerializeField]
	private float snapForce = 4f;

	private int skip;

	private bool isActive;

	private bool forceFocus;

	private void Awake()
	{
		scrollRect = GetComponent<ScrollRect>();
		scrollRect.onValueChanged.AddListener(OnValueChanged);
		canvasGroup = GetComponent<CanvasGroup>();
		skinCount = scrollRect.content.childCount - 1;
		skins = new Skin[skinCount + 1];
		for (int j = 0; j < skins.Length; j++)
		{
			int i = j;
			Skin component = scrollRect.content.GetChild(j).GetComponent<Skin>();
			component.GetComponent<Button>().onClick.AddListener(delegate
			{
				ScrollToSkin(i);
			});
			skins[j] = component;
		}
	}

	private void Start()
	{
		scrollRect.horizontalNormalizedPosition = (float)currentSkin * 1f / (float)skinCount;
	}

	private void Update()
	{
		if (skip == 2)
		{
			return;
		}
		if (skip == 0)
		{
			if (currentSnapSpeed < snapSpeed)
			{
				currentSnapSpeed += snapForce * Time.deltaTime;
				if (currentSnapSpeed > snapSpeed)
				{
					currentSnapSpeed = snapSpeed;
				}
			}
			float horizontalNormalizedPosition = scrollRect.horizontalNormalizedPosition;
			float num = ((!forceFocus) ? ClosestSnap(horizontalNormalizedPosition) : ((float)currentSkin * 1f / (float)skinCount));
			Vector2 velocity = scrollRect.velocity;
			scrollRect.horizontalNormalizedPosition += (num - horizontalNormalizedPosition) * currentSnapSpeed * Time.deltaTime;
			scrollRect.velocity = velocity;
			if (Mathf.Abs(scrollRect.horizontalNormalizedPosition - num) < 0.001f && Mathf.Abs(velocity.x) < 0.001f)
			{
				skip = 2;
			}
		}
		if (!forceFocus)
		{
			int num2 = Mathf.RoundToInt(scrollRect.horizontalNormalizedPosition * (float)skinCount);
			if (num2 < 0)
			{
				num2 = 0;
			}
			else if (num2 > skinCount)
			{
				num2 = skinCount;
			}
			if (num2 != currentSkin)
			{
				currentSkin = num2;
				OnSkinViewed(skins[num2]);
			}
		}
	}

	public void OnBeginDrag(PointerEventData data)
	{
		currentSnapSpeed = 0f;
		skip = 1;
		forceFocus = false;
	}

	public void OnEndDrag(PointerEventData data)
	{
		skip = 0;
	}

	private void OnValueChanged(Vector2 position)
	{
		float num = scrollRect.horizontalNormalizedPosition * (float)skinCount;
		for (int i = 0; i < skins.Length; i++)
		{
			float num2 = Mathf.Abs((float)i - num);
			if (num2 >= 1f)
			{
				skins[i].transform.localScale = Vector3.one;
				continue;
			}
			num2 = 1.2f - num2 * 0.2f;
			skins[i].transform.localScale = new Vector3(num2, num2, num2);
		}
	}

	private float ClosestSnap(float horizontalValue)
	{
		return Mathf.Max(0f, Mathf.Min(1f, Mathf.Round(horizontalValue * (float)skinCount) / (float)skinCount));
	}

	public void BecomeActive()
	{
		isActive = true;
		OnSkinViewed(skins[currentSkin]);
		canvasGroup.alpha = 1f;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
	}

	public void BecomeInactive()
	{
		isActive = false;
		canvasGroup.alpha = 0f;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
	}

	public void OnSkinViewed(Skin skin)
	{
		if (isActive)
		{
			if (skin.Seen())
			{
				CheckIsNoSeen();
			}
			Singleton<SkinsPage>.i.ViewSkin(skin);
		}
	}

	private void CheckIsNoSeen()
	{
		Skin skin = null;
		for (int i = 0; i < skins.Length; i++)
		{
			if (!skins[i].HasBeenSeen())
			{
				skin = skins[i];
				break;
			}
		}
		if (skin != null)
		{
			Singleton<SkinTypeSelection>.i.RemovePin(skin.GetSkinType());
		}
	}

	private void ScrollToSkin(int i)
	{
		if (i != currentSkin)
		{
			currentSkin = i;
			OnSkinViewed(skins[i]);
			forceFocus = true;
			skip = 0;
			currentSnapSpeed = snapSpeed;
		}
	}
}
