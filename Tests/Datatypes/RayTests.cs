using DeltaEngine.Datatypes;
using NUnit.Framework;

namespace DeltaEngine.Tests.Datatypes
{
	public class RayTests
	{
		[Test]
		public void EqualityOfRay()
		{
			Assert.AreEqual(new Ray(Vector.UnitZ, Vector.One), new Ray(Vector.UnitZ, Vector.One));
			Assert.AreNotEqual(new Ray(Vector.UnitX, Vector.One), new Ray(Vector.UnitZ, Vector.One));
			Assert.AreNotEqual(new Ray(Vector.UnitZ, Vector.One), new Ray(Vector.UnitZ, Vector.One * 2));
		}

		[Test]
		public void CreateRay()
		{
			var ray = new Ray(Vector.Zero, Vector.UnitZ);
			Assert.AreEqual(ray.Origin, Vector.Zero);
			Assert.AreEqual(ray.Direction, Vector.UnitZ);
		}

		[Test]
		public void RayToString()
		{
			Assert.AreEqual("Origin [1, 0, 0] Direction [0, 1, 0]",
				new Ray(Vector.UnitX, Vector.UnitY).ToString());
		}
	}
}