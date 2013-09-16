using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Shapes3D;
using NUnit.Framework;

namespace DeltaEngine.Rendering.Cameras.Tests
{
	public class LookAtCameraTests : TestWithMocksOrVisually
	{
		[Test, CloseAfterFirstFrame]
		public void PositionToTargetDistance()
		{
			var camera = CreateLookAtCamera(Vector.UnitZ * 5.0f, Vector.Zero);
			Assert.AreEqual(5.0f, camera.Distance);
		}

		private static LookAtCamera CreateLookAtCamera(Vector position, Vector target)
		{
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = position;
			camera.Target = target;
			return camera;
		}

		[Test, CloseAfterFirstFrame]
		public void RotateCamera90DegreesYAxis()
		{
			var camera = CreateLookAtCamera(Vector.UnitZ, Vector.Zero);
			camera.Rotation = new Vector(0.0f, 90.0f, 0.0f);
			Assert.AreEqual(Vector.UnitX, camera.Position);
			Assert.AreEqual(Vector.Zero, camera.Target);
		}

		[Test, CloseAfterFirstFrame]
		public void LookAtEntity3D()
		{
			var entity = new Entity3D(Vector.One * 5.0f, Quaternion.Identity);
			var camera = CreateLookAtCamera(Vector.Zero, entity);
			Assert.AreEqual(camera.Target, entity.Position);
		}

		private static LookAtCamera CreateLookAtCamera(Vector position, Entity3D target)
		{
			var camera = Camera.Use<LookAtCamera>();
			camera.Position = position;
			camera.EntityTarget = target;
			return camera;
		}

		[Test, CloseAfterFirstFrame]
		public void ZoomTowardTheTarget()
		{
			var camera = CreateLookAtCamera(Vector.UnitX * 2.0f, Vector.Zero);
			camera.Zoom(1.0f);
			Assert.AreEqual(Vector.UnitX, camera.Position);
		}

		[Test, CloseAfterFirstFrame]
		public void ZoomOutwardTheTarget()
		{
			var camera = CreateLookAtCamera(Vector.UnitX, Vector.Zero);
			camera.Zoom(-1.0f);
			Assert.AreEqual(Vector.UnitX * 2.0f, camera.Position);
		}

		[Test, CloseAfterFirstFrame]
		public void OverZoomTowardTheTarget()
		{
			var camera = CreateLookAtCamera(Vector.UnitX * 3.0f, Vector.Zero);
			camera.Zoom(100.0f);
			Assert.AreEqual(Vector.Zero, camera.Position);
		}

		[Test, CloseAfterFirstFrame]
		public void WorldToScreenPoint()
		{
			var camera = CreateLookAtCamera(Vector.One, Vector.Zero);
			var point = camera.WorldToScreenPoint(Vector.Zero);
			Assert.AreEqual(Point.Half, point);
		}

		[Test]
		public void RenderGrid()
		{
			var camera = CreateLookAtCamera(new Vector(0.0f, -5.0f, 5.0f), Vector.Zero);
			new Grid3D(10);
			new Command(Command.Zoom, camera.Zoom);
		}

		[Test, CloseAfterFirstFrame]
		public void CenterOfScreenWithLookAtCameraPointsToTarget()
		{
			VerifyScreenCenterIsTarget(new Vector(3.0f, 3.0f, 3.0f), new Vector(1.0f, 1.0f, 2.0f));
			VerifyScreenCenterIsTarget(new Vector(1.0f, 4.0f, 1.5f), new Vector(-2.9f, 0.0f, 2.5f));
			VerifyScreenCenterIsTarget(new Vector(-1.0f, -4.0f, 2.5f), new Vector(2.9f, -1.0f, 3.5f));
		}

		private static void VerifyScreenCenterIsTarget(Vector position, Vector target)
		{
			var camera = CreateLookAtCamera(position, target);
			var floor = new Plane(Vector.UnitY, target);
			Ray ray = camera.ScreenPointToRay(Point.Half);
			Assert.AreEqual(target, floor.Intersect(ray));
		}
	}
}