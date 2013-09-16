using System.Linq;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Physics2D.Farseer.Tests
{
	public class PhysicsTests : TestWithMocksOrVisually
	{
		[SetUp, CloseAfterFirstFrame]
		public void SetUp()
		{
			physics = new FarseerPhysics();
		}

		private FarseerPhysics physics;

		[Test]
		public void CheckDefaultValues()
		{
			Assert.IsFalse(physics.IsPaused);
			Assert.AreEqual(new Point(0.0f, 98.2f), physics.Gravity);
			Assert.AreEqual(0, physics.Bodies.Count());
		}

		[Test]
		public void ChangeGravity()
		{
			physics.Gravity = Point.UnitY;
			Assert.AreEqual(Point.UnitY, physics.Gravity);
		}

		[Test]
		public void CreateBody()
		{
			VerifyBodyIsCreated(physics.CreateRectangle(Size.One));
		}

		private void VerifyBodyIsCreated(PhysicsBody body)
		{
			Assert.IsNotNull(body);
			Assert.AreEqual(1, physics.Bodies.Count());
		}

		[Test]
		public void CreateEdge()
		{
			VerifyBodyIsCreated(physics.CreateEdge(Point.Zero, Point.One));
		}

		[Test]
		public void CreateEdgeMultiPoints()
		{
			VerifyBodyIsCreated(physics.CreateEdge(Points));
		}

		private static readonly Point[] Points = new[]
		{ Point.Zero, Point.UnitX, Point.One, Point.UnitY };

		[Test]
		public void CreatePolygon()
		{
			VerifyBodyIsCreated(physics.CreatePolygon(Points));
		}

		[Test]
		public void CheckWorldIsSimulated()
		{
			var body = physics.CreateRectangle(Size.One);
			AdvanceTimeAndUpdateEntities();
			Assert.AreNotEqual(Point.Zero, body.Position);
		}
	}
}