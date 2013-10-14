using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Rendering3D.Tests
{
	public class MeshTests : TestWithMocksOrVisually
	{
		[Test]
		public void CreateMeshDynamically()
		{
			var mesh = new Mesh(ContentLoader.Load<Geometry>("AnyGeometry"),
				ContentLoader.Load<Material>("AnyMaterial"));
			Assert.IsNotNull(mesh.Geometry);
			Assert.IsNotNull(mesh.Material);
			Assert.AreEqual(Matrix.Identity, mesh.LocalTransform);
		}

		[Test]
		public void LoadMeshFromContent()
		{
			var mesh = ContentLoader.Load<Mesh>("AnyMeshCustomTransform");
			Assert.IsNotNull(mesh.Geometry);
			Assert.IsNotNull(mesh.Material);
			Assert.AreNotEqual(new Matrix(), mesh.LocalTransform);
			Assert.AreNotEqual(Matrix.Identity, mesh.LocalTransform);
		}
	}
}
