using System;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;

namespace DeltaEngine.ScreenSpaces
{
	/// <summary>
	/// Converts to and from quadratic space. https://deltaengine.net/Learn/ScreenSpace
	/// </summary>
	public class QuadraticScreenSpace : ScreenSpace
	{
		public QuadraticScreenSpace(Window window)
			: base(window)
		{
			CalculateScalesAndOffsets();
		}

		private void CalculateScalesAndOffsets()
		{
			quadraticToPixelScale = CalculateToPixelScale();
			quadraticToPixelOffset = CalculateToPixelOffset();
			pixelToQuadraticScale = CalculateToQuadraticScale();
			pixelToQuadraticOffset = CalculateToQuadraticOffset();
		}

		private Size quadraticToPixelScale;
		private Point quadraticToPixelOffset;
		private Size pixelToQuadraticScale;
		private Point pixelToQuadraticOffset;

		private Size CalculateToPixelScale()
		{
			Size scale = viewportPixelSize;
			if (viewportPixelSize.AspectRatio < 1.0f)
				scale.Width *= 1.0f / viewportPixelSize.AspectRatio;
			else if (viewportPixelSize.AspectRatio > 1.0f)
				scale.Height *= viewportPixelSize.AspectRatio;
			return scale;
		}

		private Point CalculateToPixelOffset()
		{
			Point offset = Point.Zero;
			if (viewportPixelSize.AspectRatio < 1.0f)
				offset.X = (viewportPixelSize.Width - quadraticToPixelScale.Width) * 0.5f;
			else
				offset.Y = (viewportPixelSize.Height - quadraticToPixelScale.Height) * 0.5f;
			return offset;
		}

		private Size CalculateToQuadraticScale()
		{
			return 1.0f / quadraticToPixelScale;
		}

		private Point CalculateToQuadraticOffset()
		{
			return new Point(-quadraticToPixelOffset.X / quadraticToPixelScale.Width,
				-quadraticToPixelOffset.Y / quadraticToPixelScale.Height);
		}

		protected override void Update(Size newViewportSize)
		{
			base.Update(newViewportSize);
			CalculateScalesAndOffsets();
		}

		public override Point FromPixelSpace(Point pixelPosition)
		{
			var scaledPixelPosition = new Point(pixelToQuadraticScale.Width * pixelPosition.X,
				pixelToQuadraticScale.Height * pixelPosition.Y);
			return scaledPixelPosition + pixelToQuadraticOffset;
		}

		public override Size FromPixelSpace(Size pixelSize)
		{
			return pixelToQuadraticScale * pixelSize;
		}

		public override Point ToPixelSpace(Point currentScreenSpacePos)
		{
			var pixelPos =
				new Point(quadraticToPixelScale.Width * currentScreenSpacePos.X + quadraticToPixelOffset.X,
					quadraticToPixelScale.Height * currentScreenSpacePos.Y + quadraticToPixelOffset.Y);
			return new Point((float)Math.Round(pixelPos.X, 2), (float)Math.Round(pixelPos.Y, 2));
		}

		public override Size ToPixelSpace(Size currentScreenSpaceSize)
		{
			return quadraticToPixelScale * currentScreenSpaceSize;
		}

		public override Point TopLeft
		{
			get { return pixelToQuadraticOffset; }
		}

		public override Point BottomRight
		{
			get { return new Point(1 - pixelToQuadraticOffset.X, 1 - pixelToQuadraticOffset.Y); }
		}

		public override float Left
		{
			get { return pixelToQuadraticOffset.X; }
		}

		public override float Top
		{
			get { return pixelToQuadraticOffset.Y; }
		}

		public override float Right
		{
			get { return 1 - pixelToQuadraticOffset.X; }
		}

		public override float Bottom
		{
			get { return 1 - pixelToQuadraticOffset.Y; }
		}

		public override Point GetInnerPoint(Point relativePoint)
		{
			return new Point(Left + (Right - Left) * relativePoint.X,
				Top + (Bottom - Top) * relativePoint.Y);
		}
	}
}