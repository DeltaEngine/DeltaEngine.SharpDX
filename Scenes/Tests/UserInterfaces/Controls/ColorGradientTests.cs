using DeltaEngine.Datatypes;
using DeltaEngine.Scenes.UserInterfaces.Controls;
using NUnit.Framework;
using DeltaEngine.Platforms;

namespace DeltaEngine.Scenes.Tests.UserInterfaces.Controls
{
	public class ColorGradientTests :TestWithMocksOrVisually
	{
		[Test]
		public void CreateGradientGraph()
		{
			RangeGraph<Color> colors = new RangeGraph<Color>(Color.Blue, Color.Green);
			var graph = new ColorGradient(Rectangle.FromCenter(Vector2D.Half, new Size(0.6f, 0.1f)), colors);
		}
	}
}
