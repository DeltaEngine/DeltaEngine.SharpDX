using System.Diagnostics;

namespace DeltaEngine.Datatypes
{
	/// <summary>
	/// Contains a center position in 3D and the radius. Allows quick collision and intersection tests.
	/// </summary>
	[DebuggerDisplay("BoundingSphere(Center={Center}, Radius={Radius})")]
	public class BoundingSphere
	{
		public BoundingSphere(Vector center, float radius)
		{
			Center = center;
			Radius = radius;
		}

		public Vector Center { get; set; }
		public float Radius { get; set; }
	}
}
