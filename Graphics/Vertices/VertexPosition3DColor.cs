using System.Runtime.InteropServices;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Graphics.Vertices
{
	/// <summary>
	/// Simplest vertex format with just 3D positions and vertex colors (12 + 4 bytes).
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexPosition3DColor : Lerp<VertexPosition3DColor>, Vertex
	{
		public VertexPosition3DColor(Vector position, Color color)
		{
			Position = position;
			Color = color;
		}

		public Vector Position;
		public Color Color;

		public VertexPosition3DColor(Point position, Color color)
			: this(new Vector(position.X, position.Y, 0.0f), color) {}

		public static readonly int SizeInBytes = VertexFormat.Position3DColor.Stride;

		public VertexPosition3DColor Lerp(VertexPosition3DColor other, float interpolation)
		{
			return new VertexPosition3DColor(Position.Lerp(other.Position, interpolation),
				Color.Lerp(other.Color, interpolation));
		}

		public VertexFormat Format
		{
			get { return VertexFormat.Position3DColor; }
		}
	}
}