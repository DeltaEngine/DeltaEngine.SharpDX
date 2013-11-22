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
			interactionLogic = new InteractionLogic();
		}

		private InteractionLogic interactionLogic;

		[Test, CloseAfterFirstFrame]
		public void AsteroidCreatedWhenTimeReached()
		{
			interactionLogic.BeginGame();
			AdvanceTimeAndUpdateEntities(1.1f);
			Assert.GreaterOrEqual(EntitiesRunner.Current.GetEntitiesOfType<Asteroid>().Count, 1);
		}

		//ncrunch: no coverage start
		[Test, CloseAfterFirstFrame, Ignore]
		public void ProjectileAndAsteroidDisposedOnCollision()
		{
			var projectile = new Projectile(Vector2D.Half, 0);
			EntitiesRunner.Current.GetEntitiesOfType<Projectile>().Add(projectile);
			interactionLogic.CreateAsteroidsAtPosition(Vector2D.Half, 1, 1);
			AdvanceTimeAndUpdateEntities(1.0f);
			Assert.IsFalse(projectile.IsActive);
		}
		//ncrunch: no coverage end

		[Test, CloseAfterFirstFrame]
		public void PlayerShipAndAsteroidCollidingResultsInGameOver()
		{
			bool gameOver = false;
			interactionLogic.BeginGame();
			interactionLogic.GameOver += () => { gameOver = true; };
			interactionLogic.Player.Set(new Rectangle(Vector2D.Half, new Size(.05f)));
			interactionLogic.CreateAsteroidsAtPosition(Vector2D.Half, 1, 1);
			AdvanceTimeAndUpdateEntities();
			Assert.IsTrue(gameOver);
		}

		[Test, CloseAfterFirstFrame]
		public void InteractionLogicsIsPauseable()
		{
			Assert.IsTrue(interactionLogic.IsPauseable);
		}
	}
}