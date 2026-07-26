using UnityEngine;


namespace EndlessMode
{
	public abstract class Singleton<T> : Neuron where T : Singleton<T>
	{
		public static T i { get; private set; }

		public Singleton()
		{
			// 已有存活实例时禁止覆盖：切场景会出现场景内重复 Singleton，
			// 若覆盖 i，DontDestroy 的旧实例会失联，新实例还会再次 Init/LoadScene。
			if ((Object)i != (Object)null && i != this)
			{
				Debug.LogError("Singleton has to be unique !");
				return;
			}
			i = (T)this;
		}
	}
}
