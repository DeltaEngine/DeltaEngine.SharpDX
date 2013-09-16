using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace Asteroids.Tests
{
	internal class GameLogicTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void InitGameLogic()
		{
			Resolve<Window>();
			interactionLogics = new InteractionLogics();
		}

		private InteractionLogics interactionLogics;

		[Test]
		public void AsteroidCreatedWhenTimeReached()
		{
			AdvanceTimeAndUpdateEntities(1.1f);
			Assert.GreaterOrEqual(EntitiesRunner.Current.GetEntitiesOfType<Asteroid>().Count, 2);
		}

		[Test]
		public void ProjectileAndAsteroidDisposedOnCollision()
		{
			var projectile = new Projectile(new Material(Shader.Position2DColorUv, "DeltaEngineLogo"),
				Point.Half, 0);
			EntitiesRunner.Current.GetEntitiesOfType<Projectile>().Add(projectile);
			interactionLogics.CreateAsteroidsAtPosition(Point.Half, 1, 1);
			AdvanceTimeAndUpdateEntities(1.0f);
			Assert.IsFalse(projectile.IsActive);
		}

		[Test]
		public void PlayerShipAndAsteroidCollidingResultsInGameOver()
		{
			bool gameOver = false;
			interactionLogics.GameOver += () =>
			{
				gameOver = true;
			};
			interactionLogics.Player.Set(new Rectangle(Point.Half, new Size(.05f)));
			interactionLogics.CreateAsteroidsAtPosition(Point.Half, 1, 1);
			AdvanceTimeAndUpdateEntities(0.2f);
			Assert.IsTrue(gameOver);
		}
	}
}