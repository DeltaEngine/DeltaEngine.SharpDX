using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using Color = DeltaEngine.Datatypes.Color;
using Resource = SharpDX.Direct3D11.Resource;

namespace DeltaEngine.Graphics.SharpDX
{
	/// <summary>
	/// Images under SharpDX.
	/// </summary>
	public class SharpDXImage : Image
	{
		protected SharpDXImage(string contentName, SharpDXDevice device)
			: base(contentName)
		{
			this.device = device;
		}

		private SharpDXImage(ImageCreationData data, SharpDXDevice device)
			: base(data)
		{
			this.device = device;
		}

		private readonly SharpDXDevice device;

		protected override void SetSamplerStateAndTryToLoadImage(Stream fileData)
		{
			TryLoadImage(fileData);
			SetSamplerState();
		}

		protected override void LoadImage(Stream fileData)
		{
			NativeTexture =
				(Texture2D)Resource.FromStream(device.NativeDevice, fileData, (int)fileData.Length);
			WarnAboutWrongAlphaFormat(NativeTexture.Description.Format == Format.R8G8B8A8_UNorm);
			var textureSize = new Size(NativeTexture.Description.Width, NativeTexture.Description.Height);
			CompareActualSizeMetadataSize(textureSize);
		}

		public Texture2D NativeTexture { get; protected set; }

		public override void Fill(Color[] colors)
		{
			if (PixelSize.Width * PixelSize.Height != colors.Length)
				throw new InvalidNumberOfColors(PixelSize);
			Utilities.Pin(colors, ptr =>
			{
			  NativeTexture = new Texture2D(device.NativeDevice, CreateTextureDescription(),
					new DataRectangle(ptr, 16));
			});
			SetSamplerState();
		}

		private Texture2DDescription CreateTextureDescription()
		{
			return new Texture2DDescription
			{
				Width = (int)PixelSize.Width,
				Height = (int)PixelSize.Height,
				ArraySize = 1,
				MipLevels = 1,
				Format = Format.R8G8B8A8_UNorm,
				Usage = ResourceUsage.Immutable,
				BindFlags = BindFlags.ShaderResource,
				SampleDescription = new SampleDescription(1, 0),
			};
		}

		public override void Fill(byte[] colors)
		{
			if (PixelSize.Width * PixelSize.Height * 4 != colors.Length)
				throw new InvalidNumberOfBytes(PixelSize);
			Utilities.Pin(colors, ptr =>
			{
			  NativeTexture = new Texture2D(device.NativeDevice, CreateTextureDescription(),
					new DataRectangle(ptr, (int)PixelSize.Width * 4));
			});
			SetSamplerState();
		}

		protected override void SetSamplerState()
		{
			NativeResourceView = new ShaderResourceView(device.NativeDevice, NativeTexture);
		}

		public ShaderResourceView NativeResourceView { get; protected set; }

		protected override void DisposeData()
		{
			if (NativeTexture != null)
				NativeTexture.Dispose();
			if (NativeResourceView != null)
				NativeResourceView.Dispose();
		}
	}
}