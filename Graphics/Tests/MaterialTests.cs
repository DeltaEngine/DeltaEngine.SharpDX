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
			var shader = ContentLoader.Create<Shader>(
				new ShaderCreationData(ShaderCodeOpenGL.PositionUVOpenGLVertexCode,
					ShaderCodeOpenGL.PositionUVOpenGLFragmentCode, ShaderCodeDX11.PositionUVDX11,
					ShaderCodeDX9.Position2DUVDX9, VertexFormat.Position2DUV));
			var generatedMaterial = new Material(shader, null, new Size(4));
			Assert.IsNotNull(generatedMaterial);
			Assert.AreEqual(shader, generatedMaterial.Shader);
			Assert.IsNull(generatedMaterial.DiffuseMap);
			Assert.AreEqual(ScreenSpace.Current.FromPixelSpace(new Size(4)),
				generatedMaterial.MaterialRenderSize);
		}

		[Test, CloseAfterFirstFrame]
		public void LoadSavedMaterial()
		{
			var loadedMaterial = ContentLoader.Load<Material>("DefaultMaterial");
			Assert.AreEqual("DefaultMaterial", loadedMaterial.Name);
			Assert.AreEqual("Position2DUV", loadedMaterial.Shader.Name);
			Assert.AreEqual("DeltaEngineLogo", loadedMaterial.DiffuseMap.Name);
			Assert.AreEqual(Color.White, loadedMaterial.DefaultColor);
		}

		[Test, CloseAfterFirstFrame]
		public void LoadSavedMaterialWithPadding()
		{
			var loadedMaterial = ContentLoader.Load<Material>("DefaultMaterial");
			loadedMaterial.RenderingCalculator = new RenderingCalculator(new AtlasRegion
			{
				PadLeft = 0.5f,
				PadRight = 0.5f,
				PadTop = 0.5f,
				PadBottom = 0.5f,
				IsRotated = false
			});
			var result = loadedMaterial.RenderingCalculator.GetUVAndDrawArea(Rectangle.HalfCentered,
				Rectangle.One, FlipMode.None);
			Assert.AreEqual(result.RequestedUserUV, Rectangle.HalfCentered);
		}

		[Test, CloseAfterFirstFrame]
		public void LoadSavedMaterialWithClearPadding()
		{
			var loadedMaterial = ContentLoader.Load<Material>("DefaultMaterial");
			loadedMaterial.RenderingCalculator = new RenderingCalculator(new AtlasRegion
			{
				PadLeft = 0,
				PadRight = 0,
				PadTop = 0,
				PadBottom = 0,
				IsRotated = false
			});
			var result = loadedMaterial.RenderingCalculator.GetUVAndDrawArea(Rectangle.HalfCentered,
				Rectangle.One, FlipMode.None);
			Assert.AreEqual(result.RequestedUserUV, Rectangle.HalfCentered);
		}

		[Test, CloseAfterFirstFrame]
		public void LoadSavedMaterialWithRotation()
		{
			var loadedMaterial = ContentLoader.Load<Material>("DefaultMaterial");
			loadedMaterial.RenderingCalculator = new RenderingCalculator(new AtlasRegion
			{
				PadLeft = 0.5f,
				PadRight = 0.5f,
				PadTop = 0.5f,
				PadBottom = 0.5f,
				IsRotated = true
			});
			var result = loadedMaterial.RenderingCalculator.GetUVAndDrawArea(Rectangle.HalfCentered,
				Rectangle.One, FlipMode.None);
			Assert.AreEqual(result.RequestedUserUV, Rectangle.HalfCentered);
		}

		[Test, CloseAfterFirstFrame]
		public void SetRenderSize()
		{
			var loadedMaterial = ContentLoader.Load<Material>("DefaultMaterial");
			var resolution = new MockSettings().Resolution;
			loadedMaterial.RenderSizeMode = RenderSizeMode.PixelBased;
			var pixelBasedSize = ScreenSpace.Current.ToPixelSpace(loadedMaterial.MaterialRenderSize);
			Assert.AreEqual(new Size(128), pixelBasedSize);
			SetPixelModeAndCheckTargetResolution(loadedMaterial, RenderSizeMode.SizeFor800X480, 800);
			SetPixelModeAndCheckTargetResolution(loadedMaterial, RenderSizeMode.SizeFor1024X768, 1024);
			SetPixelModeAndCheckTargetResolution(loadedMaterial, RenderSizeMode.SizeFor1280X720, 1280);
			SetPixelModeAndCheckTargetResolution(loadedMaterial, RenderSizeMode.SizeFor1920X1080, 1920);
			SetPixelModeAndCheckTargetResolution(loadedMaterial, RenderSizeMode.SizeForSettingsResolution, resolution.Width);
		}

		private static void SetPixelModeAndCheckTargetResolution(Material material, RenderSizeMode sizeMode,
			float width)
		{
			material.RenderSizeMode = sizeMode;
			var for800X480Size = ScreenSpace.Current.ToPixelSpace(material.MaterialRenderSize);
			var targetSize = new Size(128 * Settings.Current.Resolution.Width / width);
			Assert.IsTrue(targetSize.IsNearlyEqual(for800X480Size),
				for800X480Size + " should be " + targetSize);
		}
	}
}