using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.Rendering.Models;

namespace DeltaEngine.Rendering.Sprites
{
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
					? CreatePlaneVerticesColored(size, material.DefaultColor)
					: CreatePlaneVerticesJustUv(size), Indices());
			return geometry;
		}

		private static Vertex[] CreatePlaneVerticesJustUv(Size size)
		{
			float xWidthHalf = size.Width / 2;
			float zWidthHalf = size.Height / 2;
			var vertices = new Vertex[]
			{
				new VertexPosition3DUV(new Vector(-xWidthHalf, 0, zWidthHalf), Point.Zero),
				new VertexPosition3DUV(new Vector(xWidthHalf, 0, zWidthHalf), Point.UnitX),
				new VertexPosition3DUV(new Vector(xWidthHalf, 0, -zWidthHalf), Point.One),
				new VertexPosition3DUV(new Vector(-xWidthHalf, 0, -zWidthHalf), Point.UnitY)
			};
			return vertices;
		}

		private static Vertex[] CreatePlaneVerticesColored(Size size, Color color)
		{
			float xWidthHalf = size.Width / 2;
			float zWidthHalf = size.Height / 2;
			var vertices = new Vertex[]
			{
				new VertexPosition3DColorUV(new Vector(-xWidthHalf, 0, zWidthHalf), color, Point.Zero),
				new VertexPosition3DColorUV(new Vector(xWidthHalf, 0, zWidthHalf), color, Point.UnitX),
				new VertexPosition3DColorUV(new Vector(xWidthHalf, 0, -zWidthHalf), color, Point.One),
				new VertexPosition3DColorUV(new Vector(-xWidthHalf, 0, -zWidthHalf), color, Point.UnitY)
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
					? CreatePlaneVerticesColored(size, Material.DefaultColor)
					: CreatePlaneVerticesJustUv(size), Indices());
			}
		}
	}
}