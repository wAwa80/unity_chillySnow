using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JuiceInternal
{
	public class AnimatedToggle : MonoBehaviour
	{
		[Serializable]
		public class ToggleEvent : UnityEvent<bool>
		{
		}

		private bool m_isOn;

		private Image image;

		private Button button;

		[SerializeField]
		private Color onColor = new Color(0f, 0.5f, 1f);

		[SerializeField]
		private Color offColor = Color.grey;

		[SerializeField]
		private RectTransform handle;

		private float onPosX;

		private float offPosX;

		[SerializeField]
		private float offset = 4f;

		[SerializeField]
		private float switchSpeed = 10f;

		private bool initialized;

		public ToggleEvent onToggle = new ToggleEvent();

		private float rate;

		public bool isOn
		{
			get
			{
				return m_isOn;
			}
			set
			{
				Switch(value);
			}
		}

		private void Awake()
		{
			image = GetComponent<Image>();
			button = GetComponent<Button>();
			button.onClick.AddListener(OnClick);
			float x = handle.sizeDelta.x;
			float x2 = GetComponent<RectTransform>().sizeDelta.x;
			onPosX = x2 * 0.5f - x * 0.5f - offset;
			offPosX = 0f - onPosX;
			initialized = true;
			if (m_isOn)
			{
				image.color = onColor;
				handle.localPosition = new Vector3(onPosX, 0f, 0f);
				rate = 1f;
			}
			else
			{
				image.color = offColor;
				handle.localPosition = new Vector3(offPosX, 0f, 0f);
				rate = 0f;
			}
		}

		private void OnClick()
		{
			Switch(!m_isOn);
		}

		private void Switch(bool isOn)
		{
			if (m_isOn != isOn)
			{
				m_isOn = isOn;
				onToggle.Invoke(m_isOn);
				if (initialized)
				{
					base.enabled = true;
				}
			}
		}

		private void Update()
		{
			if (m_isOn)
			{
				rate += (1f - rate) * switchSpeed * Time.deltaTime;
				if (rate > 0.999f)
				{
					rate = 1f;
					base.enabled = false;
					image.color = onColor;
					handle.localPosition = new Vector3(onPosX, 0f, 0f);
					return;
				}
			}
			else
			{
				rate -= rate * switchSpeed * Time.deltaTime;
				if (rate < 0.001f)
				{
					rate = 0f;
					base.enabled = false;
					image.color = offColor;
					handle.localPosition = new Vector3(offPosX, 0f, 0f);
					return;
				}
			}
			image.color = Color.Lerp(offColor, onColor, rate);
			handle.localPosition = new Vector3(offPosX + (onPosX - offPosX) * rate, 0f, 0f);
		}
	}
}
