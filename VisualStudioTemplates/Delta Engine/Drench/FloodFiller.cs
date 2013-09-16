using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Datatypes;

namespace $safeprojectname$
{
	internal class FloodFiller
	{
		public FloodFiller(Color[,] colors)
		{
			this.colors = colors;
			width = colors.GetLength(0);
			height = colors.GetLength(1);
		}

		private readonly Color[,] colors;
		private readonly int width;
		private readonly int height;

		public void SetColor(int x, int y, Color color)
		{
			oldColor = colors [x, y];
			newColor = color;
			processedPoints.Clear();
			if (oldColor == newColor)
				return;

			unprocessedPoints.Clear();
			unprocessedPoints.Add(new Point(x, y));
			while (unprocessedPoints.Any())
				ProcessNextUnprocessedPoint();
		}

		private Color oldColor;
		private Color newColor;
		private readonly HashSet<Point> processedPoints = new HashSet<Point>();
		private readonly HashSet<Point> unprocessedPoints = new HashSet<Point>();

		private void ProcessNextUnprocessedPoint()
		{
			var point = unprocessedPoints.First();
			unprocessedPoints.Remove(point);
			processedPoints.Add(point);
			colors [(int)point.X, (int)point.Y] = newColor;
			foreach (var direction in Directions)
				ProcessNeighbour(point, direction);
		}

		private static readonly Point[] Directions = new[] {
			-Point.UnitX,
			Point.UnitX,
			-Point.UnitY,
			Point.UnitY
		};

		private void ProcessNeighbour(Point point, Point direction)
		{
			var x = (int)point.X + (int)direction.X;
			var y = (int)point.Y + (int)direction.Y;
			if (x >= 0 && x < width && y >= 0 && y < height)
				ProcessValidNeighbour(new Point(x, y), colors [x, y]);
		}

		private void ProcessValidNeighbour(Point point, Color color)
		{
			if (!processedPoints.Contains(point) && color == oldColor)
				unprocessedPoints.Add(point);
		}

		public int ProcessedCount
		{
			get
			{
				return processedPoints.Count;
			}
		}
	}
}