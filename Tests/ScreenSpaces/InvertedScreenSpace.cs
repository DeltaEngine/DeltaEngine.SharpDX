using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.ScreenSpaces;

namespace DeltaEngine.Tests.ScreenSpaces
{
	/// <summary>
	/// Converts to and from Inverted space. https://deltaengine.fogbugz.com/default.asp?W101
	/// (0, 0) == BottomLeft, (1, 1) == TopRight
	/// </summary>
	internal class InvertedScreenSpace : ScreenSpace
	{
		public InvertedScreenSpace(Window window)
			: base(window)
		{
			pixelToRelativeScale = 1.0f / window.ViewportPixelSize;
		}

		private Size pixelToRelativeScale;

		protected override void Update(Size newViewportSize)
		{
			base.Update(newViewportSize);
			pixelToRelativeScale = 1.0f / viewportPixelSize;
		}

		public override Point ToPixelSpace(Point currentScreenSpacePos)
		{
			Point point = Point.Zero;
			point.X = currentScreenSpacePos.X * viewportPixelSize.Width;
			point.Y = (1 - currentScreenSpacePos.Y) * viewportPixelSize.Height;
			return point;
		}

		public override Size ToPixelSpace(Size currentScreenSpaceSize)
		{
			return currentScreenSpaceSize * viewportPixelSize;
		}

		public override Point FromPixelSpace(Point pixelPosition)
		{
			Point point = Point.Zero;
			point.X = pixelPosition.X * pixelToRelativeScale.Width;
			point.Y = 1 - pixelPosition.Y * pixelToRelativeScale.Height;
			return point;
		}

		public override Size FromPixelSpace(Size pixelSize)
		{
			return pixelSize * pixelToRelativeScale;
		}

		public override Point TopLeft
		{
			get { return Point.UnitY; }
		}
		public override Point BottomRight
		{
			get { return Point.UnitX; }
		}
		public override float Left
		{
			get { return 0.0f; }
		}
		public override float Top
		{
			get { return 1.0f; }
		}
		public override float Right
		{
			get { return 1.0f; }
		}
		public override float Bottom
		{
			get { return 0.0f; }
		}

		public override Point GetInnerPoint(Point relativePoint)
		{
			Point point = Point.Zero;
			point.X = relativePoint.X;
			point.Y = 1 - relativePoint.Y;
			return point;
		}
	}
}