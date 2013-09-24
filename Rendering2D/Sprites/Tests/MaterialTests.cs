using DeltaEngine.Content;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Rendering2D.Sprites.Tests
{
	public class MaterialTests : TestWithMocksOrVisually
	{
		[Test]
		public void LoadSimpleMaterial()
		{
			Assert.IsNotNull(ContentLoader.Load<Material>("MyMaterial"));
		}

		[Test]
		public void ThrowExceptionWithoutShaderName()
		{
			Assert.Throws<Material.UnableToCreateMaterialWithoutValidShaderName>(
				() => new Material("", "AnyImageAnimation"));
		}

		[Test]
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
	}
}
