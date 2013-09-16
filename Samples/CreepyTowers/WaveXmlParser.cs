using System;
using System.Collections.Generic;
using System.Globalization;
using DeltaEngine.Content;
using DeltaEngine.Content.Xml;
using DeltaEngine.Datatypes;

namespace CreepyTowers
{
	public class WaveXmlParser
	{
		public WaveXmlParser()
		{
			WaveObjectsList = new Queue<Wave>();
			SpawnPointsList = new List<Tuple<int, int>>();
			ExitPointsList = new List<Tuple<int, int>>();
		}

		public Queue<Wave> WaveObjectsList { get; private set; }
		public List<Tuple<int, int>> SpawnPointsList { get; private set; }
		public List<Tuple<int, int>> ExitPointsList { get; private set; }
		public Size GridSize { get; private set; }

		public void ParseXml(string xmlName)
		{
			var xmlContent = ContentLoader.Load<XmlContent>(xmlName);
			if (xmlContent == null)
				return;
			var size = xmlContent.Data.GetAttributeValue("Size").Split(',');
			GridSize = new Size(int.Parse(size[0]), int.Parse(size[1]));
			foreach (XmlData spawnPoint in xmlContent.Data.GetChildren("SpawnPoint"))
			{
				var spawnPosition = spawnPoint.GetAttributeValue("Position").Split(',');
				SpawnPointsList.Add(new Tuple<int, int>(int.Parse(spawnPosition[0]),
					int.Parse(spawnPosition[1])));
			}

			foreach (XmlData goalPoint in xmlContent.Data.GetChildren("GoalPoint"))
			{
				var goalPosition = goalPoint.GetAttributeValue("Position").Split(',');
				ExitPointsList.Add(new Tuple<int, int>(int.Parse(goalPosition[0]),
					int.Parse(goalPosition[1])));
			}

			var waves = xmlContent.Data.GetChild("Waves");
			if (waves == null || waves.Children.Count == 0)
				return;

			foreach (XmlData wave in waves.GetChildren("Wave"))
				WaveObjectsList.Enqueue(CreateNewWave(wave));
		}

		private static Wave CreateNewWave(XmlData wave)
		{
			return new Wave
			{
				WaitTime = float.Parse(wave.GetAttributeValue("WaitTime"), CultureInfo.InvariantCulture),
				CreepSpawnInterval = float.Parse(wave.GetAttributeValue("SpawnInterval"), 
				CultureInfo.InvariantCulture),
				MaxCreeps = int.Parse(wave.GetAttributeValue("MaxCreeps"), CultureInfo.InvariantCulture),
				CreepList = new List<string>(wave.GetAttributeValue("CreepTypeList").Split(','))
			};
		}
	}
}