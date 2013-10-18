using DeltaEngine.Core;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering2D.Particles
{
	public class Particle2DTorusEmitter : ParticleEmitter
	{
		public Particle2DTorusEmitter(ParticleEmitterData emitterData, Vector3D center, float radius)
			: base(emitterData, center)
		{
			this.radius = radius;
			ParticleMovement = TypeOfMovement.Normal;
		}

		private readonly float radius;
		public TypeOfMovement ParticleMovement;

		protected override Vector3D GetParticleSpawnPosition2D()
		{
			var point = new Vector3D(Randomizer.Current.Get(-1.0f), Randomizer.Current.Get(-1.0f), 0.0f);
			point.Normalize();
			point *= radius;
			return (Rotation.Equals(Quaternion.Identity))
				? Position + point
				: Position + point.Transform(Rotation);
		}

		protected override bool UpdateParticle(int index)
		{
			if (ParticleMovement == TypeOfMovement.Normal)
				return base.UpdateParticle(index);
			if (ParticleMovement == TypeOfMovement.Escaping)
				return particles[index].UpdateEscapingParticleIfStillActive(EmitterData, Position);
			return ParticleMovement == TypeOfMovement.AroundCenter && 
				particles[index].UpdateRoundingParticleIfStillActive(EmitterData, Position);
		}
	}

	public enum TypeOfMovement
	{
		Normal,
		AroundCenter,
		Escaping
	}
}
