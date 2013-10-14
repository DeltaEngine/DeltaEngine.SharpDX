using System;
using System.Collections.Generic;
using CreepyTowers.Content;
using CreepyTowers.Creeps;
using CreepyTowers.Levels;
using CreepyTowers.Towers;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests.Creeps
{
	[Category("Slow")]
	public class CreepStateDamageOutputTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void StartTutorial()
		{
			new Game(Resolve<Window>());
			manager = new Manager(6.0f);
			creep = manager.CreateCreep(Vector3D.Zero, CreepType.Cloth, MovementData());
			gridProp = creep.Grid.PropertyMatrix;
			creep.Position = gridProp[2, 2].MidPoint;
		}

		private Manager manager;
		private GridProperties[,] gridProp;
		private Creep creep;

		[Test]
		public void ShootClothCreepWithWaterTower()
		{
			manager.CreateCreep(gridProp[2, 2].MidPoint, CreepType.Cloth, MovementData());
			manager.CreateTower(gridProp[2, 4].MidPoint, TowerType.Water,
				TowerModels.TowerWaterRangedWatersprayHigh.ToString());
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			AdvanceTimeAndUpdateEntities(2.5f);
			Assert.AreEqual(85.0f, creepList[0].CurrentHp);
		}

		private static MovementInGrid.MovementData MovementData()
		{
			return new MovementInGrid.MovementData
			{
				Velocity = new Vector3D(0.0f, 0.0f, 0.0f),
				StartGridPos = new Tuple<int, int>(11, 8),
				FinalGridPos = new Tuple<int, int>(11, 18),
				Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(11, 18) }
			};
		}

		[TestCase(TowerType.Water, 107.5f),
		 TestCase(TowerType.Fire, 107.5f),
		 TestCase(TowerType.Ice, 107.5f),
		 TestCase(TowerType.Slice, 107.5f),
		 TestCase(TowerType.Impact, 107.5f),
		 TestCase(TowerType.Acid, 107.0f)]
		public void TestImmuneStateDamageOutput(TowerType towerType, float expectedDamage)
		{
			creep.state.SetVulnerabilitiesToNormal();
			manager.CreateTower(gridProp[2, 3].MidPoint, towerType, TowerModels.TowerWaterRangedWatersprayHigh.ToString());
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			creepList[0].state.SetVulnerability(towerType, CreepState.VulnerabilityType.Immune);
			AdvanceTimeAndUpdateEntities(2.4f);
			Assert.AreEqual(expectedDamage, creepList[0].CurrentHp);
		}

		[TestCase(TowerType.Water, 60.0f),
		 TestCase(TowerType.Fire, 60.0f),
		 TestCase(TowerType.Ice, 60.0f),
		 TestCase(TowerType.Slice, 60.0f),
		 TestCase(TowerType.Impact, 60.0f),
		 TestCase(TowerType.Acid, 50.0f)]
		public void TestWeakStateDamageOutput(TowerType towerType, float expectedDamage)
		{
			creep.state.SetVulnerabilitiesToNormal();
			manager.CreateTower(gridProp[2, 3].MidPoint, towerType, TowerModels.TowerWaterRangedWatersprayHigh.ToString());
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			creepList[0].state.SetVulnerability(towerType, CreepState.VulnerabilityType.Weak);
			AdvanceTimeAndUpdateEntities(2.1f);
			Assert.AreEqual(expectedDamage, creepList[0].CurrentHp);
		}

		[TestCase(TowerType.Water, 97.5f),
		 TestCase(TowerType.Fire, 97.5f),
		 TestCase(TowerType.Ice, 97.5f),
		 TestCase(TowerType.Slice, 97.5f),
		 TestCase(TowerType.Impact, 97.5f),
		 TestCase(TowerType.Acid, 95.0f)]
		public void TestResistantStateDamageOutput(TowerType towerType, float expectedDamage)
		{
			creep.state.SetVulnerabilitiesToNormal();
			manager.CreateTower(gridProp[2, 3].MidPoint, towerType, TowerModels.TowerWaterRangedWatersprayHigh.ToString());
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			creepList[0].state.SetVulnerability(towerType, CreepState.VulnerabilityType.Resistant);
			AdvanceTimeAndUpdateEntities(2.1f);
			Assert.AreEqual(expectedDamage, creepList[0].CurrentHp);
		}

		[TestCase(TowerType.Water, 103.75f),
		 TestCase(TowerType.Fire, 103.75f),
		 TestCase(TowerType.Ice, 103.75f),
		 TestCase(TowerType.Slice, 103.75f),
		 TestCase(TowerType.Impact, 103.75f),
		 TestCase(TowerType.Acid, 102.5f)]
		public void TestHardBoiledStateDamageOutput(TowerType towerType, float expectedDamage)
		{
			creep.state.SetVulnerabilitiesToNormal();
			manager.CreateTower(gridProp[2, 3].MidPoint, towerType, TowerModels.TowerWaterRangedWatersprayHigh.ToString());
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			creepList[0].state.SetVulnerability(towerType, CreepState.VulnerabilityType.HardBoiled);
			AdvanceTimeAndUpdateEntities(2.1f);
			Assert.AreEqual(expectedDamage, creepList[0].CurrentHp);
		}

		[TestCase(TowerType.Water, 35.0f),
		 TestCase(TowerType.Fire, 35.0f),
		 TestCase(TowerType.Ice, 35.0f),
		 TestCase(TowerType.Slice, 35.0f),
		 TestCase(TowerType.Impact, 35.0f),
		 TestCase(TowerType.Acid, 20.0f)]
		public void TestVulnerableStateDamageOutput(TowerType towerType, float expectedDamage)
		{
			creep.state.SetVulnerabilitiesToNormal();
			manager.CreateTower(gridProp[2, 3].MidPoint, towerType, TowerModels.TowerWaterRangedWatersprayHigh.ToString());
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			creepList[0].state.SetVulnerability(towerType, CreepState.VulnerabilityType.Vulnerable);
			AdvanceTimeAndUpdateEntities(2.1f);
			Assert.AreEqual(expectedDamage, creepList[0].CurrentHp);
		}

		[TestCase(TowerType.Water, 85.0f), TestCase(TowerType.Fire, 85.0f),
		 TestCase(TowerType.Ice, 85.0f), TestCase(TowerType.Slice, 85.0f),
		 TestCase(TowerType.Impact, 85.0f), TestCase(TowerType.Acid, 80.0f)]
		public void TestNormalStateDamageOutput(TowerType towerType, float expectedDamage)
		{
			creep.state.SetVulnerabilitiesToNormal();
			manager.CreateTower(gridProp[2, 3].MidPoint, towerType, TowerModels.TowerWaterRangedWatersprayHigh.ToString());
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			AdvanceTimeAndUpdateEntities(2.1f);
			Assert.AreEqual(expectedDamage, creepList[0].CurrentHp);
		}

		[Test]
		public void BurstDamageOutput()
		{
			creep.state.SetVulnerabilitiesToNormal();
			manager.CreateTower(gridProp[2, 3].MidPoint, TowerType.Fire,
				TowerModels.TowerFireCandlehulaHigh.ToString());
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			AdvanceTimeAndUpdateEntities(2.1f);
			Assert.AreEqual(85.0f, creepList[0].CurrentHp);
			AdvanceTimeAndUpdateEntities(1.0f);
			Assert.AreEqual(75.8333359f, creepList[0].CurrentHp);
			AdvanceTimeAndUpdateEntities(1.0f);
			Assert.AreEqual(41.6666718f, creepList[0].CurrentHp);
		}

		[Test]
		public void BurnDamageOutput()
		{
			creep.state.SetVulnerabilitiesToNormal();
			creep.state.Burn = true;
			manager.CreateTower(gridProp[2, 3].MidPoint, TowerType.Slice,
				TowerModels.TowerFireCandlehulaHigh.ToString());
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			AdvanceTimeAndUpdateEntities(2.1f);
			Assert.AreEqual(71.25f, creepList[0].CurrentHp);
			AdvanceTimeAndUpdateEntities(1.0f);
			Assert.AreEqual(64.375f, creepList[0].CurrentHp);
			AdvanceTimeAndUpdateEntities(1.0f);
			Assert.AreEqual(32.5f, creepList[0].CurrentHp);
		}
	}
}