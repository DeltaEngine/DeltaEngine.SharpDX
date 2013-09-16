using System.Diagnostics;

namespace DeltaEngine.Datatypes
{
	/// <summary>
	/// Min and max vector for a 3D bounding box. Can also be used to calculate a BoundingSphere.
	/// </summary>
	[DebuggerDisplay("BoundingBox(Min={Min}, Max={Max})")]
	public class BoundingBox
	{
		public BoundingBox(Vector min, Vector max)
		{
			Min = min;
			Max = max;
		}

		public Vector Min;
		public Vector Max;
	}
}