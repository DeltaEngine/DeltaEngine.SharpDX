using System;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D;
using DeltaEngine.Rendering2D.Particles;

namespace $safeprojectname$
{
	public class InteractionLogics : Entity, Updateable
	{
		public InteractionLogics()
		{
			explosionData = ContentLoader.Load<ParticleEmitterData>("ExplosionEmitter");
			shipExplosionData = ContentLoader.Load<ParticleEmitterData>("ExplosionEmitter");
			IncreaseScore += i => 
			{
			};
		}

		public void BeginGame()
		{
			gameRunning = true;
			Player = new PlayerShip();
		}

		public PlayerShip Player
		{
			get;
			private set;
		}

		private ParticleEmitterData explosionData;
		private ParticleEmitterData shipExplosionData;
		private bool gameRunning;

		public void CreateRandomAsteroids(int howMany, int sizeMod = 1)
		{
			for (int asteroidCount = 0; asteroidCount < howMany; asteroidCount++)
			{
				new Asteroid(this, sizeMod);
			}
		}

		public void CreateAsteroidsAtPosition(Vector2D position, int sizeMod = 1, int howMany = 2)
		{
			for (int asteroidCount = 0; asteroidCount < howMany; asteroidCount++)
			{
				var asteroid = new Asteroid(this, sizeMod);
				asteroid.SetDrawAreaNoInterpolation(new Rectangle(position, asteroid.DrawArea.Size));
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
						var explosionEmitter = new ParticleEmitter(explosionData, new 
							Vector3D(projectile.Center));
						explosionEmitter.RenderLayer = 10;
						explosionEmitter.DisposeAfterSeconds(0.7f);
						projectile.Dispose();
						asteroid.Fracture();
					}

				if (Player.IsActive && ObjectsInHitRadius(Player, asteroid, 0.06f / asteroid.sizeModifier))
				{
					Player.IsActive = false;
					var explosionEmitter = new ParticleEmitter(shipExplosionData, new 
						Vector3D(Player.Center));
					explosionEmitter.RenderLayer = 10;
					explosionEmitter.DisposeAfterSeconds(0.5f);
					if (GameOver != null)
						GameOver();
				}
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
			DisposeObjects();
			BeginGame();
			gameRunning = true;
		}

		public void DisposeObjects()
		{
			foreach (Asteroid asteroid in EntitiesRunner.Current.GetEntitiesOfType<Asteroid>())
				asteroid.IsActive = false;

			foreach (Projectile projectile in EntitiesRunner.Current.GetEntitiesOfType<Projectile>())
				projectile.Dispose();

			foreach (PlayerShip playerShip in EntitiesRunner.Current.GetEntitiesOfType<PlayerShip>())
				playerShip.IsActive = false;
		}

		public void Update()
		{
			if (!gameRunning)
				return;

			CheckAsteroidCollisions();
			CreateNewAsteroidIfNecessary();
		}

		public void PauseUpdate()
		{
			gameRunning = false;
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