using System.Collections;
using System.Collections.Generic;


namespace EndlessMode
{
	public abstract class Multiton<T> : Neuron where T : Multiton<T>
	{
		private static SortedDictionary<string, T> instances;

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
			if (instances == null)
			{
				instances = new SortedDictionary<string, T>();
			}
			else if (instances.ContainsKey(Name))
			{
				return;
			}
			instances.Add(Name, (T)this);
		}
	}
}
