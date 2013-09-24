using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Graphics.Tests
{
	public class MaterialTests : TestWithMocksOrVisually
	{
		[Test, CloseAfterFirstFrame]
		public void CreateCustomMaterial()
		{
			var shader =
				ContentLoader.Create<Shader>(new ShaderCreationData(ShaderCodeOpenGL.PositionUvOpenGLVertexCode,
					ShaderCodeOpenGL.PositionUvOpenGLFragmentCode, ShaderCodeDX11.PositionUvDx11, 
					ShaderCodeDX9.Position2DUvDx9, VertexFormat.Position2DUv));
			var image = ContentLoader.Create<Image>(new ImageCreationData(new Size(4)));
			var generatedMaterial = new Material(shader, image);
			Assert.IsNotNull(generatedMaterial);
			Assert.AreEqual(shader, generatedMaterial.Shader);
			Assert.AreEqual(image, generatedMaterial.DiffuseMap);
		}

		[Test, Ignore, CloseAfterFirstFrame]
		public void LoadSavedMaterial()
		{
			var loadedMaterial = ContentLoader.Load<Material>("LogoMaterial");
			Assert.AreEqual("LogoMaterial", loadedMaterial.Name);
			Assert.AreEqual("Position2DColorUv", loadedMaterial.Shader.Name);
			Assert.AreEqual("DeltaEngineLogoAlpha", loadedMaterial.DiffuseMap.Name);
			Assert.AreEqual(Color.Red, loadedMaterial.DefaultColor);
		}
	}
}