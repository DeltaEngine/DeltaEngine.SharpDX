using DeltaEngine.Extensions;
using System;
using System.Runtime.InteropServices;

namespace DeltaEngine.Datatypes
{
	/// <summary>
	/// Plane struct represented by a normal vector and a distance from the origin.
	/// Details can be found at: http://en.wikipedia.org/wiki/Plane_%28geometry%29
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Plane : IEquatable<Plane>
	{
		public Plane(Vector normal, float distance)
		{
			Normal = Vector.Normalize(normal);
			Distance = distance;
		}

		public Vector Normal;
		public float Distance;

		public Plane(Vector normal, Vector vectorOnPlane)
		{
			Normal = Vector.Normalize(normal);
			Distance = -Vector.Dot(normal, vectorOnPlane);
		}

		public Vector? Intersect(Ray ray)
		{
			float numerator = Vector.Dot(Normal, ray.Origin) + Distance;
			float denominator = Vector.Dot(Normal, ray.Direction);
			if (denominator.IsNearlyEqual(0.0f))
				return null;
			float distance = -(numerator / denominator);
			if (distance < 0.0f)
				return null;
			return ray.Origin + ray.Direction * distance;
		}

		public bool Equals(Plane other)
		{
			return Normal == other.Normal && Distance == other.Distance;
		}
	}
}