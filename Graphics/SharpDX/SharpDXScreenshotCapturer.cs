using SharpDX.Direct3D11;
using DxDevice = SharpDX.Direct3D11.Device;

namespace DeltaEngine.Graphics.SharpDX
{
	/// <summary>
	/// Takes a screenshot under SharpDX.
	/// </summary>
	public class SharpDXScreenshotCapturer : ScreenshotCapturer
	{
		public SharpDXScreenshotCapturer(Device device)
		{
			this.device = device;
		}

		private readonly Device device;

		public void MakeScreenshot(string fileName)
		{
			var sharpDevice = (SharpDXDevice)device;
			Resource.ToFile(sharpDevice.Context, sharpDevice.BackBuffer, ImageFileFormat.Png, fileName);
		}
	}
}