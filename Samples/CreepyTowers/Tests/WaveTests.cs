using System.Collections.Generic;
using CreepyTowers.Creeps;
using DeltaEngine.Core;
using DeltaEngine.Graphics;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	public class WaveTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void Initialize()
		{
			new Game(Resolve<Window>(), Resolve<Device>());
			wave = new Wave
			{
				WaitTime = 5.0f,
				CreepSpawnInterval = 1.0f,
				MaxCreeps = 3,
				MaxTimeTillNextWave = 10.0f,
				CreepList = new List<string> {"Cloth", "Iron", "Paper", "Wood", "Glass", "Sand", "Plastic"}
			};
		}

		private Wave wave;

		[Test]
		public void WaveDataCreation()
		{
			Assert.AreEqual(5.0f, wave.WaitTime);
			Assert.AreEqual(1.0f, wave.CreepSpawnInterval);
			Assert.AreEqual(3, wave.MaxCreeps);
			Assert.AreEqual(10.0f, wave.MaxTimeTillNextWave);
			Assert.AreEqual("Cloth", wave.CreepList[0]);
			Assert.AreEqual(7, wave.CreepList.Count);
		}

		[Test, CloseAfterFirstFrame]
		public void CheckXmlNotNull()
		{
			var waveXmlParser = new WaveXmlParser();
			waveXmlParser.ParseXml(Names.XmlChildrensRoom);
			Assert.NotNull(waveXmlParser);
		}

		[Test]
		public void CheckCreepList()
		{
			Assert.AreEqual(7, wave.GetCreepList().Count);
			Assert.IsTrue(wave.GetCreepList().Contains(Creep.CreepType.Cloth));
			Assert.IsTrue(wave.GetCreepList().Contains(Creep.CreepType.Glass));
			Assert.IsTrue(wave.GetCreepList().Contains(Creep.CreepType.Iron));
			Assert.IsTrue(wave.GetCreepList().Contains(Creep.CreepType.Paper));
			Assert.IsTrue(wave.GetCreepList().Contains(Creep.CreepType.Plastic));
			Assert.IsTrue(wave.GetCreepList().Contains(Creep.CreepType.Sand));
			Assert.IsTrue(wave.GetCreepList().Contains(Creep.CreepType.Wood));
		}
	}
}