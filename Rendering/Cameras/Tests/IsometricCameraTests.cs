using System;
using DeltaEngine.Commands;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering.Shapes3D;
using NUnit.Framework;

namespace DeltaEngine.Rendering.Cameras.Tests
{
	public class IsometricCameraTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetUp()
		{
			camera = Camera.Use<IsometricCamera>(Vector.UnitY);
			CreateGridFacingCamera();
		}

		private IsometricCamera camera;

		private static void CreateGridFacingCamera()
		{
			const int Min = -1000;
			const int Max = 1000;
			const int Step = 100;
			for (float i = Min; i <= Max; i += Step)
			{
				new Line3D(new Vector(Min, 0, i), new Vector(Max, 0, i), Color.White);
				new Line3D(new Vector(i, 0, Min), new Vector(i, 0, Max), Color.White);
			}
		}

		[Test]
		public void MoveIsometricCamera()
		{
			RegisterCommand(Key.J, () => camera.MoveLeft(MoveDistance));
			RegisterCommand(Key.L, () => camera.MoveRight(MoveDistance));
			RegisterCommand(Key.I, () => camera.MoveUp(MoveDistance));
			RegisterCommand(Key.K, () => camera.MoveDown(MoveDistance));
			RegisterCommand(Key.U, () => camera.Zoom(ZoomAmount));
			RegisterCommand(Key.O, () => camera.Zoom(1 / ZoomAmount));
		}

		private const int MoveDistance = 5;
		private const float ZoomAmount = 1.05f;

		private static void RegisterCommand(Key key, Action action)
		{
			new Command(action).Add(new KeyTrigger(key, State.Pressed));
		}

		[Test, CloseAfterFirstFrame]
		public void SettingPositionMovesTarget()
		{
			camera.Position = new Vector(10.0f, 10.0f, 10.0f);
			Assert.AreEqual(new Vector(10.0f, 11.0f, 10.0f), camera.Target);
		}

		[Test, CloseAfterFirstFrame]
		public void SettingTargetMovesPosition()
		{
			camera.Target = new Vector(10.0f, 10.0f, 10.0f);
			Assert.AreEqual(new Vector(10.0f, 9.0f, 10.0f), camera.Position);
		}

		[Test, CloseAfterFirstFrame]
		public void MoveLeft()
		{
			camera.MoveLeft(1.0f);
			Assert.AreEqual(new Vector(-1.0f, -1.0f, 0.0f), camera.Position);
		}

		[Test, CloseAfterFirstFrame]
		public void MoveRight()
		{
			camera.MoveRight(1.0f);
			Assert.AreEqual(new Vector(1.0f, -1.0f, 0.0f), camera.Position);
		}

		[Test, CloseAfterFirstFrame]
		public void MoveUp()
		{
			camera.MoveUp(1.0f);
			Assert.AreEqual(new Vector(0.0f, -1.0f, 1.0f), camera.Position);
		}

		[Test, CloseAfterFirstFrame]
		public void MoveDown()
		{
			camera.MoveDown(1.0f);
			Assert.AreEqual(new Vector(0.0f, -1.0f, -1.0f), camera.Position);
		}
	}
}