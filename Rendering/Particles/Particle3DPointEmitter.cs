using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering.Particles
{
	public class Particle3DPointEmitter : Particle3DEmitter
	{
		public Particle3DPointEmitter(ParticleEmitterData data, Vector spawnPosition)
			: base(data, spawnPosition) {}

		protected override Vector GetParticleSpawnPosition()
		{
			return Position;
		}
	}
}