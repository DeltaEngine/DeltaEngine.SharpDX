using System;
using System.Linq;
using DeltaEngine.Datatypes;

namespace $safeprojectname$.PathFinding
{
	public class PathFindingGraph : Graph
	{
		public PathFindingGraph(int nodesColumns, int nodesRows) : base(nodesColumns * nodesRows)
		{
			this.nodesColumns = nodesColumns;
			this.nodesRows = nodesRows;
			NumberOfNodes = nodesColumns * nodesRows;
			unreachableNodes = new bool[NumberOfNodes];
		}

		protected int nodesColumns;
		protected int nodesRows;
		protected bool[] unreachableNodes;

		public void SetUnreachableNode(int nodeIndex)
		{
			unreachableNodes [nodeIndex] = true;
		}

		public bool IsUnreachableNode(int nodeIndex)
		{
			return unreachableNodes [nodeIndex];
		}

		public void SetNodePosition(int row, int column, Vector2D position)
		{
			int nodeIndex = row * nodesColumns + column;
			if (nodeIndex < 0 || nodeIndex >= NumberOfNodes)
				throw new ArgumentOutOfRangeException("nodeIndex is not a valid value", new Exception());

			Nodes [nodeIndex].X = position.X;
			Nodes [nodeIndex].Y = position.Y;
		}

		public void MakeConnections()
		{
			for (int row = 0; row < nodesRows; row++)
				for (int column = 0; column < nodesColumns; column++)
					if (!unreachableNodes [row * nodesColumns + column])
						ConnectNodes(row, column);
		}

		private void ConnectNodes(int row, int column)
		{
			int nodeIndex = row * nodesColumns + column;
			if (row != 0)
				Connect(nodeIndex, nodeIndex - nodesColumns, 10);

			if (row != 0 && column != nodesColumns - 1)
				Connect(nodeIndex, nodeIndex - nodesColumns + 1, 14);

			if (column != nodesColumns - 1)
				Connect(nodeIndex, nodeIndex + 1, 10);

			if (row != nodesRows - 1 && column != nodesColumns - 1)
				Connect(nodeIndex, nodeIndex + nodesColumns + 1, 14);

			if (row != nodesRows - 1)
				Connect(nodeIndex, nodeIndex + nodesColumns, 10);

			if (row != nodesRows - 1 && column != 0)
				Connect(nodeIndex, nodeIndex + nodesColumns - 1, 14);

			if (column != 0)
				Connect(nodeIndex, nodeIndex - 1, 10);

			if (row != 0 && column != 0)
				Connect(nodeIndex, nodeIndex - nodesColumns - 1, 14);
		}

		public override void Connect(int nodeA, int nodeB, int weight, bool bidirectional = true)
		{
			if (!unreachableNodes [nodeB])
				base.Connect(nodeA, nodeB, weight, bidirectional);
		}

		public void SetUnreachableAndUpdate(int index)
		{
			foreach (var adjacency in AdjacencyList[index])
				AdjacencyList [adjacency.destinyNode].First(x => x.destinyNode == index).isActive = false;

			SetUnreachableNode(index);
		}

		public void SetReachableAndUpdate(int index)
		{
			foreach (var adjacency in AdjacencyList[index])
				AdjacencyList [adjacency.destinyNode].First(x => x.destinyNode == index).isActive = true;

			unreachableNodes [index] = false;
		}

		public int GetClosestNode(Vector2D position)
		{
			var minimumDistance = (Nodes [0] - position).LengthSquared;
			int index = 0;
			for (int i = 1; i < Nodes.Length; i++)
			{
				var distanceSquared = (Nodes [i] - position).LengthSquared;
				if (distanceSquared < minimumDistance)
				{
					minimumDistance = distanceSquared;
					index = i;
				}
			}
			return index;
		}

		public Vector2D GetPositionOfNode(int index)
		{
			return Nodes [index];
		}
	}
}