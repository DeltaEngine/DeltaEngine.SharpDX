using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Cameras;
using DeltaEngine.Rendering.Models;
using NUnit.Framework;

namespace DeltaEngine.Rendering.Sprites.Tests
{
	internal class PlaneQuadTests : TestWithMocksOrVisually
	{
		[Test]
		public void DrawPlane()
		{
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = Vector.One;
			new Model(new ModelData(CreatePlaneQuad()), Vector.Zero);
		}

		private static PlaneQuad CreatePlaneQuad()
		{
			var material = new Material(Shader.Position3DColorUv, "DeltaEngineLogo");
			material.DefaultColor = Color.Red;
			return new PlaneQuad(Size.Half, material);
		}
	}
}
