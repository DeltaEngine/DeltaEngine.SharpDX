using System.Collections.Generic;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Extensions;

namespace DeltaEngine.Rendering2D.Shapes
{
	/// <summary>
	/// Renders a filled 2D ellipse shape.
	/// </summary>
	public class Ellipse : Polygon2D
	{
		public Ellipse(Vector2D center, float radiusX, float radiusY, Color color)
			: this(Rectangle.FromCenter(center, new Size(2 * radiusX, 2 * radiusY)), color) {}

		public Ellipse(Rectangle drawArea, Color color)
			: base(drawArea, color)
		{
			Start<UpdatePoints>();
		}

		public float RadiusX
		{
			get { return DrawArea.Width / 2.0f; }
			set
			{
				var drawArea = DrawArea;
				DrawArea = Rectangle.FromCenter(drawArea.Center, new Size(2 * value, drawArea.Height));
			}
		}

		public float RadiusY
		{
			get { return DrawArea.Height / 2.0f; }
			set
			{
				var drawArea = DrawArea;
				DrawArea = Rectangle.FromCenter(drawArea.Center, new Size(drawArea.Width, 2 * value));
			}
		}

		public float Radius
		{
			get { return MathExtensions.Max(RadiusX, RadiusY); }
			set
			{
				var drawArea = DrawArea;
				DrawArea = Rectangle.FromCenter(drawArea.Center, new Size(2 * value, 2 * value));
			}
		}

		/// <summary>
		/// This recalculates the points of an Ellipse if they change
		/// </summary>
		public class UpdatePoints : UpdateBehavior
		{
			public override void Update(IEnumerable<Entity> entities)
			{
				foreach (var entity in entities)
				{
					InitializeVariables((Entity2D)entity);
					FormEllipsePoints(entity);
				}
			}

			private void InitializeVariables(Entity2D entity)
			{
				var rotation = entity.Rotation;
				rotationSin = MathExtensions.Sin(rotation);
				rotationCos = MathExtensions.Cos(rotation);
				var drawArea = entity.DrawArea;
				rotationCenter = entity.RotationCenter;
				center = entity.DrawArea.Center;
				if (center != rotationCenter)
					center = center.RotateAround(rotationCenter, rotationSin, rotationCos);
				radiusX = drawArea.Width / 2.0f;
				radiusY = drawArea.Height / 2.0f;
				float maxRadius = MathExtensions.Max(radiusX, radiusY);
				pointsCount = GetPointsCount(maxRadius);
				theta = -360.0f / (pointsCount - 1);
			}

			private float rotationSin;
			private float rotationCos;
			private Vector2D center;
			private Vector2D rotationCenter;
			private float radiusX;
			private float radiusY;
			private int pointsCount;
			private float theta;

			private static int GetPointsCount(float maxRadius)
			{
				var pointsCount = (int)(MaxPoints * MathExtensions.Max(0.22f + maxRadius / 2, maxRadius));
				return MathExtensions.Max(pointsCount, MinPoints);
			}

			private const int MinPoints = 5;
			private const int MaxPoints = 96;

			private void FormEllipsePoints(Entity entity)
			{
				ellipsePoints = entity.Get<List<Vector2D>>();
				ellipsePoints.Clear();
				ellipsePoints.Add(center);
				for (int i = 0; i <= pointsCount; i++)
					FormRotatedEllipsePoint(i);
				entity.Set(ellipsePoints);
			}

			private List<Vector2D> ellipsePoints;

			private void FormRotatedEllipsePoint(int i)
			{
				var ellipsePoint = new Vector2D(radiusX * MathExtensions.Sin(i * theta),
					radiusY * MathExtensions.Cos(i * theta));
				ellipsePoints.Add(center +
					ellipsePoint.RotateAround(Vector2D.Zero, rotationSin, rotationCos));
			}
		}
	}
}