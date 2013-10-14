using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Mocks
{
	public sealed class MockImage : Image
	{
		public MockImage(string contentName)
			: base(contentName) {}

		public MockImage(ImageCreationData creationData)
			: base(creationData) {}

		protected override void SetSamplerStateAndTryToLoadImage(Stream fileData) {}
		protected override void LoadImage(Stream fileData) {}

		public override void Fill(Color[] colors)
		{
			if (PixelSize.Width * PixelSize.Height != colors.Length)
				throw new InvalidNumberOfColors(PixelSize);
		}

		public override void Fill(byte[] colors)
		{
			if (PixelSize.Width * PixelSize.Height * 4 != colors.Length)
				throw new InvalidNumberOfBytes(PixelSize);
		}

		public void CallCompareActualSizeMetadataSizeMethod(Size actualSize)
		{
			CompareActualSizeMetadataSize(actualSize);
		}

		protected override void SetSamplerState() {}

		protected override void DisposeData() {}

		public void CheckAlphaIsCorrect(bool hasAlpha)
		{
			WarnAboutWrongAlphaFormat(hasAlpha);
		}
	}
}