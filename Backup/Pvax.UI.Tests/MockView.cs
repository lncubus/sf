using System;
using System.Drawing;

using NUnit.Framework;

using Pvax.UI.Views;

namespace Pvax.UI.Views.Tests
{
	class MockView: View
	{
		protected override void Draw(Graphics graphics)
		{}
	}
}
