using System;
using System.Collections.Generic;
using CreepyTowers.Creeps;
using CreepyTowers.Levels;
using CreepyTowers.Towers;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	public class ClothCreepStateChangerTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Initialize()
		{
			new Game(Resolve<Window>(), Resolve<Device>());
			creepProp = new CreepProperties
			{
				Name = Names.CreepCottonMummy,
				CreepType = Creep.CreepType.Cloth,
				CurrentHp = 100.0f,
				MaxHp = 200.0f,
				GoldReward = 100,
				Resistance = 3.0f,
				Speed = 3.0f
			};
			creep = new Creep(Vector3D.Zero, Creep.CreepType.Cloth, Names.CreepCottonMummy);
			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector3D(0.0f, 0.0f, 0.0f),
				StartGridPos = new Tuple<int, int>(4, 0),
				Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(1, 0) }
			});
		}

		private CreepProperties creepProp;
		private Creep creep;

		[Test]
		public void CheckForIceTowerEffect()
		{
			ClothCreepStateChanger.ChangeStatesIfClothCreep(Tower.TowerType.Ice, creep, creepProp);
			Assert.IsTrue(creep.state.Slow);
			Assert.AreEqual(0, creep.state.SlowTimer);
		}

		[Test]
		public void CheckForImpactTowerEffect()
		{
			ClothCreepStateChanger.ChangeStatesIfClothCreep(Tower.TowerType.Impact, creep, creepProp);
			Assert.IsTrue(creep.state.Slow);
			Assert.AreEqual(0, creep.state.SlowTimer);
		}

		[Test]
		public void ChecForkWaterTowerEffect()
		{
			ClothCreepStateChanger.ChangeStatesIfClothCreep(Tower.TowerType.Water, creep, creepProp);
			Assert.IsTrue(creep.state.Slow);
			Assert.AreEqual(0, creep.state.SlowTimer);
		}

		[Test]
		public void CheckForAcidTowerEffect()
		{
			ClothCreepStateChanger.ChangeStatesIfClothCreep(Tower.TowerType.Acid, creep, creepProp);
			Assert.IsTrue(creep.state.Enfeeble);
			Assert.AreEqual(0, creep.state.EnfeebleTimer);
		}

		[Test]
		public void CheckForFireTowerOnDryClothCreep()
		{
			ClothCreepStateChanger.ChangeStatesIfClothCreep(Tower.TowerType.Fire, creep, creepProp);
			Assert.IsTrue(creep.state.Burst);
			Assert.AreEqual(0, creep.state.BurstTimer);
		}

		[Test]
		public void CheckForFireTowerWetClothCreep()
		{
			creep.state.Wet = true;
			ClothCreepStateChanger.ChangeStatesIfClothCreep(Tower.TowerType.Fire, creep, creepProp);
			Assert.IsFalse(creep.state.Wet);
			Assert.AreEqual(CreepState.VulnerabilityType.Resistant, creep.state.GetVulnerability(Tower.TowerType.Impact));
			Assert.AreEqual(CreepState.VulnerabilityType.HardBoiled, creep.state.GetVulnerability(Tower.TowerType.Ice));
			Assert.AreEqual(CreepState.VulnerabilityType.Weak, creep.state.GetVulnerability(Tower.TowerType.Blade));
			Assert.AreEqual(CreepState.VulnerabilityType.Vulnerable, creep.state.GetVulnerability(Tower.TowerType.Acid));
			Assert.AreEqual(CreepState.VulnerabilityType.Vulnerable, creep.state.GetVulnerability(Tower.TowerType.Fire));
		}

		[Test]
		public void CheckForFireTowerOnFrozenClothCreep()
		{
			creep.state.Frozen = true;
			ClothCreepStateChanger.ChangeStatesIfClothCreep(Tower.TowerType.Fire, creep, creepProp);
			Assert.IsFalse(creep.state.Frozen);
			Assert.IsFalse(creep.state.Burst);
			Assert.IsFalse(creep.state.Burn);
			Assert.IsTrue(creep.state.Wet);
			Assert.AreEqual(0, creep.state.WetTimer);
			Assert.AreEqual(CreepState.VulnerabilityType.Resistant, creep.state.GetVulnerability(Tower.TowerType.Fire));
			Assert.AreEqual(CreepState.VulnerabilityType.HardBoiled, creep.state.GetVulnerability(Tower.TowerType.Impact));
			Assert.AreEqual(CreepState.VulnerabilityType.Weak, creep.state.GetVulnerability(Tower.TowerType.Ice));
		}

		[Test]
		public void CheckForWaterTowerOnFrozenClothCreep()
		{
			creep.state.Frozen = true;
			ClothCreepStateChanger.ChangeStatesIfClothCreep(Tower.TowerType.Water, creep, creepProp);
			Assert.IsTrue(creep.state.Frozen);
		}

		[Test]
		public void CheckForWaterTowerOnDryClothCreep()
		{
			ClothCreepStateChanger.ChangeStatesIfClothCreep(Tower.TowerType.Water, creep, creepProp);
			Assert.IsFalse(creep.state.Frozen);
			Assert.IsFalse(creep.state.Burst);
			Assert.IsFalse(creep.state.Burn);
			Assert.IsTrue(creep.state.Wet);
			Assert.AreEqual(0, creep.state.WetTimer);
			Assert.AreEqual(CreepState.VulnerabilityType.Resistant, creep.state.GetVulnerability(Tower.TowerType.Fire));
			Assert.AreEqual(CreepState.VulnerabilityType.HardBoiled, creep.state.GetVulnerability(Tower.TowerType.Impact));
			Assert.AreEqual(CreepState.VulnerabilityType.Weak, creep.state.GetVulnerability(Tower.TowerType.Ice));
		}

		[Test]
		public void CheckForIceTowerOnWetClothCreep()
		{
			creep.state.Wet = true;
			ClothCreepStateChanger.ChangeStatesIfClothCreep(Tower.TowerType.Ice, creep, creepProp);
			Assert.IsTrue(creep.state.Frozen);
			Assert.IsTrue(creep.state.Paralysed);
			Assert.AreEqual(0, creep.state.FrozenTimer);
			Assert.AreEqual(CreepState.VulnerabilityType.Resistant, creep.state.GetVulnerability(Tower.TowerType.Blade));
			Assert.AreEqual(CreepState.VulnerabilityType.Resistant, creep.state.GetVulnerability(Tower.TowerType.Water));
			Assert.AreEqual(CreepState.VulnerabilityType.Vulnerable, creep.state.GetVulnerability(Tower.TowerType.Impact));
			Assert.AreEqual(CreepState.VulnerabilityType.Immune, creep.state.GetVulnerability(Tower.TowerType.Fire));
		}


		[Test]
		public void CheckForIceTowerOnDryClothCreep()
		{
			ClothCreepStateChanger.ChangeStatesIfClothCreep(Tower.TowerType.Ice, creep, creepProp);
			Assert.IsFalse(creep.state.Wet);
			Assert.IsFalse(creep.state.Burst);
			Assert.IsFalse(creep.state.Burn);
		}


		[Test]
		public void CheckForImpactTowerOnFrozentClothCreep()
		{
			creep.state.Frozen = true;
			ClothCreepStateChanger.ChangeStatesIfClothCreep(Tower.TowerType.Ice, creep, creepProp);
		}
	}
}