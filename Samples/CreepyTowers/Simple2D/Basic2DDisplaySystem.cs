using System.Collections.Generic;
using System.Linq;
using CreepyTowers.PathFinding;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Rendering.Fonts;
using DeltaEngine.Rendering.Shapes;
using DeltaEngine.ScreenSpaces;

namespace CreepyTowers.Simple2D
{
  public class Basic2DDisplaySystem : Entity, Updateable
  {
		public Basic2DDisplaySystem(string levelName)
    {
			var parser = new WaveXmlParser();
			parser.ParseXml(levelName);
			Size = parser.GridSize;
			start = new Point(parser.SpawnPointsList[0].Item1, parser.SpawnPointsList[0].Item2);
			end = new Point(parser.ExitPointsList[0].Item1, parser.ExitPointsList[0].Item2);
			wave = parser.WaveObjectsList.First();
      calculateDamage = new CalculateDamage2D();
			graph = new PathFindingGraph((int)Size.Width, (int)Size.Height);
			InitializeScaling(Size);
      InitializeTopLeftText();
			CreateBackgroundGrid(Size);
      InitializePathFindingGraph();
    }

    public Size Size { get; private set; }
    private readonly PathFindingGraph graph;
		private Point start;
		private Point end;
		private readonly Wave wave;

    private void InitializeScaling(Size size)
    {
      Point center = Point.Half;
      var gridSize = new Size(0.55f);
      gridOffset = center - gridSize / 2;
      gridScaling = gridSize / Size;
    }

    private Point gridOffset;
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
      topLeftText.Text = "Gold: " + gold + "\n" + "Kills: " + kills + "\n" + "Towers: " +
        towers.Count + "\n" + "Creeps: " +
        EntitiesRunner.Current.GetEntitiesOfType<Creep2D>().Count;
    }

    private FontText topLeftText;
    private int gold = 150;
    private int kills;

