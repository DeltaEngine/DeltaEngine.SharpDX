using DeltaEngine.Content;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Rendering2D.Tests
{
	public class MaterialTests : TestWithMocksOrVisually
	{
		[Test, CloseAfterFirstFrame]
		public void LoadSimpleMaterial()
		{
			Assert.IsNotNull(ContentLoader.Load<Material>("Earth"));
		}

		[Test, CloseAfterFirstFrame]
		public void ThrowExceptionWithoutShaderName()
		{
			Assert.Throws<Material.UnableToCreateMaterialWithoutValidShaderName>(
				() => new Material("", "AnyImageAnimation"));
		}

		[Test, CloseAfterFirstFrame]
		public void ThrowExceptionIfMaterialDataHasNoShaderSpecified()
		{
			Assert.Throws<Material.UnableToCreateMaterialWithoutValidShaderName>(
				() => ContentLoader.Load<Material>("NoShader"));
		}

		[Test]
		public void SkipImageDataLoadingIfNoImageNameWasSpecified()
		{
			var noImageMaterial = ContentLoader.Load<Material>("NoImageMaterial");
			Assert.IsNull(noImageMaterial.DiffuseMap);
			Assert.IsNull(noImageMaterial.Animation);
			Assert.IsNull(noImageMaterial.SpriteSheet);
		}

		[Test]
		public void LoadMaterialWithAnimation()
		{
			ContentLoader.Load<ImageAnimation>("MyImageAnimation");
			var noImageMaterial = ContentLoader.Load<Material>("MaterialWithImageAnimation");
			Assert.IsNotNull(noImageMaterial.Animation);
			Assert.IsNull(noImageMaterial.SpriteSheet);
		}

		[Test]
		public void LoadMaterialWithSpriteSheet()
		{
			ContentLoader.Load<SpriteSheetAnimation>("MySpriteSheet");
			var noImageMaterial = ContentLoader.Load<Material>("MaterialWithSpriteSheet");
			Assert.IsNotNull(noImageMaterial.SpriteSheet);
		}

		[Test]
		public void Load3DMaterial()
		{
			var material = ContentLoader.Load<Material>("3DMaterial");
			var shader = material.Shader as ShaderWithFormat;
			Assert.IsTrue(shader.Format.Is3D);
		}
	}
}
