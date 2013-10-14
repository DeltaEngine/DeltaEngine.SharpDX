using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.GameLogic;
using DeltaEngine.Rendering2D.Shapes;

namespace DeltaEngine.Rendering3D.Shapes3D
{
	/// <summary>
	/// Simple 2D map of TileType data to be used for 2D tile games or for game logic tests.
	/// </summary>
	public class LevelRenderer
	{
		public LevelRenderer(Level map)
		{
			this.map = map;
			rectangles = new FilledRect[(int)map.Size.Width,(int)map.Size.Height];
			boxes = new Model[(int)map.Size.Width,(int)map.Size.Height];
			RecreateTiles(map.Size);
			map.SizeChanged += RecreateTiles;
			map.TileChanged += position => UpdateTile((int)position.X, (int)position.Y);
		}

		private readonly Level map;
		private FilledRect[,] rectangles;
		private Model[,] boxes;
		public readonly List<Line2D> lines2D = new List<Line2D>();
		public readonly List<Line3D> lines3D = new List<Line3D>();

		private void CreateForegroundGrid()
		{
			if (!map.RenderIn3D)
			{
				for (int x = 0; x < map.Size.Width + 1; x++)
					lines2D.Add(new Line2D(map.CalculateGridScreenPosition(new Vector2D(x, 0)),
						map.CalculateGridScreenPosition(new Vector2D(x, (int)map.Size.Height)), Color.Gray)
					{
						RenderLayer = 10
					});
				for (int y = 0; y < map.Size.Height + 1; y++)
					lines2D.Add(new Line2D(map.CalculateGridScreenPosition(new Vector2D(0, y)),
						map.CalculateGridScreenPosition(new Vector2D((int)map.Size.Width, y)), Color.Gray)
					{
						RenderLayer = 10
					});
			}
			else
			{
				for (int x = 0; x < map.Size.Width + 1; x++)
					lines3D.Add(new Line3D(map.CalculateGridScreenPosition(new Vector2D(x, 0)),
						map.CalculateGridScreenPosition(new Vector2D(x, (int)map.Size.Height)), Color.Gray)
					{
						RenderLayer = 10
					});
				for (int y = 0; y < map.Size.Height + 1; y++)
					lines3D.Add(new Line3D(map.CalculateGridScreenPosition(new Vector2D(0, y)),
						map.CalculateGridScreenPosition(new Vector2D((int)map.Size.Width, y)), Color.Gray)
					{
						RenderLayer = 10
					});
			}
		}

		private void RecreateTiles(Size newMapSize)
		{
			Dispose();
			CreateForegroundGrid();
			rectangles = new FilledRect[(int)map.Size.Width,(int)map.Size.Height];
			boxes = new Model[(int)map.Size.Width,(int)map.Size.Height];
			for (int y = 0; y < map.Size.Height; y++)
				for (int x = 0; x < map.Size.Width; x++)
					UpdateTile(x, y);
		}

		private void UpdateTile(int x, int y)
		{
			var position = new Vector2D(x, y);
			var tileType = map.MapData[x, y];
			var color = map.GetColor(tileType);
			if (!map.RenderIn3D)
			{
				if (rectangles[x, y] == null)
				{
					var rect = new FilledRect(map.CalculateGridScreenDrawArea(position), color);
					rectangles[x, y] = rect;
				}
				else if (rectangles[x, y].Color != color)
				{
					rectangles[x, y].IsActive = false;
					var rect = new FilledRect(map.CalculateGridScreenDrawArea(position), color);
					rectangles[x, y] = rect;
				}
			}
			else if (boxes[x, y] == null)
			{
				if (tileType != LevelTileType.Nothing)
				{
					var box = new Box(new Vector3D(BoxSize / map.Size.Width, BoxSize / map.Size.Width, BoxSize / map.Size.Width), color);
					boxes[x, y] = new Model(new ModelData(box),
						new Vector3D(
							map.CalculateGridScreenDrawArea(position).Left + (map.gridScaling.Width / 2),
							map.CalculateGridScreenDrawArea(position).Top + (map.gridScaling.Height / 2), 0));
					boxes[x, y].Set(color);
				}
			}
			else if (boxes[x, y].Get<Color>() != color)
			{
				boxes[x, y].IsActive = false;
				var box =
					new Box(
						new Vector3D(BoxSize / map.Size.Width, BoxSize / map.Size.Width, BoxSize / map.Size.Width),
						color);
				boxes[x, y] = new Model(new ModelData(box),
					new Vector3D(
						map.CalculateGridScreenDrawArea(position).Left + (map.gridScaling.Width / 2),
						map.CalculateGridScreenDrawArea(position).Top + (map.gridScaling.Height / 2), 0));
				boxes[x, y].Set(color);
			}
		}

		private const float BoxSize = 24.0f;

		public void Dispose()
		{
			foreach (var entity in EntitiesRunner.Current.GetEntitiesOfType<FilledRect>())
				entity.IsActive = false;
			foreach (var entity in lines2D)
				entity.IsActive = false;
			lines2D.Clear();
			foreach (var entity in lines3D)
				entity.IsActive = false;
			lines3D.Clear();
			for (int y = 0; y < rectangles.GetLength(1); y++)
				for (int x = 0; x < rectangles.GetLength(0); x++)
					if (rectangles[x, y] != null)
						rectangles[x, y].IsActive = false;
			for (int y = 0; y < boxes.GetLength(1); y++)
				for (int x = 0; x < boxes.GetLength(0); x++)
					if (boxes[x, y] != null)
						boxes[x, y].IsActive = false;
		}

		public void UpdateLevel()
		{
			RecreateTiles(map.Size);
		}
	}
}