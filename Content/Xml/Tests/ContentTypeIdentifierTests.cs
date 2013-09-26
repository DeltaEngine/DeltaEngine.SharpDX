using System.IO;
using System.Text;
using System.Xml.Linq;
using NUnit.Framework;

namespace DeltaEngine.Content.Xml.Tests
{
	public class ContentTypeIdentifierTests
	{
		[TestCase("Image.png", ContentType.Image), TestCase("Sound.wav", ContentType.Sound),
		TestCase("Animation.gif", ContentType.JustStore), TestCase("Music.wma", ContentType.Music),
		TestCase("Video.mp4", ContentType.Video), TestCase("Json.json", ContentType.Json),
		TestCase("Model.fbx", ContentType.Model), TestCase("Mesh.deltamesh", ContentType.Mesh),
		TestCase("Particle.deltaparticle", ContentType.ParticleEmitter),
		TestCase("Shader.deltashader", ContentType.Shader),
		TestCase("Material.deltamaterial", ContentType.Material),
		TestCase("Geometry.deltageometry", ContentType.Geometry)]
		public void CheckPngFileIsOfImageType(string fileName, ContentType contentType)
		{
			var type = ContentTypeIdentifier.ExtensionToType(fileName);
			Assert.AreEqual(contentType, type);
		}

		[Test]
		public void CheckTypeForFontFile()
		{
			var type = CheckTypeForFile("<Font></Font>");
			Assert.AreEqual(ContentType.Font, type);
		}

		public ContentType CheckTypeForFile(string xmlFile)
		{
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlFile)))
			{
				var file = XDocument.Load(stream);
				return ContentTypeIdentifier.DetermineTypeForXmlFile(file);
			}
		}

		[Test]
		public void CheckTypeForInputCommandsFile()
		{
			var type = CheckTypeForFile("<InputCommand></InputCommand>");
			Assert.AreEqual(ContentType.InputCommand, type);
		}

		[Test]
		public void CheckTypeForLevelFile()
		{
			var type = CheckTypeForFile("<Level></Level>");
			Assert.AreEqual(ContentType.Level, type);
		}

		[Test]
		public void CheckTypeForOtherXmlFiles()
		{
			var type = CheckTypeForFile("<Test></Test>");
			Assert.AreEqual(ContentType.Xml, type);
		}

		[Test]
		public void CheckUnsupportedTypeThrowsException()
		{
			const string FileName = "unsupported";
			Assert.Throws<ContentTypeIdentifier.UnsupportedContentFileFoundCannotParseType>(
				() => ContentTypeIdentifier.ExtensionToType(FileName));
		}
	}
}