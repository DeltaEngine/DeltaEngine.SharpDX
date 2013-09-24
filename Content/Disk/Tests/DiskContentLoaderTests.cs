using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using DeltaEngine.Content.Mocks;
using DeltaEngine.Content.Xml;
using DeltaEngine.Core;
using DeltaEngine.Mocks;
using NUnit.Framework;
using Size = DeltaEngine.Datatypes.Size;

namespace DeltaEngine.Content.Disk.Tests
{
	/// <summary>
	/// ContentMetaData.xml does not exist on purpose, it should be created automatically!
	/// </summary>
	[Category("Slow"), Ignore]
	internal class DiskContentLoaderTests
	{
		//ncrunch: no coverage start
		[TestFixtureSetUp]
		public void Setup()
		{
			CreateContentMetaDataAndRealFiles();
			ContentLoader.Use<DiskContentLoader>();
			image = ContentLoader.Load<MockImage>("DeltaEngineLogo");
		}

		private static void CreateContentMetaDataAndRealFiles()
		{
			Directory.CreateDirectory("Content");
			var root = new XmlData("ContentMetaData");
			root.AddAttribute("Name", "DeltaEngine.Content.Tests");
			root.AddAttribute("Type", "Scene");
			root.AddChild(CreateImageEntryAndFile("DeltaEngineLogo", new Size(128, 128)));
			root.AddChild(CreateImageEntryAndFile("SmallImage", new Size(32, 32)));
			root.AddChild(CreateAnimationNode());
			root.AddChild(CreateXmlEntryAndFile());
			var contentMetaData = new XmlFile(root);
			contentMetaData.Save(Path.Combine("Content", "ContentMetaData.xml"));
		}

		private static XmlData CreateImageEntryAndFile(string name, Size pixelSize)
		{
			var image = new XmlData("ContentMetaData");
			image.AddAttribute("Name", name);
			image.AddAttribute("Type", "Image");
			string filename = name + ".png";
			CreateImageFile(image, filename, pixelSize);
			image.AddAttribute("LocalFilePath", filename);
			image.AddAttribute("LastTimeUpdated", DateTime.Now);
			image.AddAttribute("PlatformFileId", --platformFileId);
			return image;
		}

		private static int platformFileId;

		private static void CreateImageFile(XmlData image, string filename, Size pixelSize)
		{
			var newBitmap = new Bitmap((int)pixelSize.Width, (int)pixelSize.Height,
				PixelFormat.Format32bppArgb);
			for (int y = 0; y < newBitmap.Height; y++)
				for (int x = 0; x < newBitmap.Width; x++)
					newBitmap.SetPixel(x, y, Color.White);
			if (filename == "DeltaEngineLogo.png")
				newBitmap.SetPixel(50, 70, Color.FromArgb(0, 0, 0, 0));
			string filePath = Path.Combine("Content", filename);
			newBitmap.Save(filePath);
			image.AddAttribute("FileSize", new FileInfo(filePath).Length);
			image.AddAttribute("PixelSize", pixelSize.ToString());
		}

		private static XmlData CreateAnimationNode()
		{
			var animation = new XmlData("ContentMetaData");
			animation.AddAttribute("Name", "TestAnimation");
			animation.AddAttribute("Type", "ImageAnimation");
			var frame1 = CreateImageEntryAndFile("ImageAnimation01", new Size(64, 64));
			var frame2 = CreateImageEntryAndFile("ImageAnimation02", new Size(64, 64));
			return animation.AddChild(frame1).AddChild(frame2);
		}

		private Image image;

		private static XmlData CreateXmlEntryAndFile()
		{
			var xml = new XmlData("ContentMetaData");
			xml.AddAttribute("Name", "Test");
			xml.AddAttribute("Type", "Xml");
			const string Filename = "Test.xml";
			using (var textWriter = File.CreateText(Path.Combine("Content", Filename)))
				textWriter.WriteLine("<Test></Test>");
			xml.AddAttribute("LocalFilePath", Filename);
			xml.AddAttribute("LastTimeUpdated", DateTime.Now);
			xml.AddAttribute("PlatformFileId", --platformFileId);
			return xml;
		}

