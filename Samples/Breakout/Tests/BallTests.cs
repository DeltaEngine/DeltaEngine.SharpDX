using System;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Input;
using DeltaEngine.Input.Mocks;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace Breakout.Tests
{
	public class BallTests : TestWithMocksOrVisually
	{
		[Test]
		public void Draw(Type type)
		{
			Resolve<Paddle>();
			Resolve<EmptyLevel>();
			Resolve<TestBall>();
		}

		[Test]
		public void FireBall()
		{
			var ball = Resolve<Ball>();
			Assert.IsTrue(ball.IsVisible == true);
			AdvanceTimeAndUpdateEntities(0.1f);
			var initialBallPosition = new Vector2D(0.5f, 0.86f);
			Assert.AreEqual(initialBallPosition, ball.Position);
			Resolve<MockKeyboard>().SetKeyboardState(Key.Space, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.1f);
			Assert.AreNotEqual(initialBallPosition, ball.Position);
		}

		[Test]
		public void ReflectBall()
		{
			var ball = Resolve<Ball>();
			var keyboard = Resolve<MockKeyboard>();
			keyboard.SetKeyboardState(Key.Space, State.Pressing);
			keyboard.SetKeyboardState(Key.CursorRight, State.Pressing);
			ball.DrawArea = Rectangle.FromCenter(new Vector2D(0.1f, 0.2f), ball.Size);
			AdvanceTimeAndUpdateEntities(0.1f);
			Assert.AreNotEqual(0.5f, ball.Position.X);
		}

		[Test]
		public void BallShouldFollowPaddle()
		{
			var ball = Resolve<Ball>();
			var paddle = Resolve<Paddle>();
			Assert.AreEqual(0.5f, ball.Position.X);
			Assert.AreEqual(ball.Position.X, paddle.Position.X);
			Resolve<MockKeyboard>().SetKeyboardState(Key.CursorLeft, State.Pressed);
			AdvanceTimeAndUpdateEntities(0.1f);
			Assert.AreNotEqual(0.5f, ball.Position.X);
			Assert.AreEqual(ball.Position.X, paddle.Position.X);
		}

		[Test, Ignore]
		public void BounceOnRightSideToMoveLeft()
		{
			var ball = Resolve<TestBall>();
			Resolve<Paddle>();
			ball.CurrentVelocity = new Vector2D(0.5f, 0f);
			Assert.AreEqual(new Vector2D(0.5f, 0f), ball.CurrentVelocity);
			ball.SetPosition(new Vector2D(1, 0.5f));
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(new Vector2D(-0.5f, 0f), ball.CurrentVelocity);
		}

		[Test]
		public void BallSize()
		{
			Assert.AreEqual(new Size(0.04f), Ball.BallSize);
		}

		[Test]
		public void BounceOnLeftSideToMoveRight()
		{
			var paddle = Resolve<Paddle>();
			var ball = new TestBall(paddle);
			
			ball.CurrentVelocity = new Vector2D(0.5f, 0.1f);
			ball.SetPosition(new Vector2D(0, 0.5f));
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(new Vector2D(0.5f, 0.1f), ball.CurrentVelocity);
		}

		[Test,Ignore]
		public void BounceOnTopSideToMoveDown()
		{
			var ball = Resolve<TestBall>();
			Resolve<Paddle>();
			ball.CurrentVelocity = new Vector2D(-0.5f, -0.5f);
			ball.SetPosition(new Vector2D(0.5f, 0));
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(new Vector2D(-0.5f, 0.5f), ball.CurrentVelocity);
		}

		[Test, Ignore]
		public void BounceOnBottomSideToLoseBall()
		{
			var ball = Resolve<TestBall>();
			Resolve<Paddle>();
			ball.CurrentVelocity = new Vector2D(-0.5f, -0.5f);
			ball.SetPosition(new Vector2D(0.5f, 1.0f));
			Assert.IsFalse(ball.IsCurrentlyOnPaddle);
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.IsTrue(ball.IsCurrentlyOnPaddle);
			Assert.AreEqual(Vector2D.Zero, ball.CurrentVelocity);
		}

		[Test, Ignore]
		public void PaddleCollision()
		{
			var ball = Resolve<TestBall>();
			var paddle = Resolve<Paddle>();
			AdvanceTimeAndUpdateEntities(0.01f);
			ball.CurrentVelocity = new Vector2D(0f, 0.1f);
			ball.SetPosition(paddle.Position);
			Assert.IsFalse(ball.IsCurrentlyOnPaddle);
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(new Vector2D(0f, -0.1015f), ball.CurrentVelocity);
		}

		[Test]
		public void ReleaseBallTwice()
		{
			var ball = Resolve<TestBall>();
			Resolve<Paddle>();
			Resolve<MockKeyboard>().SetKeyboardState(Key.Space, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.01f);
			Vector2D velocity = ball.CurrentVelocity;
			Resolve<MockKeyboard>().SetKeyboardState(Key.Space, State.Pressing);
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(velocity, ball.CurrentVelocity);
		}

		[Test, Ignore]
		public void GetDrawPosition()
		{
			var ball = Resolve<TestBall>();
			Resolve<Paddle>();
			AdvanceTimeAndUpdateEntities(0.01f);
			Assert.AreEqual(new Rectangle(0.48f, 0.84f, 0.04f, 0.04f), ball.DrawArea);
		}
	}
}