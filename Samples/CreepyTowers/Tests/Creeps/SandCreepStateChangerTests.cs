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

namespace CreepyTowers.Tests.Creeps
{
	public class SandCreepStateChangerTests : TestWithMocksOrVisually
	{

		[SetUp]
		public void Initialize()
		{
			new Game(Resolve<Window>(), Resolve<Device>());
			creepProp = new CreepProperties
			{
				Name = Names.CreepsSandSandy,
				CreepType = Creep.CreepType.Sand,
				CurrentHp = 100.0f,
				MaxHp = 200.0f,
				GoldReward = 100,
				Resistance = 3.0f,
				Speed = 3.0f
			};
			creep = new Creep(Vector3D.Zero, Names.CreepsSandSandy, new CreepProperties());
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
		public void CheckForImapactTowerEffect()
		{
			SandCreepStateChanger.ChangeStatesIfSandCreep(Tower.TowerType.Impact, creep, creepProp);
			Assert.IsTrue(creep.state.Slow);
			Assert.AreEqual(0, creep.state.SlowTimer);
		}

		[Test]
		public void CheckForWaterTowerEffect()
		{
			SandCreepStateChanger.ChangeStatesIfSandCreep(Tower.TowerType.Water, creep, creepProp);
			Assert.IsTrue(creep.state.Slow);
			Assert.AreEqual(0, creep.state.SlowTimer);
			Assert.IsTrue(creep.state.Wet);
		}

		[Test]
		public void CheckForIceTowerOnWetCreep()
		{
			creep.state.Wet = true;
			SandCreepStateChanger.ChangeStatesIfSandCreep(Tower.TowerType.Ice, creep, creepProp);
			Assert.IsTrue(creep.state.Frozen);
			Assert.IsFalse(creep.state.Wet);
		}

		[Test]
		public void CheckForImpactTowerOnFrozenCreep()
		{
			creep.state.Frozen = true;
			SandCreepStateChanger.ChangeStatesIfSandCreep(Tower.TowerType.Impact, creep, creepProp);
			Assert.IsTrue(creep.state.Frozen);
		}

		[Test]
		public void CheckForFireTowerEffectOnWetCreep()
		{
			creep.state.Wet = true;
			SandCreepStateChanger.ChangeStatesIfSandCreep(Tower.TowerType.Fire, creep, creepProp);
			Assert.IsFalse(creep.state.Wet);
		}
	}
}