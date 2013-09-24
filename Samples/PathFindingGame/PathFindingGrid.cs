using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Rendering3D.Models;
using DeltaEngine.Rendering3D.Shapes3D;

namespace PathfindingGame
{
	public class PathFindingGrid
	{
		public PathFindingGrid(int rowNumbers, int columnNumbers, float gridSize = 0.5f)
		{
			gridRowSize = rowNumbers;
			gridColumnSize = columnNumbers;
			halfGridRowSize = gridRowSize / 2;
			halfGridColumnSize = gridColumnSize / 2;
			gridScale = gridSize;
			graph = new PathFindingGraph(gridColumnSize, gridRowSize);
			lines = new List<Line3D>();
			CreateGridLines();
			PaintGridCenter();
			graph.MakeConnections();
		}

		private readonly int gridRowSize;
		private readonly int gridColumnSize;
		private static float gridScale = 0.5f;
		private readonly int halfGridRowSize;
		private readonly int halfGridColumnSize;
		private readonly PathFindingGraph graph;
		private List<Line3D> lines;

		private void CreateGridLines()
		{
			for (int i = -halfGridRowSize; i <= halfGridRowSize; i++)
				for (int j = -halfGridColumnSize; j <= halfGridColumnSize; j++)
				{
					new Line3D(new Vector3D(-halfGridColumnSize * gridScale, i * gridScale, 0.0f),
						new Vector3D(halfGridColumnSize * gridScale, i * gridScale, 0.0f), Color.White);
					new Line3D(new Vector3D(j * gridScale, -halfGridRowSize * gridScale, 0.0f),
						new Vector3D(j * gridScale, halfGridRowSize * gridScale, 0.0f), Color.White);
				}
		}

		private void PaintGridCenter()
		{
			for (int i = 0; i < gridColumnSize; i++)
				for (int j = 0; j < gridRowSize; j++)
				{
					var box = new Box(new Vector3D(0.04f, 0.04f, 0.04f), Color.Red);
					var y = Vector3D.UnitY.Y * gridScale * -halfGridRowSize + gridScale / 2.0f + j * gridScale;
					var x = Vector3D.UnitX.X * gridScale * -halfGridColumnSize + gridScale / 2.0f + i * gridScale;
					graph.SetNodePosition(j, i, new Vector3D(x, y, 0));
					new Model(new ModelData(box), new Vector3D(x, y, 0));
				}
		}

		public void GetPathAndPaint(Vector3D start, Vector3D end)
		{
			var indexStart = graph.GetClosestNode(start);
			new Line3D(start, graph.GetPositionOfNode(indexStart), Color.Green);
			var indexEnd = graph.GetClosestNode(end);
			new Line3D(end, graph.GetPositionOfNode(indexEnd), Color.Green);
			var aStar = new AStar();
			if (aStar.Search(graph, indexStart, indexEnd))
			{
				var pathIndex = aStar.GetPath();
				for (int i = 0; i < pathIndex.Length - 1; i++)
					lines.Add(new Line3D(graph.GetPositionOfNode(pathIndex[i]), 
						graph.GetPositionOfNode(pathIndex[i + 1]), Color.Green));
			}
		}

		public void SetUnreachableNode(Vector3D position)
		{
			var index = graph.GetClosestNode(position);
			graph.SetUnreachableAndUpdate(index);
			var box = new Box(new Vector3D(0.2f, 0.2f, 0.2f), Color.Blue);
			new Model(new ModelData(box), graph.GetPositionOfNode(index));
		}

		public void AddCubeInTheGrid(Ray ray)
		{
			var floor = new Plane(Vector3D.UnitZ, 0.0f);
			var position = floor.Intersect(ray);
			var index = graph.GetClosestNode((Vector3D)position);
			if (graph.IsUnreachableNode(index))
				return;
			var box = new Box(new Vector3D(0.2f, 0.2f, 0.2f), Color.Blue);
			new Model(new ModelData(box), graph.GetPositionOfNode(index));
			graph.SetUnreachableAndUpdate(index);
			foreach (var line in lines)
				line.IsActive = false;
			lines.Clear();
		}
	}
}
