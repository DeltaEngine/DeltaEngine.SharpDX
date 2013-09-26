using System.Collections.Generic;
using CreepyTowers.Creeps;
using DeltaEngine.Core;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests.Creeps
{
	public class GenerateWavesTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Initialize()
		{
			new Game(Resolve<Window>(), Resolve<Device>());
			createWaves = new WaveManager(CreateWaveList(), new Manager(6.0f));
		}

		private WaveManager createWaves;

		private List<Wave> CreateWaveList()
		{
			list = new List<Wave>
			{
				new Wave
				{
					WaitTime = 2.0f,
					CreepSpawnInterval = 2.0f,
					MaxCreeps = 2,
					MaxTimeTillNextWave = 10.0f,
					CreepList = new List<string> { "Cloth", "Plastic" }
				},
				new Wave
				{
					WaitTime = 3.0f,
					CreepSpawnInterval = 1.0f,
					MaxCreeps = 3,
					MaxTimeTillNextWave = 10.0f,
					CreepList = new List<string> { "Cloth", "Plastic", "Wood" }
				}
			};

			return list;
		}

		private List<Wave> list;

		[Test]
		public void WaveCreateOnlyAfterWaitTime()
		{
			Assert.AreEqual(2, createWaves.waveList.Count);
			AdvanceTimeAndUpdateEntities(6.0f);
			Assert.AreEqual(1, createWaves.waveList.Count);
			AdvanceTimeAndUpdateEntities(10.0f);
			Assert.AreEqual(0, createWaves.waveList.Count);
		}

		[Test]
		public void CreepSpawnedInWaveOnlyAfterSpawnInterval()
		{
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
			AdvanceTimeAndUpdateEntities(3.0f);
			Assert.AreEqual(1, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
			AdvanceTimeAndUpdateEntities(3.0f);
			Assert.AreEqual(2, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
		}
	}
}