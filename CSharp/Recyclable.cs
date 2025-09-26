using UnityEngine;

public abstract class Recyclable<T> : Neuron, IRecyclable where T : Recyclable<T>
{
	private static readonly Pool<T> pool;

	private bool alive;

	static Recyclable()
	{
		pool = new Pool<T>();
	}

	public static T Get()
	{
		T val = ((!pool.IsEmpty()) ? pool.Pop() : ((GameObject)Object.Instantiate(Resources.Load(typeof(T).Name))).GetComponent<T>());
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
