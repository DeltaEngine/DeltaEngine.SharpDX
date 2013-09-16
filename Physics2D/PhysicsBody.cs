using DeltaEngine.Datatypes;

namespace DeltaEngine.Physics2D
{
	/// <summary>
	/// Represents a body which responds to physics
	/// </summary>
	public interface PhysicsBody
	{
		Point Position { get; set; }
		bool IsStatic { get; set; }
		float Restitution { get; set; }
		float Friction { get; set; }
		float Rotation { get; set; }
		Point LinearVelocity { get; set; }
		Point[] LineVertices { get; }
		void ApplyLinearImpulse(Point impulse);
		void ApplyAngularImpulse(float impulse);
		void ApplyTorque(float torque);
		void Dispose();
	}
}