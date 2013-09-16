using System;
using System.Collections.Generic;

namespace PathfindingGame
{
	public abstract class GraphSearch
	{
		protected Graph graph;
		protected int startNode;
		protected int targetNode;

		protected int[] costSoFar;
		protected int[] previousNode;
		protected List<int> nodesToCheck = new List<int>();

		protected const int Infinity = Int32.MaxValue;
		protected const int InvalidNodeIndex = -1;

		protected int visitedNodes;

		public abstract bool Search(Graph graph, int startNode, int targetNode);

		public int[] GetPath()
		{
			Stack<int> shortestPath = new Stack<int>();
			int currentNode = targetNode;

			while (currentNode != startNode)
			{
				shortestPath.Push(currentNode);
				currentNode = previousNode[currentNode];
			}

			shortestPath.Push(currentNode);

			return shortestPath.ToArray();
		}

		protected void Initialize()
		{
			costSoFar = new int[graph.NumberOfNodes];
			previousNode = new int[graph.NumberOfNodes];
			nodesToCheck.Clear();
			visitedNodes = 0;

			for (int i = 0; i < graph.NumberOfNodes; i++)
			{
				costSoFar[i] = Infinity;
				previousNode[i] = InvalidNodeIndex;
				nodesToCheck.Add(i);
			}

			costSoFar[startNode] = 0;
		}

		protected int GetNextNode()
		{
			int minCost = Infinity;
			int nextNode = InvalidNodeIndex;
			int currentNode;
			int cost;

			for (int i = 0; i < nodesToCheck.Count; i++)
			{
				currentNode = nodesToCheck[i];
				cost = costSoFar[currentNode];

				if (cost < minCost)
				{
					minCost = costSoFar[currentNode];
					nextNode = currentNode;
				}
			}

			nodesToCheck.Remove(nextNode);

			return nextNode;
		}

		public int GetNumberOfVisitedNodes()
		{
			return visitedNodes;
		}
	}
}
