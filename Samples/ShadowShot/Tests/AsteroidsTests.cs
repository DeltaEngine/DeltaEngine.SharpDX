using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace ShadowShot.Tests
{
	public class AsteroidsTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			Resolve<Window>().ViewportPixelSize = new Size(800, 800);
			var image = new Material(Shader.Position2DColorUV, "asteroid");
			var drawArea = Rectangle.FromCenter(new Vector2D(0.5f, 0.1f), new Size(0.1f));
			asteroid = new Asteroid(image, drawArea, Resolve<ScreenSpace>().Viewport.Bottom);
		}
		private Asteroid asteroid;

		[Test]
		public void CreateAsteroid()
		{
			Assert.AreEqual(new Vector2D(0.5f, 0.1f), asteroid.DrawArea.Center);
		}

		[Test]
		public void CheckAsteroidFreeFall()
		{
			AdvanceTimeAndUpdateEntities();
			Assert.LessOrEqual(0.1f, asteroid.DrawArea.Center.Y);
			var changedY = asteroid.DrawArea.Center.Y;
			AdvanceTimeAndUpdateEntities();
			Assert.LessOrEqual(changedY, asteroid.DrawArea.Center.Y);
		}
	}
}