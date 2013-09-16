using System.Collections.Generic;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests
{
	public class WaveTests : TestWithMocksOrVisually
	{

		[Test]
		public void WaveDataCreation()
		{
			var wave = new Wave
			{
				WaitTime = 5.0f,
				CreepSpawnInterval = 1.0f,
				MaxCreeps = 3,
				MaxTimeTillNextWave = 10.0f,
				CreepList = new List<string> { Names.CreepCottonMummy, Names.CreepCottonMummy}
			};

			Assert.AreEqual(5.0f, wave.WaitTime);
			Assert.AreEqual(1.0f, wave.CreepSpawnInterval);
			Assert.AreEqual(3, wave.MaxCreeps);
			Assert.AreEqual(10.0f, wave.MaxTimeTillNextWave);
			Assert.AreEqual(Names.CreepCottonMummy, wave.CreepList[0]);
		}


		[Test, CloseAfterFirstFrame]
		public void CheckXmlNotNull()
		{
			var waveXmlParser = new WaveXmlParser();
			waveXmlParser.ParseXml(Names.XmlChildrensRoom);
			Assert.NotNull(waveXmlParser);
		}
	}
}