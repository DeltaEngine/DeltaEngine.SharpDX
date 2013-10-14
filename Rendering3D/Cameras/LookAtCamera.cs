using DeltaEngine.Commands;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics;

namespace DeltaEngine.Rendering3D.Cameras
{
	/// <summary>
	/// Basic camera for 3D which can zoom, move and rotate around the Target.
	/// </summary>
	public class LookAtCamera : TargetedCamera
	{
		public LookAtCamera(Device device, Window window)
			: base(device, window)
		{
			new Command(Command.Zoom, Zoom);
			new Command(Command.MoveLeft, () => Move(-MoveSpeedPerSecond, 0));
			new Command(Command.MoveRight, () => Move(MoveSpeedPerSecond, 0));
			new Command(Command.MoveUp, () => Move(0, MoveSpeedPerSecond));
			new Command(Command.MoveDown, () => Move(0, -MoveSpeedPerSecond));
			new Command(Command.Drag,
				(startPos, currentPos, isDragDone) => RotateByDragCommand(currentPos, isDragDone)); //ncrunch: no coverage
		}

		private const float MoveSpeedPerSecond = 10;

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
			Vector3D lookVector = NormalizedLookVector;
			yawPitchRoll.Y = MathExtensions.Asin(-lookVector.Z);
			yawPitchRoll.X = -MathExtensions.Atan2(-lookVector.X, -lookVector.Y);
		}

		private Vector3D NormalizedLookVector
		{
			get { return Vector3D.Normalize(LookVector); }
		}

		private Vector3D LookVector
		{
			get { return Target - Position; }
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
			float lookDistance = LookVector.Length;
			Position = Target + rotationMatrix.TransformNormal(new Vector3D(0, lookDistance, 0));
		}

		public void Zoom(float amount)
		{
			Vector3D lookVector = LookVector;
			float lookDistance = lookVector.Length;
			if (amount > lookDistance)
				amount = lookDistance - MathExtensions.Epsilon;
			Position = Position + lookVector / lookDistance * amount;
		}

		//ncrunch: no coverage start
		private void Move(float rightMovement, float forwardMovement)
		{
			Vector3D lookDirection = NormalizedLookVector;
			lookDirection.Z = 0;
			Vector3D forward = lookDirection * forwardMovement;
			Vector3D right = Vector3D.Cross(lookDirection, UpDirection) * rightMovement;
			Vector3D moveOffset = forward + right;
			moveOffset.Normalize();
			Target += moveOffset;
			Position += moveOffset;
		}

		private void RotateByDragCommand(Vector2D currentScreenPosition, bool isDragDone)
		{
			Vector2D moveDifference = currentScreenPosition - lastMovePosition;
			lastMovePosition = isDragDone ? Vector2D.Zero : currentScreenPosition;
			if (moveDifference == currentScreenPosition)
				return;
			Vector3D newYawPitchRoll = YawPitchRoll;
			newYawPitchRoll.X += moveDifference.X * RotationSpeed;
			newYawPitchRoll.Y += moveDifference.Y * RotationSpeed;
			YawPitchRoll = newYawPitchRoll;
		}
		//ncrunch: no coverage end

		private Vector2D lastMovePosition;
		private const float RotationSpeed = 100;
	}
}