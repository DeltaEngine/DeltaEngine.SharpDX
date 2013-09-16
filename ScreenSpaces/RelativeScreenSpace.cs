using DeltaEngine.Core;
using DeltaEngine.Datatypes;

namespace DeltaEngine.ScreenSpaces
{
	/// <summary>
	/// Converts to and from Relative space. https://deltaengine.fogbugz.com/default.asp?W101
	/// </summary>
	public class RelativeScreenSpace : ScreenSpace
	{
		public RelativeScreenSpace(Window window)
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
			return ToPixelSpace((Size)currentScreenSpacePos);
		}

		public override Size ToPixelSpace(Size currentScreenSpaceSize)
		{
			return currentScreenSpaceSize * viewportPixelSize;
		}

		public override Point FromPixelSpace(Point pixelPosition)
		{
			return FromPixelSpace((Size)pixelPosition);
		}

		public override Size FromPixelSpace(Size pixelSize)
		{
			return pixelSize * pixelToRelativeScale;
		}

		public override Point TopLeft
		{
			get { return Point.Zero; }
		}
		public override Point BottomRight
		{
			get { return Point.One; }
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
			get { return 1.0f; }
		}
		public override float Bottom
		{
			get { return 1.0f; }
		}

		public override Point GetInnerPoint(Point relativePoint)
		{
			return relativePoint;
		}
	}
}