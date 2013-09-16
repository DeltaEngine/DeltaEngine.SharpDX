using DeltaEngine.Core;
using DeltaEngine.Datatypes;

namespace DeltaEngine.ScreenSpaces
{
	/// <summary>
	/// Converts to and from Pixel space. https://deltaengine.fogbugz.com/default.asp?W101
	/// </summary>
	public class PixelScreenSpace : ScreenSpace
	{
		public PixelScreenSpace(Window window)
			: base(window) {}

		public override Point ToPixelSpace(Point currentScreenSpacePos)
		{
			return currentScreenSpacePos;
		}

		public override Size ToPixelSpace(Size currentScreenSpaceSize)
		{
			return currentScreenSpaceSize;
		}

		public override Point FromPixelSpace(Point pixelPosition)
		{
			return pixelPosition;
		}

		public override Size FromPixelSpace(Size pixelSize)
		{
			return pixelSize;
		}

		public override Point TopLeft
		{
			get { return Point.Zero; }
		}
		public override Point BottomRight
		{
			get { return viewportPixelSize; }
		}
		public override float Left
		{
			get { return 0.0f; }
		}
		public override float Top
		{
			get { return 0.0f; }
		}
		public override float Right
		{
			get { return viewportPixelSize.Width; }
		}
		public override float Bottom
		{
			get { return viewportPixelSize.Height; }
		}

		public override Point GetInnerPoint(Point relativePoint)
		{
			return (Size)relativePoint * viewportPixelSize;
		}
	}
}