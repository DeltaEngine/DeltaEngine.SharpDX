using System;
using System.Collections.Generic;
using DeltaEngine.Entities;

namespace DeltaEngine.Rendering3D.Particles
{
	public abstract class ParticleEmitter : DrawableEntity, Updateable
	{
		public void Update()
		{
			if (IsActive)
				UpdateEmitterAndParticles();
		}

		public class UnableToCreateWithoutMaterial : Exception {}

		protected void UpdateEmitterAndParticles()
		{
			UpdateAndLimitNumberOfActiveParticles();
			UpdateAnimation();
			SpawnNewParticles();
		}

		protected abstract void UpdateAndLimitNumberOfActiveParticles();
		protected abstract void UpdateAnimation();
		protected abstract void SpawnNewParticles();

		public float ElapsedSinceLastSpawn { get; set; }

		public const int MaxParticles = 1024;

		protected class MaximumNumberOfParticlesExceeded : Exception
		{
			public MaximumNumberOfParticlesExceeded(int specified, int maxAllowed)
				: base("Specified=" + specified + ", Maximum allowed=" + maxAllowed) {}
		}

		public int NumberOfActiveParticles { get; protected set; }

		public void DisposeAfterSeconds(float remainingSeconds)
		{
			Add(new Duration(remainingSeconds));
			Start<SelfDestructTimer>();
		}

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

		public bool IsPauseable { get { return true; } }
	}
}