    public int Gold
    {
      get { return gold; }
      set
      {
        gold = value;
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
        new Line2D(CalculateGridScreenPosition(new Point(x, 0)),
          CalculateGridScreenPosition(new Point(x, (int)size.Height)), Color.Gray);
      for (int y = 0; y < size.Height + 1; y++)
        new Line2D(CalculateGridScreenPosition(new Point(0, y)),
          CalculateGridScreenPosition(new Point((int)size.Width, y)), Color.Gray);
    }

    private Point CalculateGridScreenPosition(Point position)
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

    public List<Point> GetPath(Point startNode, Point endNode)
    {
      var indexStart = graph.GetClosestNode(startNode);
      var indexEnd = graph.GetClosestNode(endNode);
      var aStar = new AStar();
      var path = new List<Point>();
      if (aStar.Search(graph, indexStart, indexEnd))
        path = aStar.GetPath();
      return GetListWithCoordinates(path);
    }

    private List<Point> GetListWithCoordinates(List<Point> path)
    {
      var list = new List<Point>();
      foreach (var node in path)
        list.Add(GetGridPosition(node));
      return list;
    }

    public Point GetGridPosition(Point screenPosition)
    {
      var gridPosition = (screenPosition - gridOffset) / gridScaling;
      return new Point((int)gridPosition.X, (int)gridPosition.Y);
    }

    public Point GetPositionOfNode(int column, int row)
    {
      return CalculateGridScreenPosition(new Point(column + 0.5f, row + 0.5f));
    }

    public void AddWall(Point position)
    {
      if (IsPossibleAddUnreachableGrid(position))
        walls.Add(new FilledRect(CalculateGridScreenDrawArea(position), Color.White));
    }

    private readonly List<FilledRect> walls = new List<FilledRect>();

    private bool IsPossibleAddUnreachableGrid(Point position)
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

    private bool UpdateListStartEnd(List<Point> list, int index, Point position)
    {
      if (!UpdateExistingCreeps(position) || list.Count == 0)
      {
        graph.SetReachableAndUpdate(index);
        return false;
      }
      return true;
    }

    private bool UpdateExistingCreeps(Point position)
    {
      foreach (var creep in EntitiesRunner.Current.GetEntitiesOfType<Creep2D>())
      {
        if (creep.listOfNodes.Count == 0 || !creep.listOfNodes.Contains(position))
          continue;
        var list =
          GetPath(GetPositionOfNode((int)creep.listOfNodes[0].X, (int)creep.listOfNodes[0].Y),
            GetPositionOfNode((int)creep.Target.X, (int)creep.Target.Y));
        if (list.Count == 0)
          return false;
        creep.listOfNodes = list;
      }
      return true;
    }

    public Rectangle CalculateGridScreenDrawArea(Point position)
    {
      return new Rectangle(gridOffset + position * gridScaling, gridScaling);
    }

    public void AddTower(Point position, Tower.TowerType towerType)
		{
			if (new TowerPropertiesXmlParser().TowerPropertiesData[towerType].Cost <= Gold)
				if (IsPossibleAddUnreachableGrid(position))
					towers.Add(new Tower2D(this, position, towerType));
    }

    private readonly ChangeableList<Tower2D> towers = new ChangeableList<Tower2D>();

    public void Update()
    {
			if (Time.CheckEvery(wave.CreepSpawnInterval))
        SpawnCreeps();
      RemoveDeadCreeps();
      foreach (var line in lines)
        line.IsActive = false;
      lines.Clear();
      foreach (var tower in towers)
        ShootCreeps(tower);
      UpdateTopLeftText();
    }

    private void SpawnCreeps()
    {
			//AddCreep(new Point(0, 4), new Point(24, 4), Creep.CreepType.Cloth);
			AddCreep(start, end, wave.GetCreepList()[0]);
			//AddCreep(new Point(0, 16), new Point(24, 16), Creep.CreepType.Glass);
    }

    private readonly List<Line2D> lines = new List<Line2D>();
    private readonly CalculateDamage2D calculateDamage;

    public void AddCreep(Point startNode, Point targetNode, Creep.CreepType creepType)
    {
      var list = GetPath(GetPositionOfNode((int)startNode.X, (int)startNode.Y),
        GetPositionOfNode((int)targetNode.X, (int)targetNode.Y));
      var creep = new Creep2D(this, startNode, creepType) { Target = targetNode };
      creep.listOfNodes = list;
    }

    private void RemoveDeadCreeps()
    {
      foreach (var creep in EntitiesRunner.Current.GetEntitiesOfType<Creep2D>())
      {
        if (creep.Hitpoints > 0)
          continue;
        gold += creep.GoldReward;
        kills++;
        creep.hitpointBar.IsActive = false;
        creep.IsActive = false;
      }
    }

    private void ShootCreeps(Tower2D tower)
    {
      if (tower.IsOnCooldown)
				return;
      foreach (var creep in EntitiesRunner.Current.GetEntitiesOfType<Creep2D>())
      {
        creep.UpdateStateTimersAndTimeBasedDamage();
				if (tower.GetAttackType() == Tower.AttackType.DirectShot)
					ShootDirectShot(tower, creep);
      }
    }

	  private void ShootDirectShot(Tower2D tower, Creep2D creep)
	  {
		  if (!((tower.Position - creep.Position).Length < tower.Range))
			  return;
		  lines.Add(new Line2D(tower.Center, creep.Center, tower.Color));
		  creep.Hitpoints = DamageCalculation(creep, tower);
		  tower.SetOnCooldown();
	  }

	  private float DamageCalculation(Creep2D creep, Tower2D tower)
    {
      var properties = creep.data;
      CheckCreepState(tower.Type, creep);
      if (!IsActive)
        return 0;
	    var interactionEffect = calculateDamage.CalculateResistanceBasedOnStates(tower.Type, creep);
			var dmg = CalculateDamage(creep, tower, interactionEffect);
			properties.CurrentHp = properties.CurrentHp - dmg < properties.MaxHp ?
				properties.CurrentHp - dmg : properties.MaxHp;
			return properties.CurrentHp;
		}

		private static float CalculateDamage(Creep2D creep, Tower2D tower, float interactionEffect)
		{
			var properties = creep.data;
      float dmg;
			if (creep.state.Healing)
			{
				dmg = -(tower.Damage * 0.5f);
				creep.state.Healing = false;
			}
			else if (creep.state.Enfeeble)
        dmg = (tower.Damage - (properties.Resistance / 2)) * interactionEffect;
      else
        dmg = (tower.Damage - properties.Resistance) * interactionEffect;
			return dmg;
    }

    private void CheckCreepState(Tower.TowerType type, Creep2D creep)
    {
      if (creep.Type == Creep.CreepType.Cloth)
        ClothCreepStateChanger2D.ChangeStatesIfClothCreep(type, creep);
      else if (creep.Type == Creep.CreepType.Sand)
        SandCreepStateChanger2D.ChangeStatesIfSandCreep(type, creep, this);
      else if (creep.Type == Creep.CreepType.Glass)
        GlassCreepStateChanger2D.ChangeStatesIfGlassCreep(type, creep);
      else if (creep.Type == Creep.CreepType.Wood)
        WoodCreepStateChanger2D.ChangeStatesIfWoodCreep(type, creep);
      else if (creep.Type == Creep.CreepType.Plastic)
        PlasticCreepStateChanger2D.ChangeStatesIfPlasticCreep(type, creep);
      else if (creep.Type == Creep.CreepType.Iron)
        IronCreepStateChanger2D.ChangeStatesIfIronCreep(type, creep);
      else if (creep.Type == Creep.CreepType.Paper)
        PaperCreepStateChanger2D.ChangeStatesIfPaperCreep(type, creep);
    }

    public void RemoveElement(Point position)
    {
      var index = (int)(position.Y * Size.Width + position.X);
      if (!graph.IsUnreachableNode(index))
        return;
      graph.SetReachableAndUpdate(index);
      RemoveTowersOrWalls(position);
    }

    private void RemoveTowersOrWalls(Point position)
    {
      RemoveTower(position);
      RemoveWall(position);
    }

    private void RemoveTower(Point position)
    {
      var tower = towers.FirstOrDefault(x => x.Position == position);
      if (tower == null)
        return;
      tower.Dispose();
			towers.Remove(tower);
    }

    private void RemoveWall(Point position)
    {
      var wall = walls.FirstOrDefault(x => x.DrawArea == CalculateGridScreenDrawArea(position));
      if (wall == null)
        return;
      wall.IsActive = false;
      walls.Remove(wall);
    }
  }
}