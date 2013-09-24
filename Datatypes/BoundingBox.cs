using System.Diagnostics;
using DeltaEngine.Extensions;

namespace DeltaEngine.Datatypes
{
	/// <summary>
	/// Min and max vector for a 3D bounding box. Can also be used to calculate a BoundingSphere.
	/// </summary>
	[DebuggerDisplay("BoundingBox(Min={Min}, Max={Max})")]
	public class BoundingBox
	{
		public BoundingBox(Vector3D min, Vector3D max)
		{
			Min = min;
			Max = max;
		}

		public Vector3D Min;
		public Vector3D Max;

		public Vector3D? Intersect(Ray ray)
		{
			var oneOverDirection = new Vector3D(1.0f / ray.Direction.X, 1.0f / ray.Direction.Y,
				1.0f / ray.Direction.Z);
			float distMinX = (Min.X - ray.Origin.X) * oneOverDirection.X;
			float distMaxX = (Max.X - ray.Origin.X) * oneOverDirection.X;
			float distMinY = (Min.Y - ray.Origin.Y) * oneOverDirection.Y;
			float distMaxY = (Max.Y - ray.Origin.Y) * oneOverDirection.Y;
			float distMinZ = (Min.Z - ray.Origin.Z) * oneOverDirection.Z;
			float distMaxZ = (Max.Z - ray.Origin.Z) * oneOverDirection.Z;
			float distMax =
				MathExtensions.Min(
					MathExtensions.Min(MathExtensions.Max(distMinX, distMaxX),
						MathExtensions.Max(distMinY, distMaxY)), MathExtensions.Max(distMinZ, distMaxZ));
			if (distMax < 0)
				return null;
			float distMin =
				MathExtensions.Max(
					MathExtensions.Max(MathExtensions.Min(distMinX, distMaxX),
						MathExtensions.Min(distMinY, distMaxY)), MathExtensions.Min(distMinZ, distMaxZ));
			if (distMin > distMax)
				return null;
			return ray.Origin + ray.Direction * distMin;
		}
	}
}