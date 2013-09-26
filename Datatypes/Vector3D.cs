using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using DeltaEngine.Extensions;

namespace DeltaEngine.Datatypes
{
	/// <summary>
	/// Specifies a position in 3D space
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3D : IEquatable<Vector3D>, Lerp<Vector3D>
	{
		public Vector3D(float setX, float setY, float setZ)
			: this()
		{
			X = setX;
			Y = setY;
			Z = setZ;
		}

		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }

		public Vector3D(Vector2D setFromVector2D, float setZ = 0.0f)
			: this()
		{
			X = setFromVector2D.X;
			Y = setFromVector2D.Y;
			Z = setZ;
		}

		public Vector3D(string vectorAsString)
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

		public static readonly Vector3D Zero;
		public static readonly Vector3D One = new Vector3D(1, 1, 1);
		public static readonly Vector3D UnitX = new Vector3D(1, 0, 0);
		public static readonly Vector3D UnitY = new Vector3D(0, 1, 0);
		public static readonly Vector3D UnitZ = new Vector3D(0, 0, 1);
		public static readonly int SizeInBytes = Marshal.SizeOf(typeof(Vector3D));

		public static float Dot(Vector3D vector1, Vector3D vector2)
		{
			return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
		}

		public static Vector3D Cross(Vector3D vector1, Vector3D vector2)
		{
			return new Vector3D(vector1.Y * vector2.Z - vector1.Z * vector2.Y,
				vector1.Z * vector2.X - vector1.X * vector2.Z,
				vector1.X * vector2.Y - vector1.Y * vector2.X);
		}

		public static Vector3D Normalize(Vector3D vector)
		{
			float distanceSquared = vector.LengthSquared;
			if (distanceSquared == 0.0f)
				return vector;

			float distanceInverse = 1.0f / MathExtensions.Sqrt(distanceSquared);
			return new Vector3D(vector.X * distanceInverse, vector.Y * distanceInverse,
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

		public Vector3D TransformNormal(Matrix matrix)
		{
			return matrix.TransformNormal(this);
		}

		public float LengthSquared
		{
			get { return X * X + Y * Y + Z * Z; }
		}

		[Pure]
		public Vector3D Lerp(Vector3D other, float interpolation)
		{
			return new Vector3D(X.Lerp(other.X, interpolation), Y.Lerp(other.Y, interpolation),
				Z.Lerp(other.Z, interpolation));
		}

		public static float AngleBetweenVectors(Vector3D vector1, Vector3D vector2)
		{
			float dotProduct = Dot(vector1, vector2);
			var cosine = dotProduct / (vector1.Length * vector2.Length);
			return (float)(Math.Acos(cosine) * 180.0 / Math.PI);
		}

		public float Length
		{
			get { return MathExtensions.Sqrt(X * X + Y * Y + Z * Z); }
		}

		public static Vector3D operator +(Vector3D v1, Vector3D v2)
		{
			return new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
		}

		public static Vector3D operator -(Vector3D v1, Vector3D v2)
		{
			return new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
		}

		public static Vector3D operator *(Vector3D v, float f)
		{
			return new Vector3D(v.X * f, v.Y * f, v.Z * f);
		}

		public static Vector3D operator *(float f, Vector3D v)
		{
			return new Vector3D(v.X * f, v.Y * f, v.Z * f);
		}

		public static Vector3D operator *(Vector3D left, Vector3D right)
		{
			return new Vector3D(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
		}

		public static Vector3D operator /(Vector3D v, float f)
		{
			return new Vector3D(v.X / f, v.Y / f, v.Z / f);
		}

		public static void Divide(ref Vector3D v, float f, ref Vector3D result)
		{
			float inverse = 1.0f / f;
			result.X = v.X * inverse;
			result.Y = v.Y * inverse;
			result.Z = v.Z * inverse;
		}

		public static Vector3D operator -(Vector3D value)
		{
			return new Vector3D(-value.X, -value.Y, -value.Z);
		}

		public static bool operator !=(Vector3D v1, Vector3D v2)
		{
			return v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z;
		}

		public static bool operator ==(Vector3D v1, Vector3D v2)
		{
			return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
		}

		[Pure]
		public bool Equals(Vector3D other)
		{
			return X == other.X && Y == other.Y && Z == other.Z;
		}

		[Pure]
		public override bool Equals(object other)
		{
			return other is Vector3D ? Equals((Vector3D)other) : base.Equals(other);
		}

		[Pure]
		public bool IsNearlyEqual(Vector3D other)
		{
			return X.IsNearlyEqual(other.X) && Y.IsNearlyEqual(other.Y) && Z.IsNearlyEqual(other.Z);
		}

		public static implicit operator Vector3D(Vector2D vector2D)
		{
			return new Vector3D(vector2D.X, vector2D.Y, 0);
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
		}

		public override string ToString()
		{
			return "{" + X.ToInvariantString() + ", " + Y.ToInvariantString() + ", " +
				Z.ToInvariantString() + "}";
		}

		public Vector2D GetVector2D()
		{
			return new Vector2D(X, Y);
		}
	}
}