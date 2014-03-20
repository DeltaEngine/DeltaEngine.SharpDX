using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using MapFlags = SharpDX.Direct3D11.MapFlags;
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
			DisposeData();
			LoadImage(fileData);
			SetSamplerState();
		}

		protected override void TryLoadImage(Stream fileData)
		{
			NativeTexture =
				(Texture2D)Resource.FromStream(device.NativeDevice, fileData, (int)fileData.Length);
			WarnAboutWrongAlphaFormat(NativeTexture.Description.Format == Format.R8G8B8A8_UNorm);
			var textureSize = new Size(NativeTexture.Description.Width, NativeTexture.Description.Height);
			CompareActualSizeMetadataSize(textureSize);
		}

		public Texture2D NativeTexture { get; protected set; }

		public override void FillRgbaData(byte[] rgbaColors)
		{
			if (PixelSize.Width * PixelSize.Height * 4 != rgbaColors.Length)
				throw new InvalidNumberOfBytes(PixelSize);
			if (NativeTexture == null)
				CreateNewDynamicTexture();
			DataStream subResource;
			device.Context.MapSubresource(NativeTexture, 0, MapMode.WriteDiscard, MapFlags.None,
				out subResource);
			subResource.Write(rgbaColors, 0, rgbaColors.Length);
			device.Context.UnmapSubresource(NativeTexture, 0);
		}

		private void CreateNewDynamicTexture()
		{
			var description = new Texture2DDescription
			{
				Width = (int)PixelSize.Width,
				Height = (int)PixelSize.Height,
				ArraySize = 1,
				MipLevels = 1,
				Format = Format.R8G8B8A8_UNorm,
				Usage = ResourceUsage.Dynamic,
				CpuAccessFlags = CpuAccessFlags.Write,
				BindFlags = BindFlags.ShaderResource,
				SampleDescription = new SampleDescription(1, 0),
			};
			NativeTexture = new Texture2D(device.NativeDevice, description);
			SetSamplerState();
		}

		protected override void SetSamplerState()
		{
			NativeResourceView = new ShaderResourceView(device.NativeDevice, NativeTexture);
		}

		public ShaderResourceView NativeResourceView { get; protected set; }

		protected override void DisposeData()
		{
			if (NativeResourceView != null)
				NativeResourceView.Dispose();
			if (NativeTexture != null)
				NativeTexture.Dispose();
		}
	}
}