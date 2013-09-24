using DeltaEngine.Datatypes;

namespace DeltaEngine.Graphics.Vertices
{
	/// <summary>
	/// Vertex format with 3D position, texture UV and vertex skinning data (12 + 8 + 8 + 8 bytes).
	/// </summary>
	public struct VertexPosition3DUvSkinned : Lerp<VertexPosition3DUvSkinned>, Vertex
	{
		public VertexPosition3DUvSkinned(Vector3D position, Vector2D uv, SkinningData skinning)
		{
			Position = position;
			UV = uv;
			Skinning = skinning;
		}

		public Vector3D Position;
		public Vector2D UV;
		public SkinningData Skinning;

		public static readonly int SizeInBytes = VertexFormat.Position3DColorSkinned.Stride;

		public VertexPosition3DUvSkinned Lerp(VertexPosition3DUvSkinned other, float interpolation)
		{
			return new VertexPosition3DUvSkinned(Position.Lerp(other.Position, interpolation),
				UV.Lerp(other.UV, interpolation), Skinning.Lerp(other.Skinning, interpolation));
		}

		public VertexFormat Format
		{
			get { return VertexFormat.Position3DUvSkinned; }
		}
	}
}