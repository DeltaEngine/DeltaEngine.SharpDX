using System.Runtime.InteropServices;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Graphics.Vertices
{
	/// <summary>
	/// Vertex struct that describes 2D position and texture coordinate.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexPosition2DUV : Lerp<VertexPosition2DUV>, Vertex
	{
		public VertexPosition2DUV(Point position, Point uv)
		{
			Position = position;
			UV = uv;
		}

		public Point Position;
		public Point UV;

		public static readonly int SizeInBytes = VertexFormat.Position2DUv.Stride;

		public VertexPosition2DUV Lerp(VertexPosition2DUV other, float interpolation)
		{
			return new VertexPosition2DUV(Position.Lerp(other.Position, interpolation), UV);
		}

		public VertexFormat Format
		{
			get { return VertexFormat.Position2DUv; }
		}
	}
}