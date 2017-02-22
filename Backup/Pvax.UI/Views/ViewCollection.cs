#region License
/*  Pvax.UI.Views
    Copyright (c) 2005, 2006, Alexey A. Popov
    All rights reserved.

    Redistribution and use in source and binary forms, with or without modification, are
    permitted provided that the following conditions are met:

    - Redistributions of source code must retain the above copyright notice, this list
      of conditions and the following disclaimer.

    - Redistributions in binary form must reproduce the above copyright notice, this list
      of conditions and the following disclaimer in the documentation and/or other materials
      provided with the distribution.

    - Neither the name of the Alexey A. Popov nor the names of its contributors may be used to
      endorse or promote products derived from this software without specific prior written
      permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS *AS IS* AND ANY EXPRESS
    OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY
    AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
    CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
    DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
    DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER
    IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT
    OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
#endregion

using System;
using System.Collections;

namespace Pvax.UI.Views
{
	/// <summary>
	/// Represents a typed collection of <see cref="IView"/> objects.
	/// </summary>
	/// <remarks>
	/// This is an abstract class. The <see cref="ViewContainer"/> and
	/// <see cref="CompositeView"/> classes implement their own version of this
	/// collection class.
	/// </remarks>
	[Serializable]
	public class ViewCollection: IList, ICollection, IEnumerable
	{
		/// <summary>
		/// Represents a enumerator for the <see cref="ViewCollection"/> class.
		/// </summary>
		public class Enumerator: IEnumerator
		{
			private int current;

			private int version;

			private ViewCollection collection;

			internal Enumerator(ViewCollection collection)
			{
				this.version = collection.version;
				this.collection = collection;
				Reset();
			}

			#region IEnumerator interface implementation
			/// <summary>
			/// Gets the current view in the collection.
			/// </summary>
			public IView Current
			{
				get
				{
					return collection[current];
				}
			}

			object IEnumerator.Current
			{
				get
				{
					if(version != collection.version)
						throw new InvalidOperationException();
					return this.Current;
				}
			}

			/// <summary>
			/// Sets the enumerator to its initial position, which is before
			/// the first element in the collection.
			/// </summary>
			/// <exception cref="InvalidOperationException">
			/// The collection was modified after the enumerator was created.
			/// </exception>
			public void Reset()
			{
				if(version != collection.version)
					throw new InvalidOperationException();
				current = -1;
			}

			/// <summary>
			/// Advances the enumerator to the next element of the collection.
			/// </summary>
			/// <returns>
			/// <c>true</c> if the enumerator was successfully advanced to
			/// the next element; <c>false</c> if the enumerator has passed
			/// the end of the collection.
			/// </returns>
			/// <exception cref="InvalidOperationException">
			/// The collection was modified after the enumerator was created.
			/// </exception>
			public bool MoveNext()
			{
				if(version != collection.version)
					throw new InvalidOperationException();
				if(current < collection.Count - 1)
				{
					current++;
					return true;
				}
				return false;
			}
			#endregion
		}

		private ArrayList list;

		private int version;

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewCollection"/>
		/// class.
		/// </summary>
		public ViewCollection()
		{
			list = new ArrayList();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewCollection"/>
		/// class with <paramref name="views"/>
		/// </summary>
		/// <param name="views">
		/// An array of <see cref="IView"/> objects to take the contents from.
		/// </param>
		public ViewCollection(IView[] views):
			this()
		{
			AddRange(views);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewCollection"/>
		/// class with <paramref name="collection"/>.
		/// </summary>
		/// <param name="collection">
		/// Another collection to take the contents from.
		/// </param>
		public ViewCollection(ViewCollection collection):
			this()
		{
			AddRange(collection);
		}

		#region IList interface implementation
		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="index">
		/// The zero-based index of the <see cref="IView"/> to get or set.
		/// </param>
		/// <value>
		/// The zero-based index of the view to get or set.
		/// </value>
		public virtual IView this[int index]
		{
			get
			{
				return ((IView)(list[index]));
			}

			set
			{
				list[index] = value;
				version++;
			}
		}

		object IList.this[int index]
		{
			get
			{
				return this[index];
			}

			set
			{
				this[index] = (IView)value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="ViewCollection"/>
		/// is read-only.
		/// </summary>
		/// <value>
		/// <c>true</c> if the <see cref="ViewCollection"/> is read-only;
		/// otherwise, <c>false</c>.
		/// </value>
		public bool IsReadOnly
		{
			get
			{
				return list.IsReadOnly;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="ViewCollection"/>
		/// has a fixed size.
		/// </summary>
		/// <value>
		/// <c>true</c> if the <see cref="ViewCollection"/> has a fixed size;
		/// otherwise, <c>false</c>.
		/// </value>
		public bool IsFixedSize
		{
			get
			{
				return list.IsFixedSize;
			}
		}

		/// <summary>
		/// Removes the <see cref="IView"/> at the specified index of
		/// the <see cref="ViewCollection"/>.
		/// </summary>
		/// <param name="index">
		/// The zero-based index of the element to remove.
		/// </param>
		public virtual void RemoveAt(int index)
		{
			list.RemoveAt(index);
			version++;
		}

		/// <summary>
		/// Removes the first occurrence of a specific <see cref="IView"/> from
		/// the <see cref="ViewCollection"/>.
		/// </summary>
		/// <param name="view">
		/// The <see cref="IView"/> to remove from
		/// the <see cref="ViewCollection"/>.
		/// </param>
		public virtual void Remove(IView view)
		{
			if(null == view)
				throw new ArgumentNullException("view", RD.GetString("errNullParam"));
			list.Remove(view);
			version++;
		}

		void IList.Remove(object value)
		{
			this.Remove((IView)value);
		}

		/// <summary>
		/// Inserts an <see cref="IView"/> into the <see cref="ViewCollection"/>
		/// at the specified <paramref name="index"/>.
		/// </summary>
		/// <param name="index">
		/// The zero-based index at which value should be inserted.
		/// </param>
		/// <param name="view">
		/// The <see cref="IView"/> to insert.
		/// </param>
		public virtual void Insert(int index, IView view)
		{
			if((index < 0) || (index > list.Count))
				throw new ArgumentOutOfRangeException("index", RD.GetString("errIndexOutOfRange"));
			if(null == view)
				throw new ArgumentNullException("view", RD.GetString("errNullParam"));
			list.Insert(index, view);
			version++;
		}

		void IList.Insert(int index, object value)
		{
			this.Insert(index, (IView)value);
		}

		/// <summary>
		/// Searches for the specified <see cref="IView"/> and returns
		/// the zero-based index of the first occurrence within
		/// the entire <see cref="ViewCollection"/>.
		/// </summary>
		/// <param name="view">
		/// The <see cref="IView"/> to locate in
		/// the <see cref="ViewCollection"/>.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of
		/// <paramref name="view"/> within the entire
		/// <see cref="ViewCollection"/>, if found; otherwise, -1.
		/// </returns>
		public int IndexOf(IView view)
		{
			if(null == view)
				throw new ArgumentNullException("view", RD.GetString("errNullParam"));
			return list.IndexOf(view);
		}

		int IList.IndexOf(object value)
		{
			return this.IndexOf((IView)value);
		}

		/// <summary>
		/// Removes all views from the <see cref="ViewCollection"/>.
		/// </summary>
		public virtual void Clear()
		{
			list.Clear();
			version++;
		}

		/// <summary>
		/// Determines whether the <see cref="ViewCollection"/> contains
		/// a specific <see cref="IView"/>.
		/// </summary>
		/// <param name="view">
		/// The <see cref="IView"/> to locate in
		/// the <see cref="ViewCollection"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the <see cref="IView"/> is found in
		/// the <see cref="ViewCollection"/>; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(IView view)
		{
			return list.Contains(view);
		}

		bool IList.Contains(object value)
		{
			return this.Contains((IView)value);
		}

		/// <summary>
		/// Adds an <see cref="IView"/> to the end of
		/// the <see cref="ViewCollection"/>.
		/// </summary>
		/// <param name="view">
		/// The <see cref="IView"/> to be added to the end of
		/// the <see cref="ViewCollection"/>.
		/// </param>
		/// <returns>
		/// The index at which the <paramref name="view"/> has been added.
		/// </returns>
		public virtual int Add(IView view)
		{
			if(null == view)
				throw new ArgumentNullException("view", RD.GetString("errNullParam"));
			int result = list.Add(view);
			version++;
			return result;
		}

		int IList.Add(object value)
		{
			return this.Add((IView)value);
		}
		#endregion

		/// <summary>
		/// Adds all elements of the <paramref name="views"/> array to the end
		/// of the <see cref="ViewCollection"/>.
		/// </summary>
		/// <param name="views">
		/// An array of <see cref="IView"/> objects.
		/// </param>
		public virtual void AddRange(IView[] views)
		{
			if(null == views)
				throw new ArgumentNullException("views", RD.GetString("errNullParam"));
			list.AddRange(views);
			version++;
		}

		/// <summary>
		/// Adds all elements of the <paramref name="collection"/> array to
		/// the end of the <see cref="ViewCollection"/>.
		/// </summary>
		/// <param name="collection">
		/// An instance of the <see cref="ViewCollection"/> class.
		/// </param>
		public virtual void AddRange(ViewCollection collection)
		{
			if(null == collection)
				throw new ArgumentNullException("collection", RD.GetString("errNullParam"));
			list.AddRange(collection);
			version++;
		}

		#region ICollection interface implementation
		/// <summary>
		/// Gets the number of elements actually contained in
		/// the <see cref="ViewCollection"/>.
		/// </summary>
		/// <value>
		/// The number of elements actually contained in the collection.
		/// </value>
		public int Count
		{
			get
			{
				return list.Count;
			}
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to
		/// the <see cref="ViewCollection"/>.
		/// </summary>
		/// <value>
		/// An object that can be used to synchronize access to
		/// the <see cref="ViewCollection"/>.
		/// </value>
		public object SyncRoot
		{
			get
			{
				return list.SyncRoot;
			}
		}

		/// <summary>
		/// Gets a value indicating whether access to
		/// the <see cref="ViewCollection"/> is synchronized (thread-safe).
		/// </summary>
		/// <value>
		/// <c>true</c> if access to the <see cref="ViewCollection"/> is
		/// synchronized (thread-safe); otherwise, <c>false</c>.
		/// </value>
		public bool IsSynchronized
		{
			get
			{
				return list.IsSynchronized;
			}
		}

		/// <summary>
		/// Copies the <see cref="ViewCollection"/> or a portion of it to
		/// a one-dimensional array.
		/// </summary>
		/// <param name="array">
		/// An array of <see cref="IView"/> objects.
		/// </param>
		/// <param name="index">
		/// The zero-based index in <paramref name="array"/> at which copying
		/// begins.
		/// </param>
		public void CopyTo(IView[] array, int index)
		{
			list.CopyTo(array, index);
		}

		void ICollection.CopyTo(Array array, int index)
		{
			this.CopyTo((IView[])array, index);
		}
		#endregion

		#region IEnumerable interface implementation
		/// <summary>
		/// Returns an enumerator for the entire <see cref="ViewCollection"/>.
		/// </summary>
		/// <returns>
		/// An instance of the <see cref="ViewCollection.Enumerator"/> for
		/// the entire <see cref="ViewCollection"/>.
		/// </returns>
		public Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion

		/// <summary>
		/// Makes a view most forward.
		/// </summary>
		/// <param name="view">A view to move forward.</param>
		/// <returns>
		/// <c>true</c> if the view successfully moved; <c>false</c> otherwise.
		/// </returns>
		public virtual bool MoveForward(IView view)
		{
			int index = IndexOf(view);
			if(-1 == index)
				return false;
			if(index == list.Count - 1)
				return false;
			IView temp = (IView)list[index];
			temp.Invalidate(0, 0, temp.Width, temp.Height);
			list.RemoveAt(index);
			list.Insert(index + 1, view);
			view.Invalidate(0, 0, view.Width, view.Height);
			return true;
		}

		/// <summary>
		/// Makes a view most backward.
		/// </summary>
		/// <param name="view">A view to move backward.</param>
		/// <returns>
		/// <c>true</c> if the view successfully moved; <c>false</c> otherwise.
		/// </returns>
		public virtual bool MoveBackward(IView view)
		{
			int index = IndexOf(view);
			if(-1 == index)
				return false;
			if(index == 1)
				return false;
			IView temp = (IView)list[index];
			temp.Invalidate(0, 0, temp.Width, temp.Height);
			list.RemoveAt(index);
			list.Insert(index + 1, view);
			view.Invalidate(0, 0, view.Width, view.Height);
			return true;
		}

		/// <summary>
		/// Moves a view one step up in z-order of views.
		/// </summary>
		/// <param name="view">A view to move forward.</param>
		/// <returns>
		/// <c>true</c> if the view successfully moved; <c>false</c> otherwise.
		/// </returns>
		public virtual bool MoveFront(IView view)
		{
			int index = IndexOf(view);
			if(-1 == index)
				return false;
			if(index == list.Count - 1)
				return false;
			IView temp = (IView)list[index];
			temp.Invalidate(0, 0, temp.Width, temp.Height);
			list.RemoveAt(index);
			list.Add(view);
			view.Invalidate(0, 0, view.Width, view.Height);
			return true;
		}

		/// <summary>
		/// Moves a view one step down in z-order of views.
		/// </summary>
		/// <param name="view">A view to move backward.</param>
		/// <returns>
		/// <c>true</c> if the view successfully moved; <c>false</c> otherwise.
		/// </returns>
		public virtual bool MoveBack(IView view)
		{
			int index = IndexOf(view);
			if(-1 == index)
				return false;
			if(0 == index)
				return false;
			IView temp = (IView)list[index];
			temp.Invalidate(0, 0, temp.Width, temp.Height);
			list.RemoveAt(index);
			list.Insert(0, view);
			view.Invalidate(0, 0, view.Width, view.Height);
			return true;
		}
	}
}
