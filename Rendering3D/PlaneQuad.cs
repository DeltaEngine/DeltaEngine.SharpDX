using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.Vertices;

namespace DeltaEngine.Rendering3D
{
	/// <summary>
	/// Textured rectangular mesh
	/// </summary>
	public class PlaneQuad : Mesh
	{
		public PlaneQuad(Size size, Material material)
			: base(CreatePlaneGeometry(size, material), material)
		{
			this.size = size;
		}

		private Size size;

		private static Geometry CreatePlaneGeometry(Size size, Material material)
		{
			var shader = material.Shader as ShaderWithFormat;
			var creationData = new GeometryCreationData(shader.Format, 4, 6);
			var geometry = ContentLoader.Create<Geometry>(creationData);
			geometry.SetData(
				shader.Format.HasColor
					? CreateColoredVertices(size, material.DefaultColor)
					: CreateVertices(size), Indices());
			return geometry;
		}

		private static Vertex[] CreateColoredVertices(Size size, Color color)
		{
			float xWidthHalf = size.Width / 2;
			float zWidthHalf = size.Height / 2;
			var vertices = new Vertex[]
			{
				new VertexPosition3DColorUV(new Vector3D(-xWidthHalf, 0, zWidthHalf), color, Vector2D.Zero),
				new VertexPosition3DColorUV(new Vector3D(xWidthHalf, 0, zWidthHalf), color, Vector2D.UnitX),
				new VertexPosition3DColorUV(new Vector3D(xWidthHalf, 0, -zWidthHalf), color, Vector2D.One),
				new VertexPosition3DColorUV(new Vector3D(-xWidthHalf, 0, -zWidthHalf), color, Vector2D.UnitY)
			};
			return vertices;
		}

		private static Vertex[] CreateVertices(Size size)
		{
			float xWidthHalf = size.Width / 2;
			float zWidthHalf = size.Height / 2;
			var vertices = new Vertex[]
			{
				new VertexPosition3DUV(new Vector3D(-xWidthHalf, 0, zWidthHalf), Vector2D.Zero),
				new VertexPosition3DUV(new Vector3D(xWidthHalf, 0, zWidthHalf), Vector2D.UnitX),
				new VertexPosition3DUV(new Vector3D(xWidthHalf, 0, -zWidthHalf), Vector2D.One),
				new VertexPosition3DUV(new Vector3D(-xWidthHalf, 0, -zWidthHalf), Vector2D.UnitY)
			};
			return vertices;
		}

		private static short[] Indices()
		{
			return new short[] { 0, 1, 2, 0, 2, 3 };
		}

		public Size Size
		{
			get { return size; }
			set
			{
				var shader = Material.Shader as ShaderWithFormat;
				size = value;
				Geometry.SetData(
					shader.Format.HasColor
					? CreateColoredVertices(size, Material.DefaultColor)
					: CreateVertices(size), Indices());
			}
		}
	}
}