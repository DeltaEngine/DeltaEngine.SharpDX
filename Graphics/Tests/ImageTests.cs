using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Graphics.Tests
{
	/// <summary>
	/// The image tests here are limited to loading and integration tests, not visual tests, which
	/// you can find in DeltaEngine.Rendering2D.Tests.SpriteTests.
	/// </summary>
	public class ImageTests : TestWithMocksOrVisually
	{
		[Test, ApproveFirstFrameScreenshot]
		public void DrawOpaqueImageWithVertexColors()
		{
			Resolve<Window>().BackgroundColor = Color.CornflowerBlue;
			new Sprite(ContentLoader.Load<Image>("DeltaEngineLogoOpaque"));
			RunAfterFirstFrame(
				() => Assert.AreEqual(4, Resolve<Drawing>().NumberOfDynamicVerticesDrawnThisFrame));
		}

		private class Sprite : DrawableEntity
		{
			public Sprite(Image image)
			{
				material = new Material(ContentLoader.Load<Shader>(Shader.Position2DColorUV), image);
				OnDraw<DrawSprite>();
			}

			private readonly Material material;

			private class DrawSprite : DrawBehavior
			{
				public DrawSprite(Drawing drawing)
				{
					this.drawing = drawing;
				}

				private readonly Drawing drawing;

				public void Draw(List<DrawableEntity> visibleEntities)
				{
					foreach (var sprite in visibleEntities.OfType<Sprite>())
						drawing.Add(sprite.material, QuadVertices, QuadIndices);
				}

				private static readonly VertexPosition2DColorUV[] QuadVertices = new[]
				{
					new VertexPosition2DColorUV(new Vector2D(175, 25), Color.Yellow, Vector2D.Zero),
					new VertexPosition2DColorUV(new Vector2D(475, 25), Color.Red, Vector2D.UnitX),
					new VertexPosition2DColorUV(new Vector2D(475, 325), Color.Blue, Vector2D.One),
					new VertexPosition2DColorUV(new Vector2D(175, 325), Color.Teal, Vector2D.UnitY)
				};
				private static readonly short[] QuadIndices = new short[] { 0, 1, 2, 0, 2, 3 };
			}
		}

		[Test, CloseAfterFirstFrame]
		public void LoadExistingImage()
		{
			var image = ContentLoader.Load<Image>("DeltaEngineLogoOpaque");
			Assert.AreEqual("DeltaEngineLogoOpaque", image.Name);
			Assert.IsFalse(image.IsDisposed);
			Assert.AreEqual(new Size(128, 128), image.PixelSize);
		}

		[Test]
		public void ShouldThrowIfImageNotLoadedWithDebuggerAttached()
		{
			//ncrunch: no coverage start
			if (Debugger.IsAttached)
				Assert.Throws<ContentLoader.ContentNotFound>(
					() => ContentLoader.Load<Image>("UnavailableImage"));
			//ncrunch: no coverage end
			RunTestAndDisposeResolverWhenDone();
		}

		[Test, ApproveFirstFrameScreenshot]
		public void DrawDefaultTexture()
		{
			Resolve<Window>().BackgroundColor = Color.CornflowerBlue;
			new Sprite(ContentLoader.Load<Image>("UnavailableImage"));
			RunAfterFirstFrame(
				() => Assert.AreEqual(4, Resolve<Drawing>().NumberOfDynamicVerticesDrawnThisFrame));
		}

		[Test, ApproveFirstFrameScreenshot]
		public void DrawCustomImage()
		{
			var customImage = ContentLoader.Create<Image>(new ImageCreationData(new Size(8, 8)));
			var colors = new Color[8 * 8];
			for (int i = 0; i < 8 * 8; i++)
				colors[i] = Color.Purple;
			customImage.Fill(colors);
			new Sprite(customImage);
		}

		[Test, ApproveFirstFrameScreenshot]
		public void FillCustomImageWitDiffrentSizeThanImageCausesException()
		{
			var customImage = ContentLoader.Create<Image>(new ImageCreationData(new Size(8, 9)));
			var colors = new Color[8 * 8];			
			Assert.Throws<Image.InvalidNumberOfColors>(() => customImage.Fill(colors));
			var byteArray = new byte[8 * 8];
			Assert.Throws<Image.InvalidNumberOfBytes>(() => customImage.Fill(byteArray));
			var goodByteArray = new byte[8 * 9 * 4];
			customImage.Fill(goodByteArray);
		}

		[Test, ApproveFirstFrameScreenshot]
		public void BlendModes()
		{
			new DrawableEntity().OnDraw<RenderBlendModes>();
		}

		private class RenderBlendModes : DrawBehavior
		{
			public RenderBlendModes(Drawing drawing)
			{
				this.drawing = drawing;
				logoOpaque = new Material(Shader.Position2DUV, "DeltaEngineLogoOpaque");
				logoAlpha = new Material(Shader.Position2DUV, "DeltaEngineLogoAlpha");
				additive = new Material(Shader.Position2DUV, "CoronaAdditive");
			}

			private readonly Drawing drawing;
			private readonly Material logoOpaque;
			private readonly Material logoAlpha;
			private readonly Material additive;

			public void Draw(List<DrawableEntity> visibleEntities)
			{
				DrawAlphaImageTwice(25, 80, BlendMode.Opaque);
				DrawAlphaImageTwice(225, 80, BlendMode.Normal);
				DrawAlphaImageTwice(425, 80, BlendMode.Additive);
			}

			private void DrawAlphaImageTwice(int x, int y, BlendMode blendMode)
			{
				drawing.Add(logoOpaque, GetVertices(x, y));
				Material top = blendMode == BlendMode.Normal
					? logoAlpha : (blendMode == BlendMode.Additive ? additive : logoOpaque);
				drawing.Add(top, GetVertices(x + Size / 2, y + Size / 2));
			}

			private const int Size = 120;

			private static VertexPosition2DUV[] GetVertices(int x, int y)
			{
				return new[]
				{
					new VertexPosition2DUV(new Vector2D(x, y), Vector2D.Zero),
					new VertexPosition2DUV(new Vector2D(x + Size, y), Vector2D.UnitX),
					new VertexPosition2DUV(new Vector2D(x + Size, y + Size), Vector2D.One),
					new VertexPosition2DUV(new Vector2D(x, y + Size), Vector2D.UnitY)
				};
			}
		}
	}
}