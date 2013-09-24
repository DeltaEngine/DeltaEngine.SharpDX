using DeltaEngine.Datatypes;

namespace DeltaEngine.Graphics.Vertices
{
	/// <summary>
	/// Vertex format with 3D position, color and vertex skinning data (12 + 4 + 8 + 8 bytes).
	/// </summary>
	public struct VertexPosition3DColorSkinned : Lerp<VertexPosition3DColorSkinned>, Vertex
	{
		public VertexPosition3DColorSkinned(Vector3D position, Color color, SkinningData skinning)
		{
			Position = position;
			Color = color;
			Skinning = skinning;
		}

		public Vector3D Position;
		public Color Color;
		public SkinningData Skinning;

		public static readonly int SizeInBytes = VertexFormat.Position3DColorSkinned.Stride;

		public VertexPosition3DColorSkinned Lerp(VertexPosition3DColorSkinned other, float interpolation)
		{
			return new VertexPosition3DColorSkinned(Position.Lerp(other.Position, interpolation),
				Color.Lerp(other.Color, interpolation), Skinning.Lerp(other.Skinning, interpolation));
		}

		public VertexFormat Format
		{
			get { return VertexFormat.Position3DColorSkinned; }
		}
	}
}