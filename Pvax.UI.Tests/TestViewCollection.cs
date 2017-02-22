using System;

using NUnit.Framework;

using Pvax.UI.Views;

namespace Pvax.UI.Views.Tests
{
	[TestFixture]
	public class TestViewCollection: ViewCollectionTestBase
	{
		public override void Init()
		{
			viewCollection = new ViewCollection();
		}
	}
}
