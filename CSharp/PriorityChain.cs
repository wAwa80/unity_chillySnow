using System.Collections;
using System.Collections.Generic;

public sealed class PriorityChain<T> : IEnumerable<T>, IEnumerable where T : IPriority
{
	private class Link
	{
		private readonly T item;

		private Link previous;

		private Link next;

		public Link(T item)
		{
			this.item = item;
		}

		public T GetItem()
		{
			return item;
		}

		public void AddAfter(Link other)
		{
			if (next != null)
			{
				next.previous = other;
				other.next = next;
			}
			next = other;
			other.previous = this;
		}

		public void Remove()
		{
			if (previous != null)
			{
				previous.next = next;
			}
			if (next != null)
			{
				next.previous = previous;
			}
		}

		public Link GetNext()
		{
			return next;
		}
	}

	private Link first;

	public void Add(T item)
	{
		if (first == null)
		{
			first = new Link(item);
			return;
		}
		int priority = item.GetPriority();
		if (first.GetItem().GetPriority() > priority)
		{
			Link link = new Link(item);
			link.AddAfter(first);
			first = link;
			return;
		}
		Link link2 = first;
		Link next = link2.GetNext();
		while (next != null && next.GetItem().GetPriority() <= priority)
		{
			link2 = next;
			next = link2.GetNext();
		}
		link2.AddAfter(new Link(item));
	}

	public void Remove(T item)
	{
		if (first == null)
		{
			return;
		}
		while (first.GetItem().Equals(item))
		{
			Link link = first;
			first = link.GetNext();
			link.Remove();
			if (first == null)
			{
				return;
			}
		}
		Link link2 = first;
		for (Link next = link2.GetNext(); next != null; next = link2.GetNext())
		{
			if (next.GetItem().Equals(item))
			{
				next.Remove();
			}
			else
			{
				link2 = next;
			}
		}
	}

	public IEnumerator<T> GetEnumerator()
	{
		for (Link current = first; current != null; current = current.GetNext())
		{
			yield return current.GetItem();
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
