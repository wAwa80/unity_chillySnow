using UnityEngine.UI;

namespace LevelMode
{

	public class MotivationalButton : SingletonButton<MotivationalButton>
	{
		private Image onIcon;

		private Image offIcon;

		public static bool motivationalOn { get; private set; }

		protected override void Awake()
		{
			base.Awake();
			onIcon = childTransform.GetChild(0).GetComponent<Image>();
			offIcon = childTransform.GetChild(1).GetComponent<Image>();
			motivationalOn = true;
			// SingletonButton.Awake 会 ApplyHiddenVisualState（Button/Image 禁用、子节点 scale=0）；
			// Settings 面板里的按钮需立刻 Show，实际显隐由父 CanvasGroup 控制
			Show();
		}

		private void Start()
		{
			if (!Data.LoadBool("motivationalOn", defaultValue: true))
			{
				OnClick();
			}
		}

		protected override void OnClick()
		{
			motivationalOn = !motivationalOn;
			Data.SaveBool("motivationalOn", motivationalOn);
			if (motivationalOn)
			{
				onIcon.enabled = true;
				offIcon.enabled = false;
			}
			else
			{
				onIcon.enabled = false;
				offIcon.enabled = true;
			}
		}
	}
}
