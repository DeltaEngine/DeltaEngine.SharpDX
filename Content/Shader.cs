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
		public abstract void Bind();
		public abstract void BindVertexDeclaration();

		public const string Position2DUv = "Position2DUv";
		public const string Position2DColorUv = "Position2DColorUv";
		public const string Position2DColor = "Position2DColor";
		public const string Position3DUv = "Position3DUv";
		public const string Position3DColorUv = "Position3DColorUv";
		public const string Position3DColor = "Position3DColor";
		public const string Position3DNormalUv = "Position3DNormalUv";
		public const string Position3DTexturedLightmap = "Position3DTexturedLightmap";
		public const string Position3DUvSkinned = "Position3DUvSkinned";
	}
}