using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using DeltaEngine.Extensions;

namespace DeltaEngine.Datatypes
{
	/// <summary>
	/// Holds data for a rectangle by specifying its top left corner and the width and height.
	/// </summary>
	[DebuggerDisplay("Rectangle(Left={Left}, Top={Top}, Width={Width}, Height={Height})")]
	public struct Rectangle : IEquatable<Rectangle>, Lerp<Rectangle>
	{
		public Rectangle(float left, float top, float width, float height)
			: this()
		{
			Left = left;
			Top = top;
			Width = width;
			Height = height;
		}

		public float Left { get; set; }
		public float Top { get; set; }
		public float Width { get; set; }
		public float Height { get; set; }

		public Rectangle(Point position, Size size)
			: this(position.X, position.Y, size.Width, size.Height) {}

		public Rectangle(string rectangleAsString)
			: this()
		{
			string[] componentStrings = rectangleAsString.Split(' ');
			if (componentStrings.Length != 4)
				throw new InvalidNumberOfComponents();
			Left = componentStrings[0].Convert<float>();
			Top = componentStrings[1].Convert<float>();
			Width = componentStrings[2].Convert<float>();
			Height = componentStrings[3].Convert<float>();
		}

		public class InvalidNumberOfComponents : Exception {}

		public static readonly Rectangle Zero = new Rectangle();
		public static readonly Rectangle One = new Rectangle(Point.Zero, Size.One);
		public static readonly Rectangle HalfCentered = Rectangle.FromCenter(Point.Half, Size.Half);
		public static readonly Rectangle Unused = new Rectangle(Point.Unused, Size.Unused);
		public static readonly int SizeInBytes = Marshal.SizeOf(typeof(Rectangle));

		public float Right
		{
			get { return Left + Width; }
			set { Left = value - Width; }
		}

		public float Bottom
		{
			get { return Top + Height; }
			set { Top = value - Height; }
		}

		public Size Size
		{
			get { return new Size(Width, Height); }
		}

		public Point TopLeft
		{
			get { return new Point(Left, Top); }
		}

		public Point TopRight
		{
			get { return new Point(Left + Width, Top); }
		}

		public Point BottomLeft
		{
			get { return new Point(Left, Top + Height); }
		}

		public Point BottomRight
		{
			get { return new Point(Left + Width, Top + Height); }
		}

		public Point Center
		{
			get { return new Point(Left + Width / 2, Top + Height / 2); }
			set
			{
				Left = value.X - Width / 2;
				Top = value.Y - Height / 2;
			}
		}

		[Pure]
		public Rectangle Lerp(Rectangle other, float interpolation)
		{
			return new Rectangle(TopLeft.Lerp(other.TopLeft, interpolation),
				Size.Lerp(other.Size, interpolation));
		}

		public static Rectangle FromCenter(float x, float y, float width, float height)
		{
			return FromCenter(new Point(x, y), new Size(width, height));
		}

		public static Rectangle FromCenter(Point center, Size size)
		{
			return new Rectangle(new Point(center.X - size.Width / 2, center.Y - size.Height / 2), size);
		}

		public static Rectangle FromCorners(Point topLeft, Point bottomRight)
		{
			return new Rectangle(topLeft, new Size(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y));
		}

		[Pure]
		public bool Contains(Point position)
		{
			return position.X >= Left && position.X < Right && position.Y >= Top && position.Y < Bottom;
		}

		public float Aspect
		{
			get { return Width / Height; }
		}

		public Rectangle Increase(Size size)
		{
			return new Rectangle(Left - size.Width / 2, Top - size.Height / 2, Width + size.Width,
				Height + size.Height);
		}

		public Rectangle Reduce(Size size)
		{
			return new Rectangle(Left + size.Width / 2, Top + size.Height / 2, Width - size.Width,
				Height - size.Height);
		}

		[Pure]
		public Rectangle GetInnerRectangle(Rectangle relativeRectangle)
		{
			return new Rectangle(Left + Width * relativeRectangle.Left,
				Top + Height * relativeRectangle.Top, Width * relativeRectangle.Width,
				Height * relativeRectangle.Height);
		}

		[Pure]
		public Point GetRelativePoint(Point point)
		{
			return new Point((point.X - Left) / Width, (point.Y - Top) / Height);
		}

