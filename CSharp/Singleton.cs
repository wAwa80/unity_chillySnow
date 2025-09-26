public abstract class Singleton<T> : Neuron where T : Singleton<T>
{
	public static T i { get; private set; }

	public Singleton()
	{
		i = (T)this;
	}
}
