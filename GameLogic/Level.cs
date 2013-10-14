using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Content.Xml;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering3D.Cameras;

namespace DeltaEngine.GameLogic
{
	/// <summary>
	/// Simple 2D map of TileType data to be used for 2D tile games or for game logic tests.
	/// </summary>
	public class Level : ContentData
	{
		public Level(Size size)
			: base("<GeneratedTileMap" + size + ">")
		{
			Size = size;
			InitializeLevelProperties();
			InitializeScalingAndMap();
		}

		public Size Size
		{
			get { return mapSize; }
			set
			{
				if (mapSize == value)
					return;
				mapSize = value;
				InitializeScalingAndMap();
				if (SizeChanged != null)
					SizeChanged(mapSize);
			}
		}

		public Action<Size> SizeChanged { get; set; }
		private Size mapSize;

		private void InitializeScalingAndMap()
		{
			Vector2D center = Vector2D.Half;
			var gridSize = RenderIn3D ? Size : new Size(0.55f);
			gridScaling = gridSize / Size;
			Position = RenderIn3D ? Vector2D.Zero : center - gridSize / 2;
			MapData = new LevelTileType[(int)mapSize.Width,(int)mapSize.Height];
		}

		public LevelTileType[,] MapData { get; private set; }
		public Vector2D Position;
		public Size gridScaling;

		private void InitializeLevelProperties()
		{
			Waves = new List<Wave>();
			Triggers = new List<GameTrigger>();
			SpawnPoints = new List<Vector2D>();
			GoalPoints = new List<Vector2D>();
			Current = this;
		}

		public List<Wave> Waves;
		public List<GameTrigger> Triggers;
		public List<Vector2D> SpawnPoints;
		public List<Vector2D> GoalPoints;
		public static Level Current;
		public bool RenderIn3D
		{
			get { return renderIn3D; } 
			set
			{
				renderIn3D = value;
				gridScaling = renderIn3D ? Size.One : new Size(0.55f) / Size;
				Position = renderIn3D ? Vector2D.Zero : Vector2D.Half - new Size(0.55f) / 2;
			}
		}

		private bool renderIn3D;

		public Vector2D CalculateGridScreenPosition(Vector2D position)
		{
			return Position + position * gridScaling;
		}

		protected Level(string contentName)
			: base(contentName) {}

		protected override void LoadData(Stream fileData)
		{
			LoadDataFromStream(fileData);
		}

		public void LoadDataFromStream(Stream fileData)
		{
			data = new XmlFile(fileData).Root;
			var mapXml = data.GetChild("Map");
			if (mapXml == null || string.IsNullOrEmpty(mapXml.Value))
				throw new InvalidTileMapData();
			Size = new Size(data.GetAttributeValue("Size"));
			InitializeLevelProperties();
			StoreSpawnAndGoalPoints();
			StoreWaves();
			StoreGameTriggers();
			InitializeMapFromXml(mapXml.Value);
		}

		protected XmlData data;

		public class InvalidTileMapData : Exception {}

		private void StoreSpawnAndGoalPoints()
		{
			var spawnPoints = data.GetChildren("SpawnPoint");
			foreach (var spawnPoint in spawnPoints)
				SpawnPoints.Add(new Vector2D(spawnPoint.GetAttributeValue("Position")));
			var goalPoints = data.GetChildren("GoalPoint");
			foreach (var goalPoint in goalPoints)
				GoalPoints.Add(new Vector2D(goalPoint.GetAttributeValue("Position")));
		}

		private void StoreWaves()
		{
			var loadedWaves = data.GetChildren("Wave");
			foreach (var loadedWave in loadedWaves)
				Waves.Add(new Wave(float.Parse(loadedWave.GetAttributeValue("WaitTime")),
					float.Parse(loadedWave.GetAttributeValue("SpawnInterval"), CultureInfo.InvariantCulture),
					loadedWave.GetAttributeValue("SpawnTypeList"),
					float.Parse(loadedWave.GetAttributeValue("MaxTime"), CultureInfo.InvariantCulture),
					int.Parse(loadedWave.GetAttributeValue("MaxNumber"), CultureInfo.InvariantCulture)));
		}

		protected virtual void StoreGameTriggers() {}

		private void InitializeMapFromXml(string mapXmlData)
		{
			mapXmlData = mapXmlData.Replace("\t", "");
			int x = 0;
			int y = -1;
			foreach (var letter in mapXmlData)
				if (letter == '\n')
				{
					x = 0;
					y++;
				}
				else
				{
					SetTile(new Vector2D(x, y), LetterToTileType(letter));
					x++;
				}
		}

