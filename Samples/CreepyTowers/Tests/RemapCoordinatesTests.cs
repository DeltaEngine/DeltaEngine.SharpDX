using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	public class RemapCoordinatesTests : TestWithMocksOrVisually
	{
		[Test]
		public void CheckForCoordinatesRemappingGivenSize()
		{
			new Game(Resolve<Window>());
			var window = Resolve<Window>();
			window.ViewportPixelSize = new Size(800, 600);
			var remap = new RemapCoordinates();
			var newSize = remap.RemapCoordinateSpaces(new Size(100, 100));
			Assert.GreaterOrEqual(0.75f, newSize.AspectRatio);
		}

		[Test]
		public void CheckForCoordinatesSpacesRemappingGivenPoint()
		{
			new Game(Resolve<Window>());
			var window = Resolve<Window>();
			window.ViewportPixelSize = new Size(800, 600);
			var remap = new RemapCoordinates();
			var newSize = remap.RemapCoordinateSpaces(Vector2D.One);
			Assert.GreaterOrEqual(0.75f, newSize.AspectRatio);
		}
	}
}