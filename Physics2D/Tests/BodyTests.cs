using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Physics2D.Tests
{
	public class BodyTests : TestWithMocksOrVisually
	{
		[SetUp, CloseAfterFirstFrame]
		public void SetUp()
		{
			physics = Resolve<Physics>();
			physics.Gravity = Point.UnitY;
			body = physics.CreateRectangle(new Size(0.5f));
		}

		private Physics physics;
		private PhysicsBody body;

		[Test]
		public void ChangeLinearVelocity()
		{
			body.LinearVelocity = new Point(5, -5);
			Assert.AreEqual(new Point(5, -5), body.LinearVelocity);
		}

		[Test]
		public void ChangeRotation()
		{
			body.Rotation = 90;
			Assert.AreEqual(90, body.Rotation);
		}

		[Test]
		public void ChangePosition()
		{
			body.Position = Point.One;
			Assert.AreEqual(Point.One, body.Position);
		}

		[Test]
		public void ChangeStatic()
		{
			body.IsStatic = true;
			Assert.IsTrue(body.IsStatic);
		}

		[Test]
		public void ChangeRestitution()
		{
			body.Restitution = 1;
			Assert.AreEqual(1, body.Restitution);
		}

		[Test]
		public void ChangeFriction()
		{
			body.Friction = 0.5f;
			Assert.AreEqual(0.5f, body.Friction);
		}

		[Test]
		public void ApplyLinearImpulse()
		{
			body.ApplyLinearImpulse(Point.Zero);
			AdvanceTimeAndUpdateEntities(0.1f);
			Assert.AreNotEqual(Point.Zero, body.Position);
		}

		[Test]
		public void ApplyAngularImpulse()
		{
			body.ApplyAngularImpulse(10.0f);
			AdvanceTimeAndUpdateEntities();
			Assert.AreNotEqual(0.0f, body.Rotation);
		}

		[Test]
		public void ApplyTorque()
		{
			body.ApplyTorque(10.0f);
			AdvanceTimeAndUpdateEntities();
			Assert.AreNotEqual(0.0f, body.Rotation);
		}

		[Test]
		public void CreateEdgeFromTwoSinglePoints()
		{
			Point[] edgePoints = { Point.Zero, Point.Half };
			var edge = physics.CreateEdge(Point.Zero, Point.Half);
			Assert.AreEqual(edgePoints, edge.LineVertices);
		}

		[Test]
		public void CreateEdgeFromPointArray()
		{
			Point[] edgePoints = { Point.Zero, Point.Half };
			var edge = physics.CreateEdge(edgePoints);
			Assert.AreEqual(edgePoints, edge.LineVertices);
		}

		[Test]
		public void CreateCircleAndGetVertices()
		{
			var circle = physics.CreateCircle(0.5f);
			Assert.IsNotEmpty(circle.LineVertices);
		}

		[Test]
		public void CreatePolygonFromPoints()
		{
			Point[] polyPoints = { Point.Zero, Point.Half, Point.One, Point.UnitX };
			var polygon = physics.CreatePolygon(polyPoints);
			Assert.IsNotEmpty(polygon.LineVertices);
		}

		[Test]
		public void DisposeBody()
		{
			body.Dispose();
		}
	}
}