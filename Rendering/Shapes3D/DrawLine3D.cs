using System.Collections.Generic;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.Vertices;

namespace DeltaEngine.Rendering.Shapes3D
{
	/// <summary>
	/// Responsible for rendering 3D lines
	/// </summary>
	internal class DrawLine3D : DrawBehavior
	{
		public DrawLine3D(Drawing draw)
		{
			this.draw = draw;
			material = new Material(Shader.Position3DColor, "");
		}

		private readonly Drawing draw;
		private readonly Material material;

		public void Draw(IEnumerable<DrawableEntity> entities)
		{
			foreach (var entity in entities)
				AddVerticesFromLine(entity);
			if (vertices.Count == 0)
				return;
			draw.AddLines(material, vertices.ToArray());
			vertices.Clear();
		}

		private void AddVerticesFromLine(DrawableEntity entity)
		{
			var color = entity.Get<Color>();
			var points = entity.GetInterpolatedList<Vector>();
			foreach (Vector point in points)
				vertices.Add(new VertexPosition3DColor(point, color));
		}

		private readonly List<VertexPosition3DColor> vertices = new List<VertexPosition3DColor>();
	}
}