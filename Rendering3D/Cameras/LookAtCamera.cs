using DeltaEngine.Commands;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics;

namespace DeltaEngine.Rendering3D.Cameras
{
	// Can be rotated at a fixed distance from and always looking at a target
	public class LookAtCamera : Camera
	{
		public LookAtCamera(Device device, Window window)
			: base(device, window)
		{
			new Command(Command.Zoom, Zoom);
		}

		public override Vector3D Target
		{
			get { return base.Target; }
			set
			{
				base.Target = value;
				ComputeCameraRotation();
			}
		}

		private void ComputeCameraRotation()
		{
			Vector3D lookVector = GetLookVector();
			lookVector.Normalize();
			yawPitchRoll.Y = MathExtensions.Asin(-lookVector.Z);
			yawPitchRoll.X = -MathExtensions.Atan2(-lookVector.X, -lookVector.Y);
		}

		private Vector3D GetLookVector()
		{
			return Target - Position;
		}

		private Vector3D yawPitchRoll;

		public Vector3D YawPitchRoll
		{
			get { return yawPitchRoll; }
			set
			{
				yawPitchRoll = value;
				UpdatePosition();
			}
		}

		private void UpdatePosition()
		{
			Matrix rotationMatrix = Matrix.CreateRotationX(yawPitchRoll.Y) *
				Matrix.CreateRotationZ(yawPitchRoll.X);
			float lookDistance = GetLookVector().Length;
			Position = Target + rotationMatrix.TransformNormal(new Vector3D(0, lookDistance, 0));
		}

		public void Zoom(float amount)
		{
			Vector3D lookVector = GetLookVector();
			float lookDistance = lookVector.Length;
			if (amount > lookDistance)
				amount = lookDistance - MathExtensions.Epsilon;
			Position = Position + lookVector / lookDistance * amount;
		}
	}
}