using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LevelMode
{

	public abstract class Recyclable<T> : Neuron, IRecyclable where T : Recyclable<T>
	{
		/// <summary>
		/// List 尾部入/出；OnDestroy 可 Remove，避免 Stack 无法按引用剔除。
		/// </summary>
		private static readonly List<T> pool = new List<T>();

		private static readonly Dictionary<Type, string> customResourceNames = new Dictionary<Type, string>();

		private static GameObject cachedPrefab;

		private static Transform poolRoot;

		private bool alive;

		/// <summary>
		/// 当前池内闲置数量（不含活跃实例）。
		/// </summary>
		public static int PoolCount => pool.Count;

		/// <summary>
		/// 是否处于活跃（已 Get 且未 Kill）。
		/// </summary>
		protected bool IsAlive => alive;

		/// <summary>
		/// 当 Resources 中的 prefab 名与类型名不一致时，在子类 static 构造函数中注册。
		/// </summary>
		protected static void RegisterResourceName(string resourceName)
		{
			customResourceNames[typeof(T)] = resourceName;
		}

		private static string GetResourceName()
		{
			// typeof(T) 不会触发 T 的静态构造；WarmUp 早于 Pine.ResetState 时会回退为 "Pine" 并 Load 到 EndlessRes。
			RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);
			string resourceName;
			if (customResourceNames.TryGetValue(typeof(T), out resourceName))
			{
				return resourceName;
			}
			return typeof(T).Name;
		}

		private static GameObject GetCachedPrefab()
		{
			if (cachedPrefab == null)
			{
				cachedPrefab = Resources.Load<GameObject>(GetResourceName());
			}
			return cachedPrefab;
		}

		private static void EnsurePoolRoot()
		{
			if (poolRoot != null)
			{
				return;
			}
			GameObject root = GameObject.Find("_RecyclablePoolRoot");
			if (root == null)
			{
				root = new GameObject("_RecyclablePoolRoot");
			}
			poolRoot = root.transform;
		}

		/// <summary>
		/// 剔除 Unity 已销毁的池项（域不重载时常见）。
		/// </summary>
		private static void PurgeDestroyed()
		{
			for (int i = pool.Count - 1; i >= 0; i--)
			{
				if (pool[i] == null)
				{
					pool.RemoveAt(i);
				}
			}
		}

		public static T Get()
		{
			PurgeDestroyed();
			T val = null;
			while (pool.Count > 0)
			{
				int last = pool.Count - 1;
				val = pool[last];
				pool.RemoveAt(last);
				if (val != null)
				{
					break;
				}
				val = null;
			}
			if (val == null)
			{
				GameObject prefab = GetCachedPrefab();
				if (prefab == null)
				{
					string resourceName = GetResourceName();
					Debug.LogError($"Recyclable<{typeof(T).Name}>: Resources.Load(\"{resourceName}\") 返回 null，请确认 prefab 位于 Resources 且名称一致。");
					return null;
				}
				GameObject instance = UnityEngine.Object.Instantiate(prefab);
				val = ResolveComponent(instance);
				if (val == null)
				{
					UnityEngine.Object.Destroy(instance);
					return null;
				}
			}
			val.alive = true;
			val.OnEnabled();
			return val;
		}

		public void Kill()
		{
			if (alive)
			{
				alive = false;
				OnDisabled();
				pool.Add((T)this);
			}
		}

		/// <summary>
		/// 分帧预热：只 Instantiate + 关 Renderer，不调 OnEnabled，避免滚石进集合等副作用。
		/// </summary>
		public static IEnumerator WarmUpInactiveCoroutine(int targetCount, int perFrame)
		{
			EnsurePoolRoot();
			PurgeDestroyed();
			int need = Mathf.Max(0, targetCount - pool.Count);
			int created = 0;
			int consecutiveFailures = 0;
			while (created < need)
			{
				int batch = Mathf.Min(perFrame, need - created);
				for (int i = 0; i < batch; i++)
				{
					T instance = CreateInactiveInstance();
					if (instance != null)
					{
						pool.Add(instance);
						created++;
						consecutiveFailures = 0;
					}
					else
					{
						// 仅成功创建才计数；连续失败则中止，避免空转加 created
						Debug.LogError($"Recyclable<{typeof(T).Name}> WarmUp: CreateInactiveInstance 失败，跳过计数。");
						consecutiveFailures++;
						if (consecutiveFailures >= 3)
						{
							yield break;
						}
					}
				}
				yield return null;
			}
		}

		private static T CreateInactiveInstance()
		{
			GameObject prefab = GetCachedPrefab();
			if (prefab == null)
			{
				Debug.LogError($"Recyclable<{typeof(T).Name}> WarmUp: prefab 加载失败。Resources 中需有 \"{GetResourceName()}\" prefab。");
				return null;
			}
			GameObject instance = UnityEngine.Object.Instantiate(prefab);
			instance.transform.SetParent(poolRoot, false);
			instance.transform.localPosition = new Vector3(0f, -9999f, 0f);
			T component = ResolveComponent(instance);
			if (component == null)
			{
				UnityEngine.Object.Destroy(instance);
				return null;
			}
			component.alive = false;
			component.PrepareWarmUpInactive();
			return component;
		}

		/// <summary>
		/// 取组件：先 GetComponent，失败再按类型扫描（避免 Missing/导入器异常时误报「没挂脚本」）。
		/// Prefab 在 Inspector 里能看到脚本，但 Resources.Load 若 meta 导入器不匹配，实例上可能拿不到组件。
		/// </summary>
		private static T ResolveComponent(GameObject instance)
		{
			if (instance == null)
			{
				return null;
			}
			T val = instance.GetComponent<T>();
			if (val != null)
			{
				return val;
			}
			// 再扫一遍：部分情况下泛型 GetComponent 失败，但组件实际存在
			T[] all = instance.GetComponentsInChildren<T>(true);
			if (all != null && all.Length > 0 && all[0] != null)
			{
				return all[0];
			}
			// 禁止 AddComponent 兜底：若误 Load 到 EndlessRes/Pine，硬挂 LevelMode.Pine
			// 会踩 NightShadow 无 SpriteRenderer，刷树异常中断整局 Refresh。
			Debug.LogError($"Recyclable<{typeof(T).Name}>: prefab \"{GetResourceName()}\" 实例上无 {typeof(T).FullName}，拒绝 AddComponent。请确认 Resources 下为 LevelPine 且挂 LevelMode.Pine。");
			Component[] comps = instance.GetComponents<Component>();
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for (int i = 0; i < comps.Length; i++)
			{
				if (comps[i] == null)
				{
					sb.Append("Missing");
				}
				else
				{
					sb.Append(comps[i].GetType().FullName);
				}
				if (i < comps.Length - 1)
				{
					sb.Append(", ");
				}
			}
			Debug.LogError($"Recyclable<{typeof(T).Name}>: prefab \"{GetResourceName()}\" 无法获得 {typeof(T).FullName}。实例组件: [{sb}]");
			return null;
		}

		/// <summary>
		/// WarmUp 专用：关脚本与 Renderer，不触发 OnEnabled 副作用。
		/// </summary>
		protected virtual void PrepareWarmUpInactive()
		{
			enabled = false;
			Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
			for (int i = 0; i < renderers.Length; i++)
			{
				renderers[i].enabled = false;
			}
		}

		protected override void OnDestroy()
		{
			if (alive)
			{
				alive = false;
				OnDisabled();
			}
			pool.Remove((T)this);
			base.OnDestroy();
		}

		protected virtual void OnEnabled()
		{
		}

		protected virtual void OnDisabled()
		{
		}
	}
}
