using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;
using System.Collections.Generic;

namespace DeltaEngine.Rendering2D.Graphs.Tests
{
	class GradientGraphTests : TestWithMocksOrVisually
	{
		[Test]
		public void DrawGradientGraph()
		{
			var colors = new List<Color>(new[] {Color.DarkGreen, Color.Red, Color.Orange, Color.Green, Color.Black ,Color.Gold, Color.PaleGreen});
			var colorRanges = new RangeGraph<Color>(colors);
			new GradientGraph(new Rectangle(0.1f, 0.4f, 0.8f, 0.2f), colorRanges);
		}
	}
}
