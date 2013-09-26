using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D.Shapes3D;
using NUnit.Framework;

namespace DeltaEngine.Rendering3D.Cameras.Tests
{
	public class LookAtCameraTests : TestWithMocksOrVisually
	{
		[Test, CloseAfterFirstFrame]
		public void PositionToTargetDistance()
		{
			var camera = CreateLookAtCamera(Vector3D.UnitZ * 5.0f, Vector3D.Zero);
			Assert.AreEqual(5.0f, camera.Position.Length - camera.Target.Length);
		}

		private static LookAtCamera CreateLookAtCamera(Vector3D position, Vector3D target)
		{
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = position;
			camera.Target = target;
			return camera;
		}

		[Test, CloseAfterFirstFrame]
		public void RotateCamera90DegreesYAxis()
		{
			var camera = CreateLookAtCamera(Vector3D.UnitZ, Vector3D.Zero);
			camera.YawPitchRoll = new Vector3D(0.0f, 90.0f, 0.0f);
			Assert.AreEqual(Vector3D.UnitZ, camera.Position);
			Assert.AreEqual(Vector3D.Zero, camera.Target);
		}

		[Test, CloseAfterFirstFrame]
		public void LookAtEntity3D()
		{
			var entity = new Entity3D(Vector3D.One * 5.0f, Quaternion.Identity);
			var camera = CreateLookAtCamera(Vector3D.Zero, entity);
			Assert.AreEqual(camera.Target, entity.Position);
		}

		private static LookAtCamera CreateLookAtCamera(Vector3D position, Entity3D target)
		{
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = position;
			camera.Target = target.Position;
			return camera;
		}

		[Test, CloseAfterFirstFrame]
		public void ZoomTowardTheTarget()
		{
			var camera = CreateLookAtCamera(Vector3D.UnitX * 2.0f, Vector3D.Zero);
			camera.Zoom(1.0f);
			Assert.AreEqual(Vector3D.UnitX, camera.Position);
		}

		[Test, CloseAfterFirstFrame]
		public void ZoomOutwardTheTarget()
		{
			var camera = CreateLookAtCamera(Vector3D.UnitX, Vector3D.Zero);
			camera.Zoom(-1.0f);
			Assert.AreEqual(Vector3D.UnitX * 2.0f, camera.Position);
		}

		[Test, CloseAfterFirstFrame]
		public void OverZoomTowardTheTarget()
		{
			var camera = CreateLookAtCamera(Vector3D.UnitX * 3.0f, Vector3D.Zero);
			camera.Zoom(100.0f);
			Assert.AreEqual(Vector3D.Zero, camera.Position);
		}

		[Test, CloseAfterFirstFrame]
		public void WorldToScreenPoint()
		{
			var camera = CreateLookAtCamera(Vector3D.One, Vector3D.Zero);
			var point = camera.WorldToScreenPoint(Vector3D.Zero);
			Assert.AreEqual(Vector2D.Half, point);
		}

		[Test, CloseAfterFirstFrame]
		public void CenterOfScreenWithLookAtCameraPointsToTarget()
		{
			VerifyScreenCenterIsTarget(new Vector3D(3.0f, 3.0f, 3.0f), new Vector3D(1.0f, 1.0f, 2.0f));
			VerifyScreenCenterIsTarget(new Vector3D(1.0f, 4.0f, 1.5f), new Vector3D(-2.9f, 0.0f, 2.5f));
			VerifyScreenCenterIsTarget(new Vector3D(-1.0f, -4.0f, 2.5f), new Vector3D(2.9f, -1.0f, 3.5f));
		}

		private static void VerifyScreenCenterIsTarget(Vector3D position, Vector3D target)
		{
			var camera = CreateLookAtCamera(position, target);
			var floor = new Plane(Vector3D.UnitY, target);
			Ray ray = camera.ScreenPointToRay(Vector2D.Half);
			Assert.AreEqual(target, floor.Intersect(ray));
		}

		[Test]
		public void RenderGrid()
		{
			LookAtCamera camera = CreateLookCenterCamera(new Vector3D(0.0f, -5.0f, 5.0f), Vector3D.Zero);
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

		private static LookAtCamera CreateLookCenterCamera(Vector3D position, Vector3D target)
		{
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = position;
			camera.Target = target;
			return camera;
		}
	}
}