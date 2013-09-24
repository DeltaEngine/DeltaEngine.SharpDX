using System.Collections.Generic;
using System.Linq;
using $safeprojectname$.Creeps;
using $safeprojectname$.PathFinding;
using $safeprojectname$.Towers;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Rendering2D.Shapes;
using DeltaEngine.ScreenSpaces;

namespace $safeprojectname$.Simple2D
{
	public class Basic2DDisplaySystem : Entity, Updateable
	{
		public Basic2DDisplaySystem(string levelName)
		{
			ParseLevel(levelName);
			graph = new PathFindingGraph((int)Size.Width, (int)Size.Height);
			Creeps = new ChangeableList<Creep2D>();
			shootingManager = new ShootingManager(this);
			InitializeScaling(Size);
			InitializeTopLeftText();
			CreateBackgroundGrid(Size);
			InitializePathFindingGraph();
		}

		public Size Size
		{
			get;
			private set;
		}

		private readonly PathFindingGraph graph;
		private readonly ShootingManager shootingManager;

		private void ParseLevel(string levelName)
		{
			var parser = new WaveXmlParser();
			parser.ParseXml(levelName);
			Size = parser.GridSize;
			start = new Vector2D(parser.SpawnPointsList [0].Item1, parser.SpawnPointsList [0].Item2);
			end = new Vector2D(parser.ExitPointsList [0].Item1, parser.ExitPointsList [0].Item2);
			wave = parser.WaveObjectsList.First();
			gold = parser.Gold;
		}

		private Vector2D start;
		private Vector2D end;
		private Wave wave;

		private void InitializeScaling(Size size)
		{
			Vector2D center = Vector2D.Half;
			var gridSize = new Size(0.55f);
			gridOffset = center - gridSize / 2;
			gridScaling = gridSize / Size;
		}

		private Vector2D gridOffset;
		private Size gridScaling;

		private void InitializeTopLeftText()
		{
			topLeftText = new FontText(Font.Default, "", ScreenSpace.Current.Viewport.Increase(new 
				Size(-0.015f)));
			topLeftText.HorizontalAlignment = HorizontalAlignment.Left;
			topLeftText.VerticalAlignment = VerticalAlignment.Top;
			UpdateTopLeftText();
		}

		private void UpdateTopLeftText()
		{
			topLeftText.Text = "Gold: " + gold + "\n" + "Kills: " + kills + "\n" + "Towers: " + 
				towers.Count + "\n" + "Creeps: " + Creeps.Count;
		}

		private FontText topLeftText;
		private int gold;
		private int kills;

		public int Gold
		{
			get
			{
				return gold;
			}
			set
			{
				gold = value;
				UpdateTopLeftText();
			}
		}

		public int Kills
		{
			get
			{
				return kills;
			}
			set
			{
				kills = value;
				UpdateTopLeftText();
			}
		}

		private void CreateBackgroundGrid(Size size)
		{
			for (int x = 0; x < size.Height + 1; x++)
				new Line2D(CalculateGridScreenPosition(new Vector2D(x, 0)), 
					CalculateGridScreenPosition(new Vector2D(x, (int)size.Height)), Color.Gray);

			for (int y = 0; y < size.Height + 1; y++)
				new Line2D(CalculateGridScreenPosition(new Vector2D(0, y)), 
					CalculateGridScreenPosition(new Vector2D((int)size.Width, y)), Color.Gray);
		}

		private Vector2D CalculateGridScreenPosition(Vector2D position)
		{
			return gridOffset + position * gridScaling;
		}

		private void InitializePathFindingGraph()
		{
			for (int x = 0; x < Size.Width; x++)
				for (int y = 0; y < Size.Height; y++)
					graph.SetNodePosition(y, x, GetPositionOfNode(x, y));

			graph.MakeConnections();
		}

		public List<Vector2D> GetPath(Vector2D startNode, Vector2D endNode)
		{
			var indexStart = graph.GetClosestNode(startNode);
			var indexEnd = graph.GetClosestNode(endNode);
			var aStar = new AStar();
			var path = new List<Vector2D>();
			if (aStar.Search(graph, indexStart, indexEnd))
				path = aStar.GetPath();

			return GetListWithCoordinates(path);
		}

		private List<Vector2D> GetListWithCoordinates(List<Vector2D> path)
		{
			var list = new List<Vector2D>();
			foreach (var node in path)
				list.Add(GetGridPosition(node));

			return list;
		}

		public Vector2D GetGridPosition(Vector2D screenPosition)
		{
			var gridPosition = (screenPosition - gridOffset) / gridScaling;
			return new Vector2D((int)gridPosition.X, (int)gridPosition.Y);
		}

		public Vector2D GetPositionOfNode(int column, int row)
		{
			return CalculateGridScreenPosition(new Vector2D(column + 0.5f, row + 0.5f));
		}

		public void AddWall(Vector2D position)
		{
			if (position.X < 0 || position.Y < 0 || position.X >= Size.Width || position.Y >= Size.Height)
				return;

			if (IsPossibleAddUnreachableGrid(position))
				walls.Add(new FilledRect(CalculateGridScreenDrawArea(position), Color.White));
		}

