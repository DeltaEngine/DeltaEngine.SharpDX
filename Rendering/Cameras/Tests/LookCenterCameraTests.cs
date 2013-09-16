using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Shapes3D;
using NUnit.Framework;

namespace DeltaEngine.Rendering.Cameras.Tests
{
	public class LookCenterCameraTests : TestWithMocksOrVisually
	{
		[Test]
		public void RenderGrid()
		{
			LookCenterCamera camera = CreateLookCenterCamera(new Vector(0.0f, -5.0f, 5.0f), Vector.Zero);
			new Grid3D(9);
			Command.Register(Command.Zoom, new MouseZoomTrigger());
			new Command(Command.Zoom, delegate(float zoomAmount) { camera.Zoom(zoomAmount); });
			Point lastMovePosition = Point.Zero;
			new Command(Command.Drag, (startPos, currentPosition, isDragDone) =>
			{
				Point moveDifference = currentPosition - lastMovePosition;
				lastMovePosition = isDragDone ? Point.Zero : currentPosition;
				if (moveDifference == currentPosition)
					return;
				const float RotationSpeed = 100;
				Vector newYawPitchRoll = camera.YawPitchRoll;
				newYawPitchRoll.X += moveDifference.X * RotationSpeed;
				newYawPitchRoll.Y += moveDifference.Y * RotationSpeed;
				camera.YawPitchRoll = newYawPitchRoll;
			});
		}

		private static LookCenterCamera CreateLookCenterCamera(Vector position, Vector target)
		{
			var camera = Camera.Use<LookCenterCamera>();
			camera.Position = position;
			camera.Target = target;
			return camera;
		}
	}
}