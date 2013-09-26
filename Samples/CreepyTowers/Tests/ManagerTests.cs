using System;
using System.Collections.Generic;
using CreepyTowers.Creeps;
using CreepyTowers.Levels;
using CreepyTowers.Towers;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;
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
			new Game(Resolve<Window>(), Resolve<Device>());
			manager = new Manager(6.0f);
		}

		private Manager manager;

		[Test]
		public void ChangeCameraSize()
		{
			Assert.AreEqual(6.0f, ((OrthoCamera)Camera.Current).Size.Width);
			Game.CameraAndGrid.GameCamera.Size = new Size(4.0f);
			Assert.AreEqual(4.0f, ((OrthoCamera)Camera.Current).Size.Width);
		}

		[Test, Ignore]
		public void CheckCreepCreation()
		{
			var creep = CreateDefaultCreep();
			Assert.AreEqual(1, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
			Assert.AreEqual(Creep.CreepType.Cloth, creep.Get<CreepProperties>().CreepType);
		}

		private Creep CreateDefaultCreep()
		{
			var creep = manager.CreateCreep(Vector3D.Zero, Names.CreepCottonMummy, Creep.CreepType.Cloth, MovementData());
			creep.Remove<CreepProperties>();
			creep.Add(new CreepProperties
			{
				MaxHp = 100.0f,
				CurrentHp = 100.0f,
				Resistance = 1.0f,
				CreepType = Creep.CreepType.Cloth,
				GoldReward = 20,
				Speed = 2.0f,
				Name = Names.CreepCottonMummy,
			});
			return creep;
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
			manager.CreateTower(Vector3D.Zero, Tower.TowerType.Water, Names.TowerWaterRangedWaterspray);
			Assert.AreEqual(1, EntitiesRunner.Current.GetEntitiesOfType<Tower>().Count);
			Assert.AreEqual(Tower.TowerType.Water,
				EntitiesRunner.Current.GetEntitiesOfType<Tower>()[0].Get<TowerProperties>().TowerType);
		}

		[Test, Ignore]
		public void ClearAllData()
		{
			var creep = CreateDefaultCreep();
			manager.CreateTower(Vector3D.Zero, Tower.TowerType.Water, Names.TowerWaterRangedWaterspray);
			manager.Dispose();
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Tower>().Count);
			Assert.IsFalse(creep.IsActive);
		}

		[Test, Ignore]
		public void CreepInRangeIsAttacked()
		{
			var gridProp = Game.CameraAndGrid.Grid.PropertyMatrix;
			manager.CreateCreep(gridProp[2, 2].MidPoint, Names.CreepCottonMummy, Creep.CreepType.Cloth, MovementData());
			manager.CreateTower(gridProp[2, 4].MidPoint, Tower.TowerType.Water,
				Names.TowerWaterRangedWaterspray);
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

		[Test, Ignore]
		public void CreepOutOfRangeIsNotAttacked()
		{
			var gridProp = Game.CameraAndGrid.Grid.PropertyMatrix;
			manager.CreateCreep(gridProp[2, 2].MidPoint, Names.CreepCottonMummy, Creep.CreepType.Cloth, MovementData());
			manager.CreateTower(gridProp[2, 10].MidPoint, Tower.TowerType.Water,
				Names.TowerWaterRangedWaterspray);
			var towerList = EntitiesRunner.Current.GetEntitiesOfType<Tower>();
			var creepList = EntitiesRunner.Current.GetEntitiesOfType<Creep>();
			var distanceBetweenCreepAndTower = (towerList[0].Position - creepList[0].Position).Length;
			Assert.LessOrEqual(towerList[0].Get<TowerProperties>().Range, distanceBetweenCreepAndTower);
			var creepHpBeforeHit = creepList[0].Get<CreepProperties>().CurrentHp;
			AdvanceTimeAndUpdateEntities(2.0f);
			var creepHpAfterHit = creepList[0].Get<CreepProperties>().CurrentHp;
			Assert.AreEqual(creepHpAfterHit, creepHpBeforeHit);
		}

		[Test, Ignore]
		public void DisplayCreepHealthBar()
		{
			var gridProp = Game.CameraAndGrid.Grid.PropertyMatrix;
			var creep = CreateDefaultCreep(gridProp, CreateMovementData());
			manager.UpdateCreepHealthBar(creep);
		}

		private static MovementInGrid.MovementData CreateMovementData()
		{
			return new MovementInGrid.MovementData
			{
				Velocity = new Vector3D(0.0f, 0.0f, 0.0f),
				StartGridPos = new Tuple<int, int>(16, 0),
				FinalGridPos = new Tuple<int, int>(16, 0),
				Waypoints = new List<Tuple<int, int>>()
			};
		}

		private Creep CreateDefaultCreep(GridProperties[,] gridProp,
			MovementInGrid.MovementData movementData)
		{
			var creep = manager.CreateCreep(gridProp[16, 0].MidPoint, Names.CreepCottonMummy, Creep.CreepType.Cloth, 
				movementData);
			creep.Remove<MovementInGrid>();
			creep.Remove<CreepProperties>();
			creep.Add(new CreepProperties
			{
				Name = Names.CreepCottonMummy,
				CreepType = Creep.CreepType.Cloth,
				CurrentHp = 100.0f,
				GoldReward = 30,
				MaxHp = 100.0f,
				Resistance = 1.0f,
				Speed = 1.0f
			});
			return creep;
		}

		[Test]
		public void DisposingManagerDisposesAllActiveCreepsAndTowers()
		{
			new Creep(Vector3D.Zero, Names.CreepCottonMummy, new CreepProperties());
			new Tower(Vector3D.Zero, Names.TowerWaterRangedWaterspray, new TowerProperties());
			manager.Dispose();
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Tower>().Count);
		}

		[Test]
		public void CheckingForCreepHealthStatus100To80()
		{
			var creep = CreateCreepWithHpValues(90);
			Assert.AreEqual(new Material(Shader.Position2DUv, Names.ImageHealthBarGreen100).Name,
				creep.HealthBar.Material.Name);
		}

		private Creep CreateCreepWithHpValues(float currHp)
		{
			var creep = new Creep(Vector3D.Zero, Names.CreepCottonMummy, new CreepProperties
			{
				Name = Names.CreepCottonMummy,
				CreepType = Creep.CreepType.Cloth,
				CurrentHp = currHp,
				GoldReward = 30,
				MaxHp = 100.0f,
				Resistance = 1.0f,
				Speed = 1.0f
			});
			
			creep.Add(MovementData());
			manager.UpdateCreepHealthBar(creep);
			return creep;
		}

		[Test]
		public void CheckingForCreepHealthStatus80To60()
		{
			var creep = CreateCreepWithHpValues(70.0f);
			Assert.AreEqual(new Material(Shader.Position2DUv, Names.ImageHealthBarGreen80).Name,
				creep.HealthBar.Material.Name);
		}

		[Test]
		public void CheckingForCreepHealthStatus60To50()
		{
			var creep = CreateCreepWithHpValues(53.0f);
			Assert.AreEqual(new Material(Shader.Position2DUv, Names.ImageHealthBarGreen60).Name,
				creep.HealthBar.Material.Name);
		}

		[Test]
		public void CheckingForCreepHealthStatus50To40()
		{
			var creep = CreateCreepWithHpValues(42.0f);
			Assert.AreEqual(new Material(Shader.Position2DUv, Names.ImageHealthBarOrange50).Name,
				creep.HealthBar.Material.Name);
		}

		[Test]
		public void CheckingForCreepHealthStatus40To25()
		{
			var creep = CreateCreepWithHpValues(30.0f);
			Assert.AreEqual(new Material(Shader.Position2DUv, Names.ImageHealthBarOrange40).Name,
				creep.HealthBar.Material.Name);
		}

		[Test]
		public void CheckingForCreepHealthStatus25To20()
		{
			var creep = CreateCreepWithHpValues(22.0f);
			Assert.AreEqual(new Material(Shader.Position2DUv, Names.ImageHealthBarOrange25).Name,
				creep.HealthBar.Material.Name);
		}

		[Test]
		public void CheckingForCreepHealthStatus25To10()
		{
			var creep = CreateCreepWithHpValues(15.0f);
			Assert.AreEqual(new Material(Shader.Position2DUv, Names.ImageHealthBarRed20).Name,
				creep.HealthBar.Material.Name);
		}

		[Test]
		public void CheckingForCreepHealthStatus10To5()
		{
			var creep = CreateCreepWithHpValues(7.0f);
			Assert.AreEqual(new Material(Shader.Position2DUv, Names.ImageHealthBarRed10).Name,
				creep.HealthBar.Material.Name);
		}

		[Test]
		public void CheckingForCreepHealthStatusLessThan5()
		{
			var creep = CreateCreepWithHpValues(4.0f);
			Assert.AreEqual(new Material(Shader.Position2DUv, Names.ImageHealthBarRed05).Name,
				creep.HealthBar.Material.Name);
		}
	}
}