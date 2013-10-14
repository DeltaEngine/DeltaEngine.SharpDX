using System.Collections.Generic;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering2D.Shapes
{
	/// <summary>
	/// Sets up an Entity that can be used in 2D line rendering.
	/// </summary>
	public class Line2D : Entity2D
	{
		public Line2D(Vector2D startPoint, Vector2D endPoint, Color color)
			: base(new Rectangle(startPoint, (Size)(endPoint - startPoint)))
		{
			Color = color;
			Add(new List<Vector2D> { startPoint, endPoint });
			OnDraw<Line2DRenderer>();
		}

		public List<Vector2D> Points
		{
			get { return Get<List<Vector2D>>(); }
			set { Set(value); }
		}

		public Vector2D StartPoint
		{
			get { return Points[0]; }
			set { Points[0] = value; }
		}

		public Vector2D EndPoint
		{
			get
			{
				var points = Points;
				return points[points.Count - 1];
			}
			set
			{
				var points = Points;
				points[points.Count - 1] = value;
			}
		}

		public void AddLine(Vector2D startPoint, Vector2D endPoint)
		{
			var points = Points;
			points.Add(startPoint);
			points.Add(endPoint);
		}

		public void ExtendLine(Vector2D nextPoint)
		{
			var points = Points;
			points.Add(points[points.Count - 1]);
			points.Add(nextPoint);
		}

		public void Clip(Rectangle clippingBounds)
		{
			var line = new ClippedLine(StartPoint, EndPoint, clippingBounds);
			StartPoint = line.StartPoint;
			EndPoint = line.EndPoint;
			IsVisible = line.IsVisible;
		}
	}
}