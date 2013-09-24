using System;
using System.Collections.Generic;
using CreepyTowers.Creeps;
using CreepyTowers.Levels;
using CreepyTowers.Towers;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	[Category("Slow")]
	public class CreepStateDamageOutputTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void StartTutorial()
		{
			new Game(Resolve<Window>(), Resolve<Device>());
			manager = new Manager(6.0f);
			gridProp = Game.CameraAndGrid.Grid.PropertyMatrix;
			creep = manager.CreateCreep(gridProp[2, 2].MidPoint, Names.CreepCottonMummy, MovementData());
		}

		private Manager manager;
		private GridProperties[,] gridProp;
		private static Creep creep;

		[Test]
		public void TestNormalRectaionOfClothCreepWithWaterTower()
		{
			gridProp = Game.CameraAndGrid.Grid.PropertyMatrix;
			manager.CreateCreep(gridProp[2, 2].MidPoint, Names.CreepCottonMummy, MovementData());
			manager.CreateTower(gridProp[2, 4].MidPoint, Tower.TowerType.Water,
				Names.TowerWaterRangedWaterspray);
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			AdvanceTimeAndUpdateEntities(2.5f);
			Assert.AreEqual(85.0f, creepList[0].Get<CreepProperties>().CurrentHp);
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

		[TestCase(Tower.TowerType.Water, 107.5f),
		 TestCase(Tower.TowerType.Fire, 107.5f),
		 TestCase(Tower.TowerType.Ice, 107.5f),
		 TestCase(Tower.TowerType.Blade, 107.5f),
		 TestCase(Tower.TowerType.Impact, 107.5f),
		 TestCase(Tower.TowerType.Acid, 107.0f)]
		public void TestImmuneStateDamageOutput(Tower.TowerType towerType, float expectedDamage)
		{
			creep.state.SetVulnerabilitiesToNormal();
			manager.CreateTower(gridProp[2, 3].MidPoint, towerType, Names.TowerWaterRangedWaterspray);
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			creepList[0].state.SetVulnerability(towerType, CreepState.VulnerabilityType.Immune);
			AdvanceTimeAndUpdateEntities(2.4f);
			Assert.AreEqual(expectedDamage, creepList[0].Get<CreepProperties>().CurrentHp);
		}

		[TestCase(Tower.TowerType.Water, 60.0f),
		 TestCase(Tower.TowerType.Fire, 60.0f),
		 TestCase(Tower.TowerType.Ice, 60.0f),
		 TestCase(Tower.TowerType.Blade, 60.0f),
		 TestCase(Tower.TowerType.Impact, 60.0f),
		 TestCase(Tower.TowerType.Acid, 50.0f)]
		public void TestWeakStateDamageOutput(Tower.TowerType towerType, float expectedDamage)
		{
			creep.state.SetVulnerabilitiesToNormal();
			manager.CreateTower(gridProp[2, 3].MidPoint, towerType, Names.TowerWaterRangedWaterspray);
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			creepList[0].state.SetVulnerability(towerType, CreepState.VulnerabilityType.Weak);
			AdvanceTimeAndUpdateEntities(2.1f);
			Assert.AreEqual(expectedDamage, creepList[0].Get<CreepProperties>().CurrentHp);
		}

		[TestCase(Tower.TowerType.Water, 97.5f),
		 TestCase(Tower.TowerType.Fire, 97.5f),
		 TestCase(Tower.TowerType.Ice, 97.5f),
		 TestCase(Tower.TowerType.Blade, 97.5f),
		 TestCase(Tower.TowerType.Impact, 97.5f),
		 TestCase(Tower.TowerType.Acid, 95.0f)]
		public void TestResistantStateDamageOutput(Tower.TowerType towerType, float expectedDamage)
		{
			creep.state.SetVulnerabilitiesToNormal();
			manager.CreateTower(gridProp[2, 3].MidPoint, towerType, Names.TowerWaterRangedWaterspray);
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			creepList[0].state.SetVulnerability(towerType, CreepState.VulnerabilityType.Resistant);
			AdvanceTimeAndUpdateEntities(2.1f);
			Assert.AreEqual(expectedDamage, creepList[0].Get<CreepProperties>().CurrentHp);
		}

		[TestCase(Tower.TowerType.Water, 103.75f),
		 TestCase(Tower.TowerType.Fire, 103.75f),
		 TestCase(Tower.TowerType.Ice, 103.75f),
		 TestCase(Tower.TowerType.Blade, 103.75f),
		 TestCase(Tower.TowerType.Impact, 103.75f),
		 TestCase(Tower.TowerType.Acid, 102.5f)]
		public void TestHardBoiledStateDamageOutput(Tower.TowerType towerType, float expectedDamage)
		{
			creep.state.SetVulnerabilitiesToNormal();
			manager.CreateTower(gridProp[2, 3].MidPoint, towerType, Names.TowerWaterRangedWaterspray);
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			creepList[0].state.SetVulnerability(towerType, CreepState.VulnerabilityType.HardBoiled);
			AdvanceTimeAndUpdateEntities(2.1f);
			Assert.AreEqual(expectedDamage, creepList[0].Get<CreepProperties>().CurrentHp);
		}

		[TestCase(Tower.TowerType.Water, 35.0f),
		 TestCase(Tower.TowerType.Fire, 35.0f),
		 TestCase(Tower.TowerType.Ice, 35.0f),
		 TestCase(Tower.TowerType.Blade, 35.0f),
		 TestCase(Tower.TowerType.Impact, 35.0f),
		 TestCase(Tower.TowerType.Acid, 20.0f)]
		public void TestVulnerableStateDamageOutput(Tower.TowerType towerType, float expectedDamage)
		{
			creep.state.SetVulnerabilitiesToNormal();
			manager.CreateTower(gridProp[2, 3].MidPoint, towerType, Names.TowerWaterRangedWaterspray);
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			creepList[0].state.SetVulnerability(towerType, CreepState.VulnerabilityType.Vulnerable);
			AdvanceTimeAndUpdateEntities(2.1f);
			Assert.AreEqual(expectedDamage, creepList[0].Get<CreepProperties>().CurrentHp);
		}

		[TestCase(Tower.TowerType.Water, 85.0f), TestCase(Tower.TowerType.Fire, 85.0f),
		 TestCase(Tower.TowerType.Ice, 85.0f), TestCase(Tower.TowerType.Blade, 85.0f),
		 TestCase(Tower.TowerType.Impact, 85.0f), TestCase(Tower.TowerType.Acid, 80.0f)]
		public void TestNormalStateDamageOutput(Tower.TowerType towerType, float expectedDamage)
		{
			creep.state.SetVulnerabilitiesToNormal();
			manager.CreateTower(gridProp[2, 3].MidPoint, towerType, Names.TowerWaterRangedWaterspray);
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			AdvanceTimeAndUpdateEntities(2.1f);
			Assert.AreEqual(expectedDamage, creepList[0].Get<CreepProperties>().CurrentHp);
		}

		[Test]
		public void BurstDamageOutput()
		{
			creep.state.SetVulnerabilitiesToNormal();
			manager.CreateTower(gridProp[2, 3].MidPoint, Tower.TowerType.Fire,
				Names.TowerFireCandlehula);
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			AdvanceTimeAndUpdateEntities(2.1f);
			Assert.AreEqual(85.0f, creepList[0].Get<CreepProperties>().CurrentHp);
			AdvanceTimeAndUpdateEntities(1.0f);
			Assert.AreEqual(75.8333359f, creepList[0].Get<CreepProperties>().CurrentHp);
			AdvanceTimeAndUpdateEntities(1.0f);
			Assert.AreEqual(41.6666718f, creepList[0].Get<CreepProperties>().CurrentHp);
		}

		[Test]
		public void BurnDamageOutput()
		{
			creep.state.SetVulnerabilitiesToNormal();
			creep.state.Burn = true;
			manager.CreateTower(gridProp[2, 3].MidPoint, Tower.TowerType.Blade,
				Names.TowerFireCandlehula);
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			AdvanceTimeAndUpdateEntities(2.1f);
			Assert.AreEqual(71.25f, creepList[0].Get<CreepProperties>().CurrentHp);
			AdvanceTimeAndUpdateEntities(1.0f);
			Assert.AreEqual(64.375f, creepList[0].Get<CreepProperties>().CurrentHp);
			AdvanceTimeAndUpdateEntities(1.0f);
			Assert.AreEqual(32.5f, creepList[0].Get<CreepProperties>().CurrentHp);
		}
	}
}