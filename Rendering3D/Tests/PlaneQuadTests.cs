using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D.Cameras;
using NUnit.Framework;

namespace DeltaEngine.Rendering3D.Tests
{
	internal class PlaneQuadTests : TestWithMocksOrVisually
	{
		[Test]
		public void DrawRedPlane()
		{
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = Vector3D.One;
			PlaneQuad quad = CreatePlaneQuad();
			new Model(new ModelData(quad), Vector3D.Zero);
		}

		private static PlaneQuad CreatePlaneQuad()
		{
			var material = new Material(Shader.Position3DColorUV, "DeltaEngineLogo");
			material.DefaultColor = Color.Red;
			return new PlaneQuad(Size, material);
		}

		private static readonly Size Size = new Size(2);

		[Test]
		public void DrawUncoloredPlane()
		{
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = Vector3D.One;
			new Model(new ModelData(CreateUncoloredPlaneQuad()), Vector3D.Zero);
		}

		private static PlaneQuad CreateUncoloredPlaneQuad()
		{
			var material = new Material(Shader.Position3DUV, "DeltaEngineLogo");
			return new PlaneQuad(Size.One, material);
		}

		[Test, CloseAfterFirstFrame]
		public void ChangeSize()
		{
			PlaneQuad quad = CreatePlaneQuad();
			Assert.AreEqual(Size, quad.Size);
			quad.Size = 2 * Size;
			Assert.AreEqual(2 * Size, quad.Size);
		}
	}
}