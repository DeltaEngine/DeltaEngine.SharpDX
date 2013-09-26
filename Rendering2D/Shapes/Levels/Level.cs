using System;
using System.IO;
using DeltaEngine.Content.Xml;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering2D.Shapes.Levels
{
	/// <summary>
	/// Simple 2D map of TileType data to be used for 2D tile games or for game logic tests.
	/// </summary>
	public class Level : XmlContent
	{
		public Level(Size size)
			: base("<GeneratedTileMap"+size+">")
		{
			Size = size;
			InitializeScalingAndMap();
		}

		private void InitializeScalingAndMap()
		{
			Vector2D center = Vector2D.Half;
			var gridSize = new Size(0.55f);
			gridScaling = gridSize / Size;
			gridOffset = center - gridSize / 2;
			MapData = new LevelTileType[(int)mapSize.Width, (int)mapSize.Height];
		}

		private Vector2D gridOffset;
		private Size gridScaling;

		public Vector2D CalculateGridScreenPosition(Vector2D position)
		{
			return gridOffset + position * gridScaling;
		}

		protected Level(string contentName)
			: base(contentName) {}

		protected override void LoadData(Stream fileData)
		{
			base.LoadData(fileData);
			var mapXml = Data.GetChild("Map");
			if (mapXml == null || string.IsNullOrEmpty(mapXml.Value))
				throw new InvalidTileMapData();
			Size = Data.GetAttributeValue("Map", new Size(8, 8));
			InitializeMapFromXml(mapXml.Value);
		}

		private void InitializeMapFromXml(string mapXmlData)
		{
			int x = 0;
			int y = 0;
			foreach (var letter in mapXmlData)
				if (letter > ' ')
				{
					SetTile(new Vector2D(x, y), LetterToTileType(letter));
					x++;
					if (letter == '\n')
					{
						x = 0;
						y++;
					}
				}
		}

		public class InvalidTileMapData : Exception {}

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

		public Rectangle CalculateGridScreenDrawArea(Vector2D position)
		{
			return new Rectangle(gridOffset + position * gridScaling, gridScaling);
		}

		public Vector2D GetGridPosition(Vector2D screenPosition)
		{
			var gridPosition = (screenPosition - gridOffset) / gridScaling;
			return new Vector2D((int)gridPosition.X, (int)gridPosition.Y);
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

		private Size mapSize;
		public LevelTileType[,] MapData { get; private set; }
		public Action<Size> SizeChanged { get; set; }
		public Action<Vector2D> TileChanged { get; set; }

		public void SetTile(Vector2D screenPosition, LevelTileType selectedTileType)
		{
			var gridPosition = GetGridPosition(screenPosition);
			if (gridPosition.X >= 0 && gridPosition.Y >= 0 && gridPosition.X < mapSize.Width &&
			    gridPosition.Y < mapSize.Height)
			{
				MapData[(int)gridPosition.X, (int)gridPosition.Y] = selectedTileType;
				if (TileChanged != null)
					TileChanged(gridPosition);
			}
		}

		public string ToTextForXml()
		{
			string result = Environment.NewLine;
			for (int y = 0; y < Size.Height; y++)
			{
				for (int x = 0; x < Size.Width; x++)
					result += LetterForTileType(MapData[x, y]);
				result += Environment.NewLine;
			}
			return result;
		}

		private static char LetterForTileType(LevelTileType tileType)
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
	}
}