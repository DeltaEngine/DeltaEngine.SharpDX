using DeltaEngine.Datatypes;
using NUnit.Framework;

namespace DeltaEngine.GameLogic.Tests
{
	public class ActorTests
	{
		[Test]
		public void TestActorSpawnDespawnNotChangeAnything()
		{
			var actor = new MockActor(Vector3D.One, Quaternion.Identity);
			Assert.AreEqual(Vector3D.One, actor.Position);
		}
	}

	public class MockActor : Actor
	{
		public MockActor(Vector3D position, Quaternion orientation)
		{
			Position = position;
			Orientation = orientation;
			Scale = Vector3D.One;
		}

		public Vector3D Position { get; set; }
		public Quaternion Orientation { get; set; }
		public Vector3D Scale { get; set; }
	}
}