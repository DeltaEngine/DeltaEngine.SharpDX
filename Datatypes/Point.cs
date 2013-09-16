using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using DeltaEngine.Extensions;

namespace DeltaEngine.Datatypes
{
	/// <summary>
	/// Represents a 2D vector, which is useful for screen positions (sprites, mouse, touch, etc.)
	/// </summary>
	[DebuggerDisplay("Point({X}, {Y})")]
	public struct Point : IEquatable<Point>, Lerp<Point>
	{
		public Point(float x, float y)
			: this()
		{
			X = x;
			Y = y;
		}
		
		public Point(string pointAsString)
			: this()
		{
			float[] components = pointAsString.SplitIntoFloats();
			if (components.Length != 2)
				throw new InvalidNumberOfComponents();
			X = components[0];
			Y = components[1];
		}

		public float X { get; set; }
		public float Y { get; set; }

		public class InvalidNumberOfComponents : Exception {}

		public static readonly Point Zero = new Point();
		public static readonly Point One = new Point(1, 1);
		public static readonly Point Half = new Point(0.5f, 0.5f);
		public static readonly Point UnitX = new Point(1, 0);
		public static readonly Point UnitY = new Point(0, 1);
		public static readonly Point Unused = new Point(-1, -1);
		public static readonly int SizeInBytes = Marshal.SizeOf(typeof(Point));

		public static Point operator +(Point p1, Point p2)
		{
			return new Point(p1.X + p2.X, p1.Y + p2.Y);
		}

		public static Point operator -(Point p1, Point p2)
		{
			return new Point(p1.X - p2.X, p1.Y - p2.Y);
		}

		public static Point operator *(float f, Point p)
		{
			return new Point(p.X * f, p.Y * f);
		}

		public static Point operator *(Point p, float f)
		{
			return new Point(p.X * f, p.Y * f);
		}

		public static Point operator *(Point p, Size scale)
		{
			return new Point(p.X * scale.Width, p.Y * scale.Height);
		}

		public static Point operator /(Point p, float f)
		{
			return new Point(p.X / f, p.Y / f);
		}

		public static Point operator /(Point p, Size scale)
		{
			return new Point(p.X / scale.Width, p.Y / scale.Height);
		}

		public static bool operator !=(Point p1, Point p2)
		{
			return p1.Equals(p2) == false;
		}

		public static bool operator ==(Point p1, Point p2)
		{
			return p1.Equals(p2);
		}

		public static Point operator -(Point p)
		{
			return new Point(-p.X, -p.Y);
		}

		[Pure]
		public bool Equals(Point other)
		{
			return X.IsNearlyEqual(other.X) && Y.IsNearlyEqual(other.Y);
		}

		[Pure]
		public override bool Equals(object other)
		{
			return other is Point ? Equals((Point)other) : base.Equals(other);
		}

		public static implicit operator Point(Size s)
		{
			return new Point(s.Width, s.Height);
		}

		[Pure]
		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode();
		}

		[Pure]
		public override string ToString()
		{
			return X.ToInvariantString() + ", " + Y.ToInvariantString();
		}

		[Pure]
		public float Length
		{
			get { return (float)Math.Sqrt(X * X + Y * Y); }
		}

		[Pure]
		public float LengthSquared
		{
			get { return X * X + Y * Y; }
		}

		[Pure]
		public float DistanceTo(Point other)
		{
			float distanceX = X - other.X;
			float distanceY = Y - other.Y;
			return (float)Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
		}

		[Pure]
		public float DistanceToSquared(Point other)
		{
			float distanceX = X - other.X;
			float distanceY = Y - other.Y;
			return distanceX * distanceX + distanceY * distanceY;
		}

		[Pure]
		public Point DirectionTo(Point other)
		{
			return other - this;
		}

