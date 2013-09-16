using System.Collections.Generic;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering.Shapes
{
	/// <summary>
	/// A 2D shape to be rendered defined by its border points, will be rendered with a filled color.
	/// </summary>
	public class Polygon2D : Entity2D
	{
		public Polygon2D(Rectangle drawArea, Color color)
			: base(drawArea)
		{
			Color = color;
			Add(new List<Point>());
			OnDraw<DrawPolygon2D>();
		}

		public List<Point> Points
		{
			get { return Get<List<Point>>(); }
		}
	}
}