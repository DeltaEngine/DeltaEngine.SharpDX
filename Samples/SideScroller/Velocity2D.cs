using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;

namespace SideScroller
{
	/// <summary>
	/// Component for any Entity moving around in 2D space with velocity limited to a maximum value
	/// It can be accelerated (or decelerated, which is the same) by a vector, by magnitude and direction,
	/// or by a scalar factor
	/// </summary>
	public class Velocity2D
	{
		public Velocity2D(Point velocity, float maximumSpeed)
		{
			this.velocity = velocity;
			this.maximumSpeed = maximumSpeed;
		}

		public Point velocity;
		public readonly float maximumSpeed;

		public void Accelerate(Point acceleration2D)
		{
			Velocity += acceleration2D;
		}

		public Point Velocity
		{
			get { return velocity; }
			set
			{
				velocity = value;
				CapAtMaximumSpeed();
			}
		}

		private void CapAtMaximumSpeed()
		{
			float speed = velocity.Length;
			if (speed > maximumSpeed)
				velocity *= maximumSpeed / speed;
		}

		public void Accelerate(float magnitude, float angle)
		{
			Velocity = new Point(velocity.X + MathExtensions.Sin(angle) * magnitude,
				velocity.Y - MathExtensions.Cos(angle) * magnitude);
		}

		public void Accelerate(float factor)
		{
			Velocity *= factor;
		}
	}
}