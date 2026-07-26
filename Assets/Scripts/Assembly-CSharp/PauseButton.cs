using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelMode
{
	/// <summary>
	/// 局内暂停入口（HUD 右上角小按钮）。无 PausePage 时二次点击改为 Unpause（降级 toggle）。
	/// 
	/// 自动挂载：在含 Button 但无 CanvasGroup 的 "Pause" 节点上自动附加。
	/// // TODO: [User Action] 确保 HUD 中有名为 "Pause" 的节点（有 Button，无 CanvasGroup），脚本会自动附加。
	/// </summary>
	public sealed class PauseButton : SingletonButton<PauseButton>
	{
		protected override void OnClick()
		{
			Debug.Log($"[PauseButton] OnClick IsPaused={Neuron.IsPaused()} CanPause={Neuron.CanPause()} PausePage={(PausePage.i != null)}");
			// 降级：无 PausePage 且已暂停 → 点按钮恢复
			if (Neuron.IsPaused() && PausePage.i == null)
			{
				Neuron.Unpause();
				return;
			}
			if (Neuron.IsPaused()) return;
			Neuron.Pause();
		}

		protected override void OnStartRun(Run run)
		{
			CancelInvoke(nameof(ShowDelay));
			Invoke(nameof(ShowDelay), 0.5f);
		}

		protected override void OnEndRun()
		{
			CancelInvoke(nameof(ShowDelay));
			Hide();
		}

		protected override void OnRefresh()
		{
			CancelInvoke(nameof(ShowDelay));
			Hide();
		}

		protected override void OnContinue()
		{
			// 续命后需再 Tap 才 StartRun；等 OnStartRun 延迟显示
			CancelInvoke(nameof(ShowDelay));
			Hide();
		}

		protected override void OnPause()
		{
			// 有 PausePage 时藏钮，避免与全屏层叠点
			if (PausePage.i != null) Hide();
		}

		protected override void OnUnpause()
		{
			if (Neuron.CanPause()) Show();
		}

		private void ShowDelay()
		{
			if (Neuron.CanPause() && !Neuron.IsPaused()) Show();
		}

		// ─────────────────────────────────────────────────────────────────
		// 自动挂载：区分 HUD 暂停按钮（有 Button，无 CanvasGroup）vs 全屏暂停层。
		// ─────────────────────────────────────────────────────────────────
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void RegisterAutoAttach()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (!scene.name.Contains("Level")) return;
			if (i != null) return;

			// 遍历所有名为 "Pause" 的节点，找到 HUD 按钮（有 Button，无 CanvasGroup）
			GameObject target = null;
			foreach (GameObject go in FindAllActiveByName("Pause"))
			{
				if (go.GetComponent<UnityEngine.UI.Button>() != null &&
					go.GetComponent<CanvasGroup>() == null)
				{
					target = go;
					break;
				}
			}
			if (target == null) return;

			RemoveEndlessComponents(target);
			target.AddComponent<PauseButton>();
			Debug.Log("[LevelMode] PauseButton 自动挂载到 Pause 节点（HUD 按钮）");
		}

		private static System.Collections.Generic.List<GameObject> FindAllActiveByName(string name)
		{
			var result = new System.Collections.Generic.List<GameObject>();
			foreach (GameObject go in Object.FindObjectsOfType<GameObject>())
			{
				if (go.name == name) result.Add(go);
			}
			return result;
		}

		private static void RemoveEndlessComponents(GameObject go)
		{
			foreach (MonoBehaviour comp in go.GetComponents<MonoBehaviour>())
			{
				if (comp != null && comp.GetType().FullName != null &&
					comp.GetType().FullName.StartsWith("EndlessMode."))
				{
					Object.Destroy(comp);
				}
			}
		}
	}
}
