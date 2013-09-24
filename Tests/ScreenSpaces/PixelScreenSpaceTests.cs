using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Mocks;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace DeltaEngine.Tests.ScreenSpaces
{
	public class PixelScreenSpaceTests
	{
		[Test]
		public void SquareWindowWithPixelSpace()
		{
			window.ViewportPixelSize = new Size(100, 100);
			var screen = new PixelScreenSpace(window);
			Assert.AreEqual(Vector2D.Zero, screen.TopLeft);
			Assert.AreEqual(window.ViewportPixelSize, (Size)screen.BottomRight);
			Assert.AreEqual(new Rectangle(Vector2D.Zero, window.TotalPixelSize), screen.Viewport);
			Assert.AreEqual(new Vector2D(100, 100), screen.FromPixelSpace(new Vector2D(100, 100)));
			Assert.AreEqual(new Rectangle(10, 10, 80, 80),
				screen.FromPixelSpace(new Rectangle(10, 10, 80, 80)));
			window.CloseAfterFrame();
		}

		private readonly Window window = new MockWindow();

		[Test]
		public void GetInnerPoint()
		{
			window.ViewportPixelSize = new Size(800, 600);
			ScreenSpace screen = new PixelScreenSpace(window);
			Assert.AreEqual(screen.TopLeft, screen.GetInnerPosition(Vector2D.Zero));
			Assert.AreEqual(screen.BottomRight, screen.GetInnerPosition(Vector2D.One));
			window.CloseAfterFrame();
		}

		[Test]
		public void ToPixelSpaceAndFromPixelSpace()
		{
			window.ViewportPixelSize = new Size(75, 100);
			var pixelScreen = new PixelScreenSpace(window);
			Assert.AreEqual(pixelScreen.TopLeft, pixelScreen.ToPixelSpace(pixelScreen.TopLeft));
			Assert.AreEqual(pixelScreen.BottomRight, pixelScreen.ToPixelSpace(pixelScreen.BottomRight));
			Assert.AreEqual(Size.Zero, pixelScreen.ToPixelSpace(Size.Zero));
			Assert.AreEqual(Size.One, pixelScreen.ToPixelSpace(Size.One));
			window.CloseAfterFrame();
		}

		[Test]
		public void NonSquareWindowWithPixelSpace()
		{
			window.ViewportPixelSize = new Size(100, 75);
			var screen = new PixelScreenSpace(window);
			Assert.AreEqual(0.0f, screen.Left);
			Assert.AreEqual(0.0f, screen.Top);
			Assert.AreEqual(100.0f, screen.Right);
			Assert.AreEqual(75.0f, screen.Bottom);
			Assert.AreEqual(75.0f, screen.Bottom);
			window.CloseAfterFrame();
		}

		[Test]
		public void ChangeWindowTotalPixelSize()
		{
			Size border = window.TotalPixelSize - window.ViewportPixelSize;
			window.ViewportPixelSize = PixelSize;
			Assert.AreEqual(PixelSize, window.ViewportPixelSize);
			Assert.AreEqual(PixelSize + border, window.TotalPixelSize);
			window.CloseAfterFrame();
		}

		private static readonly Size PixelSize = new Size(800, 600);

		[Test]
		public void ChangeWindowViewportPixelSize()
		{
			Size border = window.TotalPixelSize - window.ViewportPixelSize;
			window.ViewportPixelSize = PixelSize;
			Assert.AreEqual(PixelSize + border, window.TotalPixelSize);
			Assert.AreEqual(PixelSize, window.ViewportPixelSize);
			window.CloseAfterFrame();
		}

		[Test]
		public void MoveWindow()
		{
			window.PixelPosition = new Vector2D(100, 200);
			Assert.AreEqual(new Vector2D(100, 200), window.PixelPosition);
			window.CloseAfterFrame();
		}
	}
}