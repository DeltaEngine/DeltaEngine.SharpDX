using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;

namespace ShadowShot
{
	/// <summary>
	/// Component for any Entity moving around in 2D space with velocity limited to a maximum value
	/// It can be accelerated (or decelerated, which is the same) by a vector, by magnitude and direction,
	/// or by a scalar factor
	/// </summary>
	public class Velocity2D
	{
		public Velocity2D(Vector2D velocity, float maximumSpeed)
		{
			this.velocity = velocity;
			this.maximumSpeed = maximumSpeed;
		}

		public Vector2D velocity;
		public readonly float maximumSpeed;

		public void Accelerate(Vector2D acceleration2D)
		{
			Velocity += acceleration2D;
		}

		public Vector2D Velocity
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
			Velocity = new Vector2D(velocity.X + MathExtensions.Sin(angle) * magnitude,
				velocity.Y - MathExtensions.Cos(angle) * magnitude);
		}

		public void Accelerate(float factor)
		{
			Velocity *= factor;
		}
	}
}