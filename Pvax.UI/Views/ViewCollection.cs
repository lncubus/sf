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
using System.Collections.Generic;

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
	public class ViewCollection: IList<IView>, ICollection<IView>, IEnumerable<IView>
	{
		List<IView> self = new List<IView>();

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewCollection"/>
		/// class.
		/// </summary>
		public ViewCollection ()
		{
		}

		public virtual IView this [int index]
		{
			get
			{
				return self[index];
			}

			set
			{
				// TODO: throw new InvalidOperationException();
				self[index] = value;
			}
		}

		// TODO: reintroduce!
		/*
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

*/

		public virtual void Add (IView item)
		{
			self.Add(item);
		}

		public virtual void AddRange (IEnumerable<IView> views)
		{
			self.AddRange(views);
		}

		public virtual void Insert (int index, IView item)
		{
			self.Insert(index, item);
		}

		public virtual bool Remove (IView item)
		{
			return self.Remove(item);
		}

		public virtual void RemoveAt (int index)
		{
			self.RemoveAt(index);
		}

		public virtual void Clear ()
		{
			self.Clear();
		}

		public int Count
		{
			get
			{
				return self.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((ICollection<IView>)self).IsReadOnly;
			}
		}

		public bool Contains (IView item)
		{
			return self.Contains(item);
		}

		public void CopyTo (IView[] array, int arrayIndex)
		{
			self.CopyTo(array, arrayIndex);
		}

		public IEnumerator<IView> GetEnumerator ()
		{
			return self.GetEnumerator();
		}

		public int IndexOf (IView item)
		{
			return self.IndexOf(item);
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return self.GetEnumerator();
		}
	}
}
