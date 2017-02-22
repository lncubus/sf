using System;

using NUnit.Framework;

using Pvax.UI.Views;

namespace Pvax.UI.Views.Tests
{
	[TestFixture]
	public class TestView
	{
		private IView iview;
		
		private MockView view;
		
		[SetUp]
		public void Init()
		{
			view = new MockView();
			iview = view;
		}
		
		[TearDown]
		public void Done()
		{
			view = null;
			iview = null;
		}
		
		[Test]
		public void TestEnabled()
		{
			Assert.IsTrue(iview.Enabled);
			iview.Enabled = false;
			Assert.IsFalse(iview.Enabled);
		}
		
		[Test]
		public void TestVisible()
		{
			Assert.IsTrue(iview.Visible);
			iview.Visible = false;
			Assert.IsFalse(iview.Visible);
		}
		
		[Test]
		public void TestActive()
		{
			Assert.IsFalse(view.Active);
			view.Active = true;
			Assert.IsTrue(view.Active);
		}
		
		[Test]
		public void TestSelected()
		{
			Assert.IsFalse(view.Selected);
			view.Selected = true;
			Assert.IsTrue(view.Selected);
		}
	}
}
