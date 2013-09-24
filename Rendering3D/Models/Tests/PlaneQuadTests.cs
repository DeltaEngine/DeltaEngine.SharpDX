using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D.Cameras;
using NUnit.Framework;

namespace DeltaEngine.Rendering3D.Models.Tests
{
	internal class PlaneQuadTests : TestWithMocksOrVisually
	{
		[Test]
		public void DrawPlane()
		{
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = Vector3D.One;
			new Model(new ModelData(CreatePlaneQuad()), Vector3D.Zero);
		}

		private static PlaneQuad CreatePlaneQuad()
		{
			var material = new Material(Shader.Position3DColorUv, "DeltaEngineLogo");
			material.DefaultColor = Color.Red;
			return new PlaneQuad(Size.Half, material);
		}
	}
}
