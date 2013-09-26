using System;
using System.Collections.Generic;
using CreepyTowers.Creeps;
using CreepyTowers.Levels;
using CreepyTowers.Towers;
using DeltaEngine.Commands;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics;
using DeltaEngine.Input;
using DeltaEngine.Platforms;
using DeltaEngine.Rendering3D.Cameras;
using DeltaEngine.ScreenSpaces;
using NUnit.Framework;

namespace CreepyTowers.Tests.Towers
{
	public class TowerTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Initialize()
		{
			new Game(Resolve<Window>(), Resolve<Device>());
			manager = new Manager(6.0f);
			grid = Game.CameraAndGrid.Grid;
		}

		private Manager manager;
		private LevelGrid grid;

		[Test]
		public void CreateWaterTower()
		{
			CreateDefaultTower();
		}

		private Tower CreateDefaultTower()
		{
			var position = grid.PropertyMatrix[2, 3].MidPoint;
			return new Tower(position, Names.TowerWaterRangedWaterspray,
				new TowerProperties
				{
					TowerType = Tower.TowerType.Water,
					Name = Names.TowerWaterRangedWaterspray,
					AttackFrequency = 0.5f
				});
		}

		[Test]
		public void CreateFireTower()
		{
			new Tower(Vector3D.Zero, Names.TowerFireCandlehula,
				new TowerProperties { TowerType = Tower.TowerType.Fire, Name = Names.TowerFireCandlehula });
		}

		[Test]
		public void CreateSliceTower()
		{
			new Tower(Vector3D.Zero, Names.TowerSliceConeKnifeblock,
				new TowerProperties
				{
					TowerType = Tower.TowerType.Slice,
					Name = Names.TowerSliceConeKnifeblock
				});
		}

		[Test]
		public void CreateImapctTower()
		{
			new Tower(Vector3D.Zero, Names.TowerImpactRangedKnightscales,
				new TowerProperties
				{
					TowerType = Tower.TowerType.Impact,
					Name = Names.TowerImpactRangedKnightscales
				});
		}

		[Test]
		public void CreateAcidTower()
		{
			new Tower(Vector3D.Zero, Names.TowerAcidConeJanitor,
				new TowerProperties { TowerType = Tower.TowerType.Acid, Name = Names.TowerAcidConeJanitor });
		}

		[Test]
		public void CreateIceTower()
		{
			new Tower(Vector3D.Zero, Names.TowerIceConeIcelady,
				new TowerProperties { TowerType = Tower.TowerType.Ice, Name = Names.TowerIceConeIcelady });
		}

		[Test]
		public void CreateTowerAtClickedPosition()
		{
			var floor = new Plane(Vector3D.UnitY, 0.0f);
			var list = new ChangeableList<Tuple<int, int>>
			{
				new Tuple<int, int>(1, 1),
				new Tuple<int, int>(2, 2),
				new Tuple<int, int>(2, 3)
			};

			new Command(pos =>
			{
				var ray = Camera.Current.ScreenPointToRay(ScreenSpace.Current.ToPixelSpace(pos));
				var tower = CreateDefaultTower();
				var position = floor.Intersect(ray);
				tower.Position = grid.ComputeGridCoordinates(grid, (Vector3D)position, list);
			}).Add(new MouseButtonTrigger(MouseButton.Left, State.Releasing));
		}

		[Test]
		public void DisposingTowerRemovesTowerEntity()
		{
			var tower = CreateDefaultTower();
			tower.Dispose();
			Assert.IsFalse(tower.IsActive);
		}

		[Test]
		public void CheckTowerFireAtCreep()
		{
			var tower = CreateDefaultTower();
			var creep = CreateDefaultCreep();
		}

		private Creep CreateDefaultCreep()
		{
			var position = grid.PropertyMatrix[2, 2].MidPoint;
			var creep = new Creep(position, Names.CreepCottonMummy,
				new CreepProperties { Name = Names.CreepCottonMummy, CreepType = Creep.CreepType.Cloth });
			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector3D(0.0f, 0.0f, 0.0f),
				StartGridPos = new Tuple<int, int>(4, 0),
				Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(1, 0) }
			});
			creep.Remove<MovementInGrid>();
			return creep;
		}

		//[Test]
		//public void TowerCannotAttackAgainBeforeTimeHasPassed()
		//{
		//	tower = new Tower(new Vector2D(0.3f, 0.4f), Tower.TowerType.Water);
		//	creep = new Creep(Vector2D.Half, Creep.CreepType.Sand);
		//	int numberOfAttacksRecieved = 0;
		//	creep.GotAttacked += () => { numberOfAttacksRecieved++; };
		//	AdvanceTimeAndUpdateEntities(1 / tower.Get<Tower.Properties>().AttackFrequency);
		//	tower.FireAtCreep(creep);
		//	tower.FireAtCreep(creep);
		//	AdvanceTimeAndUpdateEntities(1 / tower.Get<Tower.Properties>().AttackFrequency);
		//	tower.FireAtCreep(creep);
		//	Assert.AreEqual(2, numberOfAttacksRecieved);
		//}

		//[Test]
		//public void CheckForCreepUnderAttack()
		//{
		//	var tower = CreateDefaultTower();
		//	var creep = CreateDefaultCreep();
		//	var creepHpBeforeHit = creep.Get<CreepProperties>().CurrentHp;
		//	AdvanceTimeAndUpdateEntities(1.5f);
		//	tower.FireAtCreep(creep);
		//	var creepHpAfterHit = creep.Get<CreepProperties>().CurrentHp;
		//	Assert.Less(creepHpAfterHit, creepHpBeforeHit);
		//}

		//[Test]
		//public void CheckForCreepDead()
		//{
		//	var tower = CreateDefaultTower();
		//	var creep = CreateDefaultCreep();
		//	creep.Get<CreepProperties>().CurrentHp = 2.0f;
		//	AdvanceTimeAndUpdateEntities(1.5f);
		//	tower.FireAtCreep(creep);
		//	Assert.IsFalse(creep.IsActive);
		//}

		//[Test]
		//public void CheckIfCreepIsHit()
		//{
		//	var manager = new Manager(Resolve<ScreenSpace>());
		//	CreateTower(manager);
		//	CreateCreep(manager);
		//	CreateFireTower(manager);
		//	AddSingleCreepAndTowerToManager(manager);
		//	AdvanceTimeAndUpdateEntities(1);
		//}
	}
}