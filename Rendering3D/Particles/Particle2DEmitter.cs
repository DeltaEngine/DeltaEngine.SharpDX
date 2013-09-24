using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D.Sprites;

namespace DeltaEngine.Rendering3D.Particles
{
	/// <summary>
	/// Holds emitterData on how to spawn particles and currently existant ones.
	/// </summary>
	public sealed class Particle2DEmitter : ParticleEmitter
	{
		public Particle2DEmitter(ParticleEmitterData emitterData, Vector2D spawnPosition)
		{
			EmitterData = emitterData;
			Position = spawnPosition;
			ElapsedSinceLastSpawn = emitterData.SpawnInterval;
			if (emitterData.ParticleMaterial == null)
			{
				IsActive = false;
				throw new UnableToCreateWithoutMaterial();
			}
			OnDraw<Particle2DRenderer>();
			if (emitterData.ParticleMaterial.Animation != null)
				Start<UpdateImageAnimation>();
			if (emitterData.ParticleMaterial.SpriteSheet != null)
				Start<UpdateSpriteSheetAnimation>();
		}

		public ParticleEmitterData EmitterData { get; private set; }
		public Vector2D Position { get; set; }

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
			particles[index].Acceleration =
				EmitterData.Acceleration.GetInterpolatedValue(interpolation).GetVector2D();
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
			particles[index].Image = animationData.Frames[particles[index].CurrentFrame];
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
			if (EmitterData.SpawnInterval <= 0.0f || !CheckIfNothingIsNull())
				return;
			while (ElapsedSinceLastSpawn >= EmitterData.SpawnInterval)
			{
				DoIntervalSpawn();
			}
		}

		public Particle2D[] particles;

		public void CreateParticlesArray()
		{
			if (EmitterData.MaximumNumberOfParticles > MaxParticles)
				throw new MaximumNumberOfParticlesExceeded(EmitterData.MaximumNumberOfParticles,
					MaxParticles);
			particles = new Particle2D[EmitterData.MaximumNumberOfParticles];
			Set(particles);
		}

		private bool CheckIfNothingIsNull()
		{
			if (EmitterData.Acceleration == null || EmitterData.Size == null ||
				EmitterData.StartVelocity == null || EmitterData.StartPosition == null)
				return false;
			return true;
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
			particles[freeSpot].Position = Position +
				EmitterData.StartPosition.GetRandomValue().GetVector2D();
			particles[freeSpot].SetStartVelocityRandomizedFromRange(
				EmitterData.StartVelocity.Start.GetVector2D(), EmitterData.StartVelocity.End.GetVector2D());
			particles[freeSpot].Acceleration = EmitterData.Acceleration.Start.GetVector2D();
			particles[freeSpot].Size = EmitterData.Size.Start;
			particles[freeSpot].Color = EmitterData.Color.Start;
			particles[freeSpot].Image = EmitterData.ParticleMaterial.DiffuseMap;
			particles[freeSpot].CurrentUV = EmitterData.ParticleMaterial.SpriteSheet == null
				? Rectangle.One : EmitterData.ParticleMaterial.SpriteSheet.UVs[0];
			particles[freeSpot].Rotation = EmitterData.StartRotation.Start.GetRandomValue();
		}

		private int FindFreeSpot()
		{
			for (int index = 0; index < NumberOfActiveParticles; index++)
				if (particles[index].ElapsedTime >= EmitterData.LifeTime)
					return index;
			return NumberOfActiveParticles < EmitterData.MaximumNumberOfParticles
				? NumberOfActiveParticles++ : -1;
		}

		public void SpawnBurst(int numberOfParticles, bool destroyAfterwards = false)
		{
			if (particles == null || particles.Length != EmitterData.MaximumNumberOfParticles)
				CreateParticlesArray();
			for (int i = 0; i < numberOfParticles; i++)
				SpawnOneParticle();
			if (destroyAfterwards)
				DisposeAfterSeconds(EmitterData.LifeTime);
		}
	}
}