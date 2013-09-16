using System;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using DeltaEngine.Mocks;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace DeltaEngine.Tests.ScreenSpaces
{
	public class QuadraticScreenSpaceTests
	{
		[Test]
		public void SquareWindowWithQuadraticSpace()
		{
			new QuadraticScreenSpace(window);
			window.ViewportPixelSize = new Size(100, 100);
			Assert.AreEqual(Point.Zero, ScreenSpace.Current.TopLeft);
			Assert.AreEqual(Point.One, ScreenSpace.Current.BottomRight);
			Assert.AreEqual(Rectangle.One, ScreenSpace.Current.Viewport);
			Assert.AreEqual(Point.One, ScreenSpace.Current.FromPixelSpace(new Point(100, 100)));
			Assert.AreEqual(Point.Half, ScreenSpace.Current.FromPixelSpace(new Point(50, 50)));
			Assert.AreEqual(new Rectangle(0.1f, 0.1f, 0.8f, 0.8f),
				ScreenSpace.Current.FromPixelSpace(new Rectangle(10, 10, 80, 80)));
		}

		private readonly Window window = new MockWindow();

		[Test]
		public void ToQuadraticWithUnevenSize()
		{
			window.ViewportPixelSize = new Size(99, 199);
			var screen = new QuadraticScreenSpace(window);
			Assert.AreEqual(new Point(0.2512563f, 0), screen.TopLeft);
			Assert.AreEqual(new Point(0.7487437f, 1), screen.BottomRight);
			Assert.AreEqual(screen.BottomRight, screen.FromPixelSpace(new Point(99, 199)));
		}

		[Test]
		public void ToQuadraticWithNonSquareWindow()
		{
			window.ViewportPixelSize = new Size(100, 75);
			var screen = new QuadraticScreenSpace(window);
			Assert.AreEqual(0, screen.Left);
			Assert.AreEqual(0.125f, screen.Top);
			Assert.AreEqual(1, screen.Right);
			Assert.AreEqual(0.875f, screen.Bottom);
			Assert.AreEqual(new Rectangle(0, 0.125f, 1, 0.75f), screen.Viewport);
			Assert.AreEqual(new Point(1f, 0.875f), screen.FromPixelSpace(new Point(100, 75)));
			Assert.AreEqual(Point.Half, screen.FromPixelSpace(new Point(50, 37.5f)));
			Assert.AreEqual(new Size(0.1f, 0.1f), screen.FromPixelSpace(new Size(10, 10)));
		}

		[Test]
		public void ToQuadraticWithPortraitWindow()
		{
			window.ViewportPixelSize = new Size(75, 100);
			var screen = new QuadraticScreenSpace(window);
			Assert.AreEqual(new Point(0.125f, 0), screen.TopLeft);
			Assert.AreEqual(new Point(0.875f, 1), screen.BottomRight);
			Assert.AreEqual(new Rectangle(0.125f, 0, 0.75f, 1), screen.Viewport);
			Assert.AreEqual(new Point(0.875f, 1f), screen.FromPixelSpace(new Point(75, 100)));
			Assert.AreEqual(Point.Half, screen.FromPixelSpace(new Point(37.5f, 50)));
			Assert.AreEqual(new Size(0.1f, 0.1f), screen.FromPixelSpace(new Size(10, 10)));
		}

		[Test]
		public void ToPixelWithSquareWindow()
		{
			window.ViewportPixelSize = new Size(100, 100);
			var screen = new QuadraticScreenSpace(window);
			Assert.AreEqual(new Point(100, 100), screen.ToPixelSpace(Point.One));
			Assert.AreEqual(Point.Zero, screen.ToPixelSpace(Point.Zero));
			Assert.AreEqual(new Point(50, 50), screen.ToPixelSpace(Point.Half));
		}

		[Test]
		public void ToPixelWithUnevenSizeFromQuadraticSpace()
		{
			window.ViewportPixelSize = new Size(99, 199);
			var screen = new QuadraticScreenSpace(window);
			Assert.AreEqual(new Point(149, 199), screen.ToPixelSpace(Point.One));
			Assert.AreEqual(new Point(-50, 0), screen.ToPixelSpace(Point.Zero));
			Assert.AreEqual(new Point(49.5f, 99.5f), screen.ToPixelSpace(Point.Half));
			Assert.AreEqual(new Point(50, 100), screen.ToPixelSpaceRounded(Point.Half));
			Assert.AreEqual(new Point(199, 199),
				screen.ToPixelSpaceRounded(Point.One) - screen.ToPixelSpaceRounded(Point.Zero));
		}

		[Test]
		public void ToPixelWithNonSquareWindow()
		{
			window.ViewportPixelSize = new Size(100, 75);
			var screen = new QuadraticScreenSpace(window);
			Assert.AreEqual(new Point(100, 75), screen.ToPixelSpace(new Point(1f, 0.875f)));
			Assert.AreEqual(Point.Zero, screen.ToPixelSpace(new Point(0, 0.125f)));
			Assert.AreEqual(new Point(50, 37.5f), screen.ToPixelSpace(Point.Half));
			Assert.AreEqual(new Size(10, 20), screen.ToPixelSpace(new Size(0.1f, 0.2f)));
			Assert.AreEqual(new Rectangle(20, 7.5f, 60, 60),
				screen.ToPixelSpace(new Rectangle(0.2f, 0.2f, 0.6f, 0.6f)));
		}

		[Test]
		public void ToPixelWithPortraitWindow()
		{
			window.ViewportPixelSize = new Size(75, 100);
			var screen = new QuadraticScreenSpace(window);
			Assert.AreEqual(new Point(75, 100), screen.ToPixelSpace(new Point(0.875f, 1f)));
			Assert.AreEqual(Point.Zero, screen.ToPixelSpace(new Point(0.125f, 0)));
			Assert.AreEqual(new Point(37.5f, 50), screen.ToPixelSpace(Point.Half));
			Assert.AreEqual(new Size(10, 20), screen.ToPixelSpace(new Size(0.1f, 0.2f)));
		}

		[Test]
		public void ToPixelInFullHdResolution()
		{
			window.ViewportPixelSize = new Size(1920, 1080);
			var screen = new QuadraticScreenSpace(window);
			Assert.AreEqual(new Point(1680, 1500), screen.ToPixelSpace(new Point(0.875f, 1f)));
			var somePoint = screen.FromPixelSpace(new Point(324, 483));
			var somePointPlusOne = screen.FromPixelSpace(new Point(325, 483));
			Assert.IsFalse(somePoint.X.IsNearlyEqual(somePointPlusOne.X),
				somePoint + " should not be nearly equal to " + somePointPlusOne);
			Assert.AreEqual(new Point(324, 483), screen.ToPixelSpaceRounded(somePoint));
			Assert.AreEqual(new Point(325, 483), screen.ToPixelSpaceRounded(somePointPlusOne));
		}

		[Test]
		public void GetInnerPoint()
		{
			window.ViewportPixelSize = new Size(800, 600);
			ScreenSpace screen = new QuadraticScreenSpace(window);
			Assert.AreEqual(screen.TopLeft, screen.GetInnerPoint(Point.Zero));
			Assert.AreEqual(screen.BottomRight, screen.GetInnerPoint(Point.One));
		}

		[Test]
		public void TestViewportSizeChanged()
		{
			window.ViewportPixelSize = new Size(800, 800);
			var screen = new QuadraticScreenSpace(window);
			Action checkSize = delegate { Assert.AreEqual(Rectangle.One, screen.Viewport); };
			screen.ViewportSizeChanged += checkSize;
			window.ViewportPixelSize = new Size(800, 800);
			screen.ViewportSizeChanged -= checkSize;
		}

		[Test]
		public void TestAspectRatio()
		{
			window.ViewportPixelSize = new Size(800, 800);
			Assert.AreEqual(1f, ScreenSpace.Current.AspectRatio);
			window.ViewportPixelSize = new Size(1920, 1080);
			Assert.AreEqual(0.5625f, ScreenSpace.Current.AspectRatio);
		}
	}
}