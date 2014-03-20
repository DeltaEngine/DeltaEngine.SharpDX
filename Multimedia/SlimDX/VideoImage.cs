//using System.Drawing;
//using System.Drawing.Imaging;
//using System.IO;
//using System.Runtime.InteropServices;
//using DeltaEngine.Content;
//using DeltaEngine.Graphics.SlimDX;
//using DeltaEngine.Platforms;
//using SlimDX;
//using SlimDX.Direct3D9;
//using Point = System.Drawing.Point;
//using Size = DeltaEngine.Datatypes.Size;

//namespace DeltaEngine.Multimedia.SlimDX
//{
//	/// <summary>
//	/// Image from a video playback under SlimDX.
//	/// </summary>
//	[IgnoreForResolver]
//	public class VideoImage : SlimDXImage
//	{
//		public VideoImage(SlimDXDevice device)
//			: base(new ImageCreateData(new Size(4, 4)), device)
//		{
//			this.device = device;
//		}

//		private readonly SlimDXDevice device;

//		protected override void LoadData(Stream fileData) {}

//		public void UpdateTexture(Bitmap bmp)
//		{
//			var bmpData = bmp.LockBits(new System.Drawing.Rectangle(Point.Empty, bmp.Size),
//				ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
//			NativeTexture = new Texture(device.NativeDevice, bmpData.Width, bmpData.Height, 1,
//				Usage.None, Format.A8R8G8B8, Pool.Managed);
//			DataRectangle data = NativeTexture.LockRectangle(0, LockFlags.None);
//			data.Pitch = bmpData.Stride;
//			byte[] readBack = new byte[bmpData.Stride * bmpData.Height];
//			Marshal.Copy(bmpData.Scan0, readBack, 0, readBack.Length);
//			data.Data.Write(readBack, 0, readBack.Length);
//			NativeTexture.UnlockRectangle(0);
//			bmp.UnlockBits(bmpData);
//		}
//	}
//}