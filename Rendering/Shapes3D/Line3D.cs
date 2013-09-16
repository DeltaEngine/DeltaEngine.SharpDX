using System.Collections.Generic;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Rendering.Shapes3D
{
	/// <summary>
	/// A line in 3D space. Just a start and end point plus a color, but you can add more points.
	/// </summary>
	public class Line3D : Entity3D
	{
		public Line3D(Vector start, Vector end, Color color)
			: base(Vector.Zero)
		{
			Add(color);
			Add(new List<Vector> { start, end });
			OnDraw<DrawLine3D>();
		}

		public List<Vector> Points
		{
			get { return Get<List<Vector>>(); }
			set { Set(value); }
		}

		public Vector StartPoint
		{
			get { return Points[0]; }
			set { Points[0] = value; }
		}

		public Vector EndPoint
		{
			get { return Points[1]; }
			set { Points[1] = value; }
		}
	}
}
