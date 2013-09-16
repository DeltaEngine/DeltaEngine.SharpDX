using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering.Particles
{
	public class Particle3DLineEmitter : Particle3DEmitter
	{
		public Particle3DLineEmitter(ParticleEmitterData data, Range<Vector> line)
			: base(data, Vector.Zero)
		{
			this.line = line;
		}

		private readonly Range<Vector> line;

		protected override Vector GetParticleSpawnPosition()
		{
			return line.GetRandomValue();
		}
	}
}