using System.IO;
using DeltaEngine.Content.Xml;
using DeltaEngine.Rendering2D.Fonts;

namespace DeltaEngine.Platforms.Mocks
{
	/// <summary>
	/// Mock to quickly fake loading fonts
	/// </summary>
	public class MockFont : Font
	{
		public MockFont(string contentName)
			: base("<GeneratedMockFont:" + contentName + ">") { }

		protected override void LoadData(Stream fileData)
		{
			var glyph1 = new XmlData("Glyph");
			glyph1.AddAttribute("Character", ' ');
			glyph1.AddAttribute("UV", "0 0 1 16");
			glyph1.AddAttribute("AdvanceWidth", "7.34875");
			glyph1.AddAttribute("LeftBearing", "0");
			glyph1.AddAttribute("RightBearing", "4.21875");
			var glyph2 = new XmlData("Glyph");
			glyph2.AddAttribute("Character", 'a');
			glyph2.AddAttribute("UV", "0 0 1 16");
			glyph2.AddAttribute("AdvanceWidth", "7.34875");
			glyph2.AddAttribute("LeftBearing", "0");
			glyph2.AddAttribute("RightBearing", "4.21875");
			var glyphs = new XmlData("Glyphs").AddChild(glyph1).AddChild(glyph2);
			var kerningPair = new XmlData("Kerning");
			kerningPair.AddAttribute("First", " ");
			kerningPair.AddAttribute("Second", "a");
			kerningPair.AddAttribute("Distance", "1");
			var kernings = new XmlData("Kernings");
			kernings.AddChild(kerningPair);
			var bitmap = new XmlData("Bitmap");
			bitmap.AddAttribute("Name", "Verdana12Font");
			bitmap.AddAttribute("Width", "128");
			bitmap.AddAttribute("Height", "128");
			Data = new XmlData("Font");
			Data.AddAttribute("Family", "Verdana");
			Data.AddAttribute("Size", "12");
			Data.AddAttribute("Style", "AddOutline");
			Data.AddAttribute("LineHeight", "16");
			Data.AddChild(bitmap).AddChild(glyphs).AddChild(kernings);
			InitializeDescriptionAndMaterial();
		}
	}
}