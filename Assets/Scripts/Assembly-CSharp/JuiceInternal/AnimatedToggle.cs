using UnityEngine;
using System;
using UnityEngine.Events;

namespace JuiceInternal
{
	public class AnimatedToggle : MonoBehaviour
	{
		[Serializable]
		public class ToggleEvent : UnityEvent<bool>
		{
		}

		[SerializeField]
		private Color onColor;
		[SerializeField]
		private Color offColor;
		[SerializeField]
		private RectTransform handle;
		[SerializeField]
		private float offset;
		[SerializeField]
		private float switchSpeed;
		public ToggleEvent onToggle;
	}
}