		private readonly List<FilledRect> walls = new List<FilledRect>();

		private bool IsPossibleAddUnreachableGrid(Vector2D position)
		{
			var index = (int)(position.Y * Size.Width + position.X);
			if (graph.IsUnreachableNode(index))
				return false;

			SetGridUnreachableAndUpdate(index);
			var list = GetPath(GetPositionOfNode((int)start.X, (int)start.Y), 
				GetPositionOfNode((int)end.X, (int)end.Y));
			if (!UpdateListStartEnd(list, index, position))
				return false;

			return true;
		}

		private void SetGridUnreachableAndUpdate(int index)
		{
			graph.SetUnreachableAndUpdate(index);
		}

		private bool UpdateListStartEnd(List<Vector2D> list, int index, Vector2D position)
		{
			if (!UpdateExistingCreeps(position) || list.Count == 0)
			{
				graph.SetReachableAndUpdate(index);
				return false;
			}
			return true;
		}

		private bool UpdateExistingCreeps(Vector2D position)
		{
			foreach (var creep in Creeps)
			{
				if (creep.listOfNodes.Count == 0 || !creep.listOfNodes.Contains(position))
					continue;

				var list = GetPath(GetPositionOfNode((int)creep.listOfNodes [0].X, (int)creep.listOfNodes 
					[0].Y), GetPositionOfNode((int)creep.Target.X, (int)creep.Target.Y));
				if (list.Count == 0)
					return false;

				creep.listOfNodes = list;
			}
			return true;
		}

		public Rectangle CalculateGridScreenDrawArea(Vector2D position)
		{
			return new Rectangle(gridOffset + position * gridScaling, gridScaling);
		}

		public void AddTower(Vector2D position, Tower.TowerType towerType)
		{
			if (position.X < 0 || position.Y < 0 || position.X >= Size.Width || position.Y >= Size.Height)
				return;

			if (new TowerPropertiesXmlParser().TowerPropertiesData [towerType].Cost <= Gold)
				if (IsPossibleAddUnreachableGrid(position))
					towers.Add(new Tower2D(this, position, towerType));
		}

		private readonly ChangeableList<Tower2D> towers = new ChangeableList<Tower2D>();

		public void Update()
		{
			if (Time.CheckEvery(wave.CreepSpawnInterval) && Creeps.Count < wave.MaxCreeps)
				SpawnCreeps();

			RemoveDeadCreeps();
			shootingManager.ShootCreeps(towers);
			UpdateTopLeftText();
		}

		private void SpawnCreeps()
		{
			AddCreep(start, end, wave.GetCreepList() [0]);
		}

		public ChangeableList<Creep2D> Creeps
		{
			get;
			private set;
		}

		public Creep2D AddCreep(Vector2D startNode, Vector2D targetNode, Creep.CreepType creepType)
		{
			var list = GetPath(GetPositionOfNode((int)startNode.X, (int)startNode.Y), 
				GetPositionOfNode((int)targetNode.X, (int)targetNode.Y));
			var creep = new Creep2D(this, startNode, creepType) {
				Target = targetNode
			};
			creep.listOfNodes = list;
			Creeps.Add(creep);
			return creep;
		}

		private void RemoveDeadCreeps()
		{
			foreach (var creep in Creeps)
			{
				if (creep.Hitpoints > 0 && creep.Position != creep.Target)
					continue;

				gold += creep.GoldReward;
				kills++;
				creep.hitpointBar.IsActive = false;
				creep.IsActive = false;
				Creeps.Remove(creep);
			}
		}

		public void RemoveElement(Vector2D position)
		{
			var index = (int)(position.Y * Size.Width + position.X);
			if (!graph.IsUnreachableNode(index))
				return;

			graph.SetReachableAndUpdate(index);
			RemoveTowersOrWalls(position);
		}

		private void RemoveTowersOrWalls(Vector2D position)
		{
			RemoveTower(position);
			RemoveWall(position);
		}

		private void RemoveTower(Vector2D position)
		{
			var tower = towers.FirstOrDefault(x => x.Position == position);
			if (tower == null)
				return;

			tower.Dispose();
			towers.Remove(tower);
		}

		private void RemoveWall(Vector2D position)
		{
			var wall = walls.FirstOrDefault(x => x.DrawArea == CalculateGridScreenDrawArea(position));
			if (wall == null)
				return;

			wall.IsActive = false;
			walls.Remove(wall);
		}

		public bool ThereIsWallInPosition(Vector2D position)
		{
			int nodeIndex = (int)(position.Y + 0.5f) * (int)Size.Width + (int)(position.X + 0.5f);
			if (!graph.IsUnreachableNode(nodeIndex))
				return false;

			foreach (var tower in towers)
				if ((int)(position.X + 0.5f) == (int)tower.Position.X && (int)(position.Y + 0.5f) == 
					(int)tower.Position.Y)
					return false;

			return true;
		}
	}
}