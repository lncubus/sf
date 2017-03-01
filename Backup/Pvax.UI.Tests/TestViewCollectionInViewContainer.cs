using System;

using NUnit.Framework;

using Pvax.UI.Views;

namespace Pvax.UI.Views.Tests
{
	[TestFixture]
	public class TestViewCollectionInViewContainer: ViewCollectionTestBase
	{
		private ViewContainer viewContainer;
		
		public override void Init()
		{
			viewContainer = new ViewContainer();
			viewCollection = viewContainer.Views;
		}
		
		public override void Done()
		{
			base.Done();
			viewContainer.Dispose();
			viewContainer = null;
		}
	}
}
