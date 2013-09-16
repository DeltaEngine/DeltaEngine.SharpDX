using System.Diagnostics;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics.Mocks;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Graphics.Tests
{
	internal class ScreenshotTests : TestWithMocksOrVisually
	{
		[Test]
		public void MakeScreenshotOfYellowBackground()
		{
			Resolve<Window>().BackgroundColor = Color.Yellow;
			RunAfterFirstFrame(() =>
			{
				Resolve<Device>().Present();
				var capturer = Resolve<ScreenshotCapturer>();
				capturer.MakeScreenshot(ScreenshotFileName);
				if (!StackTraceExtensions.StartedFromNCrunch)
					Process.Start(ScreenshotFileName); //ncrunch: no coverage
				else
					Assert.AreEqual(ScreenshotFileName, (capturer as MockScreenshotCapturer).LastFilename);
			});
		}

		private const string ScreenshotFileName = "Test.png";
	}
}