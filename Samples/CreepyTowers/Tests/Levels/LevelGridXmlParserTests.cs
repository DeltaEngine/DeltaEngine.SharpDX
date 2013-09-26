using CreepyTowers.Levels;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace CreepyTowers.Tests.Levels
{
	public class LevelGridXmlParserTests : TestWithMocksOrVisually
	{
		[Test]
		public void CheckLevelsChildsRoomXml()
		{
			var xmlParser = new LevelGridXmlParser();
			xmlParser.ParseXml(Names.XmlLevelsChildsRoomGrid);
			Assert.NotNull(xmlParser.CreepPathsList);
			Assert.NotNull(xmlParser.InteractablePointsList);
		}

		[Test]
		public void CheckLevelsBathroomXml()
		{
			var xmlParser = new LevelGridXmlParser();
			xmlParser.ParseXml(Names.XmlLevelsBathroomGrid);
			Assert.NotNull(xmlParser.CreepPathsList);
			Assert.NotNull(xmlParser.InteractablePointsList);
		}

		[Test]
		public void CheckLevelsLivingRoommXml()
		{
			var xmlParser = new LevelGridXmlParser();
			xmlParser.ParseXml(Names.XmlLevelsLivingRoomGrid);
			Assert.NotNull(xmlParser.CreepPathsList);
			Assert.NotNull(xmlParser.InteractablePointsList);
		}
	}
}
