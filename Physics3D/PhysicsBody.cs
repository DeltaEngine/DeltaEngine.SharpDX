using DeltaEngine.Datatypes;

namespace DeltaEngine.Physics3D
{
	/// <summary>
	/// Represents a body which responds to physics
	/// </summary>
	public abstract class PhysicsBody
	{
		protected PhysicsBody(PhysicsShape shape)
		{
			Shape = shape;
		}

		public PhysicsShape Shape { get; private set; }
		
		//PhysicsBody Collision begin and end
		protected virtual void OnCollisionBegin(PhysicsBody other)
		{
			//if (CollisionBegin != null)
			//	CollisionBegin(other);
		}

		//private Action<PhysicsBody> CollisionBegin;

		protected virtual void OnCollisionEnd(PhysicsBody other)
		{
		}

		//private Action<PhysicsBody> CollisionEnd;

		public abstract Vector Position { get; set; }
		public abstract Point Position2D { get; }
		public abstract Matrix RotationMatrix { get; set; }
		public abstract Vector LinearVelocity { get; set; }
		public abstract Vector AngularVelocity { get; set; }
		public abstract float AngularVelocity2D { get; set; }
		public virtual float Mass { get; set; }
		public virtual float Restitution { get; set; }
		public abstract BoundingBox BoundingBox { get; }
		public abstract void ApplyForce(Vector force);
		public abstract void ApplyForce(Vector force, Vector position);
		public abstract void ApplyTorque(Vector torque);
		public abstract void ApplyLinearImpulse(Vector impulse);
		public abstract void ApplyLinearImpulse(Vector impulse, Vector position);
		public abstract void ApplyAngularImpulse(Vector impulse);
		protected abstract void SetIsStatic(bool value);
		protected abstract void SetIsActive(bool value);
		protected abstract void SetFriction(float value);
	}
}