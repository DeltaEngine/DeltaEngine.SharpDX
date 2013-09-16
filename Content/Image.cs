using System;
using System.Diagnostics;
using System.IO;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Content
{
	/// <summary>
	/// Used in Sprites and 3D models to draw image textures on screen via <see cref="Material"/>.
	/// </summary>
	public abstract class Image : ContentData
	{
		protected Image(string contentName)
			: base(contentName) {}

		protected Image(ImageCreationData data)
			: base("<GeneratedImage>")
		{
			PixelSize = data.PixelSize;
			BlendMode = data.BlendMode;
			UseMipmaps = data.UseMipmaps;
			AllowTiling = data.AllowTiling;
			DisableLinearFiltering = data.DisableLinearFiltering;
		}

		protected override bool AllowCreationIfContentNotFound { get { return !Debugger.IsAttached; } }

		protected override void LoadData(Stream fileData)
		{
			ExtractMetaData();
			TryLoadImage(fileData);
			SetSamplerState();
		}

		private void ExtractMetaData()
		{
			PixelSize = MetaData.Get("PixelSize", DefaultTextureSize);
			BlendMode = MetaData.Get("BlendMode", BlendMode.Normal);
			UseMipmaps = MetaData.Get("UseMipmaps", false);
			AllowTiling = MetaData.Get("AllowTiling", false);
			DisableLinearFiltering = MetaData.Get("DisableLinearFiltering", false);
		}

		public Size PixelSize { get; private set; }
		private static readonly Size DefaultTextureSize = new Size(4, 4);
		public BlendMode BlendMode { get; set; }
		public bool UseMipmaps { get; private set; }
		public bool AllowTiling { get; private set; }
		public bool DisableLinearFiltering { get; private set; }

		private void TryLoadImage(Stream fileData)
		{
			try
			{
				LoadImage(fileData);
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				if (!Debugger.IsAttached)
					CreateDefault();
				else
					throw; // ncrunch: no coverage
			}
		}

		protected abstract void LoadImage(Stream fileData);

		protected void WarnAboutWrongAlphaFormat(bool imageHasAlphaFormat)
		{
			if (HasAlpha && !imageHasAlphaFormat)
				Logger.Warning("Image '" + Name +
					"' is supposed to have alpha pixels, but the image pixel format is not using alpha.");
			else if (!HasAlpha && imageHasAlphaFormat)
				Logger.Warning("Image '" + Name +
					"' is supposed to have no alpha pixels, but the image pixel format is using alpha.");
		}

		protected bool HasAlpha
		{
			get { return BlendMode == BlendMode.Normal || BlendMode == BlendMode.AlphaTest; }
		}

		protected override void CreateDefault()
		{
			PixelSize = DefaultTextureSize;
			DisableLinearFiltering = true;
			BlendMode = BlendMode.Opaque;
			Fill(checkerMapColors);
			SetSamplerState();
		}

		public abstract void Fill(Color[] colors);

		public class InvalidNumberOfColors : Exception
		{
			public InvalidNumberOfColors(Size pixelSize)
				: base(pixelSize.Width + "*" + pixelSize.Height) {}
		}

		public abstract void Fill(byte[] rgbaColors);

		public class InvalidNumberOfBytes : Exception
		{
			public InvalidNumberOfBytes(Size pixelSize)
				: base(pixelSize.Width + "*" + pixelSize.Height + "*4") {}
		}

		private readonly Color[] checkerMapColors =
		{
			Color.LightGray, Color.DarkGray, Color.LightGray, Color.DarkGray,
			Color.DarkGray, Color.LightGray, Color.DarkGray, Color.LightGray,
			Color.LightGray, Color.DarkGray, Color.LightGray, Color.DarkGray,
			Color.DarkGray, Color.LightGray, Color.DarkGray, Color.LightGray
		};

		protected abstract void SetSamplerState();

		protected void CompareActualSizeMetadataSize(Size actualSize)
		{
			if (actualSize != PixelSize)
				Logger.Warning("Image '" + Name + "' actual size " + actualSize +
					" is different from the MetaData PixelSize: " + PixelSize);
		}
	}
}