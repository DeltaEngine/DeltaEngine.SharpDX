using CreepyTowers.Creeps;
using CreepyTowers.Levels;
using CreepyTowers.Towers;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D.Particles;
using DeltaEngine.Rendering3D.Shapes3D;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	public class FindPossibleTargetsTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Initialize()
		{
			new Game(Resolve<Window>());
			grid = new LevelGrid(24, 0.20f);
			new Manager(6.0f);
		}

		private LevelGrid grid;

		[Test]
		public void InAbsenceOfCreepsTheTowerDoesNotFire()
		{
			new Tower(TowerType.Water, Vector3D.Zero);
			Assert.AreEqual(1, EntitiesRunner.Current.GetEntitiesOfType<Tower>().Count);
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Particle3DPointEmitter>().Count);
		}

		[Test]
		public void TowerFiresAtCreep()
		{
			var towerPos = grid.PropertyMatrix[2, 3].MidPoint;
			var creepPos = grid.PropertyMatrix[2, 2].MidPoint;
			new Tower(TowerType.Water, towerPos);
			var creep = new Creep(CreepType.Cloth, creepPos, 0);
			var creepHealthBefore = creep.CurrentHp;
			AdvanceTimeAndUpdateEntities(0.1f);
			Assert.Less(0, EntitiesRunner.Current.GetEntitiesOfType<Particle3DPointEmitter>().Count);
			Assert.Less(creepHealthBefore, creep.CurrentHp);
		}

		[Test]
		public void TowerDoesNotAttackIfAttackFrequencyIsHigherThanElapsedTime()
		{
			var towerPos = grid.PropertyMatrix[2, 3].MidPoint;
			var creepPos = grid.PropertyMatrix[2, 2].MidPoint;
			new Tower(TowerType.Water, towerPos);
			var creep = new Creep(CreepType.Cloth, creepPos, 0);
			var creepHealthBefore = creep.CurrentHp;
			AdvanceTimeAndUpdateEntities();
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Line3D>().Count);
			Assert.AreEqual(creepHealthBefore, creep.CurrentHp);
		}
	}
}