using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace DeltaEngine.Physics3D
{
	/// <summary>
	/// Main physics class used for performing simulations and creating bodies and shapes.
	/// </summary>
	public abstract class Physics : RapidUpdateable
	{
		public void RapidUpdate()
		{
			if (!IsPaused)
				Simulate(Time.Delta);
		}

		public bool IsPaused { get; set; }
		protected abstract void Simulate(float delta);

		protected void AddBody(PhysicsBody body)
		{
			bodies.Add(body);
		}

		protected readonly List<PhysicsBody> bodies = new List<PhysicsBody>();
		protected PhysicsBody groundBody;
		protected readonly List<PhysicsJoint> joints = new List<PhysicsJoint>();
		public bool IsMultithreaded { get; set; }
		public Vector DefaultGravity { get; set; }

		protected abstract void SetGravity(Vector gravity);
		protected abstract double GetTotalPhysicsTime();
		protected abstract PhysicsBody CreateBody(PhysicsShape shape, Vector initialPosition);
		protected abstract void RemoveJoint(PhysicsJoint joint);
		protected abstract void RemoveBody(PhysicsBody body);
		public abstract RaycastResult DoRayCastIncludingGround(Ray ray);
		public abstract RaycastResult DoRayCastExcludingGround(Ray ray);
		public abstract bool IsShapeSupported(ShapeType shapeType);
		public abstract bool IsJointSupported(JointType jointType);
		public abstract void SetGroundPlane(bool enable, float height);
		public abstract PhysicsJoint CreateJoint(JointType jointType, PhysicsBody bodyA,
			PhysicsBody bodyB, object[] args);
	}
}