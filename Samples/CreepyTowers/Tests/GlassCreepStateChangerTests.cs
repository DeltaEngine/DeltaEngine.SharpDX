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

namespace CreepyTowers.Tests
{
	public class GlassCreepStateChangerTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Initialize()
		{
			new Game(Resolve<Window>(), Resolve<Device>());
			creepProp = new CreepProperties
			{
				Name = Names.CreepGlass,
				CreepType = Creep.CreepType.Glass,
				CurrentHp = 100.0f,
				MaxHp = 200.0f,
				GoldReward = 100,
				Resistance = 3.0f,
				Speed = 3.0f
			};
			creep = new Creep(Vector3D.Zero, Creep.CreepType.Glass, Names.CreepGlass);
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
		public void CheckForFireTower()
		{
			GlassCreepStateChanger.ChangeStatesIfGlassCreep(Tower.TowerType.Fire, creep, creepProp);
			Assert.IsTrue(creep.state.Melt);
			Assert.IsTrue(creep.state.Slow);
			Assert.IsTrue(creep.state.Enfeeble);
		}

		[Test]
		public void CheckForImpactTower()
		{
			GlassCreepStateChanger.ChangeStatesIfGlassCreep(Tower.TowerType.Impact, creep, creepProp);
			Assert.IsTrue(creep.IsActive);
		}
	}
}