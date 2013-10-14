using System;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using NUnit.Framework;

namespace DeltaEngine.GameLogic.Tests
{
	public class LevelTests
	{
		[Test]
		public void Start()
		{
			TileMap map = CreateSimple2X3TileMap();
			Assert.AreEqual(2, map.Width);
			Assert.AreEqual(3, map.Height);
		}

		private static TileMap CreateSimple2X3TileMap()
		{
			var tilesData = new[,]
			{
				{ new CompleteSquare(LevelTileType.ExitPoint), new CompleteSquare(LevelTileType.Nothing), },
				{ new CompleteSquare(LevelTileType.Blocked), new CompleteSquare(LevelTileType.Nothing), },
				{ new CompleteSquare(LevelTileType.SpawnPoint), new CompleteSquare(LevelTileType.Nothing), }
			};
			return new TileMap(tilesData);
		}

		private class TileMap
		{
			public TileMap(CompleteSquare[,] mapData)
			{
				this.mapData = mapData;
			}

			private readonly CompleteSquare[,] mapData;

			public int Width
			{
				get { return mapData.GetLength(1); }
			}

			public int Height
			{
				get { return mapData.GetLength(0); }
			}

			public TileMap(string mapDataAsString)
			{
				if (String.IsNullOrWhiteSpace(mapDataAsString))
					throw new NoTileMapData();
				string[] tileLines = mapDataAsString.SplitAndTrim(Environment.NewLine);
				mapData = new CompleteSquare[tileLines.Length,tileLines[0].Length];
				for (int lineIndex = 0; lineIndex < Height; lineIndex++)
					for (int symbolIndex = 0; symbolIndex < Width; symbolIndex++)
						mapData[lineIndex, symbolIndex] = new CompleteSquare(tileLines[lineIndex][symbolIndex]);
			}

			public class NoTileMapData : Exception {}

			public string Save()
			{
				string data = "";
				for (int i = 0; i < Height; i++)
					SaveTileLine(i, ref data);
				return data;
			}

			private void SaveTileLine(int mapLineIndex, ref string data)
			{
				for (int i = 0; i < Width; i++)
					data += mapData[mapLineIndex, i].ToTypeSymbol();
				data += Environment.NewLine;
			}

			public void FindPath(LevelTileType typeOfStartTile, LevelTileType typeOfTargetTile)
			{
				Vector2D startPoint = FindTile(typeOfStartTile);
				if (startPoint.Equals(Vector2D.Unused))
					throw new StartPointNotFound();
				int startX = (int)startPoint.X;
				int startY = (int)startPoint.Y;
				mapData[startX, startY].DistanceCosts = 0;
				var tileCoordinates = new Vector2D[Width * Height];
				for (int lineIndex = 0; lineIndex < Height; lineIndex++)
					for (int symbolIndex = 0; symbolIndex < Width; symbolIndex++)
						tileCoordinates[symbolIndex + lineIndex * Width] = new Vector2D(symbolIndex, lineIndex);
			}

			public class StartPointNotFound : Exception {}

			private Vector2D FindTile(LevelTileType typeOfTile)
			{
				for (int lineIndex = 0; lineIndex < Height; lineIndex++)
					for (int symbolIndex = 0; symbolIndex < Width; symbolIndex++)
						if (mapData[lineIndex, symbolIndex].Type == typeOfTile)
							return new Vector2D(lineIndex, symbolIndex);
				return Vector2D.Unused;
			}
		}

		[Test]
		public void SaveAndLoadMapData()
		{
			TileMap map = CreateSimple2X3TileMap();
			string mapData = map.Save();
			string newLine = Environment.NewLine;
			string expectedData = "E." + newLine + "X." + newLine + "S." + newLine;
			Assert.AreEqual(expectedData, mapData);
			var loadedMap = new TileMap(mapData);
			Assert.AreEqual(expectedData, loadedMap.Save());
		}

		[Test]
		public void CanNotCreateTileMapWithoutData()
		{
			const string MapData = "";
			Assert.Throws<TileMap.NoTileMapData>(() => new TileMap(MapData));
		}

		[Test]
		public void CanNotCreateTileMapWithoutValidData()
		{
			const string MapData = "   ";
			Assert.Throws<TileMap.NoTileMapData>(() => new TileMap(MapData));
		}

		[Test]
		public void Start2()
		{
			TileMap map = CreateSimple2X3TileMap();
			Assert.Throws<TileMap.StartPointNotFound>(
				() => map.FindPath(LevelTileType.Red, LevelTileType.ExitPoint));
			Assert.DoesNotThrow(() => map.FindPath(LevelTileType.SpawnPoint, LevelTileType.ExitPoint));
		}

		private struct CompleteSquare
		{
			public CompleteSquare(char typeSymbol, int distanceCosts = 100, bool isPath = false)
				: this(ToTileType(typeSymbol), distanceCosts, isPath) { }

			private static LevelTileType ToTileType(char letter)
			{
				switch (letter)
				{
					case 'X':
						return LevelTileType.Blocked;
					case 'S':
						return LevelTileType.SpawnPoint;
					case 'E':
						return LevelTileType.ExitPoint;
					default:
						return LevelTileType.Nothing;
				}
			}

			public CompleteSquare(LevelTileType type, int distanceCosts = 100, bool isPath = false)
				: this()
			{
				Type = type;
				DistanceCosts = distanceCosts;
				IsPartOfBestPath = isPath;
			}

			public LevelTileType Type { get; private set; }
			public int DistanceCosts { get; set; }
			public bool IsPartOfBestPath { get; set; }

			public char ToTypeSymbol()
			{
				switch (Type)
				{
				case LevelTileType.Nothing:
					return '.';
				case LevelTileType.Blocked:
					return 'X';
				case LevelTileType.SpawnPoint:
					return 'S';
				case LevelTileType.ExitPoint:
					return 'E';
				default:
					throw new UnsupportedTileType(Type);
				}
			}

			private class UnsupportedTileType : Exception
			{
				public UnsupportedTileType(LevelTileType tileType)
					: base(tileType.ToString()) {}
			}
		}
	}
}
