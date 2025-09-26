using System;
using System.Collections;
using System.Collections.Generic;

public sealed class Pool<T> : IEnumerable<T>, IEnumerable
{
	private class Link
	{
		public readonly T item;

		public Link next;

		public Link(T item)
		{
			this.item = item;
		}
	}

	private Link front;

	private Link back;

	public void Add(T item)
	{
		if (front == null)
		{
			front = new Link(item);
			back = front;
		}
		else
		{
			Link next = new Link(item);
			back.next = next;
			back = next;
		}
	}

	public T Pop()
	{
		if (front == null)
		{
			throw new InvalidOperationException("This pool is empty!");
		}
		T item = front.item;
		front = front.next;
		return item;
	}

	public T Peek()
	{
		if (front == null)
		{
			throw new InvalidOperationException("This pool is empty!");
		}
		return front.item;
	}

	public bool IsEmpty()
	{
		return front == null;
	}

	public void Clear()
	{
		front = null;
	}

	public IEnumerator<T> GetEnumerator()
	{
		if (front != null)
		{
			for (Link current = front; current != null; current = current.next)
			{
				yield return current.item;
			}
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
