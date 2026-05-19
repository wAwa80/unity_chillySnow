using UnityEngine;
using UnityEngine.UI;


namespace EndlessMode
{
	[RequireComponent(typeof(Button), typeof(Image))]
	public class AnimatedButton<T> : Singleton<T> where T : AnimatedButton<T>
	{
		protected Image image;

		protected Button button;

		protected RectTransform childTransform;

		private bool shouldShow;

		private float timer;

		private float appearSpeed;

		private float dissappearSpeed;

		protected override void Awake()
		{
			base.Awake();
			image = GetComponent<Image>();
			button = GetComponent<Button>();
			childTransform = base.transform.GetChild(0).GetComponent<RectTransform>();
		}

		public virtual void Show(float appearSpeed = 2f)
		{
			this.appearSpeed = appearSpeed;
			image.enabled = true;
			button.enabled = true;
			shouldShow = true;
			base.enabled = true;
		}

		public virtual void Hide(float dissappearSpeed = 2f)
		{
			this.dissappearSpeed = dissappearSpeed;
			button.enabled = false;
			shouldShow = false;
			base.enabled = true;
		}

		private void Update()
		{
			if (shouldShow)
			{
				timer += appearSpeed * Time.deltaTime;
				if (timer > 1f)
				{
					timer = 1f;
					base.enabled = false;
				}
				float num = 4f * timer - 3f;
				num = (9f - num * num) * 0.125f;
				childTransform.localScale = new Vector3(num, num, num);
				return;
			}
			timer -= dissappearSpeed * Time.deltaTime;
			if (timer < 0f)
			{
				timer = 0f;
				base.enabled = false;
				image.enabled = false;
			}
			float num2 = 4f * timer - 3f;
			num2 = (9f - num2 * num2) * 0.125f;
			childTransform.localScale = new Vector3(num2, num2, num2);
		}
	}
}
