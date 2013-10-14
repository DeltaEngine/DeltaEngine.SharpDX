using System.Collections.Generic;
using System.Linq;
using CreepyTowers.Creeps;
using CreepyTowers.Levels;
using CreepyTowers.Towers;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.GameLogic;
using DeltaEngine.GameLogic.PathFinding;
using DeltaEngine.Multimedia;
using DeltaEngine.Rendering2D.Fonts;
using DeltaEngine.Rendering2D.Shapes;
using DeltaEngine.ScreenSpaces;

namespace CreepyTowers.Tests.Simple2D
{
	//TODO: merge with existing Creep, Tower, etc. classes
	public class Basic2DDisplaySystem : Entity, Updateable
	{
		public Basic2DDisplaySystem()
		{
			ParseLevel();
			graph = new LevelGraph((int)Size.Width, (int)Size.Height);
			Creeps = new ChangeableList<Creep2D>();
			towerPropertiesXml = new TowerPropertiesXml("TowerProperties");
			shootingManager = new ShootingManager(this);
			InitializeScaling(Size);
			InitializeTopLeftText();
			CreateBackgroundGrid(Size);
			var music = ContentLoader.Load<Music>("GameMusic");
			music.Loop = true;
			music.Play();

		}

		public Size Size { get; private set; }
		private readonly LevelGraph graph;
		private readonly TowerPropertiesXml towerPropertiesXml;
		private readonly ShootingManager shootingManager;

		private void ParseLevel()
		{
			level = ContentLoader.Load<GameLevelRoom>("LevelsChildrensRoomInfo");
			GameTrigger.OnGameStarting();
			Size = level.Size;
			start = level.SpawnPoints[0];
			end = level.GoalPoints[0];
			wave = new CreepWave(level.Waves.First());
		}

		private GameLevelRoom level;
		private Vector2D start;
		private Vector2D end;
		private CreepWave wave;

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
			topLeftText = new FontText(Font.Default, "",
				ScreenSpace.Current.Viewport.Increase(new Size(-0.015f)));
			topLeftText.HorizontalAlignment = HorizontalAlignment.Left;
			topLeftText.VerticalAlignment = VerticalAlignment.Top;
			UpdateTopLeftText();
		}

		private void UpdateTopLeftText()
		{
			topLeftText.Text = "Gold: " + level.Gold + "\n" + "Kills: " + kills + "\n" + "Towers: " +
				towers.Count + "\n" + "Creeps: " + Creeps.Count + "\n" + "Lives: " + level.Lives + 
				"\n" + "Time: " + level.Time;
		}

		private FontText topLeftText;
		private int kills;

		public int Gold
		{
			get { return level.Gold; }
			set
			{
				level.Gold = value;
				UpdateTopLeftText();
			}
		}

		public int Kills
		{
			get { return kills; }
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

		public List<Vector3D> GetPath(Vector2D startNode, Vector2D endNode)
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

		public Vector2D GetGridPosition(Vector2D screenPosition)
		{
			var gridPosition = (screenPosition - gridOffset) / gridScaling;
			return new Vector2D((int)gridPosition.X, (int)gridPosition.Y);
		}

		public void AddWall(Vector2D position)
		{
			if (position.X < 0 || position.Y < 0 || position.X >= Size.Width ||
				position.Y >= Size.Height)
				return;
			if (IsPossibleAddUnreachableGrid(position))
				walls.Add(new FilledRect(CalculateGridScreenDrawArea(position), Color.White));
		}

		private readonly List<FilledRect> walls = new List<FilledRect>();

		private bool IsPossibleAddUnreachableGrid(Vector2D position)
		{
			var index = (int)(position.Y * Size.Width + position.X);
			if (graph.IsUnreachableNode(index) || IsInCreepPath(position))
				return false;
			SetGridUnreachableAndUpdate(index);
			var list = GetPath(start, end);
			return UpdateListStartEnd(list, index, position);
		}

		private bool IsInCreepPath(Vector2D position)
		{
			foreach (var creep in Creeps)
				if (MathExtensions.Abs((int)creep.Position.X - (int)position.X) <= 1 &&
					MathExtensions.Abs((int)creep.Position.Y - (int)position.Y) <= 1 &&
					creep.listOfNodes.Contains(position))
					return true;
			return false;
		}

		private void SetGridUnreachableAndUpdate(int index)
		{
			graph.SetUnreachableAndUpdate(index);
		}

		private bool UpdateListStartEnd(List<Vector3D> list, int index, Vector2D position)
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
				var list = GetPath(creep.listOfNodes[0].GetVector2D(), creep.Target.GetVector2D());
				if (list.Count == 0)
					return false;
				creep.listOfNodes = list;
			}
			return true;
		}

		public Rectangle CalculateGridScreenDrawArea(Vector3D position)
		{
			return new Rectangle(gridOffset + position.GetVector2D() * gridScaling, gridScaling);
		}

		public void AddTower(Vector2D position, TowerType towerType)
		{
			if (position.X < 0 || position.Y < 0 || position.X >= Size.Width ||
				position.Y >= Size.Height)
				return;
			var tower = towers.FirstOrDefault(x => x.Position == position);
			if (tower != null)
				tower.IsTowerActive = !tower.IsTowerActive;
			if (towerPropertiesXml.Get(towerType).Cost <= Gold)
				AddAffordableTower(position, towerType);
		}

		private readonly ChangeableList<Tower2D> towers = new ChangeableList<Tower2D>();

		private void AddAffordableTower(Vector2D position, TowerType towerType)
		{
			if (!IsPossibleAddUnreachableGrid(position))
				return;
			ContentLoader.Load<Sound>("TowerBuilding").Play();
			towers.Add(new Tower2D(this, position, towerType));
		}

		public void Update()
		{
			GameTrigger.OnUpdateEverySecond();
			if (Time.CheckEvery(wave.SpawnInterval) && Creeps.Count < wave.TotalCreepCount)
				SpawnCreeps();
			RemoveDeadCreeps();
			shootingManager.ShootCreeps(towers);
			UpdateTopLeftText();
		}

		private void SpawnCreeps()
		{
			var item = wave.CreepSpawnList[0];
			if (item.GetType() == typeof(CreepType))
				AddCreep(start, end, (CreepType)wave.CreepSpawnList[0]);
			else if (item.GetType() == typeof(CreepGroup))
				AddCreep(start, end, ((CreepGroup)item).CreepList[0]);
		}

		public ChangeableList<Creep2D> Creeps { get; private set; }

		public Creep2D AddCreep(Vector2D startNode, Vector2D targetNode, CreepType creepType)
		{
			var list = GetPath(startNode, targetNode);
			var creep = new Creep2D(this, startNode, creepType) { Target = targetNode };
			creep.listOfNodes = list;
			Creeps.Add(creep);
			return creep;
		}

		private void RemoveDeadCreeps()
		{
			foreach (var creep in Creeps)
			{
				if (creep.CurrentHp > 0 && creep.Position != creep.Target)
					continue;
				if (creep.Position != creep.Target)
					level.Gold += creep.GoldReward;
				else
					GameTrigger.OnEnemyReachGoal();
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
				if ((int)(position.X + 0.5f) == (int)tower.Position.X &&
					(int)(position.Y + 0.5f) == (int)tower.Position.Y)
					return false;
			return true;
		}

		public bool IsPauseable
		{
			get { return true; }
		}
	}
}