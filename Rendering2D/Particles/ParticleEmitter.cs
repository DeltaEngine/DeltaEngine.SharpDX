using System;
using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;

namespace DeltaEngine.Rendering2D.Particles
{
	public class ParticleEmitter : DrawableEntity, Updateable
	{
		public ParticleEmitter(ParticleEmitterData emitterData, Vector3D spawnPosition)
		{
			EmitterData = ParticleEmitterData.CopyFrom(emitterData);
			ElapsedSinceLastSpawn = emitterData.SpawnInterval;
			if (emitterData.ParticleMaterial == null)
			{
				base.Dispose();
				throw new UnableToCreateWithoutMaterial();
			}
			Position = spawnPosition;
			Rotation = Quaternion.Identity;
			lastFramePosition = spawnPosition;
			CreateImageAnimationMaterials();
			OnDraw<Particle2DRenderer>();
		}

		public ParticleEmitterData EmitterData { get; private set; }
		public float ElapsedSinceLastSpawn { get; set; }
		public Vector3D Position { get; set; }
		public Quaternion Rotation { get; set; }
		private Vector3D lastFramePosition;

		public class UnableToCreateWithoutMaterial : Exception {}

		private void CreateImageAnimationMaterials()
		{
			ImageAnimation animation = EmitterData.ParticleMaterial.Animation;
			if (animation == null)
				return;
			animationMaterials = new Material[animation.Frames.Length];
			for (int i = 0; i < animation.Frames.Length; i++)
				animationMaterials[i] = new Material(EmitterData.ParticleMaterial.Shader,
					animation.Frames[i], animation.Frames[i].PixelSize);
		}

		private Material[] animationMaterials;

		public void Update()
		{
			UpdateAndLimitNumberOfActiveParticles();
			UpdateAnimation();
			SpawnNewParticles();
		}

		private void UpdateAndLimitNumberOfActiveParticles()
		{
			if (EmitterData.PositionType == ParticleEmitterPositionType.CircleAroundCenter)
			{
				UpdateParticlesTracingCircularOutline();
				return;
			}
			if (EmitterData.PositionType == ParticleEmitterPositionType.CircleEscaping)
			{
				UpdateParticlesRadialEscape();
				return;
			}
			UpdateParticlesBasic();
		}

		public Particle[] particles;

		private void UpdateParticlesTracingCircularOutline()
		{
			int lastIndex = -1;
			for (int index = 0; index < NumberOfActiveParticles; index++)
				if (particles[index].UpdateRoundingParticleIfStillActive(EmitterData, Position))
				{
					lastIndex = index;
					UpdateParticleProperties(index);
				}
			NumberOfActiveParticles = lastIndex + 1;
			lastFramePosition = Position;
		}

		private void UpdateParticlesRadialEscape()
		{
			int lastIndex = -1;
			for (int index = 0; index < NumberOfActiveParticles; index++)
				if (particles[index].UpdateEscapingParticleIfStillActive(EmitterData, Position))
				{
					lastIndex = index;
					UpdateParticleProperties(index);
				}
			NumberOfActiveParticles = lastIndex + 1;
			lastFramePosition = Position;
		}

		private void UpdateParticlesBasic()
		{
			int lastIndex = -1;
			for (int index = 0; index < NumberOfActiveParticles; index++)
				if (UpdateParticle(index))
				{
					lastIndex = index;
					UpdateParticleProperties(index);
				}
			NumberOfActiveParticles = lastIndex + 1;
			lastFramePosition = Position;
		}

		public int NumberOfActiveParticles { get; protected set; }

		private bool UpdateParticle(int index)
		{
			return particles[index].UpdateIfStillActive(EmitterData);
		}

		private void UpdateParticleProperties(int index)
		{
			var interpolation = particles[index].ElapsedTime / EmitterData.LifeTime;
			particles[index].Color = EmitterData.Color.GetInterpolatedValue(interpolation);
			particles[index].Size = EmitterData.Size.GetInterpolatedValue(interpolation);
			var acceleration = EmitterData.Acceleration.GetInterpolatedValue(interpolation);
			particles[index].Acceleration = acceleration.GetVector2D();
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
				particles[index].Material = animationMaterials[particles[index].CurrentFrame];
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
			if (EmitterData.SpawnInterval <= 0.0f || IsAnyEmitterDataNull())
				return;
			while (ElapsedSinceLastSpawn >= EmitterData.SpawnInterval)
				DoIntervalSpawn();
		}

		private bool IsAnyEmitterDataNull()
		{
			return EmitterData.Acceleration == null || EmitterData.Size == null ||
				EmitterData.StartVelocity == null || EmitterData.StartPosition == null ||
				EmitterData.ParticleMaterial.DiffuseMap == null;
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
			lastFreeSpot = -1;
			for (int i = 0; i < numberOfParticles; i++)
				SpawnOneParticle();
		}

		private int lastFreeSpot;

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


		private void SpawnOneParticle()
		{
			int freeSpot = FindFreeSpot();
			if (freeSpot < 0)
				return;
			particles[freeSpot].IsActive = true;
			particles[freeSpot].ElapsedTime = 0;
			Vector3D position = GetParticleSpawnPosition();
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
		}

