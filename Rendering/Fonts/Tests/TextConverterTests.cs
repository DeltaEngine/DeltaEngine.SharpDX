using DeltaEngine.Content;
using DeltaEngine.Content.Xml;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Rendering.Fonts.Tests
{
	public class TextConverterTests : TestWithMocksOrVisually
	{
		[Test, CloseAfterFirstFrame]
		public void GetGlyphs()
		{
			var fontData = new FontDescription(ContentLoader.Load<XmlContent>("Verdana12").Data);
			var textConverter = new TextConverter(fontData.GlyphDictionary, fontData.PixelLineHeight);
			var glyphs = textConverter.GetRenderableGlyphs("    ", HorizontalAlignment.Center);
			Assert.AreEqual(4, glyphs.Length);
		}

		[Test, CloseAfterFirstFrame]
		public void GetGlyphsRightAligned()
		{
			var fontData = new FontDescription(ContentLoader.Load<XmlContent>("Verdana12").Data);
			var textConverter = new TextConverter(fontData.GlyphDictionary, fontData.PixelLineHeight);
			var glyphs = textConverter.GetRenderableGlyphs("This shall be wrapped to lines", HorizontalAlignment.Right);
			Assert.AreEqual(7, glyphs.Length);
		}
	}
}