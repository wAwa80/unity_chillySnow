using UnityEngine;
using UnityEngine.UI;


namespace EndlessMode
{
	public class DebugPage : Page<DebugPage>
	{
		public static bool dontShowPause;

		public static bool dontShake;

		public static bool dontFever;

		public static bool dontThrowPowder;

		public static bool dontBonusText;

		public static bool dontAnimatePine;

		[SerializeField]
		private Negative negative;

		protected override void Awake()
		{
			base.Awake();
		}

		public override void Show()
		{
			if (!App.IsRelease())
			{
				base.Show();
			}
		}

		public void PauseVisibility(Toggle toggle)
		{
			dontShowPause = !toggle.isOn;
		}

		public void ScoreVisibility(Toggle toggle)
		{
			Singleton<MenuScores>.i.GetComponent<CanvasGroup>().alpha = ((!toggle.isOn) ? 0f : 1f);
		}

		public void AllowShakes(Toggle toggle)
		{
			dontShake = !toggle.isOn;
		}

		public void AllowFever(Toggle toggle)
		{
			dontFever = !toggle.isOn;
		}

		public void ShowPowderSpread(Toggle toggle)
		{
			dontThrowPowder = !toggle.isOn;
		}

		public void ShowBonusText(Toggle toggle)
		{
			dontBonusText = !toggle.isOn;
		}

		public void ShowPineAnimation(Toggle toggle)
		{
			dontAnimatePine = !toggle.isOn;
		}

		public void ChangeBackgroundColor(InputField field)
		{
			Singleton<GameCamera>.i.GetCamera().backgroundColor = Utility.HexToColor("#" + field.text);
		}

		public void ChangeBallSize(InputField field)
		{
			if (float.TryParse(field.text, out var result))
			{
				Singleton<Player>.i.transform.localScale = new Vector3(result, result, result);
			}
		}

		public void NegativeMode(Toggle toggle)
		{
			negative.enabled = toggle.isOn;
		}

		public void GetPremium()
		{
			Neuron.Purchased("chilly_noads");
		}

		public void AllSkins()
		{
			foreach (Skin item in Multiton<Skin>.Enumerate())
			{
				item.Unlock();
			}
		}
	}
}
