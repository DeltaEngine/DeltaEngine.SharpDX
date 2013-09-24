using DeltaEngine.Datatypes;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D.Cameras;
using DeltaEngine.Rendering3D.Models;
using NUnit.Framework;

namespace DeltaEngine.Rendering3D.Shapes3D.Tests
{
	public class BoxTests : TestWithMocksOrVisually
	{
		[Test]
		public void ShowRedBox()
		{
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = Vector3D.One;
			new Model(new ModelData(new Box(Vector3D.One, Color.Red)), Vector3D.Zero);
		}

		[Test, CloseAfterFirstFrame]
		public void CreateBox()
		{
			var box = new Box(Vector3D.One, Color.Red);
			Assert.AreEqual(8, box.Geometry.NumberOfVertices);
			Assert.AreEqual(36, box.Geometry.NumberOfIndices);
			Assert.AreEqual(VertexFormat.Position3DColor,
				(box.Material.Shader as ShaderWithFormat).Format);
		}
	}
}