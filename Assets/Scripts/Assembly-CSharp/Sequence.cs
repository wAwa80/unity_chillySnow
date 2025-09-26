using System.Collections;
using System.Collections.Generic;

public sealed class Sequence<T> : IEnumerable<T>, IEnumerable
{
	private class Tick
	{
		private T item;

		private Tick next;

		public Tick()
		{
			item = default(T);
			next = null;
		}

		public Tick GetNext()
		{
			return next;
		}

		public T GetItem()
		{
			return item;
		}

		public Tick Set(T item)
		{
			this.item = item;
			if (next == null)
			{
				next = new Tick();
			}
			return next;
		}
	}

	private Tick start;

	private Tick end;

	private int length;

	public Sequence()
	{
		start = null;
		end = new Tick();
		length = 0;
	}

	public void Add(T item)
	{
		if (start == null)
		{
			start = end;
		}
		length++;
		end = end.Set(item);
	}

	public void Clear()
	{
		if (start != null)
		{
			end = start;
			start = null;
			length = 0;
		}
	}

	public int GetLength()
	{
		return length;
	}

	public IEnumerator<T> GetEnumerator()
	{
		if (start != null)
		{
			for (Tick current = start; current != end; current = current.GetNext())
			{
				yield return current.GetItem();
			}
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
