using CreepyTowers.Creeps;
using CreepyTowers.Levels;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests.Levels
{
	[Category("Slow")]
	public class GameLevelRoomTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void SetLevelUp()
		{
			level = CreateMockLevelRoom();
		}

		private GameLevelRoom level;

		private static GameLevelRoom CreateMockLevelRoom()
		{
			var level = new GameLevelRoom(new Size(8, 8));
			level.SpawnPoints.Add(new Vector2D(1, 1));
			level.GoalPoints.Add(new Vector2D(7, 7));
			return level; 
		}

		[Test, CloseAfterFirstFrame]
		public void CreateLevel()
		{
			Assert.AreEqual(new Vector2D(1, 1), level.SpawnPoints[0]);
			Assert.AreEqual(new Vector2D(7, 7), level.GoalPoints[0]);
			Assert.AreEqual(new Size(8, 8), level.Size);
		}

		[Test, CloseAfterFirstFrame]
		public void SpawnCreep()
		{
			Assert.AreEqual(0, level.creeps.Count);
			level.SpawnCreep(CreepType.Glass);
			Assert.AreEqual(1, level.creeps.Count);
			var creep = level.creeps[0];
			Assert.AreEqual(CreepType.Glass, creep.Data.Type);
			Assert.AreEqual(new Vector2D(1, 1), creep.Position.GetVector2D());
			Assert.AreEqual(new Vector2D(7, 7), creep.Target.GetVector2D());
			Assert.IsTrue(creep.Path.Contains(new Vector2D(3, 3)));
		}

		[Test, CloseAfterFirstFrame]
		public void RemoveCreep()
		{
			level.SpawnCreep(CreepType.Glass);
			var creep = level.creeps[0];
			level.RemoveCreep(creep);
			Assert.IsFalse(creep.IsActive);
			Assert.IsFalse(creep.IsVisible);
			Assert.IsFalse(creep.hitpointBar.IsVisible);
			Assert.IsFalse(creep.hitpointBar.IsActive);
			Assert.AreEqual(0, level.creeps.Count);
		}
	}
}
