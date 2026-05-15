using UnityEngine;

public abstract class Singleton<T> : Neuron where T : Singleton<T>
{
	public static T i { get; private set; }

	public Singleton()
	{
		if ((Object)i != (Object)null && i != this)
		{
			Debug.LogError("Singleton has to be unique !");
		}
		i = (T)this;
	}
}
