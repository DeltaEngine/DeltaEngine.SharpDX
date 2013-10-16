using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace Asteroids.Tests
{
	internal class AsteroidTests : TestWithMocksOrVisually
	{
		[Test, CloseAfterFirstFrame]
		public void FractureAsteroid()
		{
			var asteroid = new Asteroid(new InteractionLogics());
			asteroid.Fracture();
			Assert.IsFalse(asteroid.IsActive);
		}

		[Test, CloseAfterFirstFrame]
		public void ShowAsteroidsOfSeveralSizemodsAndFracture()
		{
			var gameLogic = new InteractionLogics();
			var largeAsteroid = new Asteroid(gameLogic);
			new Asteroid(gameLogic, 2);
			new Asteroid(gameLogic, 3);
			largeAsteroid.Fracture();
			Assert.IsFalse(largeAsteroid.IsActive);
		}

		[Test, CloseAfterFirstFrame]
		public void CreateAsteroidAtDefinedPosition()
		{
			var asteroid = new Asteroid(Vector2D.Zero, new InteractionLogics());
		}
	}
}