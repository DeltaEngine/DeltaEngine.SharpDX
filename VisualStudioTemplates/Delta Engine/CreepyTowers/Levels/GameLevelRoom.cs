using System.Collections.Generic;
using $safeprojectname$.Creeps;
using DeltaEngine.Commands;
using DeltaEngine.Content.Xml;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.GameLogic;
using DeltaEngine.GameLogic.PathFinding;

namespace $safeprojectname$.Levels
{
	public class GameLevelRoom : Level
	{
		public GameLevelRoom(Size size) : base(size)
		{
			InitializeGameLevelRoom();
		}

		private void InitializeGameLevelRoom()
		{
			creeps = new List<Creep>();
			graph = new LevelGraph((int)Size.Width, (int)Size.Height);
			InitializeGameGraph();
		}

		public List<Creep> creeps;
		private LevelGraph graph;

		private void InitializeGameGraph()
		{
			for (int i = 0; i < Size.Width; i++)
				for (int j = 0; j < Size.Height; j++)
					if (MapData [i, j] == LevelTileType.Blocked)
						graph.SetUnreachableAndUpdate(j * (int)Size.Width + i);
		}

		protected GameLevelRoom(string contentName) : base(contentName)
		{
		}

		protected override void LoadData(System.IO.Stream fileData)
		{
			base.LoadData(fileData);
			InitializeGameLevelRoom();
		}

		protected override void StoreGameTriggers()
		{
			var triggers = data.GetChild("Triggers");
			if (triggers != null)
				foreach (var trigger in triggers.Children)
					InitializeGameTriggers(trigger);
		}

		private void InitializeGameTriggers(XmlData trigger)
		{
			var triggerType = trigger.Name.GetTypeFromShortNameOrFullNameIfNotFound();
			Triggers.Add(Trigger.GenerateTriggerFromType(triggerType, trigger.Name, trigger.Value) as 
				GameTrigger);
		}

		public int Gold
		{
			get;
			set;
		}

		public int Lives
		{
			get;
			set;
		}

		public int Time
		{
			get;
			set;
		}

		public Creep SpawnCreep(CreepType type)
		{
			var list = GetPath(SpawnPoints [0], GoalPoints [0]);
			var creep = new Creep(type, SpawnPoints [0], 0.0f) {
				Target = GoalPoints[0]
			};
			creep.Path = list;
			creeps.Add(creep);
			return creep;
		}

		public List<Vector3D> GetPath(Vector3D startNode, Vector3D endNode)
		{
			var indexStart = graph.GetClosestNode(startNode);
			var indexEnd = graph.GetClosestNode(endNode);
			var aStar = new AStarSearch();
			var path = new List<GraphNode>();
			if (aStar.Search(graph, indexStart, indexEnd))
				path = aStar.GetPath();

			return GetListWithCoordinates(path);
		}

		private List<Vector3D> GetListWithCoordinates(List<GraphNode> path)
		{
			var list = new List<Vector3D>();
			foreach (var node in path)
				list.Add(GetGridPosition(node.Position));

			return list;
		}

		public void RemoveCreep(Creep creep)
		{
			creeps.Remove(creep);
			creep.IsVisible = false;
			creep.Dispose();
		}
	}
}