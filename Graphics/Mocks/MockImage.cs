using System;
using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Graphics.Mocks
{
	/// <summary>
	/// Mocks images in unit tests.
	/// </summary>
	public class MockImage : Image
	{
		public MockImage(string contentName)
			: base(contentName)
		{
			// Just for DiskContentLoaderTests
		}

		protected MockImage(string contentName, Device device)
			: base(contentName)
		{
			if (device == null)
				throw new NeedDeviceForImageCreation(); //ncrunch: no coverage
		}

		public class NeedDeviceForImageCreation : Exception { }

		private MockImage(ImageCreationData data, Device device)
			: base(data)
		{
			if (device == null)
				throw new NeedDeviceForImageCreation(); //ncrunch: no coverage
		}

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
		protected override void SetSamplerState() {}
		protected override void DisposeData() {}
	}
}