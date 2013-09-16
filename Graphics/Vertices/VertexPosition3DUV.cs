using System.Runtime.InteropServices;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Graphics.Vertices
{
	/// <summary>
	/// Vertex struct that describes 3D position and texture coordinate.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexPosition3DUV : Lerp<VertexPosition3DUV>, Vertex
	{
		public VertexPosition3DUV(Vector position, Point uv)
		{
			Position = position;
			UV = uv;
		}

		public Vector Position;
		public Point UV;

		public VertexPosition3DUV(Point position, Point uv)
		{
			Position = new Vector(position.X, position.Y, 0.0f);
			UV = uv;
		}

		public static readonly int SizeInBytes = VertexFormat.Position3DUv.Stride;

		public VertexPosition3DUV Lerp(VertexPosition3DUV other, float interpolation)
		{
			return new VertexPosition3DUV(Position.Lerp(other.Position, interpolation), UV);
		}

		public VertexFormat Format
		{
			get { return VertexFormat.Position3DUv; }
		}
	}
}