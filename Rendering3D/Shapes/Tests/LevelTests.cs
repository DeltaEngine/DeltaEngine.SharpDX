using System.Collections.Generic;
using DeltaEngine.Content.Xml;
using DeltaEngine.Datatypes;
using DeltaEngine.GameLogic;
using NUnit.Framework;

namespace DeltaEngine.Rendering3D.Shapes3D.Tests
{
	public class LevelTests
	{
		[Test]
		public void CreateALevel()
		{
			level = new Level(new Size(2, 2));
			level.SetTile(new Vector2D(0, 0), LevelTileType.SpawnPoint);
			level.SetTile(new Vector2D(1, 1), LevelTileType.ExitPoint);
			level.AddWave(new Wave(2.0f, 1.0f, "Test", 20.0f));
			var stream = new XmlFile(BuildXmlData()).ToMemoryStream();
			var loadedLevel = new Level(new Size(0, 0));
			loadedLevel.LoadDataFromStream(stream);
			Assert.AreEqual(loadedLevel.Size, level.Size);
			Assert.AreEqual(loadedLevel.MapData[0, 0], level.MapData[0, 0]);
			Assert.AreEqual(loadedLevel.Waves[0].MaxTime, level.Waves[0].MaxTime);
			Assert.AreEqual(loadedLevel.Waves[0].SpawnInterval, level.Waves[0].SpawnInterval);
			Assert.AreEqual(loadedLevel.Waves[0].SpawnTypeList, level.Waves[0].SpawnTypeList);
			Assert.AreEqual(loadedLevel.Waves[0].WaitTime, level.Waves[0].WaitTime);
		}

		private Level level;

		public XmlData BuildXmlData()
		{
			var xml = new XmlData("Level");
			xml.AddAttribute("Name", "TestName");
			xml.AddAttribute("Size", level.Size);
			AddPoints(xml, LevelTileType.SpawnPoint);
			AddPoints(xml, LevelTileType.ExitPoint);
			xml.AddChild("Map", level.ToTextForXml());
			AddWaves(xml);
			return xml;
		}

		private void AddPoints(XmlData xml, LevelTileType pointType)
		{
			int counter = 0;
			foreach (var point in FindPoints(pointType))
			{
				var pointXml = new XmlData(pointType.ToString());
				pointXml.AddAttribute("Name", pointType.ToString().Replace("Point", "") + (counter++));
				pointXml.AddAttribute("Position", point);
				xml.AddChild(pointXml);
			}
		}

		private IEnumerable<Vector2D> FindPoints(LevelTileType pointType)
		{
			var result = new List<Vector2D>();
			for (int y = 0; y < level.Size.Height; y++)
				for (int x = 0; x < level.Size.Width; x++)
					if (level.MapData[x, y] == pointType)
						result.Add(new Vector2D(x, y));
			return result;
		}

		private void AddWaves(XmlData xml)
		{
			foreach (var wave in level.Waves)
				xml.AddChild(wave.AsXmlData());
		}
	}
}