using DeltaEngine.Datatypes;
using NUnit.Framework;

namespace DeltaEngine.Tests.Datatypes
{
	public class BoundingBoxTests
	{
		[Test]
		public void CreateBoundingBox()
		{
			var boundingBox = new BoundingBox(Vector3D.Zero, Vector3D.One);
			Assert.AreEqual(Vector3D.Zero, boundingBox.Min);
			Assert.AreEqual(Vector3D.One, boundingBox.Max);
		}

		[Test]
		public void IntersectBoundingBoxWithRay()
		{
			var boundingBox = new BoundingBox(Vector3D.One * -0.5f, Vector3D.One * 0.5f);
			var ray = new Ray(Vector3D.UnitY * 2.0f, -Vector3D.UnitY);
			Assert.AreEqual(new Vector3D(0.0f, 0.5f, 0.0f), boundingBox.Intersect(ray));
		}

		[Test]
		public void MissBoundingBoxWithRay()
		{
			var boundingBox = new BoundingBox(Vector3D.One * -0.5f, Vector3D.One * 0.5f);
			var ray = new Ray(Vector3D.UnitZ * 2.0f, -Vector3D.UnitY);
			Assert.IsNull(boundingBox.Intersect(ray));
		}

		[Test]
		public void MissBoundingBoxWithRayFromBehind()
		{
			var boundingBox = new BoundingBox(Vector3D.One * -0.5f, Vector3D.One * 0.5f);
			var ray = new Ray(Vector3D.UnitY * 2.0f, Vector3D.UnitY);
			Assert.IsNull(boundingBox.Intersect(ray));
		}
	}
}
