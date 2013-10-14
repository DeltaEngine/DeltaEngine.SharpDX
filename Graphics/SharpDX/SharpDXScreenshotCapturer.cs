using SharpDX.Direct3D11;
using DXDevice = SharpDX.Direct3D11.Device;

namespace DeltaEngine.Graphics.SharpDX
{
	/// <summary>
	/// Takes a screenshot under SharpDX.
	/// </summary>
	public class SharpDXScreenshotCapturer : ScreenshotCapturer
	{
		public SharpDXScreenshotCapturer(Device device)
		{
			this.device = (SharpDXDevice)device;
		}

		private readonly SharpDXDevice device;

		public void MakeScreenshot(string fileName)
		{
			Resource.ToFile(device.Context, device.BackBuffer, ImageFileFormat.Png, fileName);
		}
	}
}