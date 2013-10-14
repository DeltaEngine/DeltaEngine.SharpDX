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
using DeltaEngine.Rendering3D.Cameras;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	public class ManagerTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void StartTutorial()
		{
			new Game(Resolve<Window>());
			manager = new Manager(6.0f);
			grid = new LevelGrid(24, 0.2f);
			gridProp = grid.PropertyMatrix;
		}

		private Manager manager;
		private LevelGrid grid;
		private GridProperties[,] gridProp;

		[Test]
		public void ChangeCameraSize()
		{
			Assert.AreEqual(6.0f, ((OrthoCamera)Camera.Current).Size.Width);
			Game.CreateCamera.FovSizeFactor = 4.0f;
			Assert.AreEqual(4.0f, ((OrthoCamera)Camera.Current).Size.Width);
		}

		[Test, Ignore]
		public void CheckCreepCreation()
		{
			var creep = CreateDefaultCreep();
			Assert.IsTrue(creep.Contains<MovementInGrid.MovementData>());
			Assert.AreEqual(1, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
			Assert.AreEqual(CreepType.Cloth, creep.Get<CreepData>().Type);
		}

		//TODO: this is the wrong way to create a creep
		private Creep CreateDefaultCreep()
		{
			return manager.CreateCreep(Vector3D.Zero, CreepType.Cloth, MovementData());
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

		[Test, Ignore]
		public void CheckTowerCreation()
		{
			manager.CreateTower(Vector3D.Zero, TowerType.Water, TowerModels.TowerWaterRangedWatersprayHigh.ToString());
			Assert.AreEqual(1, EntitiesRunner.Current.GetEntitiesOfType<Tower>().Count);
			Assert.AreEqual(TowerType.Water,
				EntitiesRunner.Current.GetEntitiesOfType<Tower>()[0].Get<TowerData>().Type);
		}

		[Test]
		public void ClearAllData()
		{
			manager.CreateTower(Vector3D.Zero, TowerType.Water, TowerModels.TowerWaterRangedWatersprayHigh.ToString());
			manager.Dispose();
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Tower>().Count);
		}

		[Test]
		public void CreepInRangeIsAttacked()
		{
			CreateCreep(gridProp[2, 2].MidPoint);
			CreateTower(gridProp[2, 4].MidPoint);
			var towerList = EntitiesRunner.Current.GetEntitiesOfType<Tower>();
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			var distanceBetweenCreepAndTower = (towerList[0].Position - creepList[0].Position).Length;
			Assert.GreaterOrEqual(towerList[0].Get<TowerData>().Range,
				distanceBetweenCreepAndTower);
			var creepHpBeforeHit = creepList[0].CurrentHp;
			AdvanceTimeAndUpdateEntities(3.0f);
			var creepHpAfterHit = creepList[0].CurrentHp;
			Assert.Less(creepHpAfterHit, creepHpBeforeHit);
		}

		private static Creep CreateCreep(Vector3D creepPos)
		{
			return new Creep(CreepType.Cloth, creepPos, 0);
		}

		private static void CreateTower(Vector3D towerPos)
		{
			new Tower(TowerType.Water, towerPos);
		}

		[Test]
		public void CreepOutOfRangeIsNotAttacked()
		{
			CreateCreep(gridProp[2, 2].MidPoint);
			CreateTower(gridProp[2, 10].MidPoint);
			var towerList = EntitiesRunner.Current.GetEntitiesOfType<Tower>();
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			var distanceBetweenCreepAndTower = (towerList[0].Position - creepList[0].Position).Length;
			Assert.LessOrEqual(towerList[0].Get<TowerData>().Range, distanceBetweenCreepAndTower);
			var creepHpBeforeHit = creepList[0].CurrentHp;
			AdvanceTimeAndUpdateEntities(2.0f);
			var creepHpAfterHit = creepList[0].CurrentHp;
			Assert.AreEqual(creepHpAfterHit, creepHpBeforeHit);
		}

		[Test]
		public void DisplayCreepHealthBar()
		{
			var creep = CreateCreep(Vector3D.Zero);
			creep.RecalculateHitpointBar();
		}

		[Test]
		public void DisposingManagerDisposesAllActiveCreepsAndTowers()
		{
			new Creep(CreepType.Paper, Vector3D.Zero, 0);
			new Tower(TowerType.Water, Vector3D.Zero);
			manager.Dispose();
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Tower>().Count);
		}

		[Test]
		public void CheckingForCreepHealthStatus100To80()
		{
			CreateCreepWithHpValues(90);
		}

		private static void CreateCreepWithHpValues(float currHp)
		{
			var creep = new Creep(CreepType.Cloth, Vector3D.Zero, 0);
			creep.CurrentHp = currHp;
			creep.RecalculateHitpointBar();
		}

		[Test]
		public void CheckingForCreepHealthStatus80To60()
		{
			CreateCreepWithHpValues(70.0f);
		}

		[Test]
		public void CheckingForCreepHealthStatus60To50()
		{
			CreateCreepWithHpValues(53.0f);
		}

		[Test]
		public void CheckingForCreepHealthStatus50To40()
		{
			CreateCreepWithHpValues(42.0f);
		}

		[Test]
		public void CheckingForCreepHealthStatus40To25()
		{
			CreateCreepWithHpValues(30.0f);
		}

		[Test]
		public void CheckingForCreepHealthStatus25To20()
		{
			CreateCreepWithHpValues(22.0f);
		}

		[Test]
		public void CheckingForCreepHealthStatus25To10()
		{
			CreateCreepWithHpValues(15.0f);
		}

		[Test]
		public void CheckingForCreepHealthStatus10To5()
		{
			CreateCreepWithHpValues(7.0f);
		}

		[Test]
		public void CheckingForCreepHealthStatusLessThan5()
		{
			CreateCreepWithHpValues(4.0f);
		}
	}
}