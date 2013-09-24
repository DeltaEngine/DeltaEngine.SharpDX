using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics;

namespace DeltaEngine.Rendering3D.Cameras
{
	/// <summary>
	/// Orthogonal 3D camera.
	/// </summary>
	public class OrthoCamera : Camera
	{
		public OrthoCamera(Device device, Window window)
			: base(device, window) {}

		public Size Size
		{
			get { return size; }
			set
			{
				size = value;
				GetCurrentProjectionMatrix();
			}
		}

		private Size size;

		protected override Matrix GetCurrentProjectionMatrix()
		{
			return Matrix.CreateOrthoProjection(Size, NearPlane, FarPlane);
		}
	}
}