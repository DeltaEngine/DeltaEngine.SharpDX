using DeltaEngine;
using DeltaEngine.Core;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace Asteroids.Tests
{
	internal class AsteroidTests : TestWithMocksOrVisually
	{
		[Test]
		public void FractureAsteroid()
		{
			Resolve<Window>();
			var asteroid = new Asteroid(new PseudoRandom(), new InteractionLogics());
			asteroid.Fracture();
			Assert.IsFalse(asteroid.IsActive);
		}

		[Test]
		public void ShowAsteroidsOfSeveralSizemodsAndFracture()
		{
			Resolve<Window>();
			var randomizer = new PseudoRandom();
			var gameLogic = new InteractionLogics();
			var largeAsteroid = new Asteroid(randomizer, gameLogic);
			new Asteroid(randomizer, gameLogic, 2);
			new Asteroid(randomizer, gameLogic, 3);

			largeAsteroid.Fracture();
			Assert.IsFalse(largeAsteroid.IsActive);
		}
	}
}