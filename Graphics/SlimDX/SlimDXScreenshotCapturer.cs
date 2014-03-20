using SlimDX.Direct3D9;

namespace DeltaEngine.Graphics.SlimDX
{
	/// <summary>
	/// Allows screenshots to be taken under SlimDX.
	/// </summary>
	public class SlimDXScreenshotCapturer : ScreenshotCapturer
	{
		public SlimDXScreenshotCapturer(Device device)
		{
			this.device = (SlimDXDevice)device;
		}

		private readonly SlimDXDevice device;

		public void MakeScreenshot(string fileName)
		{
			Surface.ToFile(device.NativeDevice.GetBackBuffer(0, 0), fileName, ImageFileFormat.Png);
		}
	}
}