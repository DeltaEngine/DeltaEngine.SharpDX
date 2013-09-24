using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.Rendering3D.Models;

namespace DeltaEngine.Rendering3D.Shapes3D
{
	/// <summary>
	/// Entity 3D representing a box with normals.
	/// </summary>
	public class BoxNormal : Mesh
	{
		public BoxNormal(Vector3D size)
			: base(CreateBoxData(size), new Material(Shader.Position3DNormalUv, "DeltaEngineLogo")) { }

		private static Geometry CreateBoxData(Vector3D size)
		{
			var creationData = new GeometryCreationData(VertexFormat.Position3DNormalUv, 8, 36);
			var geometry = ContentLoader.Create<Geometry>(creationData);
			geometry.SetData(ComputeVertices(size), BoxIndices);
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

		private static readonly short[] BoxIndices = new short[]
		{
			0, 1, 2, 2, 1, 3, 4, 5, 6, 6, 5, 7, 5, 0, 7, 7, 0, 2, 1, 4, 3, 3, 4, 6, 5, 4, 0, 0, 4, 1, 6
			, 7, 3, 3, 7, 2
		};
	}
}