using CreepyTowers.Creeps;
using CreepyTowers.Towers;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;
using Randomizer = DeltaEngine.Core.Randomizer;

namespace CreepyTowers.Tests.Creeps
{
	public class ClothCreepStateChangerTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void CreateClothCreep()
		{
			creep = new Creep(CreepType.Cloth, Vector3D.Zero, 0);
		}

		private Creep creep;

		[Test]
		public void NonClothCreepsShouldBeIgnored()
		{
			var glassCreep = new Creep(CreepType.Glass, Vector3D.Zero, 0);
			ClothCreepStateChanger.ChangeStatesIfClothCreep(TowerType.Acid, glassCreep);
			Assert.AreEqual(creep.state.Wet, glassCreep.state.Wet);
		}

		[Test]
		public void CheckForIceTowerEffect()
		{
			ClothCreepStateChanger.ChangeStatesIfClothCreep(TowerType.Ice, creep);
			Assert.IsTrue(creep.state.Slow);
			Assert.AreEqual(-1, creep.state.SlowTimer);
		}

		[Test]
		public void CheckForImpactTowerEffect()
		{
			ClothCreepStateChanger.ChangeStatesIfClothCreep(TowerType.Impact, creep);
			Assert.IsTrue(creep.state.Slow);
			Assert.AreEqual(0, creep.state.SlowTimer);
		}

		[Test]
		public void ChecForkWaterTowerEffect()
		{
			ClothCreepStateChanger.ChangeStatesIfClothCreep(TowerType.Water, creep);
			Assert.IsTrue(creep.state.Slow);
			Assert.AreEqual(0, creep.state.SlowTimer);
		}

		[Test]
		public void CheckForAcidTowerEffect()
		{
			ClothCreepStateChanger.ChangeStatesIfClothCreep(TowerType.Acid, creep);
			Assert.IsTrue(creep.state.Enfeeble);
			Assert.AreEqual(0, creep.state.EnfeebleTimer);
		}

		[Test]
		public void CheckForFireTowerOnDryClothCreep()
		{
			ClothCreepStateChanger.ChangeStatesIfClothCreep(TowerType.Fire, creep);
			Assert.IsTrue(creep.state.Burst);
			Assert.AreEqual(0, creep.state.BurstTimer);
		}

		[Test]
		public void CheckForFireTowerWetClothCreep()
		{
			creep.state.Wet = true;
			ClothCreepStateChanger.ChangeStatesIfClothCreep(TowerType.Fire, creep);
			Assert.IsFalse(creep.state.Wet);
			Assert.AreEqual(CreepState.VulnerabilityType.HardBoiled,
				creep.state.GetVulnerability(TowerType.Impact));
			Assert.AreEqual(CreepState.VulnerabilityType.HardBoiled,
				creep.state.GetVulnerability(TowerType.Ice));
			Assert.AreEqual(CreepState.VulnerabilityType.Weak,
				creep.state.GetVulnerability(TowerType.Slice));
			Assert.AreEqual(CreepState.VulnerabilityType.Vulnerable,
				creep.state.GetVulnerability(TowerType.Acid));
			Assert.AreEqual(CreepState.VulnerabilityType.Vulnerable,
				creep.state.GetVulnerability(TowerType.Fire));
		}

		[Test]
		public void CheckForFireTowerOnFrozenClothCreep()
		{
			creep.state.Frozen = true;
			ClothCreepStateChanger.ChangeStatesIfClothCreep(TowerType.Fire, creep);
			Assert.IsFalse(creep.state.Frozen);
			Assert.IsFalse(creep.state.Burst);
			Assert.IsFalse(creep.state.Burn);
			Assert.IsTrue(creep.state.Wet);
			Assert.AreEqual(0, creep.state.WetTimer);
			Assert.AreEqual(CreepState.VulnerabilityType.Resistant,
				creep.state.GetVulnerability(TowerType.Fire));
			Assert.AreEqual(CreepState.VulnerabilityType.HardBoiled,
				creep.state.GetVulnerability(TowerType.Impact));
			Assert.AreEqual(CreepState.VulnerabilityType.Weak,
				creep.state.GetVulnerability(TowerType.Ice));
		}

		[Test]
		public void CheckForWaterTowerOnFrozenClothCreep()
		{
			creep.state.Frozen = true;
			ClothCreepStateChanger.ChangeStatesIfClothCreep(TowerType.Water, creep);
			Assert.IsTrue(creep.state.Frozen);
		}

		[Test]
		public void CheckForImpactTowerOnFrozenClothCreep()
		{
			Randomizer.Use(new FixedRandom(new[] { 0f }));
			creep.state.Frozen = true;
			ClothCreepStateChanger.ChangeStatesIfClothCreep(TowerType.Impact, creep);
			Assert.IsTrue(creep.state.Frozen);
		}

		[Test]
		public void CheckForIceTowerOnFrozenClothCreep()
		{
			creep.state.Frozen = true;
			ClothCreepStateChanger.ChangeStatesIfClothCreep(TowerType.Ice, creep);
		}

		[Test]
		public void CheckForWaterTowerOnDryClothCreep()
		{
			ClothCreepStateChanger.ChangeStatesIfClothCreep(TowerType.Water, creep);
			Assert.IsFalse(creep.state.Frozen);
			Assert.IsFalse(creep.state.Burst);
			Assert.IsFalse(creep.state.Burn);
			Assert.IsTrue(creep.state.Wet);
			Assert.AreEqual(0, creep.state.WetTimer);
			Assert.AreEqual(CreepState.VulnerabilityType.Resistant,
				creep.state.GetVulnerability(TowerType.Fire));
			Assert.AreEqual(CreepState.VulnerabilityType.HardBoiled,
				creep.state.GetVulnerability(TowerType.Impact));
			Assert.AreEqual(CreepState.VulnerabilityType.Weak,
				creep.state.GetVulnerability(TowerType.Ice));
		}

		[Test]
		public void CheckForIceTowerOnWetClothCreep()
		{
			creep.state.Wet = true;
			ClothCreepStateChanger.ChangeStatesIfClothCreep(TowerType.Ice, creep);
			Assert.IsTrue(creep.state.Frozen);
			Assert.IsTrue(creep.state.Paralysed);
			Assert.AreEqual(0, creep.state.FrozenTimer);
			Assert.AreEqual(CreepState.VulnerabilityType.Resistant,
				creep.state.GetVulnerability(TowerType.Slice));
			Assert.AreEqual(CreepState.VulnerabilityType.Resistant,
				creep.state.GetVulnerability(TowerType.Water));
			Assert.AreEqual(CreepState.VulnerabilityType.Vulnerable,
				creep.state.GetVulnerability(TowerType.Impact));
			Assert.AreEqual(CreepState.VulnerabilityType.Immune,
				creep.state.GetVulnerability(TowerType.Fire));
		}

		[Test]
		public void CheckForIceTowerOnDryClothCreep()
		{
			ClothCreepStateChanger.ChangeStatesIfClothCreep(TowerType.Ice, creep);
			Assert.IsFalse(creep.state.Wet);
			Assert.IsFalse(creep.state.Burst);
			Assert.IsFalse(creep.state.Burn);
		}
	}
}