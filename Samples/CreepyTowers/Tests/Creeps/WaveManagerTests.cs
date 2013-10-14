using System.Collections.Generic;
using CreepyTowers.Creeps;
using DeltaEngine.Core;
using DeltaEngine.Entities;

using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests.Creeps
{
	public class WaveManagerTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Initialize()
		{
			new Game(Resolve<Window>());
			manager = new Manager(6.0f);
			var waveA = new CreepWave(1.0f, 1.0f, "Paper, Paper, Paper, Paper, Cloth, Cloth");
			var waveB = new CreepWave(0.0f, 0.05f, "Paper, Squad");
			waveList = new List<CreepWave> { waveB };
		}

		private List<CreepWave> waveList;
		private Manager manager;

		[Test]
		public void SpawnCreepWaves()
		{
			new WaveManager(waveList, manager);
		}

		[Test]
		public void IfNoWaveDataThenNoWavesSpawned()
		{
			var waves = new List<CreepWave>();
			new WaveManager(waves, manager);
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
		}

		[Test]
		public void WaveCreateOnlyAfterWaitTime()
		{
			new WaveManager(waveList, manager);
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
			AdvanceTimeAndUpdateEntities(2.1f);
			Assert.AreEqual(1, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
		}

		[Test]
		public void CreepSpawnedInWaveOnlyAfterSpawnInterval()
		{
			new WaveManager(waveList, manager);
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
			AdvanceTimeAndUpdateEntities(1.1f);
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
			AdvanceTimeAndUpdateEntities(2.1f);
			Assert.AreEqual(1, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
			AdvanceTimeAndUpdateEntities(2.6f);
			Assert.AreEqual(1, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
		}

		[Test]
		public void NoCreepsSpawnedIfCreepListIsEmpty()
		{
			var waveA = new CreepWave(0.0f, 1.0f, "");
			var waves = new List<CreepWave> { waveA };
			new WaveManager(waves, manager);
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
		}

		[Test]
		public void CheckForSpawnOfCreepsInGroup()
		{
			var waveA = new CreepWave(0.0f, 1.0f, "Cloth, Squad");
			waveList = new List<CreepWave> { waveA };
			new WaveManager(waveList, manager);
			Assert.AreEqual(0, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
			AdvanceTimeAndUpdateEntities(1.1f);
			Assert.AreEqual(1, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
			AdvanceTimeAndUpdateEntities(1.6f);
			Assert.AreEqual(2, EntitiesRunner.Current.GetEntitiesOfType<Creep>().Count);
		}

		[Test]
		public void SpawnCreepGroup()
		{
			var waveA = new CreepWave(0.0f, 0.05f, "Cloth, Squad");
			new WaveManager(new List<CreepWave> { waveA }, manager);
		}
	}
}