using System.Collections.Generic;
using DeltaEngine.Datatypes;

namespace $safeprojectname$.PathFinding
{
	public class GraphConnection
	{
		public int destinyNode;
		public int weight;
		public bool isActive;

		public GraphConnection(int destinyNode, int weight)
		{
			this.destinyNode = destinyNode;
			this.weight = weight;
			isActive = true;
		}

		public void SetActive()
		{
			isActive = true;
		}

		public void SetInactive()
		{
			isActive = false;
		}
	}

	public class Graph
	{
		public Vector2D[] Nodes
		{
			get;
			set;
		}

		public List<GraphConnection>[] AdjacencyList
		{
			get;
			protected set;
		}

		public int NumberOfNodes
		{
			get;
			protected set;
		}

		public Graph(int numberOfNodes)
		{
			NumberOfNodes = numberOfNodes;
			Nodes = new Vector2D[numberOfNodes];
			AdjacencyList = new List<GraphConnection>[numberOfNodes];
			for (int i = 0; i < numberOfNodes; i++)
				AdjacencyList [i] = new List<GraphConnection>();
		}

		public virtual void Connect(int nodeA, int nodeB, int weight, bool bidirectional = true)
		{
			if (!AlreadyConnected(nodeA, nodeB))
				AdjacencyList [nodeA].Add(new GraphConnection(nodeB, weight));

			if (bidirectional && !AlreadyConnected(nodeB, nodeA))
				AdjacencyList [nodeB].Add(new GraphConnection(nodeA, weight));
		}

		public bool AlreadyConnected(int nodeA, int nodeB)
		{
			for (int i = 0; i < AdjacencyList[nodeA].Count; i++)
			{
				if (AdjacencyList [nodeA] [i].destinyNode == nodeB)
					return true;
			}
			return false;
		}
	}
}