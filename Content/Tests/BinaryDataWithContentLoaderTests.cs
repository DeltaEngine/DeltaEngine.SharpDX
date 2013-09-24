using System.IO;
using System.Reflection;
using DeltaEngine.Content.Mocks;
using DeltaEngine.Core;
using NUnit.Framework;

namespace DeltaEngine.Content.Tests
{
	public class BinaryDataWithContentLoaderTests
	{
		[Test]
		public void TestLoadContentType()
		{
			var stream = new MemoryStream();
			var writer = new BinaryWriter(stream);
			const string ContentName = "SomeXml";
			writer.Write(ContentName);
			ContentLoader.Use<MockContentLoader>();
			stream.Position = 0;
			var reader = new BinaryReader(stream);
			object returnedContentType = BinaryDataLoader.TryCreateAndLoad(typeof(MockXmlContentType),
				reader, Assembly.GetExecutingAssembly().GetName().Version);
			var content = returnedContentType as MockXmlContentType;
			Assert.IsNotNull(content);
			Assert.AreEqual(ContentName, content.Name);
			ContentLoader.DisposeIfInitialized();
		}

		[Test]
		public void LoadContentWithoutNameShouldThrowUnableToLoadContentDataWithoutName()
		{
			var stream = new MemoryStream();
			var writer = new BinaryWriter(stream);
			writer.Write(string.Empty);
			ContentLoader.Use<MockContentLoader>();
			stream.Position = 0;
			var reader = new BinaryReader(stream);
			Assert.That(
				() => BinaryDataLoader.TryCreateAndLoad(typeof(MockXmlContentType), reader,
					Assembly.GetExecutingAssembly().GetName().Version),
				Throws.Exception.With.InnerException.TypeOf<BinaryDataLoader.UnableToLoadContentDataWithoutName>());
			ContentLoader.DisposeIfInitialized();
		}

		[Test]
		public void SaveContentData()
		{
			ContentLoader.Use<MockContentLoader>();
			var xmlContent = ContentLoader.Load<MockXmlContent>("XmlData");
			using (var dataWriter = new BinaryWriter(new MemoryStream()))
				BinaryDataSaver.TrySaveData(xmlContent, typeof(MockXmlContent), dataWriter);
			ContentLoader.DisposeIfInitialized();
		}

		private class MockXmlContentType : ContentData
		{
			protected MockXmlContentType(string contentName)
				: base(contentName) { }

			protected override void DisposeData() { }
			protected override void LoadData(Stream fileData) { }
		}
	}
}