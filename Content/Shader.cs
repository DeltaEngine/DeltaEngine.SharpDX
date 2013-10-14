using DeltaEngine.Datatypes;

namespace DeltaEngine.Content
{
	/// <summary>
	/// Tells the GPU how to render 2D or 3D material data on screen. Usually loaded from content.
	/// This base class has no functionality, some common functionality can be found in
	/// ShaderWithFormat. Provide multiple framework specific shader codes if creating manually.
	/// </summary>
	public abstract class Shader : ContentData
	{
		protected Shader(string contentName)
			: base(contentName) { }

		public abstract void SetModelViewProjectionMatrix(Matrix matrix);
		public abstract void SetJointMatrices(Matrix[] jointMatrices);
		public abstract void SetDiffuseTexture(Image texture);
		public abstract void SetLightmapTexture(Image texture);
		public abstract void SetLightPosition(Vector3D vector);
		public abstract void SetViewPosition(Vector3D vector);
		public abstract void Bind();
		public abstract void BindVertexDeclaration();

		public const string Position2DUV = "Position2DUV";
		public const string Position2DColorUV = "Position2DColorUV";
		public const string Position2DColor = "Position2DColor";
		public const string Position3DUV = "Position3DUV";
		public const string Position3DColorUV = "Position3DColorUV";
		public const string Position3DColor = "Position3DColor";
		public const string Position3DNormalUV = "Position3DNormalUV";
		public const string Position3DNormalUVLightmap = "Position3DNormalUVLightmap";
		public const string Position3DColorSkinned = "Position3DColorSkinned";
		public const string Position3DUVSkinned = "Position3DUVSkinned";
	}
}