		[Test]
		public void LoadImageContent()
		{
			Assert.AreEqual("DeltaEngineLogo", image.Name);
			Assert.IsFalse(image.IsDisposed);
			Assert.AreEqual(new Size(128, 128), image.PixelSize);
			var smallImage = ContentLoader.Load<MockImage>("SmallImage");
			Assert.AreEqual(new Size(32, 32), smallImage.PixelSize);
		}

		[Test]
		public void LoadNonExistingImageFails()
		{
			if (Debugger.IsAttached)
				Assert.Throws<ContentLoader.ContentNotFound>(
					() => ContentLoader.Load<MockImage>("FailImage"));
		}

		[Test]
		public void LoadingCachedContentOfTheWrongTypeThrowsException()
		{
			Assert.DoesNotThrow(() => ContentLoader.Load<MockImage>("DeltaEngineLogo"));
			Assert.Throws<ContentLoader.CachedResourceExistsButIsOfTheWrongType>(
				() => ContentLoader.Load<MockSound>("DeltaEngineLogo"));
		}

		[Test]
		public void LastTimeUpdatedShouldBeSet()
		{
			Assert.Greater(image.MetaData.LastTimeUpdated, DateTime.Now.AddSeconds(-2));
		}

		[Test]
		public void PlatformFileIdShouldBeSet()
		{
			Assert.Less(image.MetaData.PlatformFileId, 0);
		}

		[Test]
		public void FileSizeShouldBeSet()
		{
			Assert.Greater(image.MetaData.FileSize, 150);
		}

		[Test]
		public void CreateMetaDataViaFileCreator()
		{
			string randomContentDir = "Content" + DateTime.Now.Ticks;
			Directory.CreateDirectory(randomContentDir);
			foreach (var filePath in Directory.GetFiles("Content", "*.png"))
				File.Copy(filePath, Path.Combine(randomContentDir, Path.GetFileName(filePath)));
			string metaDataFilePath = Path.Combine(randomContentDir, "ContentMetaData.xml");
			var xml = CreateMetaDataXmlFileAndCheckPixelSizes(null, metaDataFilePath);
			CreateMetaDataXmlFileAndCheckPixelSizes(xml, metaDataFilePath);
		}

		private static XDocument CreateMetaDataXmlFileAndCheckPixelSizes(XDocument lastXml,
			string metaDataFilePath)
		{
			var xml = new ContentMetaDataFileCreator(lastXml).CreateAndLoad(metaDataFilePath);
			Assert.AreEqual(5, xml.Root.Elements().Count());
			foreach (var element in xml.Root.Elements())
				CheckElementPixelSize(element);
			return xml;
		}

		private static void CheckElementPixelSize(XElement element)
		{
			var expectedPixelSize = "(64, 64)";
			if (element.Attribute("Name").Value == "DeltaEngineLogo")
				expectedPixelSize = SetElementTo128Pixels(element);
			else if (element.Attribute("Name").Value == "SmallImage")
				expectedPixelSize = SetElementTo32Pixels(element);
			if (element.Attribute("Type").Value == "Image")
				Assert.AreEqual(expectedPixelSize, element.Attribute("PixelSize").Value);
		}

		private static string SetElementTo128Pixels(XElement element)
		{
			Assert.IsNull(element.Attribute("BlendMode"));
			return "(128, 128)";
		}

		private static string SetElementTo32Pixels(XElement element)
		{
			Assert.AreEqual("Opaque", element.Attribute("BlendMode").Value);
			return "(32, 32)";
		}

		[Test]
		public void LoadingContentDataFromBinaryDataWillLoadItFromName()
		{
			var content = ContentLoader.Load<XmlContent>("Test");
			var data = BinaryDataExtensions.SaveDataIntoMemoryStream(content);
			Assert.AreEqual(content.Name.Length + 1, data.Length);
			var loadedContent =
				BinaryDataExtensions.LoadDataWithKnownTypeFromMemoryStream<XmlContent>(data);
			Assert.AreEqual(content.Name, loadedContent.Name);
			Assert.AreEqual(content, loadedContent);
		}
	}
}