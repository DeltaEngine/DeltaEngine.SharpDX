using System;
using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace DeltaEngine.Rendering3D.Particles
{
	public class ParticleEmitter : DrawableEntity, Updateable
	{
		public ParticleEmitter(ParticleEmitterData emitterData, Vector3D spawnPosition)
		{
			EmitterData = ParticleEmitterData.CopyFrom(emitterData);
			ElapsedSinceLastSpawn = emitterData.SpawnInterval;
			if (emitterData.ParticleMaterial == null)
			{
				base.IsActive = false;
				throw new UnableToCreateWithoutMaterial();
			}
			Position = spawnPosition;
			Rotation = Quaternion.Identity;
			lastFramePosition = spawnPosition;
			DecideRendererType(emitterData);
		}

		public Vector3D Position { get; set; }
		public Quaternion Rotation { get; set; }
		private Vector3D lastFramePosition;
		public ParticleEmitterData EmitterData { get; private set; }
		public float ElapsedSinceLastSpawn { get; set; }

		public class UnableToCreateWithoutMaterial : Exception {}

		private void DecideRendererType(ParticleEmitterData emitterData)
		{
			if (emitterData.BillboardMode == BillboardMode.Standard2D)
				OnDraw<Particle2DRenderer>();
			else
				OnDraw<Particle3DRenderer>();
		}

		public void Update()
		{
			UpdateAndLimitNumberOfActiveParticles();
			UpdateAnimation();
			SpawnNewParticles();
		}

		private void UpdateAndLimitNumberOfActiveParticles()
		{
			int lastIndex = -1;
			for (int index = 0; index < NumberOfActiveParticles; index++)
				if (particles[index].UpdateIfStillActive(EmitterData))
				{
					lastIndex = index;
					UpdateParticleProperties(index);
				}
			NumberOfActiveParticles = lastIndex + 1;
			lastFramePosition = Position;
		}

		public Particle[] particles;
		public int NumberOfActiveParticles { get; protected set; }

		private void UpdateParticleProperties(int index)
		{
			var interpolation = particles[index].ElapsedTime / EmitterData.LifeTime;
			particles[index].Color = EmitterData.Color.GetInterpolatedValue(interpolation);
			particles[index].Size = EmitterData.Size.GetInterpolatedValue(interpolation);
			var acceleration = EmitterData.Acceleration.GetInterpolatedValue(interpolation);
			particles[index].Acceleration = EmitterData.BillboardMode == BillboardMode.Standard2D
				? acceleration.GetVector2D() : acceleration;
			if (EmitterData.DoParticlesTrackEmitter)
				particles[index].Position += Position - lastFramePosition;
		}

		private void UpdateAnimation()
		{
			if (EmitterData.ParticleMaterial.Animation != null)
				UpdateAnimationForParticles();
			if (EmitterData.ParticleMaterial.SpriteSheet != null)
				UpdateSpriteSheetAnimationForParticles();
		}

		private void UpdateAnimationForParticles()
		{
			var animation = EmitterData.ParticleMaterial.Animation;
			var duration = EmitterData.ParticleMaterial.Duration;
			for (int index = 0; index < NumberOfActiveParticles; index++)
			{
				particles[index].CurrentFrame =
					(int)(animation.Frames.Length * particles[index].ElapsedTime / duration) %
						animation.Frames.Length;
				particles[index].Material.DiffuseMap = animation.Frames[particles[index].CurrentFrame];
			}
		}

		private void UpdateSpriteSheetAnimationForParticles()
		{
			var sheet = EmitterData.ParticleMaterial.SpriteSheet;
			var duration = EmitterData.ParticleMaterial.Duration;
			for (int index = 0; index < NumberOfActiveParticles; index++)
			{
				particles[index].CurrentFrame =
					(int)(sheet.UVs.Count * particles[index].ElapsedTime / duration) % sheet.UVs.Count;
				particles[index].CurrentUV = sheet.UVs[particles[index].CurrentFrame];
			}
		}

		private void SpawnNewParticles()
		{
			ElapsedSinceLastSpawn += Time.Delta;
			if (EmitterData.SpawnInterval <= 0.0f || IsAnyPhysicsNull())
				return;
			while (ElapsedSinceLastSpawn >= EmitterData.SpawnInterval)
				DoIntervalSpawn();
		}

		private bool IsAnyPhysicsNull()
		{
			return EmitterData.Acceleration == null || EmitterData.Size == null ||
				EmitterData.StartVelocity == null || EmitterData.StartPosition == null;
		}

		private void DoIntervalSpawn()
		{
			ElapsedSinceLastSpawn -= EmitterData.SpawnInterval;
			var numberOfParticles = (int)EmitterData.ParticlesPerSpawn.Start.GetRandomValue();
			Spawn(numberOfParticles);
		}

		public void Spawn(int numberOfParticles = 1)
		{
			CreateParticleArrayIfNecessary();
			for (int i = 0; i < numberOfParticles; i++)
				SpawnOneParticle();
		}

		private void CreateParticleArrayIfNecessary()
		{
			if (particles != null && particles.Length == EmitterData.MaximumNumberOfParticles)
				return;
			VerifyNumberOfParticlesDoesNotExceedMaximumAllowed();
			particles = new Particle[EmitterData.MaximumNumberOfParticles];
			Set(particles);
		}

		protected void VerifyNumberOfParticlesDoesNotExceedMaximumAllowed()
		{
			if (EmitterData.MaximumNumberOfParticles > MaxParticles)
				throw new MaximumNumberOfParticlesExceeded(EmitterData.MaximumNumberOfParticles,
					MaxParticles);
		}

		public const int MaxParticles = 1024;

		public class MaximumNumberOfParticlesExceeded : Exception
		{
			public MaximumNumberOfParticlesExceeded(int specified, int maxAllowed)
				: base("Specified=" + specified + ", Maximum allowed=" + maxAllowed) {}
		}

		private void SpawnOneParticle()
		{
			int freeSpot = FindFreeSpot();
			if (freeSpot < 0)
				return;
			particles[freeSpot].IsActive = true;
			particles[freeSpot].ElapsedTime = 0;
			Vector3D position = EmitterData.BillboardMode == BillboardMode.Standard2D
				? GetParticleSpawnPosition2D() : GetParticleSpawnPosition3D();
			particles[freeSpot].Position = position;
			particles[freeSpot].SetStartVelocityRandomizedFromRange(EmitterData.StartVelocity.Start,
				EmitterData.StartVelocity.End);
			particles[freeSpot].Acceleration = EmitterData.Acceleration.Start;
			particles[freeSpot].Size = EmitterData.Size.Start;
			particles[freeSpot].Color = EmitterData.Color.Start;
			particles[freeSpot].CurrentUV = EmitterData.ParticleMaterial.SpriteSheet == null
				? Rectangle.One : EmitterData.ParticleMaterial.SpriteSheet.UVs[0];
			particles[freeSpot].Rotation = EmitterData.StartRotation.Start.GetRandomValue();
			particles[freeSpot].Material = EmitterData.ParticleMaterial;
			particles[freeSpot].BillboardMode = EmitterData.BillboardMode;
		}

		private int FindFreeSpot()
		{
			for (int index = 0; index < NumberOfActiveParticles; index++)
				if (!particles[index].IsActive)
					return index; //ncrunch: no coverage
			return NumberOfActiveParticles < EmitterData.MaximumNumberOfParticles
				? NumberOfActiveParticles++ : -1;
		}

		private Vector3D GetParticleSpawnPosition2D()
		{
			return (Rotation.Equals(Quaternion.Identity))
				? Position + EmitterData.StartPosition.GetRandomValue()
				: Position + EmitterData.StartPosition.GetRandomValue().Transform(Rotation);
		}

		//ncrunch: no coverage start
		protected virtual Vector3D GetParticleSpawnPosition3D()
		{
			return Vector3D.Zero;
		}

		//ncrunch: no coverage end

		public void SetAcceleration(RangeGraph<Vector3D> accelerationRange)
		{
			EmitterData.Acceleration = accelerationRange;
		}

		public void SpawnAndDispose(int numberOfParticles = 1)
		{
			Spawn(numberOfParticles);
			DisposeAfterSeconds(EmitterData.LifeTime);
		}

		public void DisposeAfterSeconds(float remainingSeconds)
		{
			if (IsDisposing)
				return;
			Add(new Duration(remainingSeconds));
			Start<SelfDestructTimer>();
			IsDisposing = true;
		}

		internal bool IsDisposing { get; private set; }

		protected class SelfDestructTimer : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (var entity in entities)
				{
					var duration = entity.Get<Duration>();
					duration.Elapsed += Time.Delta;
					if (duration.Elapsed > duration.Value)
						entity.IsActive = false;
					entity.Set(duration);
				}
			}
		}

		internal struct Duration
		{
			public Duration(float duration)
				: this()
			{
				Value = duration;
			}

			public float Value { get; private set; }
			public float Elapsed { get; internal set; }
		}

		public bool IsPauseable
		{
			get { return true; }
		}
	}
}