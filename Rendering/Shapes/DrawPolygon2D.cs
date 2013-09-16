using System.Collections.Generic;
using System.Linq;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Entities;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.ScreenSpaces;

namespace DeltaEngine.Rendering.Shapes
{
	/// <summary>
	/// Responsible for rendering filled 2D shapes defined by their border points
	/// </summary>
	public class DrawPolygon2D : DrawBehavior
	{
		public DrawPolygon2D(Drawing draw)
		{
			this.draw = draw;
			material = new Material(Shader.Position2DColor, "");
		}

		private readonly Drawing draw;
		private readonly Material material;

		private void AddToBatch(Entity2D entity)
		{
			var points = entity.Get<List<Point>>();
			if (points.Count < 3)
				return;
			var color = entity.Color;
			var center = GetCenter(points);
			lastPoint = points[points.Count - 1];
			foreach (Point point in points)
				CreateAndDrawTriangle(point, center, color);
		}

		private static Point GetCenter(ICollection<Point> points)
		{
			Point center = points.Aggregate(Point.Zero, (current, point) => current + point);
			return center / points.Count;
		}

		private Point lastPoint;

		private void CreateAndDrawTriangle(Point point, Point center, Color color)
		{
			SetTrianglePoints(new Triangle2D(center, lastPoint, point), color);
			lastPoint = point;
		}

		private void SetTrianglePoints(Triangle2D triangle, Color color)
		{
			vertices.Add(
				new VertexPosition2DColor(ScreenSpace.Current.ToPixelSpaceRounded(triangle.Corner1), color));
			vertices.Add(
				new VertexPosition2DColor(ScreenSpace.Current.ToPixelSpaceRounded(triangle.Corner2), color));
			vertices.Add(
				new VertexPosition2DColor(ScreenSpace.Current.ToPixelSpaceRounded(triangle.Corner3), color));
		}

		private readonly List<VertexPosition2DColor> vertices = new List<VertexPosition2DColor>();
		private static short[] triangleIndices = { 0, 1, 2 };

		private void SetIndices()
		{
			if (vertices.Count == 0)
				return;
			var newVertices = new VertexPosition2DColor[vertices.Count + 1];
			var newIndices = new short[vertices.Count + 1];
			for (int posInList = 0; posInList < vertices.Count; ++posInList)
				NumberOfVertex(newVertices, posInList, newIndices);
			triangleIndices = newIndices;
			DrawTriangles(newVertices);
		}

		private void DrawTriangles(VertexPosition2DColor[] newVertices)
		{
			draw.Add(material, newVertices, triangleIndices);
		}

		private void NumberOfVertex(VertexPosition2DColor[] newVertices, int posInList,
			short[] newIndices)
		{
			newVertices[posInList] = vertices[posInList];
			newIndices[posInList] = (short)posInList;
		}

		public void Draw(IEnumerable<DrawableEntity> entities)
		{
			foreach (var entity in entities)
				AddToBatch((Entity2D)entity);
			SetIndices();
			vertices.Clear();
		}
	}
}