using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using SlimDX.Direct3D9;

namespace DeltaEngine.Graphics.SlimDX
{
	/// <summary>
	/// SlimDX image.
	/// </summary>
	public class SlimDXImage : Image
	{
		protected SlimDXImage(string contentName, SlimDXDevice device)
			: base(contentName)
		{
			this.device = device;
		}

		private SlimDXImage(ImageCreationData data, SlimDXDevice device)
			: base(data)
		{
			this.device = device;
		}

		private readonly SlimDXDevice device;

		protected override void SetSamplerStateAndTryToLoadImage(Stream fileData)
		{
			LoadImage(fileData);
			SetSamplerState();
		}

		protected override void TryLoadImage(Stream fileData)
		{
			NativeTexture = Texture.FromStream(device.NativeDevice, fileData, Usage.None, Pool.Managed);
			WarnAboutWrongAlphaFormat(NativeTexture.GetLevelDescription(0).Format == Format.A8R8G8B8);
			var textureSize = new Size(NativeTexture.GetLevelDescription(0).Width,
				NativeTexture.GetLevelDescription(0).Height);
			CompareActualSizeMetadataSize(textureSize);
		}

		public Texture NativeTexture { get; protected set; }

		public override void FillRgbaData(byte[] rgbaColors)
		{
			if (PixelSize.Width * PixelSize.Height * 4 != rgbaColors.Length)
				throw new InvalidNumberOfBytes(PixelSize);
			if (NativeTexture == null)
				NativeTexture = new Texture(device.NativeDevice, (int)PixelSize.Width,
					(int)PixelSize.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);
			FlipRgbaToBgra(rgbaColors);
			var rectangle = NativeTexture.LockRectangle(0, LockFlags.None);
			rectangle.Data.Write(rgbaColors, 0, rgbaColors.Length);
			NativeTexture.UnlockRectangle(0);
		}

		/// <summary>
		/// Only an issue with SlimDX, all other frameworks like RGBA data just fine.
		/// </summary>
		private static void FlipRgbaToBgra(byte[] rgbaColors)
		{
			for (int i = 0; i < rgbaColors.Length / 4; i++)
			{
				var swap = rgbaColors[i * 4];
				rgbaColors[i * 4] = rgbaColors[i * 4 + 2];
				rgbaColors[i * 4 + 2] = swap;
			}
		}

		protected override void SetSamplerState()
		{
			SetTextureAndSamplerState(0);
		}

		public void SetTextureAndSamplerState(int textureNumber)
		{
			device.NativeDevice.SetTexture(textureNumber, NativeTexture);
			device.NativeDevice.SetSamplerState(textureNumber, SamplerState.MipFilter,
				TextureFilter.None);
			device.NativeDevice.SetSamplerState(textureNumber, SamplerState.MinFilter,
				DisableLinearFiltering ? TextureFilter.Point : TextureFilter.Linear);
			device.NativeDevice.SetSamplerState(textureNumber, SamplerState.MagFilter,
				DisableLinearFiltering ? TextureFilter.Point : TextureFilter.Linear);
			device.NativeDevice.SetSamplerState(textureNumber, SamplerState.AddressU,
				AllowTiling ? TextureAddress.Wrap : TextureAddress.Clamp);
			device.NativeDevice.SetSamplerState(textureNumber, SamplerState.AddressV,
				AllowTiling ? TextureAddress.Wrap : TextureAddress.Clamp);
		}

		protected override void DisposeData()
		{
			if (NativeTexture != null)
				NativeTexture.Dispose();
		}
	}
}