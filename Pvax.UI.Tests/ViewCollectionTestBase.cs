using System;
using System.Collections;

using NUnit.Framework;

using Pvax.UI.Views;

namespace Pvax.UI.Views.Tests
{
	/// <summary>
	/// Description of ViewCollectionTestBase.
	/// </summary>
	public abstract class ViewCollectionTestBase
	{
		protected ViewCollection viewCollection;

		[SetUp]
		public abstract void Init();

		[TearDown]
		public virtual void Done()
		{
			viewCollection = null;
		}

		[Test]
		public void TestAddOne()
		{
			MockView view1 = new MockView();
			viewCollection.Add(view1);
			Assert.AreEqual(1, viewCollection.Count);
			Assert.AreSame(view1, viewCollection[0]);
		}

		[Test]
		public void TestAddThree()
		{
			MockView view1 = new MockView();
			MockView view2 = new MockView();
			MockView view3 = new MockView();
			viewCollection.Add(view1);
			viewCollection.Add(view2);
			viewCollection.Add(view3);
			Assert.AreEqual(3, viewCollection.Count);
			Assert.IsTrue(viewCollection.Contains(view3));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestAddNull()
		{
			viewCollection.Add(null);
		}

		[Test]
		public void TestAddRange()
		{
			MockView view1 = new MockView();
			MockView view2 = new MockView();
			MockView view3 = new MockView();
			viewCollection.AddRange(new IView[] {view1, view2, view3});
			Assert.AreEqual(3, viewCollection.Count);
			Assert.IsTrue(viewCollection.Contains(view3));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestAddNullRange1()
		{
			viewCollection.AddRange((IView[])null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestAddNullRange2()
		{
			viewCollection.AddRange((ViewCollection)null);
		}

		[Test]
		public void TestAddEmptyRange()
		{
			viewCollection.AddRange(new IView[] {});
			Assert.AreEqual(0, viewCollection.Count);
		}

		[Test]
		public void TestAccess()
		{
			MockView view1 = new MockView();
			MockView view2 = new MockView();
			MockView view3 = new MockView();
			viewCollection.AddRange(new IView[] {view1, view2, view3});
			Assert.AreSame(view2, viewCollection[1]);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestAccessBelow()
		{
			MockView view1 = new MockView();
			MockView view2 = new MockView();
			MockView view3 = new MockView();
			viewCollection.AddRange(new IView[] {view1, view2, view3});
			IView view = viewCollection[-1];
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestAccessAbove()
		{
			MockView view1 = new MockView();
			MockView view2 = new MockView();
			MockView view3 = new MockView();
			viewCollection.AddRange(new IView[] {view1, view2, view3});
			IView view = viewCollection[4];
		}

		[Test]
		public void TestIsReadOnly()
		{
			Assert.IsFalse(viewCollection.IsReadOnly);
		}

		[Test]
		public void TestIsFixedSize()
		{
			Assert.IsFalse(viewCollection.IsFixedSize);
		}

		[Test]
		public void TestRemoveAt()
		{
			viewCollection.AddRange(new IView[] {new MockView(), new MockView(), new MockView(), new MockView(), new MockView()});
			IView view = viewCollection[2];
			viewCollection.RemoveAt(2);
			Assert.IsFalse(Object.ReferenceEquals(view, viewCollection[2]));
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestRemoveAtBelow()
		{
			viewCollection.AddRange(new IView[] {new MockView(), new MockView(), new MockView(), new MockView(), new MockView()});
			viewCollection.RemoveAt(-1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestRemoveAtAbove()
		{
			viewCollection.AddRange(new IView[] {new MockView(), new MockView(), new MockView(), new MockView(), new MockView()});
			viewCollection.RemoveAt(viewCollection.Count);
		}

		[Test]
		public void TestRemove()
		{
			MockView view1 = new MockView();
			viewCollection.AddRange(new IView[] {new MockView(), new MockView(), view1, new MockView(), new MockView()});
			Assert.AreSame(view1, viewCollection[2]);
			viewCollection.Remove(view1);
			foreach(IView view in viewCollection)
				Assert.IsFalse(Object.ReferenceEquals(view1, view));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestRemoveNull()
		{
			viewCollection.AddRange(new IView[] {new MockView(), new MockView(), new MockView(), new MockView(), new MockView()});
			viewCollection.Remove(null);
		}

		[Test]
		public void TestInsert()
		{
			MockView view1 = new MockView();
			MockView view2 = new MockView();
			MockView view3 = new MockView();
			viewCollection.AddRange(new IView[] {view1, view2, view3});
			Assert.AreSame(view2, viewCollection[1]);
			MockView view4 = new MockView();
			viewCollection.Insert(1, view4);
			Assert.AreSame(view4, viewCollection[1]);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInsertNull()
		{
			viewCollection.AddRange(new IView[] {new MockView(), new MockView(), new MockView(), new MockView(), new MockView()});
			viewCollection.Insert(1, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestInsertBelow()
		{
			viewCollection.AddRange(new IView[] {new MockView(), new MockView(), new MockView(), new MockView(), new MockView()});
			viewCollection.Insert(-1, new MockView());
		}

		[Test]
		public void TestAddWithInsert()
		{
			viewCollection.AddRange(new IView[] {new MockView(), new MockView(), new MockView(), new MockView(), new MockView()});
			MockView view4 = new MockView();
			viewCollection.Insert(viewCollection.Count, view4);
			Assert.AreSame(view4, viewCollection[5]);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestInsertAbove()
		{
			viewCollection.AddRange(new IView[] {new MockView(), new MockView(), new MockView(), new MockView(), new MockView()});
			viewCollection.Insert(viewCollection.Count + 1, new MockView());
		}

		[Test]
		public void TestIndexOf()
		{
			MockView view1 = new MockView();
			MockView view2 = new MockView();
			MockView view3 = new MockView();
			viewCollection.AddRange(new IView[] {view1, view2, view3});
			Assert.AreEqual(0, viewCollection.IndexOf(view1));
			Assert.AreEqual(1, viewCollection.IndexOf(view2));
			Assert.AreEqual(2, viewCollection.IndexOf(view3));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestIndexOfNull()
		{
			MockView view1 = new MockView();
			MockView view2 = new MockView();
			MockView view3 = new MockView();
			viewCollection.AddRange(new IView[] {view1, view2, view3});
			viewCollection.IndexOf(null);
			Assert.Fail("viewCollection.IndexOf(null) must fail, but it didn't.");
		}

		[Test]
		public void TestIndexOfMissing()
		{
			MockView view1 = new MockView();
			MockView view2 = new MockView();
			MockView view3 = new MockView();
			viewCollection.AddRange(new IView[] {view1, view2, view3});
			MockView view4 = new MockView();
			Assert.AreEqual(-1, viewCollection.IndexOf(view4));
		}

		[Test]
		public void TestClear()
		{
			viewCollection.AddRange(new IView[] {new MockView(), new MockView(), new MockView(), new MockView(), new MockView()});
			viewCollection.Clear();
			Assert.AreEqual(0, viewCollection.Count);
		}

		[Test]
		public void TestContains()
		{
			MockView view1 = new MockView();
			MockView view2 = new MockView();
			MockView view3 = new MockView();
			viewCollection.AddRange(new IView[] {view1, view2, view3});
			MockView view4 = new MockView();
			Assert.IsTrue(viewCollection.Contains(view2));
			Assert.IsFalse(viewCollection.Contains(view4));
		}

		[Test]
		public void TestCount()
		{
			Assert.AreEqual(0, viewCollection.Count);
			viewCollection.Add(new MockView());
			Assert.AreEqual(1, viewCollection.Count);
			viewCollection.Add(new MockView());
			Assert.AreEqual(2, viewCollection.Count);
			viewCollection.Add(new MockView());
			Assert.AreEqual(3, viewCollection.Count);
			viewCollection.Clear();
			Assert.AreEqual(0, viewCollection.Count);
		}

		[Test]
		public void TestSyncRoot()
		{
			Assert.IsNotNull(viewCollection.SyncRoot);
			Assert.IsFalse(Object.ReferenceEquals(viewCollection, viewCollection.SyncRoot));
		}

		[Test]
		public void TestIsSynchronized()
		{
			Assert.IsFalse(viewCollection.IsSynchronized);
		}

		[Test]
		public void TestCopyTo()
		{
			MockView view1 = new MockView();
			MockView view2 = new MockView();
			MockView view3 = new MockView();
			MockView view4 = new MockView();
			viewCollection.AddRange(new IView[] {view1, view2, view3, view4});
			IView[] views = new IView[viewCollection.Count];
			viewCollection.CopyTo(views, 0);
			Assert.AreSame(view1, views[0]);
			Assert.AreSame(view2, views[1]);
			Assert.AreSame(view3, views[2]);
			Assert.AreSame(view4, views[3]);
		}

		[Test]
		public void TestGetEnumerator()
		{
			IEnumerable enumerable = viewCollection as IEnumerable;
			IEnumerator enumerator = enumerable.GetEnumerator();
			Assert.AreEqual(typeof(ViewCollection.Enumerator), enumerator.GetType());
		}

		[Test]
		public void TestEnumerator()
		{
			viewCollection.AddRange(new IView[] {new MockView(), new MockView(), new MockView(), new MockView(), new MockView()});

		}
	}
}
