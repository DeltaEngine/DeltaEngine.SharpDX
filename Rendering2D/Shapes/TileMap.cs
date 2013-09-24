using System;
using System.IO;
using DeltaEngine.Content.Xml;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering2D.Shapes
{
	/// <summary>
	/// Simple 2D map of TileType data to be used for 2D tile games or for game logic tests.
	/// </summary>
	public class TileMap : XmlContent
	{
		public TileMap(Size size)
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
			MapData = new TileType[(int)mapSize.Width, (int)mapSize.Height];
		}

		private Vector2D gridOffset;
		private Size gridScaling;

		public Vector2D CalculateGridScreenPosition(Vector2D position)
		{
			return gridOffset + position * gridScaling;
		}

		protected TileMap(string contentName)
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

		private static TileType LetterToTileType(char letter)
		{
			switch (letter)
			{
				case 'X':
					return TileType.Blocked;
				case 'P':
					return TileType.Placeable;
				case 'L':
					return TileType.BlockedPlaceable;
				case 'R':
					return TileType.Red;
				case 'G':
					return TileType.Green;
				case 'B':
					return TileType.Blue;
				case 'Y':
					return TileType.Yellow;
				case 'O':
					return TileType.Brown;
				case 'A':
					return TileType.Gray;
				case 'S':
					return TileType.SpawnPoint;
				case 'E':
					return TileType.ExitPoint;
				default:
					return TileType.Nothing;
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
		public TileType[,] MapData { get; private set; }
		public Action<Size> SizeChanged { get; set; }
		public Action<Vector2D> TileChanged { get; set; }

		public void SetTile(Vector2D screenPosition, TileType selectedTileType)
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

		private static char LetterForTileType(TileType tileType)
		{
			switch (tileType)
			{
			case TileType.Nothing:
				return '.';
			case TileType.Blocked:
				return 'X';
			case TileType.Placeable:
				return 'P';
			case TileType.BlockedPlaceable:
				return 'L';
			case TileType.Red:
				return 'R';
			case TileType.Green:
				return 'G';
			case TileType.Blue:
				return 'B';
			case TileType.Yellow:
				return 'Y';
			case TileType.Brown:
				return 'O';
			case TileType.Gray:
				return 'A';
			case TileType.SpawnPoint:
				return 'S';
			case TileType.ExitPoint:
				return 'E';
			default:
				throw new ArgumentOutOfRangeException("tileType");
			}
		}

		public Color GetColor(TileType tileType)
		{
			switch (tileType)
			{
			case TileType.Blocked:
				return Color.LightGray;
			case TileType.Placeable:
				return Color.CornflowerBlue;
			case TileType.BlockedPlaceable:
				return Color.LightBlue;
			case TileType.Red:
				return Color.Red;
			case TileType.Green:
				return Color.Green;
			case TileType.Blue:
				return Color.Blue;
			case TileType.Yellow:
				return Color.Yellow;
			case TileType.Brown:
				return Color.Brown;
			case TileType.Gray:
				return Color.Gray;
			case TileType.SpawnPoint:
				return Color.PaleGreen;
			case TileType.ExitPoint:
				return Color.DarkGreen;
			default:
				return Color.Black;
			}
		}
	}
}