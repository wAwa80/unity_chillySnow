using System.Collections;
using System.Collections.Generic;

public abstract class Multiton<T> : Neuron where T : Multiton<T>
{
	private static Dictionary<string, T> instances = new Dictionary<string, T>();

	public abstract string Name { get; }

	public static T Get(string name)
	{
		return instances[name];
	}

	public static int GetCount()
	{
		return instances.Count;
	}

	public static IEnumerable Enumerate()
	{
		foreach (T value in instances.Values)
		{
			yield return value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		if (instances.ContainsKey(Name))
		{
			instances[Name] = (T)this;
		}
		else
		{
			instances.Add(Name, (T)this);
		}
	}
}
