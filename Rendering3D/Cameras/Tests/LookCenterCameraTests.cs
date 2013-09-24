using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D.Shapes3D;
using NUnit.Framework;

namespace DeltaEngine.Rendering3D.Cameras.Tests
{
	public class LookCenterCameraTests : TestWithMocksOrVisually
	{
		[Test]
		public void RenderGrid()
		{
			LookCenterCamera camera = CreateLookCenterCamera(new Vector3D(0.0f, -5.0f, 5.0f), Vector3D.Zero);
			new Grid3D(9);
			Command.Register(Command.Zoom, new MouseZoomTrigger());
			new Command(Command.Zoom, delegate(float zoomAmount) { camera.Zoom(zoomAmount); });
			Vector2D lastMovePosition = Vector2D.Zero;
			new Command(Command.Drag, (startPos, currentPosition, isDragDone) =>
			{
				Vector2D moveDifference = currentPosition - lastMovePosition;
				lastMovePosition = isDragDone ? Vector2D.Zero : currentPosition;
				if (moveDifference == currentPosition)
					return;
				const float RotationSpeed = 100;
				Vector3D newYawPitchRoll = camera.YawPitchRoll;
				newYawPitchRoll.X += moveDifference.X * RotationSpeed;
				newYawPitchRoll.Y += moveDifference.Y * RotationSpeed;
				camera.YawPitchRoll = newYawPitchRoll;
			});
		}

		private static LookCenterCamera CreateLookCenterCamera(Vector3D position, Vector3D target)
		{
			var camera = Camera.Use<LookCenterCamera>();
			camera.Position = position;
			camera.Target = target;
			return camera;
		}
	}
}