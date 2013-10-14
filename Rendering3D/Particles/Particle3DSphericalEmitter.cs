using DeltaEngine.Core;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering3D.Particles
{
	public class Particle3DSphericalEmitter : ParticleEmitter
	{
		public Particle3DSphericalEmitter(ParticleEmitterData data, Vector3D center, float radius)
			: base(data, center)
		{
			this.radius = radius;
		}

		private readonly float radius;

		protected override Vector3D GetParticleSpawnPosition3D()
		{
			var direction = new Vector3D(Randomizer.Current.Get(-1.0f),
				Randomizer.Current.Get(-1.0f), Randomizer.Current.Get(-1.0f));
			direction.Normalize();
			direction *= radius;
			return Position + direction;
		}
	}
}