using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using DeltaEngine.Extensions;

namespace DeltaEngine.Datatypes
{
	/// <summary>
	/// Specifies a position in 3D space (the 3D version of Point).
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector : IEquatable<Vector>, Lerp<Vector>
	{
		public Vector(float setX, float setY, float setZ)
			: this()
		{
			X = setX;
			Y = setY;
			Z = setZ;
		}

		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }

		public Vector(Point setFromPoint, float setZ = 0.0f)
			: this()
		{
			X = setFromPoint.X;
			Y = setFromPoint.Y;
			Z = setZ;
		}

		public Vector(string vectorAsString)
			: this()
		{
			var floats = vectorAsString.SplitIntoFloats();
			if (floats.Length != 3)
				throw new InvalidNumberOfComponents();
			X = floats[0];
			Y = floats[1];
			Z = floats[2];
		}

		public class InvalidNumberOfComponents : Exception {}

		public static readonly Vector Zero;
		public static readonly Vector One = new Vector(1, 1, 1);
		public static readonly Vector UnitX = new Vector(1, 0, 0);
		public static readonly Vector UnitY = new Vector(0, 1, 0);
		public static readonly Vector UnitZ = new Vector(0, 0, 1);
		public static readonly int SizeInBytes = Marshal.SizeOf(typeof(Vector));

		public static float Dot(Vector vector1, Vector vector2)
		{
			return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
		}

		public static Vector Cross(Vector vector1, Vector vector2)
		{
			return new Vector(
				vector1.Y * vector2.Z - vector1.Z * vector2.Y,
				vector1.Z * vector2.X - vector1.X * vector2.Z,
				vector1.X * vector2.Y - vector1.Y * vector2.X);
		}

		public static Vector Normalize(Vector vector)
		{
			float distanceSquared = vector.LengthSquared;
			if (distanceSquared == 0.0f)
				return vector;

			float distanceInverse = 1.0f / MathExtensions.Sqrt(distanceSquared);
			return new Vector(
				vector.X * distanceInverse,
				vector.Y * distanceInverse,
				vector.Z * distanceInverse);
		}

		public void Normalize()
		{
			if (LengthSquared == 0.0f || LengthSquared == 1.0f)
				return;

			float distanceInverse = 1.0f / MathExtensions.Sqrt(LengthSquared);
			X *= distanceInverse;
			Y *= distanceInverse;
			Z *= distanceInverse;
		}

		public Vector TransformNormal(Matrix matrix)
		{
			return matrix.TransformNormal(this);
		}

		public float LengthSquared
		{
			get { return X * X + Y * Y + Z * Z; }
		}

		[Pure]
		public Vector Lerp(Vector other, float interpolation)
		{
			return new Vector(X.Lerp(other.X, interpolation), Y.Lerp(other.Y, interpolation),
				Z.Lerp(other.Z, interpolation));
		}

		public static float AngleBetweenVectors(Vector vector1, Vector vector2)
		{
			float dotProduct = Dot(vector1, vector2);
			var cosine = dotProduct / (vector1.Length * vector2.Length);
			return (float)(Math.Acos(cosine)*180.0/Math.PI);
		}

		public float Length
		{
			get { return MathExtensions.Sqrt(X * X + Y * Y + Z * Z); }
		}

		public static Vector operator +(Vector v1, Vector v2)
		{
			return new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
		}

		public static Vector operator -(Vector v1, Vector v2)
		{
			return new Vector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
		}

		public static Vector operator *(Vector v, float f)
		{
			return new Vector(v.X * f, v.Y * f, v.Z * f);
		}

		public static Vector operator *(float f, Vector v)
		{
			return new Vector(v.X * f, v.Y * f, v.Z * f);
		}

		public static Vector operator *(Vector left, Vector right)
		{
			return new Vector(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
		}

		public static Vector operator /(Vector v, float f)
		{
			return new Vector(v.X / f, v.Y / f, v.Z / f);
		}

		public static void Divide(ref Vector v, float f, ref Vector result)
		{
			float inverse = 1.0f / f;
			result.X = v.X * inverse;
			result.Y = v.Y * inverse;
			result.Z = v.Z * inverse;
		}

		public static Vector operator -(Vector value)
		{
			return new Vector(-value.X, -value.Y, -value.Z);
		}

		public static bool operator !=(Vector v1, Vector v2)
		{
			return v1.Equals(v2) == false;
		}

		public static bool operator ==(Vector v1, Vector v2)
		{
			return v1.Equals(v2);
		}

		public bool Equals(Vector other)
		{
			return X.IsNearlyEqual(other.X) && Y.IsNearlyEqual(other.Y) && Z.IsNearlyEqual(other.Z);
		}

		public override bool Equals(object other)
		{
			return other is Vector ? Equals((Vector)other) : base.Equals(other);
		}

		public static implicit operator Vector(Point p)
		{
			return new Vector(p.X, p.Y, 0);
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
		}

		public override string ToString()
		{
			return X.ToInvariantString() + ", " + Y.ToInvariantString() + ", " + Z.ToInvariantString();
		}

		public Point Get2DPoint()
		{
			return new Point(X, Y);
		}
	}
}