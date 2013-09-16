using System.Collections.Generic;

namespace DeltaEngine.Physics3D
{
	public class PhysicsShape
	{
		public PhysicsShape()
		{
			Properties = new Dictionary<PropertyType, object>();
		}

		public Dictionary<PropertyType, object> Properties { get; set; }

		public enum PropertyType : byte
		{
			Density,
			Radius,
			Width,
			Height,
			Depth,
			Size,
			Offset,
			Vertices,
			Heights,
			ScaleX,
			ScaleY,
			Mesh,
			LocalSpaceMatrix,
			InvertTriangles,
			NumberOfTeeth,
			TipPercentage,
			ToothHeight,
			RadiusX,
			RadiusY,
			Edges
		}

		public ShapeType ShapeType { get; set; }
	}
}