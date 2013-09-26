using System;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D;

namespace $safeprojectname$
{
	public class InteractionLogics : Entity, Updateable
	{
		public InteractionLogics()
		{
			random = new PseudoRandom();
			Player = new PlayerShip();
			CreateRandomAsteroids(1);
			CreateRandomAsteroids(1, 2);
			IncreaseScore += i => 
			{
			};
		}

		public PlayerShip Player
		{
			get;
			private set;
		}

		private readonly PseudoRandom random;

		public void CreateRandomAsteroids(int howMany, int sizeMod = 1)
		{
			for (int asteroidCount = 0; asteroidCount < howMany; asteroidCount++)
			{
				new Asteroid(random, this, sizeMod);
			}
		}

		public void CreateAsteroidsAtPosition(Vector2D position, int sizeMod = 1, int howMany = 2)
		{
			for (int asteroidCount = 0; asteroidCount < howMany; asteroidCount++)
			{
				var asteroid = new Asteroid(random, this, sizeMod);
				asteroid.DrawArea = new Rectangle(position, asteroid.DrawArea.Size);
			}
		}

		public void IncrementScore(int increase)
		{
			IncreaseScore.Invoke(increase);
		}

		private void CheckAsteroidCollisions()
		{
			foreach (var asteroid in EntitiesRunner.Current.GetEntitiesOfType<Asteroid>())
			{
				foreach (var projectile in EntitiesRunner.Current.GetEntitiesOfType<Projectile>())
					if (ObjectsInHitRadius(projectile, asteroid, 0.1f / asteroid.sizeModifier))
					{
						projectile.IsActive = false;
						asteroid.Fracture();
					}

				if (ObjectsInHitRadius(Player, asteroid, 0.06f / asteroid.sizeModifier) && GameOver != null)
					GameOver();
			}
		}

		private static bool ObjectsInHitRadius(Entity2D entityAlpha, Entity2D entitytBeta, float radius)
		{
			return entityAlpha.DrawArea.Center.DistanceTo(entitytBeta.DrawArea.Center) < radius;
		}

		private void CreateNewAsteroidIfNecessary()
		{
			if (GlobalTime.Current.Milliseconds - 1000 > timeLastNewAsteroid && 
				EntitiesRunner.Current.GetEntitiesOfType<Asteroid>().Count <= MaximumAsteroids)
			{
				CreateRandomAsteroids(1);
				timeLastNewAsteroid = GlobalTime.Current.Milliseconds;
			}
		}

		private const int MaximumAsteroids = 10;
		private float timeLastNewAsteroid;

		public event Action GameOver;
		public event Action<int> IncreaseScore;

		public void Restart()
		{
			foreach (Asteroid asteroid in EntitiesRunner.Current.GetEntitiesOfType<Asteroid>())
				asteroid.IsActive = false;

			foreach (Projectile projectile in EntitiesRunner.Current.GetEntitiesOfType<Projectile>())
				projectile.IsActive = false;

			Player = new PlayerShip();
		}

		public void Update()
		{
			CheckAsteroidCollisions();
			CreateNewAsteroidIfNecessary();
		}

		public bool IsPauseable
		{
			get
			{
				return true;
			}
		}
	}
}