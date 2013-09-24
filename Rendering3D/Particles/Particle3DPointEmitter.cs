using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering3D.Particles
{
	public class Particle3DPointEmitter : Particle3DEmitter
	{
		public Particle3DPointEmitter(ParticleEmitterData data, Vector3D spawnPosition)
			: base(data, spawnPosition) {}

		protected override Vector3D GetParticleSpawnPosition()
		{
			return Position;
		}
	}
}