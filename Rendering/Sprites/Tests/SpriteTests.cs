using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Shapes;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace DeltaEngine.Rendering.Sprites.Tests
{
	public class SpriteTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void CreateMaterial()
		{
			logoMaterial = new Material(Shader.Position2DUv, "DeltaEngineLogo");
		}

		private Material logoMaterial;

		[Test, ApproveFirstFrameScreenshot]
		public void RenderSprite()
		{
			new Sprite(logoMaterial, Rectangle.HalfCentered);
		}

		[Test, ApproveFirstFrameScreenshot]
		public void RenderSpriteWithImageName()
		{
			new Sprite("DeltaEngineLogo", Rectangle.HalfCentered);
		}

		[Test, CloseAfterFirstFrame]
		public void ResetNonAnimationSprite()
		{
			var sprite = new Sprite(logoMaterial, Rectangle.HalfCentered);
			sprite.Elapsed = 4f;
			sprite.Reset();
			Assert.AreEqual(0f, sprite.Elapsed);
		}

		[Test, ApproveFirstFrameScreenshot]
		public void RenderInactivatedAndReactivatedSprite()
		{
			var sprite = new Sprite(logoMaterial, Rectangle.HalfCentered);
			sprite.IsActive = false;
			sprite.IsActive = true;
		}

		[Test, ApproveFirstFrameScreenshot]
		public void RenderRedSpriteOverBlue()
		{
			var colorLogoMaterial = new Material(Shader.Position2DColorUv, "DeltaEngineLogo");
			colorLogoMaterial.DefaultColor = Color.Red;
			new Sprite(colorLogoMaterial, Rectangle.HalfCentered) { RenderLayer = 1 };
			colorLogoMaterial.DefaultColor = Color.Blue;
			new Sprite(colorLogoMaterial, screenTopLeft) { RenderLayer = 0 };
		}

		private readonly Rectangle screenTopLeft = Rectangle.FromCenter(0.3f, 0.3f, 0.5f, 0.5f);

		[Test, CloseAfterFirstFrame]
		public void CreateSprite()
		{
			var sprite = new Sprite(logoMaterial, Rectangle.HalfCentered);
			Assert.AreEqual(Color.White, sprite.Color);
			Assert.AreEqual("DeltaEngineLogo", sprite.Material.DiffuseMap.Name);
			Assert.IsTrue(sprite.Material.DiffuseMap.PixelSize == DiskContentSize ||
				sprite.Material.DiffuseMap.PixelSize == MockContentSize);
		}

		private static readonly Size DiskContentSize = new Size(128, 128);
		private static readonly Size MockContentSize = new Size(4, 4);

		[Test, CloseAfterFirstFrame]
		public void ChangeImage()
		{
			var sprite = new Sprite(logoMaterial, Rectangle.HalfCentered);
			Assert.AreEqual("DeltaEngineLogo", sprite.Material.DiffuseMap.Name);
			Assert.AreEqual(BlendMode.Normal, sprite.BlendMode);
			sprite.Material = new Material(Shader.Position2DUv, "Verdana12Font");
			Assert.AreEqual("Verdana12Font", sprite.Material.DiffuseMap.Name);
			Assert.AreEqual(BlendMode.Normal, sprite.BlendMode);
		}

		[Test, ApproveFirstFrameScreenshot]
		public void RenderSpriteAndLines()
		{
			new Line2D(Point.Zero, Point.One, Color.Blue);
			new Sprite(logoMaterial, Rectangle.HalfCentered);
			new Line2D(Point.UnitX, Point.UnitY, Color.Purple);
		}

		[Test, CloseAfterFirstFrame]
		public void DrawingTwoSpritesWithTheSameImageAndRenderLayerOnlyIssuesOneDrawCall()
		{
			new Sprite(logoMaterial, Rectangle.HalfCentered);
			new Sprite(logoMaterial, Rectangle.HalfCentered);
			RunAfterFirstFrame(
				() => Assert.AreEqual(1, Resolve<Drawing>().NumberOfDynamicDrawCallsThisFrame));
		}

		[Test, CloseAfterFirstFrame]
		public void DrawingTwoSpritesWithTheSameImageButDifferentRenderLayersIssuesTwoDrawCalls()
		{
			new Sprite(logoMaterial, Rectangle.HalfCentered).RenderLayer = 1;
			new Sprite(logoMaterial, Rectangle.HalfCentered).RenderLayer = 2;
			RunAfterFirstFrame(
				() => Assert.AreEqual(2, Resolve<Drawing>().NumberOfDynamicDrawCallsThisFrame));
		}

		[Test, CloseAfterFirstFrame]
		public void DrawingTwoSpritesWithDifferentImagesIssuesTwoDrawCalls()
		{
			new Sprite(logoMaterial, Rectangle.HalfCentered);
			new Sprite(new Material(Shader.Position2DUv, "EarthSpriteSheet"), Rectangle.HalfCentered);
			RunAfterFirstFrame(
				() => Assert.AreEqual(1, Resolve<Drawing>().NumberOfDynamicDrawCallsThisFrame));
		}

		[Test]
		public void DrawSpritesWithDifferentBlendModes()
		{
			Resolve<Window>().Title =
				"Blend modes: Opaque, Normal, Additive, AlphaTest, LightEffect, Subtractive";
			var opaqueMaterial = new Material(Shader.Position2DUv, "DeltaEngineLogo");
			var alphaMaterial = new Material(Shader.Position2DUv, "DeltaEngineLogoAlpha");
			var drawAreas = CreateDrawAreas(3, 2);
			new Sprite(opaqueMaterial, drawAreas[0]) { BlendMode = BlendMode.Opaque };
			new Sprite(alphaMaterial, drawAreas[1]) { BlendMode = BlendMode.Opaque };
			new Sprite(opaqueMaterial, drawAreas[2]) { BlendMode = BlendMode.Normal };
			new Sprite(alphaMaterial, drawAreas[3]) { BlendMode = BlendMode.Normal };
			new Sprite(opaqueMaterial, drawAreas[4]) { BlendMode = BlendMode.Additive };
			new Sprite(alphaMaterial, drawAreas[5]) { BlendMode = BlendMode.Additive };
			new Sprite(opaqueMaterial, drawAreas[6]) { BlendMode = BlendMode.AlphaTest };
			new Sprite(alphaMaterial, drawAreas[7]) { BlendMode = BlendMode.AlphaTest };
			new Sprite(opaqueMaterial, drawAreas[8]) { BlendMode = BlendMode.LightEffect };
			new Sprite(alphaMaterial, drawAreas[9]) { BlendMode = BlendMode.LightEffect };
			new Sprite(opaqueMaterial, drawAreas[10]) { BlendMode = BlendMode.Subtractive };
			new Sprite(alphaMaterial, drawAreas[11]) { BlendMode = BlendMode.Subtractive };
		}

		private static Rectangle[] CreateDrawAreas(int cols, int rows)
		{
			var drawAreas = new Rectangle[cols * rows * 2];
			var size = new Size(0.2f, 0.2f);
			var position1 = new Point(0.2f, 0.35f);
			for (int y = 0; y < rows; y++)
			{
				for (int x = 0; x < cols; x++)
				{
					var index = x * 2 + (y * cols * rows);
					drawAreas[index] = Rectangle.FromCenter(position1, size);
					var position2 = new Point(position1.X + 0.04f, position1.Y + 0.04f);
					drawAreas[index + 1] = Rectangle.FromCenter(position2, size);
					position1.X += 0.3f;
				}
				position1 = new Point(0.2f, position1.Y + 0.275f);
			}
			return drawAreas;
		}

		[Test, ApproveFirstFrameScreenshot]
		public void DrawSpritesWithBlendModeFromContentMetaData()
		{
			var drawAreas = CreateDrawAreas(3, 1);
			new Sprite(logoMaterial, drawAreas[0]);
			new Sprite(new Material(Shader.Position2DUv, "DeltaEngineLogoOpaque"), drawAreas[1]);
			new Sprite(logoMaterial, drawAreas[2]);
			new Sprite(new Material(Shader.Position2DUv, "DeltaEngineLogoAlpha"), drawAreas[3]);
			new Sprite(logoMaterial, drawAreas[4]);
			new Sprite(new Material(Shader.Position2DUv, "DeltaEngineLogoAdditive"), drawAreas[5]);
		}

		[Test, CloseAfterFirstFrame]
		public void RenderingHiddenSpriteDoesNotThrowException()
		{
			new Sprite(logoMaterial, Rectangle.One) { Visibility = Visibility.Hide };
			Assert.DoesNotThrow(() => AdvanceTimeAndUpdateEntities());
		}

		[Test]
		public void ResizeViewportAndRenderFullscreenSprite()
		{
			Resolve<Window>().ViewportPixelSize = new Size(800, 600);
			new Sprite(logoMaterial, Rectangle.One);
		}

		[Test]
		public void RenderFullscreenSpriteAndResizeViewport()
		{
			new Sprite(logoMaterial, Rectangle.One);
			Resolve<Window>().ViewportPixelSize = new Size(800, 600);
		}

		[Test]
		public void RenderRotatedSprite()
		{
			var sprite = new Sprite(logoMaterial, Rectangle.FromCenter(Point.Half, new Size(0.5f)));
			sprite.Rotation = 60;
		}

		[Test]
		public void DrawFlippedSprite()
		{
			new Sprite(logoMaterial, Rectangle.FromCenter(new Point(0.25f, 0.5f), new Size(0.2f)));
			var flippedX = new Sprite(logoMaterial, Rectangle.FromCenter(Point.Half, new Size(0.2f)));
			flippedX.Coordinates = new Sprite.SpriteCoordinates(Rectangle.One, FlipMode.Horizontal);
			var flippedY = new Sprite(logoMaterial,
				Rectangle.FromCenter(new Point(0.75f, 0.5f), new Size(0.2f)));
			flippedY.Coordinates = new Sprite.SpriteCoordinates(Rectangle.One, FlipMode.Vertical);
		}

		[Test]
		public void RenderPanAndZoomIntoLogo()
		{
			new Camera2DScreenSpace(Resolve<Window>());
			var logo = new Sprite(logoMaterial, Rectangle.FromCenter(Point.One, new Size(0.25f)));
			logo.Start<PanAndZoom>();
		}

		private class PanAndZoom : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				var camera = ScreenSpace.Current as Camera2DScreenSpace;
				camera.LookAt = Point.Half.Lerp(Point.One, Time.Total / 2);
				camera.Zoom = 1.0f.Lerp(2.0f, Time.Total / 4);
			}
		}

		[Test]
		public void DrawColoredSprite()
		{
			var sprite = new Sprite(new Material(Shader.Position2DColorUv, "DeltaEngineLogo"),
				Rectangle.FromCenter(new Point(0.5f, 0.5f), new Size(0.2f)));
			sprite.Color = Color.Red;
		}

		[Test]
		public void DrawModifiedUvSprite()
		{
			var sprite = new Sprite(new Material(Shader.Position2DColorUv, "DeltaEngineLogo"),
				Rectangle.FromCenter(new Point(0.5f, 0.5f), new Size(0.2f)));
			sprite.Coordinates = new Sprite.SpriteCoordinates(new Rectangle(0, 0, 0.5f, 0.5f));
		}
	}
}