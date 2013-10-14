using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering3D.Particles
{
	public class Particle3DLineEmitter : ParticleEmitter
	{
		public Particle3DLineEmitter(ParticleEmitterData data, Range<Vector3D> line)
			: base(data, Vector3D.Zero)
		{
			this.line = line;
		}

		private readonly Range<Vector3D> line;

		protected override Vector3D GetParticleSpawnPosition3D()
		{
			return line.GetRandomValue().Transform(Rotation);
		}
	}
}