using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.Vertices;

namespace DeltaEngine.Rendering3D.Shapes3D
{
	/// <summary>
	/// Entity 3D representing a box with normals.
	/// </summary>
	public class BoxNormal : Mesh
	{
		public BoxNormal(Vector3D size)
			: base(CreateBoxData(size), new Material(Shader.Position3DNormalUV, "DeltaEngineLogo")) {}

		private static Geometry CreateBoxData(Vector3D size)
		{
			var creationData = new GeometryCreationData(VertexFormat.Position3DNormalUV, 8, 36);
			var geometry = ContentLoader.Create<Geometry>(creationData);
			geometry.SetData(ComputeVertices(size), Box.BoxIndices);
			return geometry;
		}

		private static Vertex[] ComputeVertices(Vector3D size)
		{
			float right = size.X / 2.0f;
			float back = size.Z / 2.0f;
			float top = size.Y / 2.0f;
			float bottom = -size.Y / 2.0f;
			var vertices = new Vertex[]
			{
				new VertexPosition3DNormalUV(new Vector3D(-right, -back, top),
					Vector3D.Normalize(new Vector3D(-1, -1, 1)), new Vector2D(0, 0)),
				new VertexPosition3DNormalUV(new Vector3D(right, -back, top),
					Vector3D.Normalize(new Vector3D(1, -1, 1)), new Vector2D(0.33f, 0)),
				new VertexPosition3DNormalUV(new Vector3D(-right, -back, bottom),
					Vector3D.Normalize(new Vector3D(-1, -1, -1)), new Vector2D(0, 1)),
				new VertexPosition3DNormalUV(new Vector3D(right, -back, bottom),
					Vector3D.Normalize(new Vector3D(1, -1, -1)), new Vector2D(0.33f, 1)),
				new VertexPosition3DNormalUV(new Vector3D(right, back, top),
					Vector3D.Normalize(new Vector3D(1, 1, 1)), new Vector2D(0.66f, 0)),
				new VertexPosition3DNormalUV(new Vector3D(-right, back, top),
					Vector3D.Normalize(new Vector3D(-1, 1, 1)), new Vector2D(1, 0)),
				new VertexPosition3DNormalUV(new Vector3D(right, back, bottom),
					Vector3D.Normalize(new Vector3D(1, 1, -1)), new Vector2D(0.66f, 1)),
				new VertexPosition3DNormalUV(new Vector3D(-right, back, bottom),
					Vector3D.Normalize(new Vector3D(-1, 1, -1)), new Vector2D(1, 1))
			};
			return vertices;
		}
	}
}