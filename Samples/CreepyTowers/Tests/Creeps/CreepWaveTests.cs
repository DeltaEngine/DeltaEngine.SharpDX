using CreepyTowers.Creeps;
using DeltaEngine.Core;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests.Creeps
{
	public class CreepWaveTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Initialize()
		{
			new Game(Resolve<Window>());
			wave = new CreepWave(5.0f, 1.0f, "Cloth, Iron, Paper, Wood, Glass, Sand, Plastic");
		}

		private CreepWave wave;

		[Test]
		public void WaveDataCreation()
		{
			Assert.AreEqual(5.0f, wave.WaitTime);
			Assert.AreEqual(1.0f, wave.SpawnInterval);
			Assert.AreEqual(7, wave.CreepSpawnList.Count);
		}

		[Test]
		public void CheckCreepList()
		{
			Assert.AreEqual(7, wave.CreepSpawnList.Count);
			Assert.IsTrue(wave.CreepSpawnList.Contains(CreepType.Cloth));
			Assert.IsTrue(wave.CreepSpawnList.Contains(CreepType.Glass));
			Assert.IsTrue(wave.CreepSpawnList.Contains(CreepType.Iron));
			Assert.IsTrue(wave.CreepSpawnList.Contains(CreepType.Paper));
			Assert.IsTrue(wave.CreepSpawnList.Contains(CreepType.Plastic));
			Assert.IsTrue(wave.CreepSpawnList.Contains(CreepType.Sand));
			Assert.IsTrue(wave.CreepSpawnList.Contains(CreepType.Wood));
		}

		[Test]
		public void CheckForGroupSquad()
		{
			wave = new CreepWave(5.0f, 1.0f, "Squad");
			if (wave.CreepSpawnList[0].GetType() == typeof(CreepGroup))
			{
				var group = (CreepGroup)wave.CreepSpawnList[0];
				Assert.AreEqual(3, group.CreepList.Count);
				Assert.AreEqual(CreepType.Plastic, group.CreepList[0]);
				Assert.AreEqual(CreepType.Cloth, group.CreepList[1]);
				Assert.AreEqual(CreepType.Plastic, group.CreepList[2]);
			}
		}

		[Test]
		public void CheckForGroupIronMen()
		{
			wave = new CreepWave(5.0f, 1.0f, "IronMen");
			if (wave.CreepSpawnList[0].GetType() == typeof(CreepGroup))
			{
				var group = (CreepGroup)wave.CreepSpawnList[0];
				Assert.AreEqual(2, group.CreepList.Count);
				Assert.AreEqual(CreepType.Iron, group.CreepList[0]);
				Assert.AreEqual(CreepType.Iron, group.CreepList[1]);
			}
		}

		[Test]
		public void CheckForGroupWoodPeople()
		{
			wave = new CreepWave(5.0f, 1.0f, "WoodPeople");
			if (wave.CreepSpawnList[0].GetType() == typeof(CreepGroup))
			{
				var group = (CreepGroup)wave.CreepSpawnList[0];
				Assert.AreEqual(5, group.CreepList.Count);
				Assert.AreEqual(CreepType.Paper, group.CreepList[0]);
				Assert.AreEqual(CreepType.Paper, group.CreepList[1]);
				Assert.AreEqual(CreepType.Wood, group.CreepList[2]);
				Assert.AreEqual(CreepType.Wood, group.CreepList[3]);
				Assert.AreEqual(CreepType.Wood, group.CreepList[4]);
			}
		}

		[Test]
		public void CheckForGroupSandman()
		{
			wave = new CreepWave(5.0f, 1.0f, "Sandman");
			if (wave.CreepSpawnList[0].GetType() == typeof(CreepGroup))
			{
				var group = (CreepGroup)wave.CreepSpawnList[0];
				Assert.AreEqual(5, group.CreepList.Count);
				Assert.AreEqual(CreepType.Sand, group.CreepList[0]);
				Assert.AreEqual(CreepType.Sand, group.CreepList[1]);
				Assert.AreEqual(CreepType.Glass, group.CreepList[2]);
				Assert.AreEqual(CreepType.Cloth, group.CreepList[3]);
				Assert.AreEqual(CreepType.Sand, group.CreepList[4]);
			}
		}

		[Test]
		public void CheckForGroupGarbage()
		{
			wave = new CreepWave(5.0f, 1.0f, "Garbage");
			if (wave.CreepSpawnList[0].GetType() == typeof(CreepGroup))
			{
				var group = (CreepGroup)wave.CreepSpawnList[0];
				Assert.AreEqual(5, group.CreepList.Count);
				Assert.AreEqual(CreepType.Plastic, group.CreepList[0]);
				Assert.AreEqual(CreepType.Plastic, group.CreepList[1]);
				Assert.AreEqual(CreepType.Cloth, group.CreepList[2]);
				Assert.AreEqual(CreepType.Cloth, group.CreepList[3]);
				Assert.AreEqual(CreepType.Plastic, group.CreepList[4]);
			}
		}

		[Test]
		public void CheckForGroupMix()
		{
			wave = new CreepWave(5.0f, 1.0f, "Mix");
			if (wave.CreepSpawnList[0].GetType() == typeof(CreepGroup))
			{
				var group = (CreepGroup)wave.CreepSpawnList[0];
				Assert.AreEqual(6, group.CreepList.Count);
				Assert.AreEqual(CreepType.Plastic, group.CreepList[0]);
				Assert.AreEqual(CreepType.Paper, group.CreepList[1]);
				Assert.AreEqual(CreepType.Wood, group.CreepList[2]);
				Assert.AreEqual(CreepType.Wood, group.CreepList[3]);
				Assert.AreEqual(CreepType.Cloth, group.CreepList[4]);
				Assert.AreEqual(CreepType.Plastic, group.CreepList[5]);
			}
		}

		[Test]
		public void CheckTotalCreepCountForAWave()
		{
			var waveA = new CreepWave(5.0f, 1.0f, "Cloth, Iron, Paper, Wood, Glass, Sand, Plastic");
			Assert.AreEqual(7, waveA.TotalCreepCount);
			var waveB = new CreepWave(5.0f, 1.0f, "Cloth, Iron, Paper, IronMen");
			Assert.AreEqual(5, waveB.TotalCreepCount);
			var waveC = new CreepWave(5.0f, 1.0f, "Squad, WoodPeople, Sandman, Garbage, Mix, IronMen");
			Assert.AreEqual(26, waveC.TotalCreepCount);
		}
	}
}