using System;
using System.Runtime.InteropServices;
using DeltaEngine.Extensions;

namespace DeltaEngine.Datatypes
{
	/// <summary>
	/// Plane struct represented by a normal vector and a distance from the origin.
	/// Details can be found at: http://en.wikipedia.org/wiki/Plane_%28geometry%29
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Plane : IEquatable<Plane>
	{
		public Plane(Vector3D normal, float distance)
		{
			Normal = Vector3D.Normalize(normal);
			Distance = distance;
		}

		public Vector3D Normal;
		public float Distance;

		public Plane(Vector3D normal, Vector3D vectorOnPlane)
		{
			Normal = Vector3D.Normalize(normal);
			Distance = -Vector3D.Dot(normal, vectorOnPlane);
		}

		public Vector3D? Intersect(Ray ray)
		{
			float numerator = Vector3D.Dot(Normal, ray.Origin) + Distance;
			float denominator = Vector3D.Dot(Normal, ray.Direction);
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