using System.Diagnostics.Contracts;
using DeltaEngine.Core;

namespace DeltaEngine.Datatypes
{
	/// <summary>
	/// Interval of two values; Allows a random value in between to be obtained.
	/// </summary>
	public class Range<T> : Lerp<Range<T>>
		where T : Lerp<T>
	{
		public Range() {}

		public Range(T minimum, T maximum)
		{
			// ReSharper disable DoNotCallOverridableMethodsInConstructor
			Start = minimum;
			End = maximum;
			// ReSharper restore DoNotCallOverridableMethodsInConstructor
		}

		public virtual T Start { get; set; }
		public virtual T End { get; set; }

		public T GetRandomValue()
		{
			return Start.Lerp(End, Randomizer.Current.Get());
		}

		public Range<T> Lerp(Range<T> other, float interpolation)
		{
			var start = Start.Lerp(other.Start, interpolation);
			var end = End.Lerp(other.End, interpolation);
			return new Range<T>(start, end);
		}

		[Pure]
		public override string ToString()
		{
			return GetType().Name + "<" + typeof(T).Name + ">" + "{" + Start + ", " + End + "}";
		}
	}
}