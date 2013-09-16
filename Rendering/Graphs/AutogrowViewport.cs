using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering.Graphs
{
	/// <summary>
	/// Automatically grows a graph so that all points are visible.
	/// (It won't shrink a graph back down when points are removed though.)
	/// </summary>
	internal class AutogrowViewport
	{
		public void ProcessAddedPoint(Graph graph, Point point)
		{
			viewport = graph.Viewport;
			DeriveExtremities();
			AdjustViewport(point);
			graph.Viewport = viewport;
		}

		private Rectangle viewport;

		private void DeriveExtremities()
		{
			float width = viewport.Width / (1 + 2 * Buffer);
			float left = viewport.Center.X - width / 2;
			float height = viewport.Height / (1 + 2 * Buffer);
			float top = viewport.Center.Y - height / 2;
			extremities = new Rectangle(left, top, width, height);
		}

		private const float Buffer = 0.05f;
		private Rectangle extremities;

		private void AdjustViewport(Point point)
		{
			if (viewport == Rectangle.Zero)
				viewport = new Rectangle(point, Size.Zero);
			else
				UpdateEdges(point);
		}

		private void UpdateEdges(Point point)
		{
			if (extremities.Contains(point))
				return;
			UpdateExtremities(point);
			UpdateViewportFromExtremities();
		}

		private void UpdateExtremities(Point point)
		{
			if (point.X < extremities.Left)
				MoveLeftEdge(point.X);
			if (point.X > extremities.Right)
				extremities.Width = point.X - extremities.Left;
			if (point.Y < extremities.Top)
				MoveTopEdge(point.Y);
			if (point.Y > extremities.Bottom)
				extremities.Height = point.Y - extremities.Top;
		}

		private void MoveLeftEdge(float left)
		{
			extremities.Width = extremities.Right - left;
			extremities.Left = left;
		}

		private void MoveTopEdge(float top)
		{
			extremities.Height = extremities.Bottom - top;
			extremities.Top = top;
		}

		private void UpdateViewportFromExtremities()
		{
			float width = extremities.Width * (1 + 2 * Buffer);
			float left = extremities.Center.X - width / 2;
			float height = extremities.Height * (1 + 2 * Buffer);
			float top = extremities.Center.Y - height / 2;
			viewport = new Rectangle(left, top, width, height);
		}
	}
}