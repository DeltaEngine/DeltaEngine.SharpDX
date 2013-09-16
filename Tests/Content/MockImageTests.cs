using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Content.Mocks;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Mocks;
using NUnit.Framework;

namespace DeltaEngine.Tests.Content
{
	public class MockImageTests
	{
		[SetUp]
		public void CreateContentLoaderInstance()
		{
			ContentLoader.Use<MockContentLoader>();
		}

		[TearDown]
		public void DisposeContentLoader()
		{
			ContentLoader.DisposeIfInitialized();
		}

		[Test]
		public void LoadContentViaCreationData()
		{
			var imageCreationData = new ImageCreationData(new Size(12, 12));
			var image = ContentLoader.Create<MockImage>(imageCreationData);
			Assert.AreEqual(imageCreationData.PixelSize, image.PixelSize);
			Assert.AreEqual(imageCreationData.BlendMode, image.BlendMode);
			Assert.AreEqual(imageCreationData.UseMipmaps, image.UseMipmaps);
			Assert.AreEqual(imageCreationData.AllowTiling, image.AllowTiling);
			Assert.AreEqual(imageCreationData.DisableLinearFiltering, image.DisableLinearFiltering);
		}

		[Test]
		public void CheckWarningForAlpha()
		{
			var imageCreationData = new ImageCreationData(new Size(12, 12));
			var image = ContentLoader.Create<MockImage>(imageCreationData);
			image.BlendMode = BlendMode.Normal;
			var mockLogger = new MockLogger();
			image.CheckAlphaIsCorrect(false);
			Assert.IsTrue(mockLogger.LastMessage.Contains(
				"is supposed to have alpha pixels, but the image pixel format is not using alpha."));
			image.BlendMode = BlendMode.Opaque;
			image.CheckAlphaIsCorrect(true);
			Assert.IsTrue(mockLogger.LastMessage.Contains(
				"is supposed to have no alpha pixels, but the image pixel format is using alpha."));
		}

		[Test]
		public void LoadDefaultDataIfLoadDataHasFailed()
		{
			Assert.DoesNotThrow(() => ContentLoader.Load<ImageWithFailingLoadData>("DefaultImage"));
		}

		private class ImageWithFailingLoadData : Image
		{
			public ImageWithFailingLoadData(string contentName)
				: base(contentName) {}

			public ImageWithFailingLoadData(ImageCreationData data)
				: base(data) {}

			protected override void DisposeData() {}

			protected override void LoadImage(Stream fileData)
			{
				CompareActualSizeMetadataSize(Size.Zero);
				try
				{
					Fill(new Color[0]);
				}
				catch (InvalidNumberOfColors)
				{
					Fill(new byte[0]);
				}
			}

			public override void Fill(Color[] colors)
			{
				if (colors.Length == 0)
					throw new InvalidNumberOfColors(PixelSize);
			}

			public override void Fill(byte[] colors)
			{
				throw new InvalidNumberOfBytes(PixelSize);
			}

			protected override void SetSamplerState() {}
		}
	}
}