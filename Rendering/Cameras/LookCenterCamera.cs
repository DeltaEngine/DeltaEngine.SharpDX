using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics;

namespace DeltaEngine.Rendering.Cameras
{
	// Can be rotated at a fixed distance from and always looking at a target
	public class LookCenterCamera : Camera
	{
		public LookCenterCamera(Device device, Window window)
			: base(device, window) {}

		public override Vector Target
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
			Vector lookVector = GetLookVector();
			lookVector.Normalize();
			yawPitchRoll.Y = MathExtensions.Asin(-lookVector.Z);
			yawPitchRoll.X = -MathExtensions.Atan2(-lookVector.X, -lookVector.Y);
		}

		private Vector GetLookVector()
		{
			return Target - Position;
		}

		private Vector yawPitchRoll;

		public Vector YawPitchRoll
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
			Position = Target + rotationMatrix.TransformNormal(new Vector(0, lookDistance, 0));
		}

		public void Zoom(float amount)
		{
			Vector lookVector = GetLookVector();
			float lookDistance = lookVector.Length;
			if (amount > lookDistance)
				amount = lookDistance - MathExtensions.Epsilon;
			Position = Position + lookVector / lookDistance * amount;
		}
	}
}