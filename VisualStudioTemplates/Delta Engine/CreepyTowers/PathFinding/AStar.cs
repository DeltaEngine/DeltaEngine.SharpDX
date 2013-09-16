using System;

namespace $safeprojectname$.PathFinding
{
	public class AStar : GraphSearch
	{
		private const float HeuristicWeight = 500.0f;

		public override bool Search(Graph graphToSearch, int start, int end)
		{
			graph = graphToSearch;
			startNode = start;
			targetNode = end;
			Initialize();
			while (nodesToCheck.Count > 0)
			{
				int currentNode = GetNextNode();
				visitedNodes++;
				if (currentNode == InvalidNodeIndex)
					return false;

				if (currentNode == targetNode)
					return true;

				CheckAdjacencies(currentNode);
			}
			return false;
		}

		private void CheckAdjacencies(int currentNode)
		{
			for (int connection = 0; connection < graph.AdjacencyList[currentNode].Count; connection++)
			{
				GraphConnection currentNodeConnection = graph.AdjacencyList [currentNode] [connection];
				if (!currentNodeConnection.isActive)
					continue;

				int connectedNode = currentNodeConnection.destinyNode;
				int costToThisNode = costSoFar [currentNode] + currentNodeConnection.weight + 
					EstimateDistance(connectedNode, targetNode);
				if (costSoFar [connectedNode] > costToThisNode)
				{
					costSoFar [connectedNode] = costToThisNode;
					previousNode [connectedNode] = currentNode;
				}
			}
		}

		private int EstimateDistance(int nodeFrom, int nodeTo)
		{
			return (int)((Math.Abs(graph.Nodes [nodeTo].X - graph.Nodes [nodeFrom].X) + 
				Math.Abs(graph.Nodes [nodeTo].Y - graph.Nodes [nodeFrom].Y)) * HeuristicWeight);
		}
	}
}