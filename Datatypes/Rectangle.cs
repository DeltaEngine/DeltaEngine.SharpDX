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

		public Rectangle(Vector2D position, Size size)
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

		public static Rectangle FromPoints(IEnumerable<Vector2D> points)
		{
			var leftUpper = new Vector2D(float.MaxValue, float.MaxValue);
			var rightLower = new Vector2D(float.MinValue, float.MinValue);
			foreach (var point in points)
			{
				if (point.X < leftUpper.X)
					leftUpper.X = point.X;
				else if (point.X > rightLower.X)
					rightLower.X = point.X;
				if (point.Y < leftUpper.Y)
					leftUpper.Y = point.Y;
				else if (point.Y > rightLower.Y)
					rightLower.Y = point.Y;
			}
			return FromCorners(leftUpper, rightLower);
		}

		public Rectangle Merge(Rectangle other)
		{
			var leftUpper = Vector2D.Zero;
			var rightLower = Vector2D.Zero;
			leftUpper.X = MathExtensions.Min(Left, other.Left);
			leftUpper.Y = MathExtensions.Min(Top, other.Top);
			rightLower.X = MathExtensions.Max(Right, other.Right);
			rightLower.Y = MathExtensions.Max(Bottom, other.Bottom);
			return FromCorners(leftUpper, rightLower);
		}

		public class InvalidNumberOfComponents : Exception {}

		public static readonly Rectangle Zero = new Rectangle();
		public static readonly Rectangle One = new Rectangle(Vector2D.Zero, Size.One);
		public static readonly Rectangle HalfCentered = FromCenter(Vector2D.Half, Size.Half);
		public static readonly Rectangle Unused = new Rectangle(Vector2D.Unused, Size.Unused);
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

		public Vector2D TopLeft
		{
			get { return new Vector2D(Left, Top); }
		}

		public Vector2D TopRight
		{
			get { return new Vector2D(Left + Width, Top); }
		}

		public Vector2D BottomLeft
		{
			get { return new Vector2D(Left, Top + Height); }
		}

		public Vector2D BottomRight
		{
			get { return new Vector2D(Left + Width, Top + Height); }
		}

		public Vector2D Center
		{
			get { return new Vector2D(Left + Width / 2, Top + Height / 2); }
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
			return FromCenter(new Vector2D(x, y), new Size(width, height));
		}

		public static Rectangle FromCenter(Vector2D center, Size size)
		{
			return new Rectangle(new Vector2D(center.X - size.Width / 2, center.Y - size.Height / 2),
				size);
		}

		public static Rectangle FromCorners(Vector2D topLeft, Vector2D bottomRight)
		{
			return new Rectangle(topLeft, new Size(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y));
		}

		[Pure]
		public bool Contains(Vector2D point)
		{
			return point.X >= Left && point.X < Right && point.Y >= Top && point.Y < Bottom;
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
		public Vector2D GetRelativePoint(Vector2D point)
		{
			return new Vector2D((point.X - Left) / Width, (point.Y - Top) / Height);
		}

		public Rectangle Move(Vector2D translation)
		{
			return new Rectangle(Left + translation.X, Top + translation.Y, Width, Height);
		}

		public static bool operator ==(Rectangle rect1, Rectangle rect2)
		{
			return rect1.Top == rect2.Top && rect1.Left == rect2.Left && rect1.Width == rect2.Width &&
				rect1.Height == rect2.Height;
		}

		public static bool operator !=(Rectangle rect1, Rectangle rect2)
		{
			return rect1.Top != rect2.Top || rect1.Left != rect2.Left || rect1.Width != rect2.Width ||
				rect1.Height != rect2.Height;
		}

		public override bool Equals(object obj)
		{
			return obj is Rectangle ? Equals((Rectangle)obj) : base.Equals(obj);
		}

		public bool Equals(Rectangle other)
		{
			return TopLeft.Equals(other.TopLeft) && Size.Equals(other.Size);
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

		public Vector2D[] GetRotatedRectangleCorners(Vector2D center, float rotation)
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

		private static IEnumerable<Vector2D> GetAxes(Vector2D[] rectangle, Vector2D[] otherRect)
		{
			return new[]
			{
				new Vector2D(rectangle[1].X - rectangle[0].X, rectangle[1].Y - rectangle[0].Y),
				new Vector2D(rectangle[1].X - rectangle[2].X, rectangle[1].Y - rectangle[2].Y),
				new Vector2D(otherRect[0].X - otherRect[3].X, otherRect[0].Y - otherRect[3].Y),
				new Vector2D(otherRect[0].X - otherRect[1].X, otherRect[0].Y - otherRect[1].Y)
			};
		}

		public static bool IsProjectedAxisOutsideRectangles(Vector2D axis,
			IEnumerable<Vector2D> rotatedRect, IEnumerable<Vector2D> rotatedOtherRect)
		{
			var rectMin = float.MaxValue;
			var rectMax = float.MinValue;
			var otherMin = float.MaxValue;
			var otherMax = float.MinValue;
			GetRectangleProjectionResult(axis, rotatedRect, ref rectMin, ref rectMax);
			GetRectangleProjectionResult(axis, rotatedOtherRect, ref otherMin, ref otherMax);
			return rectMin > otherMax || rectMax < otherMin;
		}

		private static void GetRectangleProjectionResult(Vector2D axis,
			IEnumerable<Vector2D> cornerList, ref float min, ref float max)
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

		public bool IntersectsCircle(Vector2D center, float radius)
		{
			Vector2D clampedLocation = Vector2D.Zero;
			if (center.X > Right)
				clampedLocation.X = Right;
			else if (center.X < Left)
				clampedLocation.X = Left;
			else
				clampedLocation.X = center.X;
			if (center.Y > Bottom)
				clampedLocation.Y = Bottom;
			else if (center.Y < Top)
				clampedLocation.Y = Top;
			else
				clampedLocation.Y = center.Y;
			return clampedLocation.DistanceToSquared(center) <= (radius * radius);
		}
	}
}