using DeltaEngine.Datatypes;
using NUnit.Framework;

namespace DeltaEngine.Tests.Datatypes
{
	public class BoundingBoxTests
	{
		[Test]
		public void CreateBoundingBox()
		{
			var boundingBox = new BoundingBox(Vector.Zero, Vector.One);
			Assert.AreEqual(Vector.Zero, boundingBox.Min);
			Assert.AreEqual(Vector.One, boundingBox.Max);
		}
	}
}
