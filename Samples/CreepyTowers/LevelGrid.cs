using System;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;
using DeltaEngine.Rendering.Shapes3D;

namespace CreepyTowers
{
	public class LevelGrid : Entity
	{
		public LevelGrid(int gridSize, float gridScale)
		{
			GridSize = gridSize;
			GridScale = gridScale;
			HalfGridSize = GridSize * 0.5f;
			CreateGrid();
			DrawGrid();
			AssignGridPositions();
		}

		public int GridSize { get; private set; }
		public float GridScale { get; private set; }
		public static float HalfGridSize { get; private set; }

		private void CreateGrid()
		{
			PropertyMatrix = new GridProperties[GridSize,GridSize];
		}

		public GridProperties[,] PropertyMatrix { get; private set; }

		private void DrawGrid()
		{
			axisXy = new Point(-HalfGridSize, -HalfGridSize);
			for (int i = 0; i <= GridSize; i++, axisXy.X += 1, axisXy.Y += 1)
			{
				xLineStart = new Vector(-HalfGridSize * GridScale + XOffset, axisXy.Y * GridScale - YOffset,
					0.0f);
				xLineEnd = new Vector(HalfGridSize * GridScale + XOffset, axisXy.Y * GridScale - YOffset,
					0.0f);
				new Line3D(xLineStart, xLineEnd, Color.White);

				yLineStart = new Vector(axisXy.X * GridScale + XOffset, -HalfGridSize * GridScale - YOffset,
					0.0f);
				yLineEnd = new Vector(axisXy.X * GridScale + XOffset, HalfGridSize * GridScale - YOffset,
					0.0f);
				new Line3D(yLineStart, yLineEnd, Color.White);
			}
		}

		private Point axisXy;
		private Vector xLineStart;
		private Vector xLineEnd;
		private Vector yLineStart;
		private Vector yLineEnd;
		private const float XOffset = 0.01f;
		private const float YOffset = 0.01f;

		private void AssignGridPositions()
		{
			ComputePrerequisites();
			for (int x = 0; x < GridSize; x++)
			{
				int y = 0;
				for (; y < GridSize; y++)
				{
					CreateDrawAreaCoordinates(x, y);
					topLeft = new Vector(topLeft.X, topLeft.Y - GridScale, topLeft.Z);
				}
				y = 0;
				topLeft = new Vector(PropertyMatrix[x, y].TopLeft.X - GridScale,
					PropertyMatrix[x, y].TopLeft.Y, PropertyMatrix[x, y].TopLeft.Z);
			}
		}

		private void ComputePrerequisites()
		{
			axisXy = new Point(-HalfGridSize, -HalfGridSize);
			var startPoint = new Vector(-HalfGridSize * GridScale + XOffset, axisXy.Y * GridScale, 0.0f);
			var offsetFactor = GridSize * GridScale;
			topLeft = new Vector(startPoint.X + offsetFactor, startPoint.Y + offsetFactor, startPoint.Z);
		}

		private void CreateDrawAreaCoordinates(int x, int y)
		{
			PropertyMatrix[x, y].TopLeft = topLeft;
			PropertyMatrix[x, y].TopRight = new Vector(topLeft.X, topLeft.Y - GridScale, topLeft.Z);
			PropertyMatrix[x, y].BottomLeft = new Vector(topLeft.X - GridScale, topLeft.Y, topLeft.Z);
			PropertyMatrix[x, y].BottomRight = new Vector(topLeft.X - GridScale, topLeft.Y - GridScale,
				topLeft.Z);
			PropertyMatrix[x, y].MidPoint = new Vector(topLeft.X - GridScale * 0.5f,
				topLeft.Y - GridScale * 0.5f, topLeft.Z);
		}

		private Vector topLeft;

		public Vector ComputeGridCoordinates(LevelGrid grid, Vector position,
			ChangeableList<Tuple<int, int>> interactablePointsList)
		{
			IsClickInGrid = false;

			foreach (var tuple in interactablePointsList)
			{
				var gridBlock = grid.PropertyMatrix[tuple.Item1, tuple.Item2];
				var topLeftPoint = new Point(gridBlock.TopLeft.X, gridBlock.TopLeft.Y);
				var topRight = new Point(gridBlock.TopRight.X, gridBlock.TopRight.Y);
				var bottomRight = new Point(gridBlock.BottomRight.X, gridBlock.BottomRight.Y);
				var clickedPoint = new Point(position.X, position.Y);

				if (clickedPoint.X >= bottomRight.X && clickedPoint.X < topRight.X &&
					clickedPoint.Y >= topRight.Y && clickedPoint.Y < topLeftPoint.Y)
				{
					midPoint = gridBlock.MidPoint;
					IsClickInGrid = true;
					break;
				}
			}

			//for (int x = 0; x < grid.GridSize; x++)
			//	for (int z = 0; z < grid.GridSize; z++)
			//	{
			//		var gridBlock = grid.PropertyMatrix[x, z];
			//		var topLeftPoint = new Point(gridBlock.TopLeft.X, gridBlock.TopLeft.Z);
			//		var topRight = new Point(gridBlock.TopRight.X, gridBlock.TopRight.Z);
			//		var bottomRight = new Point(gridBlock.BottomRight.X, gridBlock.BottomRight.Z);
			//		var clickedPoint = new Point(position.X, position.Z);

			//		if (clickedPoint.X >= bottomRight.X && clickedPoint.X < topRight.X &&
			//			clickedPoint.Y >= topRight.Y && clickedPoint.Y < topLeftPoint.Y)
			//		{
			//			midPoint = gridBlock.MidPoint;
			//			IsClickInGrid = true;
			//			break;
			//		}
			//	}

			return midPoint;

			//var startPoint = grid.PropertyMatrix[0, 0].MidPoint;
			//var dist = position - startPoint;
			//var gridPosX = (int)(Math.Abs(dist.X) / grid.GridScale);
			//var gridPosZ = (int)(Math.Abs(dist.Z) / grid.GridScale);
			//var neighbouringGrids = new List<GridProperties>
			//{
			//	grid.PropertyMatrix[gridPosX - 1, gridPosZ - 1],
			//	grid.PropertyMatrix[gridPosX, gridPosZ - 1],
			//	grid.PropertyMatrix[gridPosX + 1, gridPosZ - 1],
			//	grid.PropertyMatrix[gridPosX - 1, gridPosZ],
			//	grid.PropertyMatrix[gridPosX, gridPosZ],
			//	grid.PropertyMatrix[gridPosX + 1, gridPosZ],
			//	grid.PropertyMatrix[gridPosX - 1, gridPosZ + 1],
			//	grid.PropertyMatrix[gridPosX, gridPosZ + 1],
			//	grid.PropertyMatrix[gridPosX + 1, gridPosZ + 1]
			//};

			//foreach (GridProperties gridBlock in neighbouringGrids)
			//{
			//	var point = new Point(gridBlock.TopLeft.X, gridBlock.TopLeft.Z);
			//	var drawArea = new Rectangle(point, new Size(grid.GridScale));

			//	var clickedPoint = new Point(position.X, position.Z);
			//	if (drawArea.Contains(clickedPoint))
			//		return gridBlock.MidPoint;
			//}
			//return Vector.Zero;
		}

		private Vector midPoint;
		public bool IsClickInGrid { get; private set; }
	}
}