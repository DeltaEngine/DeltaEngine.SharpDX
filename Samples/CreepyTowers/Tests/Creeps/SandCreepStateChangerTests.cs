using CreepyTowers.Creeps;
using CreepyTowers.Towers;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests.Creeps
{
	public class SandCreepStateChangerTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void CreateSandCreep()
		{
			creep = new Creep(CreepType.Sand, Vector3D.Zero,0);
		}

		private Creep creep;

		[Test]
		public void CheckForImpactTowerEffect()
		{
			SandCreepStateChanger.ChangeStatesIfSandCreep(TowerType.Impact, creep);
			Assert.IsTrue(creep.state.Slow);
			Assert.AreEqual(0, creep.state.SlowTimer);
		}

		[Test]
		public void CheckForWaterTowerEffect()
		{
			SandCreepStateChanger.ChangeStatesIfSandCreep(TowerType.Water, creep);
			Assert.IsTrue(creep.state.Slow);
			Assert.AreEqual(0, creep.state.SlowTimer);
			Assert.IsTrue(creep.state.Wet);
		}

		[Test]
		public void CheckForIceTowerOnWetCreep()
		{
			creep.state.Wet = true;
			SandCreepStateChanger.ChangeStatesIfSandCreep(TowerType.Ice, creep);
			Assert.IsTrue(creep.state.Frozen);
			Assert.IsFalse(creep.state.Wet);
		}

		[Test]
		public void CheckForImpactTowerOnFrozenCreep()
		{
			creep.state.Frozen = true;
			SandCreepStateChanger.ChangeStatesIfSandCreep(TowerType.Impact, creep);
			Assert.IsTrue(creep.state.Frozen);
		}

		[Test]
		public void CheckForFireTowerEffectOnWetCreep()
		{
			creep.state.Wet = true;
			SandCreepStateChanger.ChangeStatesIfSandCreep(TowerType.Fire, creep);
			Assert.IsFalse(creep.state.Wet);
		}
	}
}