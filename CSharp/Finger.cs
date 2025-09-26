using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class Finger : Essential<Finger>
{
	private class FingerPage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		public void OnPointerDown(PointerEventData data)
		{
			pressing = true;
			Neuron.Tap();
		}

		public void OnPointerUp(PointerEventData data)
		{
			pressing = false;
		}
	}

	private static bool pressing;

	private bool cannotLaunch;

	private bool isInGame;

	protected override void Awake()
	{
		base.Awake();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		GameObject gameObject = GameObject.Find("Canvas");
		if (gameObject != null)
		{
			FingerPage component = new GameObject("FingerPage", typeof(FingerPage), typeof(RectTransform), typeof(Image)).GetComponent<FingerPage>();
			component.transform.SetParent(gameObject.transform);
			component.transform.SetAsFirstSibling();
			RectTransform component2 = component.GetComponent<RectTransform>();
			component2.anchorMin = Vector2.zero;
			component2.anchorMax = Vector2.one;
			component2.sizeDelta = Vector2.one;
			component2.anchoredPosition = Vector2.zero;
			component2.localPosition = Vector3.zero;
			component2.localScale = Vector3.one;
			component2.localRotation = Quaternion.identity;
			component.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
		}
	}

	public static bool IsPressing()
	{
		return pressing;
	}

	protected override void OnRefresh()
	{
		cannotLaunch = false;
		isInGame = false;
	}

	protected override void OnContinue()
	{
		cannotLaunch = false;
	}

	protected override void OnTap()
	{
		if (!cannotLaunch)
		{
			cannotLaunch = true;
			Neuron.StartRun((!isInGame) ? Run.GetDefault() : Neuron.GetCurrentRun());
			isInGame = true;
		}
	}
}
