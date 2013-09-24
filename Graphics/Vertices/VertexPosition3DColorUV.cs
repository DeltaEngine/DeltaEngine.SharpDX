using System.Runtime.InteropServices;
using DeltaEngine.Datatypes;

namespace DeltaEngine.Graphics.Vertices
{
	/// <summary>
	/// Vertex struct that describes 3D position, vertex color and texture coordinate.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct VertexPosition3DColorUV : Lerp<VertexPosition3DColorUV>, Vertex
	{
		public VertexPosition3DColorUV(Vector3D position, Color color, Vector2D uv)
		{
			Position = position;
			Color = color;
			UV = uv;
		}

		public Vector3D Position;
		public Color Color;
		public Vector2D UV;

		public VertexPosition3DColorUV(Vector2D position, Color color, Vector2D uv)
		{
			Position = new Vector3D(position.X, position.Y, 0.0f);
			Color = color;
			UV = uv;
		}

		public static readonly int SizeInBytes = VertexFormat.Position3DColorUv.Stride;

		public VertexPosition3DColorUV Lerp(VertexPosition3DColorUV other, float interpolation)
		{
			return new VertexPosition3DColorUV(Position.Lerp(other.Position, interpolation),
				Color.Lerp(other.Color, interpolation), UV);
		}

		public VertexFormat Format
		{
			get { return VertexFormat.Position3DColorUv; }
		}
	}
}