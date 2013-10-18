using System;
using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using NUnit.Framework;

namespace DeltaEngine.Tests.Content
{
	public class FakeImageContentLoaderTests
	{
		[SetUp]
		public void CreateContentLoader()
		{
		}

		[TearDown]
		public void DisposeContentLoader()
		{
			ContentLoader.DisposeIfInitialized();
		}

		[Test]
		public void LoadDefaultImage()
		{
			ContentLoader.resolver = new FakeImageResolver();
			ContentLoader.Use<FakeImageContentLoader>();
			ContentLoader.Load<MockFakeImage>("Verdana12Font");
		}

		public class FakeImageResolver : ContentLoaderResolver
		{
			public override ContentData Resolve(Type contentType, string contentName)
			{
				return new MockFakeImage(contentName);
			}
		}

		public sealed class MockFakeImage : Image
		{
			public MockFakeImage(string contentName)
				: base(contentName) { }

			public MockFakeImage(ImageCreationData creationData)
				: base(creationData) { }

			protected override void SetSamplerStateAndTryToLoadImage(Stream fileData) { }

			protected override void LoadImage(Stream fileData) { }

			public override void Fill(Color[] colors) { }

			public override void Fill(byte[] colors) { }

			protected override void SetSamplerState() { }

			protected override void DisposeData()
			{
				ContentLoader.current.GetMetaData(Name, GetType()).Values.Remove("ImageName");
			}
		}
	}
}