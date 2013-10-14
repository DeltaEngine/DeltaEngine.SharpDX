using System;
using System.Collections.Generic;
using CreepyTowers.Creeps;
using DeltaEngine.Datatypes;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests.Creeps
{
	public class MovementInGridTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void CreateCreepWithMovementData()
		{
			var movementData = new MovementInGrid.MovementData
			{
				Velocity = Vector3D.One,
				StartGridPos = new Tuple<int, int>(1, 2),
				Waypoints = new List<Tuple<int, int>>()
			};
			creep = new Creep(CreepType.Cloth, Vector3D.Zero, 0);
			creep.Position = creep.Grid.PropertyMatrix[1, 2].MidPoint;
		}

		private Creep creep;

		[Test]
		public void NoCreepMovementIfNoWaypointsAvailable()
		{
			var creepStartPos = creep.Position;
			AdvanceTimeAndUpdateEntities(1.0f);
			var creepFinalPos = creep.Position;
			Assert.AreEqual(creepFinalPos, creepStartPos);
		}

		[Test]
		public void NoCreepMovementIfNotMovementDataAvailable()
		{
			creep.Remove<MovementInGrid.MovementData>();
			var creepStartPos = creep.Position;
			AdvanceTimeAndUpdateEntities(1.0f);
			var creepFinalPos = creep.Position;
			Assert.AreEqual(creepFinalPos, creepStartPos);
		}

		[Test]
		public void NoCreepMovementIfNoCreepVelocity()
		{
			creep.Get<MovementInGrid.MovementData>().Velocity = Vector3D.Zero;
			var creepStartPos = creep.Position;
			AdvanceTimeAndUpdateEntities(1.0f);
			var creepFinalPos = creep.Position;
			Assert.AreEqual(creepFinalPos, creepStartPos);
		}

		[Test]
		public void CreepMovementAlongXAxis()
		{
			var creepSpeed = creep.Get<CreepData>().Speed;
			creep.Remove<MovementInGrid.MovementData>();
			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector3D(creepSpeed, 0.0f, 0.0f),
				StartGridPos = new Tuple<int, int>(1, 2),
				Waypoints =
					new List<Tuple<int, int>> { new Tuple<int, int>(2, 2), new Tuple<int, int>(3, 2) }
			});
			var creepStartPos = creep.Position;
			AdvanceTimeAndUpdateEntities(1.0f);
			var creepFinalPos = creep.Position;
			Assert.Greater(creepStartPos.X, creepFinalPos.X);
		}

		[Test]
		public void CreepMovementAlongYAxis()
		{
			var creepSpeed = creep.Get<CreepData>().Speed;
			creep.Remove<MovementInGrid.MovementData>();
			creep.Add(new MovementInGrid.MovementData
			{
				Velocity = new Vector3D(0.0f, creepSpeed, 0.0f),
				StartGridPos = new Tuple<int, int>(1, 2),
				Waypoints =
					new List<Tuple<int, int>> { new Tuple<int, int>(1, 3), new Tuple<int, int>(1, 4) }
			});
			var creepStartPos = creep.Position;
			AdvanceTimeAndUpdateEntities(1.0f);
			var creepFinalPos = creep.Position;
			Assert.Greater(creepStartPos.Y, creepFinalPos.Y);
		}
	}
}