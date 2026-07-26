using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LevelMode
{
	/// <summary>
	/// 全屏暂停层：淡入后 timeScale=0；点击任意处 Unpause。
	/// 需要根节点有 CanvasGroup + Image（raycastTarget=true，用于捕获点击恢复）。
	/// 
	/// 自动挂载：在含 CanvasGroup 的 "Pause" 节点上自动附加，并补全 Image（若缺失）。
	/// // TODO: [User Action] 确认场景中全屏 Pause 节点（有 CanvasGroup）存在，脚本会自动附加。
	/// </summary>
	public sealed class PausePage : UiPage<PausePage>, IPointerDownHandler, IEventSystemHandler
	{
		public void OnPointerDown(PointerEventData data)
		{
			Neuron.Unpause();
		}

		protected override void OnPause()
		{
			// 保证盖住运行时插入的 FingerPage 与其它 HUD
			transform.SetAsLastSibling();
			Show();
		}

		protected override void OnUnpause()
		{
			Time.timeScale = 1f;
			Hide();
		}

		protected override void Update()
		{
			base.Update();
			// 淡入完成后冻结时间（对齐无尽 PausePage）
			if (IsVisible() && self != null && self.alpha >= 1f && Time.timeScale > 0f && Neuron.IsPaused())
			{
				Time.timeScale = 0f;
			}
		}

		// ─────────────────────────────────────────────────────────────────
		// 自动挂载：区分全屏暂停层（有 CanvasGroup）vs HUD 按钮（无 CanvasGroup）。
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

			// 遍历所有名为 "Pause" 的节点，找到全屏暂停层（有 CanvasGroup）
			GameObject target = null;
			foreach (GameObject go in FindAllActiveByName("Pause"))
			{
				if (go.GetComponent<CanvasGroup>() != null)
				{
					target = go;
					break;
				}
			}
			if (target == null) return;

			// 确保有 Image（raycastTarget=true），用于捕获点击恢复暂停
			Image img = target.GetComponent<Image>();
			if (img == null)
			{
				img = target.AddComponent<Image>();
				img.color = new Color(0f, 0f, 0f, 0.6f);
			}
			img.raycastTarget = true;

			RemoveEndlessComponents(target);
			target.AddComponent<PausePage>();
			Debug.Log("[LevelMode] PausePage 自动挂载到 Pause 节点（全屏暂停层）");
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
