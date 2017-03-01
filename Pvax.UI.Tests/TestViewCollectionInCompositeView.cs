using System;

using NUnit.Framework;

using Pvax.UI.Views;

namespace Pvax.UI.Views.Tests
{
	[TestFixture]
	public class TestViewCollectionInCompositeView: ViewCollectionTestBase
	{
		private ViewContainer viewContainer;

		private CompositeView compositeView;

		public override void Init()
		{
			viewContainer = new ViewContainer();
			compositeView = new CompositeView(0, 0, 320, 200);
			viewContainer.Views.Add(compositeView);
			viewCollection = compositeView.Views;
		}

		public override void Done()
		{
			base.Done();
			viewContainer.Dispose();
			viewContainer = null;
		}
	}
}
