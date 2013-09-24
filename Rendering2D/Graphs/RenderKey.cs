using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Rendering2D.Fonts;

namespace DeltaEngine.Rendering2D.Graphs
{
	/// <summary>
	/// Renders the key to the graph lines below the graph.
	/// </summary>
	internal class RenderKey
	{
		public void Refresh(Graph graph)
		{
			ClearOldKeyLabels();
			if (graph.Visibility == Visibility.Show && Visibility == Visibility.Show)
				CreateNewKeyLabels(graph);
		}

		public Visibility Visibility { get; set; }

		private void ClearOldKeyLabels()
		{
			foreach (FontText keyLabel in keyLabels)
				keyLabel.IsActive = false;
			keyLabels.Clear();
		}

		private readonly List<FontText> keyLabels = new List<FontText>();

		private void CreateNewKeyLabels(Graph graph)
		{
			for (int i = 0; i < graph.Lines.Count; i++)
				if (graph.Lines[i].Key != "")
					CreateKeyLabel(graph, i);
		}

		private void CreateKeyLabel(Graph graph, int index)
		{
			keyLabels.Add(new FontText(Font.Default, graph.Lines[index].Key,
				GetKeyLabelDrawArea(graph, index))
			{
				RenderLayer = graph.RenderLayer + RenderLayerOffset,
				Color = graph.Lines[index].Color,
				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top
			});
		}

		private static Rectangle GetKeyLabelDrawArea(Entity2D graph, int index)
		{
			float x = GetKeyLabelXCoord(graph, index);
			float y = GetKeyLabelYCoord(graph, index);
			return new Rectangle(x, y, 1.0f, 1.0f);
		}

		private static float GetKeyLabelXCoord(Entity2D graph, int index)
		{
			float borderWidth = graph.DrawArea.Width * Graph.Border;
			float left = graph.DrawArea.Left + borderWidth;
			int column = index % 6;
			float interval = (graph.DrawArea.Width - 2 * borderWidth) / 6;
			return left + column * interval;
		}

		private static float GetKeyLabelYCoord(Entity2D graph, int index)
		{
			int row = 1 + index / 6;
			float borderHeight = graph.DrawArea.Height * Graph.Border;
			return graph.DrawArea.Bottom + (4 * row) * borderHeight;
		}

		private const int RenderLayerOffset = 2;
	}
}