using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EndlessMode
{
	public abstract class Recyclable<T> : Neuron where T : Recyclable<T>
	{
		/// <summary>
		/// List 尾部入/出；OnDestroy 可按引用 Remove。
		/// </summary>
		private static readonly List<T> pool = new List<T>();

		private static GameObject cachedPrefab;

		private static Transform poolRoot;

		private bool alive;

		public static int PoolCount => pool.Count;

		protected bool IsAlive => alive;

		private static GameObject GetCachedPrefab()
		{
			if (cachedPrefab == null)
			{
				cachedPrefab = Resources.Load<GameObject>(typeof(T).Name);
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
					Debug.LogError($"Recyclable<{typeof(T).Name}>: Resources.Load(\"{typeof(T).Name}\") 返回 null。");
					return null;
				}
				GameObject instance = Object.Instantiate(prefab);
				val = instance.GetComponent<T>();
				if (val == null)
				{
					Debug.LogError($"Recyclable<{typeof(T).Name}>: prefab 缺少组件。");
					Object.Destroy(instance);
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
		/// 分帧预热：不调 OnEnabled；禁止对 PineDestroyedEffect 调用。
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
				Debug.LogError($"Recyclable<{typeof(T).Name}> WarmUp: prefab 加载失败。");
				return null;
			}
			GameObject instance = Object.Instantiate(prefab);
			instance.transform.SetParent(poolRoot, false);
			instance.transform.localPosition = new Vector3(0f, -9999f, 0f);
			T component = instance.GetComponent<T>();
			if (component == null)
			{
				Object.Destroy(instance);
				return null;
			}
			component.alive = false;
			component.PrepareWarmUpInactive();
			return component;
		}

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
