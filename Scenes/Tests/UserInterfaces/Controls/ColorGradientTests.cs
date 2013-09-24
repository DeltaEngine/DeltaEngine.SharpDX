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
			new ColorGradient(Rectangle.FromCenter(Vector2D.Half, new Size(0.6f, 0.1f)), Color.Red);
		}
	}
}
