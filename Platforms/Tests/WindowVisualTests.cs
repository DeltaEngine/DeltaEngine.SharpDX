using System.Windows.Forms;
using DeltaEngine.Commands;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Rendering2D.Shapes;
using NUnit.Framework;

namespace DeltaEngine.Platforms.Tests
{
	public class WindowVisualTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void CreateWindow()
		{
			window = Resolve<Window>();
			Assert.IsTrue(window.IsVisible);
		}

		private Window window;

		[Test]
		public void SetAndGetTitle()
		{
			window.Title = "TestTitle";
			Assert.AreEqual("TestTitle", window.Title);
		}

		[Test, CloseAfterFirstFrame]
		public void ChangeTotalSize()
		{
			Assert.AreEqual(new Size(640, 360), window.ViewportPixelSize);
			Size changedSize = window.TotalPixelSize;
			window.ViewportSizeChanged += size => changedSize = size;
			window.ViewportPixelSize = new Size(200, 200);
			Assert.AreEqual(new Size(200, 200), window.ViewportPixelSize);
			Assert.IsTrue(window.ViewportPixelSize.Width <= 200);
			Assert.IsTrue(window.ViewportPixelSize.Height <= 200);
			Assert.IsTrue(changedSize.Width <= 200);
			Assert.IsTrue(changedSize.Height <= 200);
		}

		/// <summary>
		/// Use the DeviceTests.SetFullscreenResolution to see the real resolution switching
		/// </summary>
		[Test]
		public void SetFullscreenMode()
		{
			var newFullscreenSize = new Size(Screen.PrimaryScreen.Bounds.Width,
				Screen.PrimaryScreen.Bounds.Height);
			Assert.IsFalse(window.IsFullscreen);
			window.SetFullscreen(newFullscreenSize);
			Assert.IsTrue(window.IsFullscreen);
			Assert.AreEqual(newFullscreenSize, window.TotalPixelSize);
		}

		[Test]
		public void SwitchToFullscreenAndWindowedMode()
		{
			Size sizeBeforeFullscreen = window.TotalPixelSize;
			window.SetFullscreen(new Size(1024, 768));
			window.SetWindowed();
			Assert.IsFalse(window.IsFullscreen);
			Assert.AreEqual(sizeBeforeFullscreen, window.TotalPixelSize);
		}

		[Test]
		public void ShowColoredEllipse()
		{
			new Ellipse(new Rectangle(Vector2D.Half, Size.Half), Color.Red);
		}

		[Test]
		public void ShowCursorAndToggleHideWhenClicking()
		{
			bool showCursor = true;
			new Command(() => window.ShowCursor = showCursor = !showCursor).Add(new MouseButtonTrigger());
		}
	}
}