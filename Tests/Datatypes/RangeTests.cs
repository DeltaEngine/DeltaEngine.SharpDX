using DeltaEngine.Datatypes;
using NUnit.Framework;

namespace DeltaEngine.Tests.Datatypes
{
	public class RangeTests
	{
		[Test]
		public void CreateEmptyRange()
		{
			var range = new Range<Point>();
			Assert.AreEqual(Point.Zero, range.Start);
			Assert.AreEqual(Point.Zero, range.End);
		}

		[Test]
		public void CreateRange()
		{
			var range = new Range<Vector>(Vector.UnitX, Vector.UnitY);
			Assert.AreEqual(Vector.UnitX, range.Start);
			Assert.AreEqual(Vector.UnitY, range.End);
		}

		[Test]
		public void ChangeRange()
		{
			var range = new Range<Vector>(Vector.UnitX, 2 * Vector.UnitX);
			range.Start = Vector.UnitY;
			range.End = 2 * Vector.UnitY;
			Assert.AreEqual(Vector.UnitY, range.Start);
			Assert.AreEqual(2 * Vector.UnitY, range.End);
		}

		[Test]
		public void GetRandomValue()
		{
			var range = new Range<Vector>(Vector.UnitX, 2 * Vector.UnitX);
			var random = range.GetRandomValue();
			Assert.IsTrue(random.X >= 1.0f && random.X <= 2.0f);
		}

		[Test]
		public void GetInterpolation()
		{
			var rangeLeft = new Range<Vector>(Vector.Zero, Vector.One);
			var rangeRight = new Range<Vector>(Vector.One, Vector.One * 3);
			var interpolation = rangeLeft.Lerp(rangeRight, 0.5f);
			var expectedInterpolation = new Range<Vector>(Vector.One * 0.5f, Vector.One * 2);
			Assert.AreEqual(expectedInterpolation.Start, interpolation.Start);
			Assert.AreEqual(expectedInterpolation.End, interpolation.End);
		}
	}
}