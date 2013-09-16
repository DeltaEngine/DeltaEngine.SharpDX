using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;

namespace DeltaEngine.Rendering.Shapes
{
	/// <summary>
	/// Sets up an Entity that can be used in 2D line rendering.
	/// </summary>
	public class Line2D : Entity2D
	{
		public Line2D(Point startPoint, Point endPoint, Color color)
			: base(new Rectangle(startPoint, (Size)(endPoint - startPoint)))
		{
			Color = color;
			Add(new List<Point> { startPoint, endPoint });
			OnDraw<DrawLine2D>();
		}

		public List<Point> Points
		{
			get { return Get<List<Point>>(); }
			set { Set(value); }
		}

		public Point StartPoint
		{
			get { return Points[0]; }
			set { Points[0] = value; }
		}

		public Point EndPoint
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

		public void AddLine(Point start, Point end)
		{
			var points = Points;
			points.Add(start);
			points.Add(end);
		}

		public void ExtendLine(Point nextPoint)
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
			Visibility = line.IsVisible ? Visibility.Show : Visibility.Hide;
		}
	}
}