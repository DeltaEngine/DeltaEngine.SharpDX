using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;

namespace DeltaEngine.Rendering.Particles
{
	public struct ValueRange : Lerp<ValueRange>
	{
		public ValueRange(float minimum, float maximum)
			: this()
		{
			Start = minimum;
			End = maximum;
		}

		public float Start { get; set; }
		public float End { get; set; }

		public float GetRandomValue()
		{
			return Start.Lerp(End, Randomizer.Current.Get());
		}

		public ValueRange Lerp(ValueRange other, float interpolation)
		{
			return new ValueRange(Start.Lerp(other.Start,interpolation),End.Lerp(other.End, interpolation));
		}
	}
}