		public Point ReflectIfHittingBorder(Rectangle box, Rectangle borders)
		{
			if (box.Width >= borders.Width || box.Height >= borders.Height)
				return this;
			if (box.Left <= borders.Left)
				X = X.Abs();
			if (box.Right >= borders.Right)
				X = -X.Abs();
			if (box.Top <= borders.Top)
				Y = Y.Abs();
			if (box.Bottom >= borders.Bottom)
				Y = -Y.Abs();
			return this;
		}

		[Pure]
		public Point Lerp(Point other, float interpolation)
		{
			return new Point(X.Lerp(other.X, interpolation), Y.Lerp(other.Y, interpolation));
		}

		[Pure]
		public Point RotateAround(Point center, float angleInDegrees)
		{
			return RotateAround(center, MathExtensions.Sin(angleInDegrees),
				MathExtensions.Cos(angleInDegrees));
		}

		[Pure]
		public Point RotateAround(Point center, float rotationSin, float rotationCos)
		{
			var translatedPoint = this - center;
			return new Point(
				center.X + translatedPoint.X * rotationCos - translatedPoint.Y * rotationSin,
				center.Y + translatedPoint.X * rotationSin + translatedPoint.Y * rotationCos);
		}

		[Pure]
		public float RotationTo(Point target)
		{
			var normal = Normalize(this - target);
			return MathExtensions.Atan2(normal.Y, normal.X);
		}

		public static Point Normalize(Point point)
		{
			var length = (float)Math.Sqrt(point.X * point.X + point.Y * point.Y);
			point.X /= length;
			point.Y /= length;
			return point;
		}

		[Pure]
		public float DotProduct(Point point)
		{
			return X * point.X + Y * point.Y;
		}

		[Pure]
		public float DistanceFromProjectAxisPointX(Point axis)
		{
			return (X * axis.X + Y * axis.Y) / (axis.X * axis.X + axis.Y * axis.Y) * axis.X;
		}

		[Pure]
		public float DistanceFromProjectAxisPointY(Point axis)
		{
			return (X * axis.X + Y * axis.Y) / (axis.X * axis.X + axis.Y * axis.Y) * axis.Y;
		}

		/// <summary>
		/// http://en.wikipedia.org/wiki/Distance_from_a_point_to_a_line
		/// </summary>
		[Pure]
		public float DistanceToLine(Point lineStart, Point lineEnd)
		{
			var lineDirection = lineEnd - lineStart;
			var lineLengthSquared = lineDirection.LengthSquared;
			if (lineLengthSquared == 0.0)
				return DistanceTo(lineStart);
			var startDirection = this - lineStart;
			var linePosition = startDirection.DotProduct(lineDirection) / lineLengthSquared;
			var projection = lineStart + linePosition * lineDirection;
			return DistanceTo(projection);
		}

		/// <summary>
		/// http://stackoverflow.com/questions/849211/shortest-distance-between-a-point-and-a-line-segment
		/// </summary>
		[Pure]
		public float DistanceToLineSegment(Point lineStart, Point lineEnd)
		{
			var lineDirection = lineEnd - lineStart;
			var lineLengthSquared = lineDirection.LengthSquared;
			if (lineLengthSquared == 0.0)
				return DistanceTo(lineStart);
			var startDirection = this - lineStart;
			var linePosition = startDirection.DotProduct(lineDirection) / lineLengthSquared;
			if (linePosition < 0.0)
				return DistanceTo(lineStart);
			if (linePosition > 1.0)
				return DistanceTo(lineEnd);
			var projection = lineStart + linePosition * lineDirection;
			return DistanceTo(projection);
		}

		/// <summary>
		/// http://stackoverflow.com/questions/3461453/determine-which-side-of-a-line-a-point-lies
		/// </summary>
		[Pure]
		public bool IsLeftOfLineOrOnIt(Point start, Point end)
		{
			return ((end.X - start.X) * (Y - start.Y) - (end.Y - start.Y) * (X - start.X)) >= 0;
		}
	}
}