using System;
using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using DeltaEngine.Rendering.Models;

namespace CreepyTowers
{
	public class Level : Model, IDisposable
	{
		public Level(string name)
			: base(name, Vector.Zero)
		{
			AddTag(name);
			LoadLevelGridXml(name);
		}

		private void LoadLevelGridXml(string name)
		{
			var xmlParser = new LevelGridXmlParser();
			var xmlFileName = LevelXmlFileName(name);

			if (xmlFileName == null)
				return;

			xmlParser.ParseXml(xmlFileName);
			Add(new GridData
			{
				CreepPathsList = xmlParser.CreepPathsList,
				InteractablePointsList = xmlParser.InteractablePointsList
			});
		}

		private static string LevelXmlFileName(string name)
		{
			string xmlName = null;
			switch (name)
			{
			case Names.LevelsChildsRoom:
				xmlName = Names.XmlLevelsChildsRoomGrid;
				break;

			case Names.LevelsBathRoom:
				xmlName = Names.XmlLevelsBathroomGrid;
				break;

			case Names.LevelsLivingRoom:
				xmlName = Names.XmlLevelsLivingRoomGrid;
				break;
			}

			return xmlName;
		}

		public class GridData
		{
			public ChangeableList<List<Tuple<int, int>>> CreepPathsList { get; set; }
			public ChangeableList<Tuple<int, int>> InteractablePointsList { get; set; }
		}

		public void Dispose()
		{
			IsActive = false;
		}
	}
}