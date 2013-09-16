using DeltaEngine.Core;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering.Particles
{
	public class Particle3DSphericalEmitter : Particle3DEmitter
	{
		public Particle3DSphericalEmitter(ParticleEmitterData data, Vector center, float radius)
			: base(data, center)
		{
			this.radius = radius;
		}

		private readonly float radius;

		protected override Vector GetParticleSpawnPosition()
		{
			var direction = new Vector(Randomizer.Current.Get(-1.0f, 1.0f),
				Randomizer.Current.Get(-1.0f, 1.0f), Randomizer.Current.Get(-1.0f, 1.0f));
			direction.Normalize();
			direction *= radius;
			return Position + direction;
		}
	}
}