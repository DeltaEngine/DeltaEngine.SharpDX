using DeltaEngine.Datatypes;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Cameras;
using DeltaEngine.Rendering.Models;
using NUnit.Framework;

namespace DeltaEngine.Rendering.Shapes3D.Tests
{
	public class BoxTests : TestWithMocksOrVisually
	{
		[Test]
		public void ShowRedBox()
		{
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = Vector.One;
			new Model(new ModelData(new Box(Vector.One, Color.Red)), Vector.Zero);
		}

		[Test, CloseAfterFirstFrame]
		public void CreateBox()
		{
			var box = new Box(Vector.One, Color.Red);
			Assert.AreEqual(8, box.Geometry.NumberOfVertices);
			Assert.AreEqual(36, box.Geometry.NumberOfIndices);
			Assert.AreEqual(VertexFormat.Position3DColor,
				(box.Material.Shader as ShaderWithFormat).Format);
		}
	}
}