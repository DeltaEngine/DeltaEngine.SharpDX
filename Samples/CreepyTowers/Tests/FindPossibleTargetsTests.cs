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
using DeltaEngine.Rendering3D.Shapes3D;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	public class FindPossibleTargetsTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Initialize()
		{
			new Game(Resolve<Window>(), Resolve<Device>());
			grid = Game.CameraAndGrid.Grid;
			new Manager(6.0f);
		}

		private LevelGrid grid;

		[Test]
		public void InAbsenceOfCreepsTheTowerDoesntFire()
		{
			CreateDefaultTower(Vector3D.Zero, 0.5f);
			Assert.AreEqual(1, EntitiesRunner.Current.GetEntitiesOfType<Tower>().Count);
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Line3D>().Count);
		}

		[Test]
		public void TowerFiresAtCreep()
		{
			var towerPos = grid.PropertyMatrix[2, 3].MidPoint;
			var creepPos = grid.PropertyMatrix[2, 2].MidPoint;
			CreateDefaultTower(towerPos, 20.0f);
			var creep = CreateDefaultCreep(creepPos);
			var creepHealthBefore = creep.Get<CreepProperties>().CurrentHp;
			AdvanceTimeAndUpdateEntities();
			Assert.AreEqual(1, EntitiesRunner.Current.GetEntitiesOfType<Line3D>().Count);
			Assert.Less(creepHealthBefore, creep.Get<CreepProperties>().CurrentHp);
		}

		private static void CreateDefaultTower(Vector3D position, float attackFreq)
		{
			new Tower(position, Names.TowerWaterRangedWaterspray,
				new TowerProperties
				{
					TowerType = Tower.TowerType.Water,
					Name = Names.TowerWaterRangedWaterspray,
					AttackFrequency = attackFreq,
					Range = 0.4f
				});
		}

		private static Creep CreateDefaultCreep(Vector3D position)
		{
			var creep = new Creep(position, Names.CreepCottonMummy,
				new CreepProperties
				{
					MaxHp = 100.0f,
					CurrentHp = 100.0f,
					Resistance = 1.0f,
					CreepType = Creep.CreepType.Cloth,
					GoldReward = 20,
					Speed = 2.0f,
					Name = Names.CreepCottonMummy,
				});

			AddDefaultMovementComponent(creep);
			return creep;
		}

		private static void AddDefaultMovementComponent(Creep creep)
		{
			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector3D(0.0f, 0.0f, 0.0f),
				StartGridPos = new Tuple<int, int>(2, 2),
				Waypoints = new List<Tuple<int, int>> { new Tuple<int, int>(4, 2) }
			});
		}

		[Test]
		public void TowerDoesntAttackIfAttackFrequencyIsHigherThanElapsedTime()
		{
			var towerPos = grid.PropertyMatrix[2, 3].MidPoint;
			var creepPos = grid.PropertyMatrix[2, 2].MidPoint;
			CreateDefaultTower(towerPos, 0.5f);
			var creep = CreateDefaultCreep(creepPos);
			var creepHealthBefore = creep.Get<CreepProperties>().CurrentHp;
			AdvanceTimeAndUpdateEntities();
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Line3D>().Count);
			Assert.AreEqual(creepHealthBefore, creep.Get<CreepProperties>().CurrentHp);
		}
	}
}