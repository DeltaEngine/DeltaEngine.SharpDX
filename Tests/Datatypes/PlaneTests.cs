using DeltaEngine.Datatypes;
using NUnit.Framework;

namespace DeltaEngine.Tests.Datatypes
{
	public class PlaneTests
	{
		[Test]
		public void EqualityOfPlanes()
		{
			const float Distance = 4.0f;
			Assert.AreEqual(new Plane(Vector.UnitZ, Distance), new Plane(Vector.UnitZ, Distance));
			Assert.AreNotEqual(new Plane(Vector.UnitZ, Distance), new Plane(Vector.UnitZ, 1));
			Assert.AreNotEqual(new Plane(Vector.UnitZ, Distance), new Plane(Vector.UnitX, Distance));
		}

		[Test]
		public void CreatePlaneFromDistance()
		{
			var plane = new Plane(Vector.UnitY, 1.0f);
			Assert.AreEqual(Vector.UnitY, plane.Normal);
			Assert.AreEqual(1.0f, plane.Distance);
		}

		[Test]
		public void CreatePlaneFromPointOnPlane()
		{
			var plane = new Plane(Vector.UnitY, new Vector(0, 1, 0));
			Assert.AreEqual(Vector.UnitY, plane.Normal);
			Assert.AreEqual(-1.0f, plane.Distance);
		}

		[Test]
		public void RayPlaneIntersect()
		{
			VerifyIntersectPoint(new Ray(Vector.UnitZ, -Vector.UnitZ), new Plane(Vector.UnitZ, 3.0f),
				-Vector.UnitZ * 3.0f);
			VerifyIntersectPoint(new Ray(3 * Vector.One, -Vector.One),
				new Plane(Vector.UnitY, Vector.One), Vector.One);
		}

		private static void VerifyIntersectPoint(Ray ray, Plane plane, Vector expectedIntersect)
		{
			Assert.AreEqual(expectedIntersect, plane.Intersect(ray));
		}

		[Test]
		public void RayPointingAwayFromPlaneDoesntIntersect()
		{
			var ray = new Ray(3 * Vector.One, Vector.One);
			var plane = new Plane(Vector.UnitY, Vector.One);
			Assert.IsNull(plane.Intersect(ray));
		}

		[Test]
		public void RayParallelToPlaneDoesntIntersect()
		{
			var ray = new Ray(Vector.One, Vector.UnitZ);
			var plane = new Plane(Vector.UnitY, Vector.Zero);
			Assert.IsNull(plane.Intersect(ray));
		}
	}
}