using System;
using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Content.Mocks;
using NUnit.Framework;

namespace DeltaEngine.Tests.Content
{
	public class FakeContentLoaderTests
	{
		[SetUp]
		public void CreateContentLoader()
		{
			ContentLoader.Use<FakeContentLoader>();
		}

		[TearDown]
		public void DisposeContentLoader()
		{
			ContentLoader.DisposeIfInitialized();
		}

		[Test]
		public void ContentLoadWithNullStream()
		{
			Assert.DoesNotThrow(() => ContentLoader.Load<MockXmlContent>("XmlContentWithNoPath"));
		}

		[Test]
		public void ContentLoadWithWrongFilePath()
		{
			Assert.Throws<ContentLoader.ContentFileDoesNotExist>(
				() => ContentLoader.Load<MockXmlContent>("ContentWithWrongPath"));
		}

		[Test]
		public void ThrowExceptionIfSecondContentLoaderInstanceIsUsed()
		{
			ContentLoader.Exists("abc");
			Assert.Throws
				<ContentLoader.ContentLoaderAlreadyExistsItIsOnlyAllowedToSetBeforeTheAppStarts>(
					() => ContentLoader.Use<FakeContentLoader>());
		}

		private class FakeContentLoader : ContentLoader
		{
			private FakeContentLoader()
			{
				contentPath = "NoPath";
			}

			protected override ContentMetaData GetMetaData(string contentName,
				Type contentClassType = null)
			{
				HasValidContentAndMakeSureItIsLoaded();
				var metaData = new ContentMetaData { Type = ContentType.Xml };
				if (contentName.Contains("WrongPath"))
					metaData.LocalFilePath = "No.xml";
				return metaData;
			}

			protected override bool HasValidContentAndMakeSureItIsLoaded()
			{
				return ContentMetaDataFilePath == null;
			}
		}

		[Test]
		public void LoadDefaultDataIfAllowed()
		{
			Assert.DoesNotThrow(
				() => ContentLoader.Load<DynamicXmlMockContent>("UnavailableDynamicContent"));
		}

		private class DynamicXmlMockContent : ContentData
		{
			public DynamicXmlMockContent(string contentName)
				: base(contentName) {}

			protected override void DisposeData() {}
			protected override void LoadData(Stream fileData) {}
			protected override bool AllowCreationIfContentNotFound
			{
				get { return true; }
			}
		}
	}
}