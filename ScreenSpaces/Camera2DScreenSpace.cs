using DeltaEngine.Core;
using DeltaEngine.Datatypes;

namespace DeltaEngine.ScreenSpaces
{
	/// <summary>
	/// Behaves like QuadraticScreenSpace but can also pan and zoom. 
	/// See https://deltaengine.fogbugz.com/default.asp?W101
	/// </summary>
	public class Camera2DScreenSpace : QuadraticScreenSpace
	{
		public Camera2DScreenSpace(Window window)
			: base(window) {}

		public override Point ToPixelSpace(Point quadraticPos)
		{
			return base.ToPixelSpace(Transform(quadraticPos));
		}

		public Point Transform(Point position)
		{
			var point = (position - LookAt) * Zoom + Point.Half;
			if (Rotation != 0.0f)
				point = point.RotateAround(RotationCenter, Rotation);

			return point;
		}

		public Point LookAt = Point.Half;
		public float Rotation = 0.0f;
		public Point RotationCenter = Point.Zero;

		public float Zoom
		{
			get { return zoom; }
			set
			{
				zoom = value;
				inverseZoom = 1.0f / zoom;
			}
		}

		private float zoom = 1.0f;
		private float inverseZoom = 1.0f;

		public override Size ToPixelSpace(Size quadraticSize)
		{
			return base.ToPixelSpace(quadraticSize) * Zoom;
		}

		public override Point FromPixelSpace(Point pixelPosition)
		{
			return InverseTransform(base.FromPixelSpace(pixelPosition));
		}

		public Point InverseTransform(Point position)
		{
			return (position - Point.Half) * inverseZoom + LookAt;
		}

		public override Size FromPixelSpace(Size pixelSize)
		{
			return base.FromPixelSpace(pixelSize) * inverseZoom;
		}

		public override Point TopLeft
		{
			get { return InverseTransform(base.TopLeft); }
		}

		public override Point BottomRight
		{
			get { return InverseTransform(base.BottomRight); }
		}

		public override float Left
		{
			get { return InverseTransformX(base.Left); }
		}

		private float InverseTransformX(float x)
		{
			return (x - 0.5f) * inverseZoom + LookAt.X;
		}

		public override float Top
		{
			get { return InverseTransformY(base.Top); }
		}

		private float InverseTransformY(float y)
		{
			return (y - 0.5f) * inverseZoom + LookAt.Y;
		}

		public override float Right
		{
			get { return InverseTransformX(base.Right); }
		}

		public override float Bottom
		{
			get { return InverseTransformY(base.Bottom); }
		}
	}
}