using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace JuiceInternal
{
	public class ClickableLink : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		[Serializable]
		public class ClickEvent : UnityEvent
		{
		}

		public ClickEvent onClick;

		private TMP_Text text;

		private void Awake()
		{
			text = GetComponent<TMP_Text>();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			int num = TMP_TextUtilities.FindIntersectingLink(text, Input.mousePosition, null);
			if (num != -1)
			{
				onClick.Invoke();
			}
		}
	}
}
