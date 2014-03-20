using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using DeltaEngine.Core;
using DXDevice = SharpDX.Direct3D11.Device;
using Rectangle = System.Drawing.Rectangle;
using Size = System.Drawing.Size;

namespace DeltaEngine.Graphics.SharpDX
{
	/// <summary>
	/// Takes a screenshot under SharpDX.
	/// </summary>
	public class SharpDXScreenshotCapturer : ScreenshotCapturer
	{
		public SharpDXScreenshotCapturer(Window window)
		{
			this.window = window;
		}

		private readonly Window window;

		/// <summary>
		/// Normally Resource.ToFile(device.Context, device.BackBuffer, ImageFileFormat.Png, fileName);
		/// should work, but it only returns a blank background. Doing it manually with BackBuffer
		/// Surface copying or doing a screenshot of the form results in the same. We have to do this
		/// very slow and ugly capture of the whole screen and then grab the section of the window
		/// to get the content of the visible render output. Only needed for SharpDX.
		/// </summary>
		public void MakeScreenshot(string fileName)
		{
			var rect = new Rectangle(
				(int)window.ViewportPixelPosition.X, (int)window.ViewportPixelPosition.Y,
				(int)window.ViewportPixelSize.Width, (int)window.ViewportPixelSize.Height);
			using (var bitmap = new Bitmap(rect.Width, rect.Height))
			{
				using (var graphics = System.Drawing.Graphics.FromImage(bitmap))
				{
					graphics.CopyFromScreen(rect.Left, rect.Top, 0, 0, new Size(rect.Width, rect.Height));
					using (Stream stream = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite))
						bitmap.Save(stream, ImageFormat.Png);
				}
			}
		}
	}
}