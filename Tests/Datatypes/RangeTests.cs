using DeltaEngine.Datatypes;
using NUnit.Framework;

namespace DeltaEngine.Tests.Datatypes
{
	public class RangeTests
	{
		[Test]
		public void CreateEmptyRange()
		{
			var range = new Range<Vector2D>();
			Assert.AreEqual(Vector2D.Zero, range.Start);
			Assert.AreEqual(Vector2D.Zero, range.End);
		}

		[Test]
		public void CreateRange()
		{
			var range = new Range<Vector3D>(Vector3D.UnitX, Vector3D.UnitY);
			Assert.AreEqual(Vector3D.UnitX, range.Start);
			Assert.AreEqual(Vector3D.UnitY, range.End);
		}

		[Test]
		public void ChangeRange()
		{
			var range = new Range<Vector3D>(Vector3D.UnitX, 2 * Vector3D.UnitX);
			range.Start = Vector3D.UnitY;
			range.End = 2 * Vector3D.UnitY;
			Assert.AreEqual(Vector3D.UnitY, range.Start);
			Assert.AreEqual(2 * Vector3D.UnitY, range.End);
		}

		[Test]
		public void GetRandomValue()
		{
			var range = new Range<Vector3D>(Vector3D.UnitX, 2 * Vector3D.UnitX);
			var random = range.GetRandomValue();
			Assert.IsTrue(random.X >= 1.0f && random.X <= 2.0f);
		}

		[Test]
		public void GetInterpolation()
		{
			var rangeLeft = new Range<Vector3D>(Vector3D.Zero, Vector3D.One);
			var rangeRight = new Range<Vector3D>(Vector3D.One, Vector3D.One * 3);
			var interpolation = rangeLeft.Lerp(rangeRight, 0.5f);
			var expectedInterpolation = new Range<Vector3D>(Vector3D.One * 0.5f, Vector3D.One * 2);
			Assert.AreEqual(expectedInterpolation.Start, interpolation.Start);
			Assert.AreEqual(expectedInterpolation.End, interpolation.End);
		}
	}
}