using System;
using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Content.Xml;
using DeltaEngine.Extensions;

namespace $safeprojectname$
{
	public class LevelGridXmlParser
	{
		public LevelGridXmlParser()
		{
			waypointsList = new List<WaypointObject>();
			InteractablePointsList = new ChangeableList<Tuple<int, int>>();
			CreepPathsList = new ChangeableList<List<Tuple<int, int>>>();
		}

		private readonly List<WaypointObject> waypointsList;

		public ChangeableList<Tuple<int, int>> InteractablePointsList
		{
			get;
			private set;
		}

		public ChangeableList<List<Tuple<int, int>>> CreepPathsList
		{
			get;
			private set;
		}

		public void ParseXml(string xmlName)
		{
			var xmlContent = ContentLoader.Load<XmlContent>(xmlName);
			if (xmlContent == null)
				return;

			var waypointsContent = xmlContent.Data.GetChild("WaypointGridSlots");
			var interactablepointsContent = xmlContent.Data.GetChild("InteractableGridSlots");
			if (waypointsContent == null || interactablepointsContent == null)
				return;

			foreach (XmlData item in waypointsContent.GetChildren("Waypoint"))
				waypointsList.Add(GetWaypointsContent(item));

			foreach (XmlData item in interactablepointsContent.GetChildren("Interactable"))
				InteractablePointsList.Add(GetGridTuple(item));

			GenerateCreepPathsList();
		}

		private static WaypointObject GetWaypointsContent(XmlData item)
		{
			var type = item.GetAttributeValue("Type");
			var tuple = GetGridTuple(item);
			return new WaypointObject {
				Type = type,
				Waypoint = tuple
			};
		}

		private static Tuple<int, int> GetGridTuple(XmlData item)
		{
			var gridX = int.Parse(item.GetAttributeValue("GridPositionX"));
			var gridZ = int.Parse(item.GetAttributeValue("GridPositionZ"));
			return new Tuple<int, int>(gridX, gridZ);
		}

		private void GenerateCreepPathsList()
		{
			var list = new List<Tuple<int, int>>();
			foreach (WaypointObject waypointObject in waypointsList)
			{
				if (waypointObject.Type == "Start" || waypointObject.Type == "Intermediate")
					list.Add(waypointObject.Waypoint);
				else
				{
					list.Add(waypointObject.Waypoint);
					CreepPathsList.Add(list.ToList());
					list.Clear();
				}
			}
		}
		public class WaypointObject
		{
			public string Type
			{
				get;
				set;
			}

			public Tuple<int, int> Waypoint
			{
				get;
				set;
			}
		}
	}
}