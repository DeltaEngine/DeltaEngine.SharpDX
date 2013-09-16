using DeltaEngine.Datatypes;
using NUnit.Framework;

namespace DeltaEngine.Tests.Datatypes
{
	public class BoundingSphereTests
	{
		[Test]
		public void CreateBoundingSphere()
		{
			var boundingSphere = new BoundingSphere(Vector.One, 2.0f);
			Assert.AreEqual(Vector.One, boundingSphere.Center);
			Assert.AreEqual(2.0f, boundingSphere.Radius);
		}
	}
}