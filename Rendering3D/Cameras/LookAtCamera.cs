using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics;

namespace DeltaEngine.Rendering3D.Cameras
{
	/// <summary>
	/// 3D camera that support setting of position and target.
	/// </summary>
	public class LookAtCamera : Camera
	{
		public LookAtCamera(Device device, Window window)
			: base(device, window) {}

		public Entity3D EntityTarget { get; set; }

		protected override Vector3D GetFinalTargetPosition()
		{
			return (EntityTarget != null) ? EntityTarget.Position : base.GetFinalTargetPosition();
		}

		private void UpdateInternalState()
		{
			cameraRotation = new Vector3D(Rotation.X, Rotation.Y.Clamp(MinPitchRotation, MaxPitchRotation),
				cameraRotation.Z);
			var rotationY = Matrix.CreateRotationY(cameraRotation.Y);
			var rotationX = Matrix.CreateRotationX(cameraRotation.X);
			var rotationMatrix = rotationX * rotationY;
			var lookVector = new Vector3D(0.0f, 0.0f, Distance);
			Position = rotationMatrix.TransformNormal(lookVector);
			Position = Position + Target;
		}

		public Vector3D Rotation
		{
			get { return cameraRotation; }
			set
			{
				cameraRotation = value;
				UpdateInternalState();
			}
		}

		private Vector3D cameraRotation;
		private const float MinPitchRotation = -90;
		private const float MaxPitchRotation = 90;

		public float Distance
		{
			get { return (Position - Target).Length; }
		}

		public void Zoom(float amount)
		{
			var lookDirection = Target - Position;
			var directionLength = lookDirection.Length;
			if (amount > directionLength)
				amount = directionLength - MathExtensions.Epsilon;
			lookDirection /= directionLength;
			Position = Position + lookDirection * amount;
		}
	}
}