		public class MaximumNumberOfParticlesExceeded : Exception
		{
			public MaximumNumberOfParticlesExceeded(int specified, int maxAllowed)
				: base("Specified=" + specified + ", Maximum allowed=" + maxAllowed) {}
		}


		private int FindFreeSpot()
		{
			for (int index = lastFreeSpot + 1; index < NumberOfActiveParticles; index++)
				if (!particles[index].IsActive)
					return lastFreeSpot = index; //ncrunch: no coverage
			if (NumberOfActiveParticles < EmitterData.MaximumNumberOfParticles)
				return lastFreeSpot = NumberOfActiveParticles++;
			lastFreeSpot = NumberOfActiveParticles;
			return -1;
		}

		protected virtual Vector3D GetParticleSpawnPosition()
		{
			switch (EmitterData.PositionType)
			{
			case ParticleEmitterPositionType.Point:
				return GetSpawnPositionPoint();
			case ParticleEmitterPositionType.Line:
				return GetSpawnPositionLine();
			case ParticleEmitterPositionType.Box:
				return GetSpawnPositionBox();
			case ParticleEmitterPositionType.Sphere:
				return GetSpawnPositionSphere();
			case ParticleEmitterPositionType.SphereBorder:
				return GetSpawnPositionSphereBorder();
			case ParticleEmitterPositionType.CircleAroundCenter:
			case ParticleEmitterPositionType.CircleEscaping:
				return GetSpawnPositionCircleOutline();
			default:
				return GetSpawnPositionPoint();
			}
		}

		protected Vector3D GetSpawnPositionPoint()
		{
			return (Rotation.Equals(Quaternion.Identity))
				? Position + EmitterData.StartPosition.Start
				: Position + EmitterData.StartPosition.Start.Transform(Rotation);
		}

		protected Vector3D GetSpawnPositionLine()
		{
			return (Rotation.Equals(Quaternion.Identity))
				? Position + EmitterData.StartPosition.GetRandomValue()
				: Position + EmitterData.StartPosition.GetRandomValue().Transform(Rotation);
		}

		protected Vector3D GetSpawnPositionBox()
		{
			var insideTheBox =
				new Vector3D(
					EmitterData.StartPosition.Start.X.Lerp(EmitterData.StartPosition.End.X,
						Randomizer.Current.Get()),
					EmitterData.StartPosition.Start.Y.Lerp(EmitterData.StartPosition.End.Y,
						Randomizer.Current.Get()),
					EmitterData.StartPosition.Start.Z.Lerp(EmitterData.StartPosition.End.Z,
						Randomizer.Current.Get()));
			return (Rotation.Equals(Quaternion.Identity))
				? Position + insideTheBox : Position + insideTheBox.Transform(Rotation);
		}

		protected Vector3D GetSpawnPositionSphere()
		{
			var insideSphere =
				new Vector3D(
					EmitterData.StartPosition.Start.X.Lerp(EmitterData.StartPosition.End.X,
						Randomizer.Current.Get()),
					EmitterData.StartPosition.Start.Y.Lerp(EmitterData.StartPosition.End.Y,
						Randomizer.Current.Get()),
					EmitterData.StartPosition.Start.Z.Lerp(EmitterData.StartPosition.End.Z,
						Randomizer.Current.Get()));
			insideSphere.Normalize();
			insideSphere *=
				0.0f.Lerp(EmitterData.StartPosition.Start.Distance(EmitterData.StartPosition.End) * 0.5f,
					Randomizer.Current.Get());
			return Position + insideSphere;
		}

		protected Vector3D GetSpawnPositionSphereBorder()
		{
			var onSphereOutline =
				new Vector3D(
					EmitterData.StartPosition.Start.X.Lerp(EmitterData.StartPosition.End.X,
						Randomizer.Current.Get()),
					EmitterData.StartPosition.Start.Y.Lerp(EmitterData.StartPosition.End.Y,
						Randomizer.Current.Get()),
					EmitterData.StartPosition.Start.Z.Lerp(EmitterData.StartPosition.End.Z,
						Randomizer.Current.Get()));
			onSphereOutline.Normalize();
			onSphereOutline *= EmitterData.StartPosition.Start.Distance(EmitterData.StartPosition.End) *
				0.5f;
			return Position + onSphereOutline;
		}

		private Vector3D GetSpawnPositionCircleOutline()
		{
			var startPosition = EmitterData.StartPosition;
			var onCircleOutline = new Vector3D( 
				startPosition.Start.X + Randomizer.Current.Get(-1.0f) * startPosition.End.X,
				startPosition.Start.Y + Randomizer.Current.Get(-1.0f) * startPosition.End.Y, 0.0f);
			onCircleOutline.Normalize();
			var diameter = Math.Max(startPosition.Start.Length, startPosition.End.Length);
			onCircleOutline *= diameter * 0.5f;
			return (Rotation.Equals(Quaternion.Identity))
				? Position + onCircleOutline : Position + onCircleOutline.Transform(Rotation);
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
						entity.Dispose();
					entity.Set(duration);
				}
			}
		}

		internal struct Duration
		{
			public Duration(float duration) : this()
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