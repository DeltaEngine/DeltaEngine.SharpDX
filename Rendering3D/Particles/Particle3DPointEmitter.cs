using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering3D.Particles
{
	public class Particle3DPointEmitter : ParticleEmitter
	{
		public Particle3DPointEmitter(ParticleEmitterData data, Vector3D spawnPosition)
			: base(data, spawnPosition) {}

		protected override Vector3D GetParticleSpawnPosition3D()
		{
			return Position;
		}
	}
}