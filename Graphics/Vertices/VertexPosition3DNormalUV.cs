using System.Runtime.InteropServices;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Graphics.Vertices
{
	/// <summary>
	/// Vertex struct that describes 3D position, vertex color and texture coordinate.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexPosition3DNormalUV : Lerp<VertexPosition3DNormalUV>, Vertex
	{
		public VertexPosition3DNormalUV(Vector position, Vector normal, Point uv)
		{
			Position = position;
			Normal = normal;
			UV = uv;
		}

		public Vector Position;
		public Vector Normal;
		public Point UV;

		public VertexPosition3DNormalUV(Point position, Vector normal, Point uv)
		{
			Position = new Vector(position.X, position.Y, 0.0f);
			Normal = normal;
			UV = uv;
		}

		public static readonly int SizeInBytes = VertexFormat.Position3DNormalUv.Stride;

		public VertexPosition3DNormalUV Lerp(VertexPosition3DNormalUV other, float interpolation)
		{
			return new VertexPosition3DNormalUV(Position.Lerp(other.Position, interpolation),
				Normal.Lerp(other.Normal, interpolation), UV);
		}

		public VertexFormat Format
		{
			get { return VertexFormat.Position3DNormalUv; }
		}
	}
}