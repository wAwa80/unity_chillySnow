using UnityEngine;
using System;
using UnityEngine.Events;

namespace JuiceInternal
{
	public class ClickableLink : MonoBehaviour
	{
		[Serializable]
		public class ClickEvent : UnityEvent
		{
		}

		public ClickEvent onClick;
	}
}
