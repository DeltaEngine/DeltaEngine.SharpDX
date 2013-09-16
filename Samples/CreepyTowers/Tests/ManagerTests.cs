using System;
using System.Collections.Generic;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	public class ManagerTests : TestWithMocksOrVisually
	{
		/*
		[SetUp]
		public void StartTutorial()
		{
			new Game(Resolve<Window>(), Resolve<Device>()).GameMainMenu.Dispose();
			manager = new Manager(6.0f);
		}

		private Manager manager;

		[Test]
		public void ChangeCameraSize()
		{
			Assert.AreEqual(6.0f, Game.CameraAndGrid.Camera.Size.Width);
			Game.CameraAndGrid.Camera.Size = new Size(4.0f);
			Assert.AreEqual(4.0f, Game.CameraAndGrid.Camera.Size.Width);
		}

		[Test]
		public void CheckCreepCreation()
		{
			var creep = manager.CreateCreep(Vector.Zero, Names.CreepCottonMummy, MovementData());
			Assert.AreEqual(1, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
			Assert.AreEqual(Creep.CreepType.Cloth, creep.Get<CreepProperties>().CreepType);
		}

		private static MovementInGrid.MovementData MovementData()
		{
			return new MovementInGrid.MovementData
			{
				Velocity = new Vector(0.0f, 0.0f, 0.0f),
				StartGridPos = new Tuple<int, int>(11, 8),
				FinalGridPos = new Tuple<int, int>(11, 18),
				Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(11, 18) }
			};
		}

		[Test]
		public void CheckTowerCreation()
		{
			manager.CreateTower(Vector.Zero, Tower.TowerType.Water, Names.TowerWaterRangedWatersprayHigh);
			Assert.AreEqual(1, EntitiesRunner.Current.GetEntitiesOfType<Tower>().Count);
			Assert.AreEqual(Tower.TowerType.Water,
				EntitiesRunner.Current.GetEntitiesOfType<Tower>()[0].Get<TowerProperties>().TowerType);
		}

		[Test]
		public void ClearAllData()
		{
			var creep = manager.CreateCreep(Vector.Zero, Names.CreepCottonMummy, MovementData());
			manager.CreateTower(Vector.Zero, Tower.TowerType.Water, Names.TowerWaterRangedWatersprayHigh);
			manager.Dispose();
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Tower>().Count);
			Assert.IsFalse(creep.IsActive);
		}

		[Test]
		public void CreepInRangeIsAttacked()
		{
			var gridProp = Game.CameraAndGrid.Grid.PropertyMatrix;
			manager.CreateCreep(gridProp[2, 2].MidPoint, Names.CreepCottonMummy, MovementData());
			manager.CreateTower(gridProp[2, 4].MidPoint, Tower.TowerType.Water,
				Names.TowerWaterRangedWatersprayHigh);
			var towerList = EntitiesRunner.Current.GetEntitiesOfType<Tower>();
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			var distanceBetweenCreepAndTower = (towerList[0].Position - creepList[0].Position).Length;
			Assert.GreaterOrEqual(towerList[0].Get<TowerProperties>().Range,
				distanceBetweenCreepAndTower);
			var creepHpBeforeHit = creepList[0].Get<CreepProperties>().CurrentHp;
			AdvanceTimeAndUpdateEntities(3.0f);
			var creepHpAfterHit = creepList[0].Get<CreepProperties>().CurrentHp;
			Assert.Less(creepHpAfterHit, creepHpBeforeHit);
		}

		[Test]
		public void CreepOutOfRangeIsNotAttacked()
		{
			var gridProp = Game.CameraAndGrid.Grid.PropertyMatrix;
			manager.CreateCreep(gridProp[2, 2].MidPoint, Names.CreepCottonMummy, MovementData());
			manager.CreateTower(gridProp[2, 10].MidPoint, Tower.TowerType.Water,
				Names.TowerWaterRangedWatersprayHigh);
			var towerList = EntitiesRunner.Current.GetEntitiesOfType<Tower>();
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			var distanceBetweenCreepAndTower = (towerList[0].Position - creepList[0].Position).Length;
			Assert.LessOrEqual(towerList[0].Get<TowerProperties>().Range, distanceBetweenCreepAndTower);
			var creepHpBeforeHit = creepList[0].Get<CreepProperties>().CurrentHp;
			AdvanceTimeAndUpdateEntities(2.0f);
			var creepHpAfterHit = creepList[0].Get<CreepProperties>().CurrentHp;
			Assert.AreEqual(creepHpAfterHit, creepHpBeforeHit);
		}

		[Test]
		public void DisplayCreepHealthBar()
		{
			var gridProp = Game.CameraAndGrid.Grid.PropertyMatrix;
			var movementData = new MovementInGrid.MovementData
			{
				Velocity = new Vector(0.0f, 0.0f, 0.0f),
				StartGridPos = new Tuple<int, int>(16, 0),
				FinalGridPos = new Tuple<int, int>(16, 0),
				Waypoints = new List<Tuple<int, int>>()
			};

			var creep = manager.CreateCreep(gridProp[16, 0].MidPoint, Names.CreepCottonMummy,
				movementData);
			creep.Remove<MovementInGrid>();
			manager.UpdateCreepHealthBar(creep);
		}

		//[Test]
		//public void StartCtGame()
		//{
		//	var manager = new Manager(Resolve<ScreenSpace>());
		//	manager.PlayGame();
		//}

		//[Test]
		//public void ExitingGameDisposesAllCtGameEntities()
		//{
		//	var manager = new Manager(Resolve<ScreenSpace>());
		//	manager.PlayGame();
		//	manager.ExitGame();
		//	Assert.AreEqual(0, manager.CtGame.Controls.Count);
		//}

		//[Test]
		//public void StartingCtGameAddsOneCreepToActiveList()
		//{
		//	var manager = new Manager(Resolve<ScreenSpace>());
		//	manager.PlayGame();
		//	Assert.AreEqual(1, manager.CtGame.ActiveCreepsList.Count);
		//}
		 */
	}
}