		public void SetTile(Vector2D gridPosition, LevelTileType selectedTileType)
		{
			if (gridPosition.X >= 0 && gridPosition.Y >= 0 && gridPosition.X < mapSize.Width &&
				gridPosition.Y < mapSize.Height)
			{
				MapData[(int)gridPosition.X, (int)gridPosition.Y] = selectedTileType;
				if (TileChanged != null)
					TileChanged(gridPosition);
			}
		}

		private static LevelTileType LetterToTileType(char letter)
		{
			switch (letter)
			{
			case 'X':
				return LevelTileType.Blocked;
			case 'P':
				return LevelTileType.Placeable;
			case 'L':
				return LevelTileType.BlockedPlaceable;
			case 'R':
				return LevelTileType.Red;
			case 'G':
				return LevelTileType.Green;
			case 'B':
				return LevelTileType.Blue;
			case 'Y':
				return LevelTileType.Yellow;
			case 'O':
				return LevelTileType.Brown;
			case 'A':
				return LevelTileType.Gray;
			case 'S':
				return LevelTileType.SpawnPoint;
			case 'E':
				return LevelTileType.ExitPoint;
			default:
				return LevelTileType.Nothing;
			}
		}

		protected override void DisposeData() {}

		public Rectangle CalculateGridScreenDrawArea(Vector2D position)
		{
			return new Rectangle(Position + position * gridScaling, gridScaling);
		}

		public Vector2D GetGridPosition(Vector2D screenPosition)
		{
			var gridPosition = (screenPosition - Position) / gridScaling;
			return new Vector2D((int)gridPosition.X, (int)gridPosition.Y);
		}

		public Action<Vector2D> TileChanged { get; set; }

		public string ToTextForXml()
		{
			string result = Environment.NewLine;
			for (int y = 0; y < Size.Height; y++)
			{
				for (int x = 0; x < Size.Width; x++)
					result += TileToLetterType(MapData[x, y]);
				result += Environment.NewLine;
			}
			return result;
		}

		private static char TileToLetterType(LevelTileType tileType)
		{
			switch (tileType)
			{
			case LevelTileType.Nothing:
				return '.';
			case LevelTileType.Blocked:
				return 'X';
			case LevelTileType.Placeable:
				return 'P';
			case LevelTileType.BlockedPlaceable:
				return 'L';
			case LevelTileType.Red:
				return 'R';
			case LevelTileType.Green:
				return 'G';
			case LevelTileType.Blue:
				return 'B';
			case LevelTileType.Yellow:
				return 'Y';
			case LevelTileType.Brown:
				return 'O';
			case LevelTileType.Gray:
				return 'A';
			case LevelTileType.SpawnPoint:
				return 'S';
			case LevelTileType.ExitPoint:
				return 'E';
			default:
				throw new ArgumentOutOfRangeException("tileType");
			}
		}

		public Color GetColor(LevelTileType tileType)
		{
			switch (tileType)
			{
			case LevelTileType.Blocked:
				return Color.LightGray;
			case LevelTileType.Placeable:
				return Color.CornflowerBlue;
			case LevelTileType.BlockedPlaceable:
				return Color.LightBlue;
			case LevelTileType.Red:
				return Color.Red;
			case LevelTileType.Green:
				return Color.Green;
			case LevelTileType.Blue:
				return Color.Blue;
			case LevelTileType.Yellow:
				return Color.Yellow;
			case LevelTileType.Brown:
				return Color.Brown;
			case LevelTileType.Gray:
				return Color.Gray;
			case LevelTileType.SpawnPoint:
				return Color.PaleGreen;
			case LevelTileType.ExitPoint:
				return Color.DarkGreen;
			default:
				return Color.Black;
			}
		}

		public void AddWave(Wave wave)
		{
			Waves.Add(wave);
		}

		public List<Vector2D> GetAllTilesOfType(LevelTileType tileType)
		{
			var list = new List<Vector2D>();
			for (int x = 0; x < Size.Width; x++)
				for (int y = 0; y < Size.Height; y++)
					if (MapData[x, y] == LevelTileType.Blocked)
						list.Add(new Vector2D(x, y));
			return list;
		}

		public void SetTileWithScreenPosition(Vector2D screenPosition, LevelTileType selectedTileType)
		{
			if (!RenderIn3D)
			{
				var gridPosition = GetGridPosition(screenPosition);
				SetTile(gridPosition, selectedTileType);
			}
			else
			{
				var ray = Camera.Current.ScreenPointToRay(screenPosition);
				var floor = new Plane(Vector3D.UnitZ, 0.0f);
				try
				{
					var position = (Vector3D)floor.Intersect(ray);
					var node = (new Vector2D(position.X, position.Y) - Position) / gridScaling;
					SetTile(node, selectedTileType);
				}
				catch {}
			}
		}
	}
}