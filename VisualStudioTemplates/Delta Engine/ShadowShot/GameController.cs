using System;
using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D;
using DeltaEngine.ScreenSpaces;

namespace $safeprojectname$
{
	public class GameController : Entity2D, IDisposable
	{
		public GameController(PlayerShip playerShip, Material material, Size size, ScreenSpace 
			screenSpace) : base(Rectangle.Zero)
		{
			this.screenSpace = screenSpace;
			ship = playerShip;
			asteroidMaterial = material;
			asteroidSize = size;
			Start<GameLogicHandler>();
		}

		private readonly PlayerShip ship;
		private readonly Material asteroidMaterial;
		private readonly Size asteroidSize;
		private readonly ScreenSpace screenSpace;
		private class GameLogicHandler : UpdateBehavior
		{
			public GameLogicHandler()
			{
				random = new PseudoRandom();
			}

			private readonly PseudoRandom random;

			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (GameController gameController in entities)
				{
					CreateRandomAsteroids(gameController);
					CheckForShipAsteroidCollision(gameController);
					CheckForProjectileAsteroidCollision(gameController);
				}
			}

			private void CreateRandomAsteroids(GameController manager)
			{
				if (random.Get() < Constants.AsteroidSpawnProbability * Time.Delta)
					if (AsteroidsCount < Constants.MaximumAsteroids)
				{
					var drawArea = GetRandomDrawArea(manager);
					new Asteroid(manager.asteroidMaterial, drawArea, manager.screenSpace.Viewport.Bottom);
					AsteroidsCount++;
				}
				AsteroidsCount = 0;
			}

			private int AsteroidsCount
			{
				get;
				set;
			}

			private Rectangle GetRandomDrawArea(GameController gameController)
			{
				var posX = random.Get(0.05f, 0.95f);
				return Rectangle.FromCenter(new Vector2D(posX, 0.1f), gameController.asteroidSize);
			}

			private static void CheckForShipAsteroidCollision(GameController gameController)
			{
				foreach (Asteroid asteroid in EntitiesRunner.Current.GetEntitiesOfType<Asteroid>())
					if (gameController.ship.Center.DistanceTo(asteroid.Center) < 0.04f)
						if (gameController.ShipCollidedWithAsteroid != null)
							gameController.ShipCollidedWithAsteroid();
			}

			private static void CheckForProjectileAsteroidCollision(GameController gameController)
			{
				var toRemove = new List<Projectile>();
				foreach (Projectile projectile in gameController.ship.ActiveProjectileList)
					if (projectile.IsActive)
						foreach (Asteroid asteroid in EntitiesRunner.Current.GetEntitiesOfType<Asteroid>())
							if (asteroid.IsActive)
								if (asteroid.DrawArea.IsColliding(0.0f, projectile.DrawArea, 0.0f))
							{
								projectile.IsActive = false;
								toRemove.Add(projectile);
								asteroid.IsActive = false;
							}

				foreach (var projectile in toRemove)
					gameController.ship.ActiveProjectileList.Remove(projectile);
			}
		}
		public event Action ShipCollidedWithAsteroid;

		public void Dispose()
		{
			ship.Dispose();
			foreach (var asteroid in EntitiesRunner.Current.GetEntitiesOfType<Asteroid>())
				asteroid.IsActive = false;

			Stop<GameLogicHandler>();
			IsActive = false;
		}
	}
}