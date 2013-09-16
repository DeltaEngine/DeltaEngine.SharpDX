using System;
using DeltaEngine.Datatypes;
using DeltaEngine.Input;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace Breakout.Tests
{
	public class PaddleTests : TestWithMocksOrVisually
	{
		[Test]
		public void Draw(Type type)
		{
			var ball = Resolve<TestBall>();
			var paddle = Resolve<Paddle>();
			Assert.AreEqual(0.5f, paddle.Position.X);
			Assert.IsTrue(ball.IsCurrentlyOnPaddle);
			Assert.AreEqual(0.5f, ball.Position.X);
		}

		[Test]
		public void ControlPaddleVirtuallyWithKeyboard()
		{
			Resolve<TestBall>();
			var paddle = Resolve<Paddle>();
			var keyboard = Resolve<MockKeyboard>();
			keyboard.SetKeyboardState(Key.CursorLeft, State.Pressed);
			AssertPaddleMovesLeftCorrectly(paddle);
			keyboard.SetKeyboardState(Key.CursorLeft, State.Released);
			keyboard.SetKeyboardState(Key.CursorRight, State.Pressed);
			AssertPaddleMovesRightCorrectly(paddle);
		}

		private void AssertPaddleMovesLeftCorrectly(Paddle paddle)
		{
			Assert.AreEqual(0.5f, paddle.Position.X);
			AdvanceTimeAndUpdateEntities(0.1f);
			Assert.IsTrue(paddle.Position.X < 0.5f);
			Assert.IsTrue(paddle.Position.Y > 0.75f);
		}

		// ReSharper disable UnusedParameter.Local
		private void AssertPaddleMovesRightCorrectly(Paddle paddle)
		{
			AdvanceTimeAndUpdateEntities(0.2f);
			Assert.IsTrue(paddle.Position.X > 0.5f);
		}

		// ReSharper restore UnusedParameter.Local

		[Test, Ignore]
		public void ControlPaddleVirtuallyWithGamePad()
		{
			var paddle = Resolve<Paddle>();
			var gamePad = Resolve<MockGamePad>();
			gamePad.SetGamePadState(GamePadButton.Left, State.Pressed);
			AssertPaddleMovesLeftCorrectly(paddle);
			gamePad.SetGamePadState(GamePadButton.Left, State.Released);
			gamePad.SetGamePadState(GamePadButton.Right, State.Pressed);
			AssertPaddleMovesRightCorrectly(paddle);
		}

		[Test, Ignore]
		public void ControlPaddleVirtuallyWithMouseAndTouch()
		{
			Resolve<TestBall>();
			var paddle = Resolve<Paddle>();
			var mouse = Resolve<MockMouse>();
			mouse.SetPosition(Point.Zero);
			mouse.SetButtonState(MouseButton.Left, State.Pressed);
			AssertPaddleMovesLeftCorrectly(paddle);
			mouse.SetButtonState(MouseButton.Left, State.Released);
			mouse.SetPosition(Point.One);
			mouse.SetButtonState(MouseButton.Left, State.Pressed);
			AssertPaddleMovesRightCorrectly(paddle);
		}

		[Test]
		public void IsBallReleasedAfterSpacePressed()
		{
			var ball = Resolve<TestBall>();
			Resolve<Paddle>();
			PressSpaceOneSecond();
			AssertBallIsReleasedAndPaddleStay(ball);
		}

		private void PressSpaceOneSecond()
		{
			Resolve<MockKeyboard>().SetKeyboardState(Key.Space, State.Pressing);
			AdvanceTimeAndUpdateEntities(1);
		}

		private static void AssertBallIsReleasedAndPaddleStay(TestBall remBall)
		{
			if (remBall != null)
			{
				Assert.IsFalse(remBall.IsCurrentlyOnPaddle);
				Assert.AreNotEqual(0.5f, remBall.Position.X);
			}
		}
	}
}