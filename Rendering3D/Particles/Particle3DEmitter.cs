using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D.Sprites;

namespace DeltaEngine.Rendering3D.Particles
{
	/// <summary>
	/// Holds data on how to spawn particles and currently existant ones.
	/// </summary>
	public abstract class Particle3DEmitter : ParticleEmitter
	{
		protected Particle3DEmitter(ParticleEmitterData data, Vector3D spawnPosition)
		{
			EmitterData = data;
			Position = spawnPosition;
			OnDraw<Particle3DRenderer>();
			if (EmitterData.ParticleMaterial.Animation != null)
				Start<UpdateImageAnimation>();
			if (EmitterData.ParticleMaterial.SpriteSheet != null)
				Start<UpdateSpriteSheetAnimation>();
		}

		public ParticleEmitterData EmitterData { get; private set; }
		public Vector3D Position { get; set; }

		protected override void UpdateAndLimitNumberOfActiveParticles()
		{
			int lastIndex = -1;
			for (int index = 0; index < NumberOfActiveParticles; index++)
				if (particles[index].UpdateIfStillActive(EmitterData))
				{
					lastIndex = index;
					UpdateParticleProperties(index);
				}
			NumberOfActiveParticles = lastIndex + 1;
		}

		private void UpdateParticleProperties(int index)
		{
			var interpolation = particles[index].ElapsedTime / EmitterData.LifeTime;
			particles[index].Color = EmitterData.Color.GetInterpolatedValue(interpolation);
			particles[index].Size = EmitterData.Size.GetInterpolatedValue(interpolation);
			particles[index].Acceleration = EmitterData.Acceleration.GetInterpolatedValue(interpolation);
		}

		protected override void UpdateAnimation()
		{
			if (EmitterData.ParticleMaterial.Animation != null)
				for (int index = 0; index < NumberOfActiveParticles; index++)
					UpdateAnimationForParticle(index, EmitterData.ParticleMaterial);
			if (EmitterData.ParticleMaterial.SpriteSheet != null)
				for (int index = 0; index < NumberOfActiveParticles; index++)
					UpdateSpriteSheetAnimationForParticle(index, EmitterData.ParticleMaterial);
		}

		private void UpdateAnimationForParticle(int index, Material material)
		{
			var animationData = material.Animation;
			particles[index].CurrentFrame =
				(int)(animationData.Frames.Length * particles[index].ElapsedTime / material.Duration) %
					animationData.Frames.Length;
		}

		private void UpdateSpriteSheetAnimationForParticle(int index, Material material)
		{
			var animationData = material.SpriteSheet;
			particles[index].CurrentFrame =
				(int)(animationData.UVs.Count * particles[index].ElapsedTime / material.Duration) %
					animationData.UVs.Count;
			particles[index].CurrentUV = animationData.UVs[particles[index].CurrentFrame];
		}

		protected override void SpawnNewParticles()
		{
			if (particles == null || particles.Length != EmitterData.MaximumNumberOfParticles)
				CreateParticlesArray();
			ElapsedSinceLastSpawn += Time.Delta;
			if (EmitterData.SpawnInterval <= 0.0f)
				return;
			while (ElapsedSinceLastSpawn >= EmitterData.SpawnInterval)
				DoIntervalSpawn();
		}

		public Particle3D[] particles;

		public void CreateParticlesArray()
		{
			if (EmitterData.MaximumNumberOfParticles > MaxParticles)
				throw new MaximumNumberOfParticlesExceeded(EmitterData.MaximumNumberOfParticles, MaxParticles);
			particles = new Particle3D[EmitterData.MaximumNumberOfParticles];
			Set(particles);
		}

		private void DoIntervalSpawn()
		{
			ElapsedSinceLastSpawn -= EmitterData.SpawnInterval;
			for (int i = 0; i < EmitterData.ParticlesPerSpawn.Start.GetRandomValue(); i++)
				SpawnOneParticle();
		}

		private void SpawnOneParticle()
		{
			int freeSpot = FindFreeSpot();
			if (freeSpot < 0)
				return;
			particles[freeSpot].IsActive = true;
			particles[freeSpot].ElapsedTime = 0;
			particles[freeSpot].Position = GetParticleSpawnPosition();
			particles[freeSpot].SetStartVelocityRandomizedFromRange(EmitterData.StartVelocity.Start,
				EmitterData.StartVelocity.End);
			particles[freeSpot].Acceleration = EmitterData.Acceleration.Start;
			particles[freeSpot].Size = EmitterData.Size.Start;
			particles[freeSpot].Color = EmitterData.Color.Start;
			particles[freeSpot].CurrentUV = EmitterData.ParticleMaterial.SpriteSheet == null
				? Rectangle.One : EmitterData.ParticleMaterial.SpriteSheet.UVs[0];
			particles[freeSpot].Rotation = EmitterData.StartRotation.Start.GetRandomValue();
			particles[freeSpot].Material = EmitterData.ParticleMaterial;
			particles[freeSpot].Vertices = Particle3D.GetVertices(EmitterData.Size.Start,
				EmitterData.Color.Start);
		}

		private int FindFreeSpot()
		{
			for (int index = 0; index < NumberOfActiveParticles; index++)
				if (particles[index].ElapsedTime >= EmitterData.LifeTime)
					return index;
			return NumberOfActiveParticles < EmitterData.MaximumNumberOfParticles
				? NumberOfActiveParticles++ : -1;
		}

		protected abstract Vector3D GetParticleSpawnPosition();

		public void SpawnBurst(int numberOfParticles, bool destroyAfterwards = false)
		{
			if (particles == null || particles.Length != EmitterData.MaximumNumberOfParticles)
				CreateParticlesArray();
			for (int i = 0; i < numberOfParticles; i++)
				SpawnOneParticle();
			if (destroyAfterwards)
				DisposeAfterSeconds(EmitterData.LifeTime);
		}

		public void SetForce(RangeGraph<Vector3D> forceRange)
		{
			EmitterData.Acceleration = forceRange;
		}
	}
}