using System.Collections.Generic;
using UnityEngine;

public abstract class Recyclable<T> : Neuron where T : Recyclable<T>
{
	private static readonly HashSet<T> pool;

	private bool alive;

	static Recyclable()
	{
		pool = new HashSet<T>();
	}

	public static T Get()
	{
		T val;
		using (HashSet<T>.Enumerator enumerator = pool.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				T current = enumerator.Current;
				val = current;
				pool.Remove(val);
				val.alive = true;
				val.OnEnabled();
				return val;
			}
		}
		val = ((GameObject)Object.Instantiate(Resources.Load(typeof(T).Name))).GetComponent<T>();
		val.alive = true;
		val.OnEnabled();
		return val;
	}

	public void Kill()
	{
		if (alive)
		{
			alive = false;
			pool.Add((T)this);
			OnDisabled();
		}
	}

	protected virtual void OnEnabled()
	{
	}

	protected virtual void OnDisabled()
	{
	}
}
