using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering3D.Particles
{
	public class Particle3DLineEmitter : Particle3DEmitter
	{
		public Particle3DLineEmitter(ParticleEmitterData data, Range<Vector3D> line)
			: base(data, Vector3D.Zero)
		{
			this.line = line;
		}

		private readonly Range<Vector3D> line;

		protected override Vector3D GetParticleSpawnPosition()
		{
			return line.GetRandomValue();
		}
	}
}