using System;
using System.Collections.Generic;
using CreepyTowers.Creeps;
using CreepyTowers.Levels;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests.Creeps
{
	public class MovementInGridTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Initialize()
		{
			new Game(Resolve<Window>(), Resolve<Device>());
			grid = new LevelGrid(10, 0.5f);
			creep = new Creep(grid.PropertyMatrix[1, 2].MidPoint, Names.CreepCottonMummy,
				new CreepProperties
				{
					Name = Names.CreepCottonMummy,
					CreepType = Creep.CreepType.Cloth,
					CurrentHp = 100.0f,
					MaxHp = 100.0f,
					GoldReward = 30,
					Resistance = 1.0f,
					Speed = 2.0f, 
				});
		}

		private static void DefaultMovement(Creep creep, List<Tuple<int, int>> wayPointsList, Vector3D velocity)
		{
			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = velocity,
				StartGridPos = new Tuple<int, int>(1, 2),
				Waypoints = wayPointsList
			});
		}

		private LevelGrid grid;
		private Creep creep;

		[Test]
		public void NoCreepMovementIfNotWaypointsAvailable()
		{
			var creepStartPos = creep.Position;
			DefaultMovement(creep, new List<Tuple<int, int>>(), Vector3D.Zero);
			AdvanceTimeAndUpdateEntities(1.0f);
			var creepFinalPos = creep.Position;
			Assert.AreEqual(creepFinalPos, creepStartPos);
		}

		[Test]
		public void CreepMovementAlongXAxis()
		{
			var creepStartPos = creep.Position;
			DefaultMovement(creep, new List<Tuple<int, int>>
				{
					new Tuple<int, int>(1, 2),
					new Tuple<int, int>(1, 3),
					new Tuple<int, int>(1, 4)
				}, new Vector3D(1.0f, 0.0f, 0.0f));

			AdvanceTimeAndUpdateEntities(1.0f);
			var creepFinalPos = creep.Position;
			Assert.Greater(creepFinalPos.X, creepStartPos.X);
		}

		[Test]
		public void CreepMovementAlongYAxis()
		{
			var creepStartPos = creep.Position;
			DefaultMovement(creep, new List<Tuple<int, int>>
				{
					new Tuple<int, int>(1, 2),
					new Tuple<int, int>(1, 3),
					new Tuple<int, int>(1, 4)
				}, new Vector3D(0.0f, 1.0f, 0.0f));

			AdvanceTimeAndUpdateEntities(1.0f);
			var creepFinalPos = creep.Position;
			Assert.Greater(creepFinalPos.Y, creepStartPos.Y);
		}

	}
}