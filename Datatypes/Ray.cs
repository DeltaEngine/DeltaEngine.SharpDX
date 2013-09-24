using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace DeltaEngine.Datatypes
{
	/// <summary>
	/// Ray struct, used to fire rays into a 3D scene to find out what we can
	/// hit with that ray (for mouse picking and other simple collision stuff).
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	[DebuggerDisplay("Ray(Origin {Origin}, Direction {Direction})")]
	public struct Ray : IEquatable<Ray>
	{
		public Ray(Vector3D origin, Vector3D direction)
		{
			Origin = origin;
			Direction = direction;
		}

		public Vector3D Origin;
		public Vector3D Direction;

		public bool Equals(Ray other)
		{
			return Origin == other.Origin && Direction == other.Direction;
		}

		[Pure]
		public override string ToString()
		{
			return "Origin [" + Origin + "] Direction [" + Direction + "]";
		}
	}
}