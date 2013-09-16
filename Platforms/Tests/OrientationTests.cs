using DeltaEngine.Commands;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using DeltaEngine.Input;
using DeltaEngine.Rendering.Shapes;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace DeltaEngine.Platforms.Tests
{
	public class OrientationTests : TestWithMocksOrVisually
	{
		[Test]
		public void TestDrawAreaWhenChangingOrientationToPortrait()
		{
			new Ellipse(new Rectangle(0.7f, 0.7f, 0.1f, 0.1f), Color.Red);
			RunAfterFirstFrame(() =>
			{
				Resolve<Window>().ViewportPixelSize = new Size(480, 800);
				var screen = ScreenSpace.Current;
				Assert.AreEqual(typeof(QuadraticScreenSpace), screen.GetType());
				var quadSize = screen.FromPixelSpace(new Point(0, 0));
				ArePointsNearlyEqual(new Point(0.2f, 0f), quadSize);
				quadSize = screen.FromPixelSpace(new Point(480, 800));
				ArePointsNearlyEqual(new Point(0.8f, 1), quadSize);
				var pixelSize = screen.ToPixelSpace(new Point(0.2f, 0));
				ArePointsNearlyEqual(Point.Zero, pixelSize);
				pixelSize = screen.ToPixelSpace(new Point(0.8f, 1));
				ArePointsNearlyEqual(new Point(480, 800), pixelSize);
			});
		}

		private static void ArePointsNearlyEqual(Point expected, Point actual)
		{
			Assert.IsTrue(actual.X.IsNearlyEqual(expected.X),
				"Actual: " + actual + ", Expected: " + expected);
			Assert.IsTrue(actual.Y.IsNearlyEqual(expected.Y),
				"Actual: " + actual + ", Expected: " + expected);
		}

		[Test, CloseAfterFirstFrame]
		public void TestDrawAreaWhenChangingOrientationToLandscape()
		{
			new Ellipse(new Rectangle(0.7f, 0.7f, 0.1f, 0.1f), Color.Red);
			RunAfterFirstFrame(() =>
			{
				Resolve<Window>().ViewportPixelSize = new Size(800, 480);
				var screen = ScreenSpace.Current;
				var quadSize = screen.FromPixelSpace(new Point(0, 0));
				ArePointsNearlyEqual(new Point(0f, 0.2f), quadSize);
				quadSize = screen.FromPixelSpace(new Point(800, 480));
				ArePointsNearlyEqual(new Point(1, 0.8f), quadSize);
				var pixelSize = screen.ToPixelSpace(new Point(0f, 0.2f));
				ArePointsNearlyEqual(Point.Zero, pixelSize);
				pixelSize = screen.ToPixelSpace(new Point(1, 0.8f));
				ArePointsNearlyEqual(new Point(800, 480), pixelSize);
			});
		}

		[Test]
		public void ChangeOrientaion()
		{
			var line = new Line2D(Point.Zero, Point.One, Color.Green);
			var window = Resolve<Window>();
			window.BackgroundColor = Color.Blue;
			new Command(() => window.ViewportPixelSize = new Size(800, 480)).Add(new KeyTrigger(Key.A));
			new Command(() => window.ViewportPixelSize = new Size(480, 800)).Add(new KeyTrigger(Key.B));
			RunAfterFirstFrame(() =>
			{
				var screen = ScreenSpace.Current;
				var startPosition = screen.Viewport.TopLeft;
				var endPosition = screen.Viewport.BottomRight;
				window.Title = "Size: " + window.ViewportPixelSize + " Start: " + startPosition +
					" End: " + endPosition;
				line.StartPoint = startPosition;
				line.EndPoint = endPosition;
			});
		}
	}
}