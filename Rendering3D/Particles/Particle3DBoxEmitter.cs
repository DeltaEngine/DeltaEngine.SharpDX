using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering3D.Particles
{
	public class Particle3DBoxEmitter : ParticleEmitter
	{
		public Particle3DBoxEmitter(ParticleEmitterData data, Range<Vector3D> diagonal)
			: base(data, Vector3D.Zero)
		{
			xRange = new ValueRange(diagonal.Start.X, diagonal.End.X);
			yRange = new ValueRange(diagonal.Start.Y, diagonal.End.Y);
			zRange = new ValueRange(diagonal.Start.Z, diagonal.End.Z);
		}

		private ValueRange xRange;
		private ValueRange yRange;
		private ValueRange zRange;

		protected override Vector3D GetParticleSpawnPosition3D()
		{
			return new Vector3D(xRange.GetRandomValue(), yRange.GetRandomValue(), zRange.GetRandomValue());
		}
	}
}