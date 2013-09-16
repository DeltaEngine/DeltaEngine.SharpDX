using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Shapes;
using DeltaEngine.Rendering.Sprites;
using NUnit.Framework;

namespace DeltaEngine.Rendering.Fonts.Tests
{
	internal class FontTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			verdana = Font.Default;
			tahoma = ContentLoader.Load<Font>("Tahoma30");
		}

		private Font verdana;
		private Font tahoma;

		[Test, ApproveFirstFrameScreenshot]
		public void DrawSmallFont()
		{
			var text = new FontText(verdana, "Hi there", Rectangle.One);
			Assert.AreEqual(3, text.NumberOfComponents);
		}

		/// <summary>
		/// FontText has a font material, the glyphs and the draw size as components.
		/// </summary>
		[Test, CloseAfterFirstFrame]
		public void FontTextHas3Components()
		{
			var text = new FontText(verdana, "Hi there", Rectangle.One);
			Assert.AreEqual(3, text.NumberOfComponents);
		}

		/// <summary>
		/// Lerp is used for its draw size, nothing else (material and glyphs are not lerped).
		/// </summary>
		[Test, CloseAfterFirstFrame]
		public void FontTextShouldNeverHaveMoreThan2LerpComponents()
		{
			var text = new DerivedFontText(verdana, "Hi there", Rectangle.One);
			AdvanceTimeAndUpdateEntities(1);
			RunAfterFirstFrame(() => Assert.AreEqual(1, text.NumberOfLastTickLerpComponents));
		}

		private class DerivedFontText : FontText
		{
			public DerivedFontText(Font font, string text, Rectangle drawArea)
				: base(font, text, drawArea) {}

			public int NumberOfLastTickLerpComponents { get { return lastTickLerpComponents.Count; } }
		}

		[Test]
		public void CreateFontText()
		{
			var fontText = new FontText(verdana, "Verdana12", new Point(0.3f, 0.1f));
			Assert.AreEqual("Verdana12", fontText.Text);
		}

		[Test, ApproveFirstFrameScreenshot]
		public void DrawBigFont()
		{
			new FontText(tahoma, "Big Fonts rule!", Rectangle.One);
		}

		[Test, ApproveFirstFrameScreenshot]
		public void DrawColoredFonts()
		{
			new FontText(tahoma, "Red", Top) { Color = Color.Red };
			new FontText(tahoma, "Yellow", Bottom) { Color = Color.Yellow };
		}

		private static readonly Rectangle Top = new Rectangle(0.5f, 0.4f, 0.0f, 0.0f);
		private static readonly Rectangle Bottom = new Rectangle(0.5f, 0.6f, 0.0f, 0.0f);

		[Test, ApproveFirstFrameScreenshot]
		public void DrawFontAndLines()
		{
			new Line2D(Point.Zero, Point.One, Color.Red) { RenderLayer = -1 };
			new FontText(tahoma, "Delta Engine", Rectangle.One);
			new Line2D(Point.UnitX, Point.UnitY, Color.Red) { RenderLayer = 1 };
		}

		[Test, ApproveFirstFrameScreenshot]
		public void DrawFontOverSprite()
		{
			new Sprite(new Material(Shader.Position2DUv, "DeltaEngineLogo"), Rectangle.HalfCentered)
			{
				Color = Color.PaleGreen,
				RenderLayer = -1
			};
			new FontText(tahoma, "Delta Engine", Rectangle.One);
		}

		[Test, ApproveFirstFrameScreenshot]
		public void DrawTwoDifferentFonts()
		{
			new FontText(tahoma, "Delta Engine", Top);
			new FontText(verdana, "Delta Engine", Bottom);
		}
	}
}