using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.Mocks;
using DeltaEngine.Platforms;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace DeltaEngine.Graphics.Tests
{
	public class MaterialTests : TestWithMocksOrVisually
	{
		[Test, CloseAfterFirstFrame]
		public void CreateCustomMaterial()
		{
			var shader =
				ContentLoader.Create<Shader>(new ShaderCreationData(ShaderCodeOpenGL.PositionUVOpenGLVertexCode,
					ShaderCodeOpenGL.PositionUVOpenGLFragmentCode, ShaderCodeDX11.PositionUVDX11, 
					ShaderCodeDX9.Position2DUVDX9, VertexFormat.Position2DUV));
			var image = ContentLoader.Create<Image>(new ImageCreationData(new Size(4)));
			var generatedMaterial = new Material(shader, image);
			Assert.IsNotNull(generatedMaterial);
			Assert.AreEqual(shader, generatedMaterial.Shader);
			Assert.AreEqual(image, generatedMaterial.DiffuseMap);
		}

		[Test, CloseAfterFirstFrame]
		public void LoadSavedMaterial()
		{
			var loadedMaterial = ContentLoader.Load<Material>("DefaultMaterial");
			Assert.AreEqual("DefaultMaterial", loadedMaterial.Name);
			Assert.AreEqual("Position3DUV", loadedMaterial.Shader.Name);
			Assert.AreEqual("DeltaEngineLogo", loadedMaterial.DiffuseMap.Name);
			Assert.AreEqual(Color.White, loadedMaterial.DefaultColor);
		}

		[Test, CloseAfterFirstFrame]
		public void SetRenderSize()
		{
			var loadedMaterial = ContentLoader.Load<Material>("DefaultMaterial");
			var materialSettings = new MockSettings();
			materialSettings.SetAsCurrent();
			var pixelSize = ScreenSpace.Current.FromPixelSpace(loadedMaterial.DiffuseMap.PixelSize);
			loadedMaterial.SetRenderSize(RenderSize.PixelBased);
			Assert.IsTrue(loadedMaterial.MaterialRenderSize.IsNearlyEqual(pixelSize));
			loadedMaterial.SetRenderSize(RenderSize.SettingsBased);
			Assert.IsTrue(loadedMaterial.MaterialRenderSize.IsNearlyEqual(pixelSize/new Size(800)));
			loadedMaterial.SetRenderSize(RenderSize.Size800X480);
			Assert.IsTrue(loadedMaterial.MaterialRenderSize.IsNearlyEqual(pixelSize / new Size(800)));
			loadedMaterial.SetRenderSize(RenderSize.Size1024X720);
			Assert.IsTrue(loadedMaterial.MaterialRenderSize.IsNearlyEqual(pixelSize / new Size(1024)));
			loadedMaterial.SetRenderSize(RenderSize.Size1280X720);
			Assert.IsTrue(loadedMaterial.MaterialRenderSize.IsNearlyEqual(pixelSize / new Size(1280)));	
			loadedMaterial.SetRenderSize(RenderSize.Size1920X1080);
			Assert.IsTrue(loadedMaterial.MaterialRenderSize.IsNearlyEqual(pixelSize / new Size(1920)));
		}
	}
}