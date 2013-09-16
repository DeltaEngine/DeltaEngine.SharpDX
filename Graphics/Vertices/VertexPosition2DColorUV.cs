using System.Runtime.InteropServices;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Graphics.Vertices
{
	/// <summary>
	/// Vertex struct that describes 3D position, vertex color and texture coordinate.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexPosition2DColorUV : Lerp<VertexPosition2DColorUV>, Vertex
	{
		public VertexPosition2DColorUV(Point position, Color color, Point uv)
		{
			Position = position;
			Color = color;
			UV = uv;
		}

		public Point Position;
		public Color Color;
		public Point UV;

		public static readonly int SizeInBytes = VertexFormat.Position2DColorUv.Stride;

		public VertexPosition2DColorUV Lerp(VertexPosition2DColorUV other, float interpolation)
		{
			return new VertexPosition2DColorUV(Position.Lerp(other.Position, interpolation),
				Color.Lerp(other.Color, interpolation), UV);
		}

		public VertexFormat Format
		{
			get { return VertexFormat.Position2DColorUv; }
		}
	}
}