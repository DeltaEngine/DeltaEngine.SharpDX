using System.Collections.Generic;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering2D.Shapes.Levels
{
	/// <summary>
	/// Simple 2D map of TileType data to be used for 2D tile games or for game logic tests.
	/// </summary>
	public class LevelRenderer
	{
		public LevelRenderer(Level map)
		{
			this.map = map;
			CreateForegroundGrid();
			rectangles = new FilledRect[(int)map.Size.Width, (int)map.Size.Height];
			map.SizeChanged += RecreateTiles;
			map.TileChanged += position => UpdateTile((int)position.X, (int)position.Y);
		}

		private readonly Level map;
		private FilledRect[,] rectangles;
		private readonly List<Line2D> lines = new List<Line2D>();

		private void CreateForegroundGrid()
		{
			for (int x = 0; x < map.Size.Height + 1; x++)
				lines.Add(new Line2D(map.CalculateGridScreenPosition(new Vector2D(x, 0)),
					map.CalculateGridScreenPosition(new Vector2D(x, (int)map.Size.Height)), Color.Gray)
				{
					RenderLayer = 10
				});
			for (int y = 0; y < map.Size.Width + 1; y++)
				lines.Add(new Line2D(map.CalculateGridScreenPosition(new Vector2D(0, y)),
					map.CalculateGridScreenPosition(new Vector2D((int)map.Size.Width, y)), Color.Gray)
				{
					RenderLayer = 10
				});
		}

		private void RecreateTiles(Size newMapSize)
		{
			Dispose();
			CreateForegroundGrid();
			rectangles = new FilledRect[(int)map.Size.Width, (int)map.Size.Height];
			for (int y=0; y<map.Size.Height; y++)
				for (int x=0; x<map.Size.Width;x++)
					UpdateTile(x, y);
		}

		private void UpdateTile(int x, int y)
		{
			var position = new Vector2D(x, y);
			var tileType = map.MapData[x, y];
			var color = map.GetColor(tileType);
			if (rectangles[x, y] == null)
			{
				if (tileType != LevelTileType.Nothing)
					rectangles[x, y] = new FilledRect(map.CalculateGridScreenDrawArea(position), color);
			}
			else
			{
				if (rectangles[x, y].Color != color)
				{
					rectangles[x, y].IsActive = false;
					rectangles[x, y] = new FilledRect(map.CalculateGridScreenDrawArea(position), color);
				}
			}
		}

		public void Dispose()
		{
			foreach (var line in lines)
				line.IsActive = false;
			lines.Clear();
			for (int y = 0; y < rectangles.GetLength(1); y++)
				for (int x = 0; x < rectangles.GetLength(0); x++)
					if (rectangles[x, y] != null)
						rectangles[x, y].IsActive = false;
		}
	}
}