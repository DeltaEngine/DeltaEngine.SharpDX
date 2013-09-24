using System;

namespace DeltaEngine.Rendering3D.Models
{
	/// <summary>
	/// Specifies how Billboards are calculated, especially used for particles
	/// </summary>
	[Flags]
	public enum BillboardMode
	{
		Standard2D,
		/// <summary>
		/// Always looks to the camera in 3D. Commonly used for 3D <see cref="Billboard"/>
		/// </summary>
		CameraFacing = 1,
		/// <summary>
		/// Creates a vertical billboard but rotates around Z axis towards camera
		/// </summary>
		UpAxis = 2,
		/// <summary>
		/// Aligned to the y axis of the coordinate system
		/// </summary>
		FrontAxis = 4,
		/// <summary>
		/// Always parallel to the xy plane, which in the coordinates used by Delta is horizontal.
		/// </summary>
		Ground = 8
	}
}