		public Rectangle Move(Point translation)
		{
			return new Rectangle(Left + translation.X, Top + translation.Y, Width, Height);
		}

		public static bool operator ==(Rectangle rect1, Rectangle rect2)
		{
			return rect1.Equals(rect2);
		}

		public static bool operator !=(Rectangle rect1, Rectangle rect2)
		{
			return !rect1.Equals(rect2);
		}

		public override bool Equals(object obj)
		{
			return obj is Rectangle ? Equals((Rectangle)obj) : base.Equals(obj);
		}

		public bool Equals(Rectangle other)
		{
			return TopLeft == other.TopLeft && Size == other.Size;
		}

		public override int GetHashCode()
		{
			// ReSharper disable NonReadonlyFieldInGetHashCode
			return Left.GetHashCode() ^ Top.GetHashCode() ^ Width.GetHashCode() ^ Height.GetHashCode();
		}

		public override string ToString()
		{
			return Left.ToInvariantString() + " " + Top.ToInvariantString() + " " +
				Width.ToInvariantString() + " " + Height.ToInvariantString();
		}

		public Point[] GetRotatedRectangleCorners(Point center, float rotation)
		{
			return new[]
			{
				TopLeft.RotateAround(center, rotation), TopRight.RotateAround(center, rotation),
				BottomRight.RotateAround(center, rotation), BottomLeft.RotateAround(center, rotation)
			};
		}

		public bool IsColliding(float rotation, Rectangle otherRect, float otherRotation)
		{
			var rotatedRect = GetRotatedRectangleCorners(Center, rotation);
			var rotatedOtherRect = otherRect.GetRotatedRectangleCorners(otherRect.Center, otherRotation);
			foreach (var axis in GetAxes(rotatedRect, rotatedOtherRect))
				if (IsProjectedAxisOutsideRectangles(axis, rotatedRect, rotatedOtherRect))
					return false;
			return true;
		}

		private static IEnumerable<Point> GetAxes(Point[] rectangle, Point[] otherRect)
		{
			return new[]
			{
				new Point(rectangle[1].X - rectangle[0].X, rectangle[1].Y - rectangle[0].Y),
				new Point(rectangle[1].X - rectangle[2].X, rectangle[1].Y - rectangle[2].Y),
				new Point(otherRect[0].X - otherRect[3].X, otherRect[0].Y - otherRect[3].Y),
				new Point(otherRect[0].X - otherRect[1].X, otherRect[0].Y - otherRect[1].Y)
			};
		}

		public static bool IsProjectedAxisOutsideRectangles(Point axis, IEnumerable<Point> rotatedRect,
			IEnumerable<Point> rotatedOtherRect)
		{
			var rectMin = float.MaxValue;
			var rectMax = float.MinValue;
			var otherMin = float.MaxValue;
			var otherMax = float.MinValue;
			GetRectangleProjectionResult(axis, rotatedRect, ref rectMin, ref rectMax);
			GetRectangleProjectionResult(axis, rotatedOtherRect, ref otherMin, ref otherMax);
			return rectMin > otherMax || rectMax < otherMin;
		}

		private static void GetRectangleProjectionResult(Point axis, IEnumerable<Point> cornerList,
			ref float min, ref float max)
		{
			foreach (var corner in cornerList)
			{
				float projectedValueX = corner.DistanceFromProjectAxisPointX(axis) * (axis.X);
				float projectedValueY = corner.DistanceFromProjectAxisPointY(axis) * (axis.Y);
				float projectedValue = projectedValueX + projectedValueY;
				if (projectedValue < min)
					min = projectedValue;
				if (projectedValue > max)
					max = projectedValue;
			}
		}

		/// <summary>
		/// Build UV rectangle for a given uv pixel rect and imagePixelSize. Used for FontData.
		/// </summary>
		public static Rectangle BuildUvRectangle(Rectangle uvInPixels, Size imagePixelSize)
		{
			return new Rectangle(uvInPixels.Left / imagePixelSize.Width,
				uvInPixels.Top / imagePixelSize.Height,
				Math.Min(1.0f, uvInPixels.Width / imagePixelSize.Width),
				Math.Min(1.0f, uvInPixels.Height / imagePixelSize.Height));
		}
	}
}