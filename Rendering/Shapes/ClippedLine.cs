using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering.Shapes
{
	/// <summary>
	/// Helps with the Line2D.Clip method to limit lines by a clipping bounds rectangle.
	/// </summary>
	internal class ClippedLine
	{
		public ClippedLine(Point startPoint, Point endPoint, Rectangle clippingBounds)
		{
			StartPoint = startPoint;
			EndPoint = endPoint;
			direction = endPoint - startPoint;
			this.clippingBounds = clippingBounds;
			isStartInside = InclusiveContains(startPoint);
			isEndInside = InclusiveContains(endPoint);
			ClipLine();
		}

		public Point StartPoint { get; private set; }
		public Point EndPoint { get; private set; }
		private readonly Point direction;
		private readonly Rectangle clippingBounds;
		private readonly bool isStartInside;
		private readonly bool isEndInside;

		private bool InclusiveContains(Point position)
		{
			return position.X >= clippingBounds.Left && position.X <= clippingBounds.Right &&
				position.Y >= clippingBounds.Top && position.Y <= clippingBounds.Bottom;
		}

		private void ClipLine()
		{
			if (isStartInside && isEndInside)
			{
				IsVisible = true;
				return;
			}

			if (CantIntersect())
			{
				IsVisible = false;
				return;
			}

			UpdateIntersects();
			bool wasStartClipped = (!isStartInside && ClipStart());
			bool wasEndClipped = (!isEndInside && ClipEnd());
			IsVisible = wasStartClipped || wasEndClipped;
		}

		public bool IsVisible { get; private set; }

		private bool CantIntersect()
		{
			if (StartPoint.X < clippingBounds.Left && EndPoint.X < clippingBounds.Left)
				return true;

			if (StartPoint.X > clippingBounds.Right && EndPoint.X > clippingBounds.Right)
				return true;

			if (StartPoint.Y < clippingBounds.Top && EndPoint.Y < clippingBounds.Top)
				return true;

			return StartPoint.Y > clippingBounds.Bottom && EndPoint.Y > clippingBounds.Bottom;
		}

		private void UpdateIntersects()
		{
			intersects[0] = Intersects(clippingBounds.TopLeft, clippingBounds.BottomLeft);
			intersects[1] = Intersects(clippingBounds.TopRight, clippingBounds.BottomRight);
			intersects[2] = Intersects(clippingBounds.TopLeft, clippingBounds.TopRight);
			intersects[3] = Intersects(clippingBounds.BottomLeft, clippingBounds.BottomRight);
		}

		private readonly Point?[] intersects = new Point?[4];

		private Point? Intersects(Point corner1, Point corner2)
		{
			Point edge = corner2 - corner1;
			float dotProduct = direction.X * edge.Y - direction.Y * edge.X;
			if (dotProduct == 0)
				return null;

			Point lineToLine = corner1 - StartPoint;
			float t = (lineToLine.X * edge.Y - lineToLine.Y * edge.X) / dotProduct;
			if (t < 0 || t > 1)
				return null;

			float u = (lineToLine.X * direction.Y - lineToLine.Y * direction.X) / dotProduct;
			if (u < 0 || u > 1)
				return null;

			return StartPoint + t * direction;
		}

		private bool ClipStart()
		{
			Point? intersect = GetClosestIntersectTo(StartPoint);
			if (intersect == null)
				return false;

			StartPoint = (Point)intersect;
			return true;
		}

		private Point? GetClosestIntersectTo(Point point)
		{
			closestIntersect = null;
			for (int i = 0; i < 4; i++)
				if (intersects[i] != null)
					UpdateClosestIntersect(point, (Point)intersects[i]);

			return closestIntersect;
		}

		private Point? closestIntersect;

		private void UpdateClosestIntersect(Point point, Point intersectCandidate)
		{
			float distance = point.DistanceToSquared(intersectCandidate);
			if (closestIntersect != null && distance >= shortestDistance)
				return;

			closestIntersect = intersectCandidate;
			shortestDistance = distance;
		}

		private float shortestDistance;

		private bool ClipEnd()
		{
			Point? intersect = GetClosestIntersectTo(EndPoint);
			if (intersect == null)
				return false;

			EndPoint = (Point)intersect;
			return true;
		}
	}
}