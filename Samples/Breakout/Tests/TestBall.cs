using DeltaEngine.Datatypes;

namespace Breakout.Tests
{
	public class TestBall : Ball
	{
		public TestBall(Paddle paddle)
			: base(paddle) {}

		public Point CurrentVelocity
		{
			get { return velocity; }
			set { velocity = value; }
		}

		public bool IsCurrentlyOnPaddle
		{
			get { return isOnPaddle; }
		}

		public void SetPosition(Point newPosition)
		{
			isOnPaddle = false;
			Position = newPosition;
		}
	}
}