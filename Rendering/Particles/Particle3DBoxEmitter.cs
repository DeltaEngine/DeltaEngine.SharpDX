using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering.Particles
{
	public class Particle3DBoxEmitter : Particle3DEmitter
	{
		public Particle3DBoxEmitter(ParticleEmitterData data, Range<Vector> diagonal)
			: base(data, Vector.Zero)
		{
			xRange = new ValueRange(diagonal.Start.X, diagonal.End.X);
			yRange = new ValueRange(diagonal.Start.Y, diagonal.End.Y);
			zRange = new ValueRange(diagonal.Start.Z, diagonal.End.Z);
		}

		private ValueRange xRange;
		private ValueRange yRange;
		private ValueRange zRange;

		protected override Vector GetParticleSpawnPosition()
		{
			return new Vector(xRange.GetRandomValue(), yRange.GetRandomValue(), zRange.GetRandomValue());
		}
	}
}