using System;
using System.Collections;
using System.Collections.Generic;

namespace LevelMode
{

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

		/// <summary>
		/// struct 枚举器：foreach 走具体类型零堆分配；
		/// 同时实现 IEnumerator&lt;T&gt; / IEnumerator / IDisposable，
		/// 使显式接口 GetEnumerator 返回值可隐式转为非泛型 IEnumerator。
		/// MoveNext 先推进 _next，回调内 Remove 自身时仍安全。
		/// </summary>
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			private Link _next;

			private T _current;

			// 构造参数用 public PriorityChain（勿传 private Link），避免 CS0051；嵌套类型可读外层 first
			internal Enumerator(PriorityChain<T> chain)
			{
				_next = chain.first;
				_current = default(T);
			}

			public T Current => _current;

			object IEnumerator.Current => _current;

			public bool MoveNext()
			{
				if (_next == null)
				{
					return false;
				}
				Link cur = _next;
				_next = cur.GetNext();
				_current = cur.GetItem();
				return true;
			}

			public void Dispose() { }

			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
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

		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
