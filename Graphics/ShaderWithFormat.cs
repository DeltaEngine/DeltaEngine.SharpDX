using System;
using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Graphics.Vertices;

namespace DeltaEngine.Graphics
{
	/// <summary>
	/// Adds graphics specific features to a shader object like VertexFormat and the shader code.
	/// </summary>
	public abstract class ShaderWithFormat : Shader
	{
		protected ShaderWithFormat(string contentName)
			: base(contentName) {}

		protected ShaderWithFormat(ShaderCreationData creationData)
			: base("<GeneratedShader>")
		{
			Initialize(creationData);
		}

		protected void Initialize(ShaderCreationData data)
		{
			if (data.Format == null || data.Format.Elements.Length == 0)
				throw new InvalidVertexFormat();
			if (string.IsNullOrEmpty(data.VertexCode) || string.IsNullOrEmpty(data.FragmentCode) ||
				string.IsNullOrEmpty(data.Dx9Code) || string.IsNullOrEmpty(data.Dx11Code))
				throw new InvalidShaderCode();
			Format = data.Format;
			OpenGLVertexCode = data.VertexCode;
			OpenGLFragmentCode = data.FragmentCode;
			Dx11Code = data.Dx11Code;
			Dx9Code = data.Dx9Code;
		}

		internal class InvalidVertexFormat : Exception {}

		internal class InvalidShaderCode : Exception {}

		public VertexFormat Format { get; private set; }
		protected string OpenGLVertexCode { get; private set; }
		protected string OpenGLFragmentCode { get; private set; }
		protected string Dx11Code { get; private set; }
		protected string Dx9Code { get; private set; }

		protected override void LoadData(Stream fileData)
		{
			if (fileData.Length == 0)
				throw new EmptyShaderFileGiven();
			var data = new BinaryReader(fileData).Create();
			Initialize(data as ShaderCreationData);
			Create();
		}

		public class EmptyShaderFileGiven : Exception {}

		protected abstract void Create();

		protected override bool AllowCreationIfContentNotFound
		{
			get
			{
				return Name == Position2DUv || Name == Position3DUv || Name == Position2DColor ||
					Name == Position3DColor || Name == Position2DColorUv || Name == Position3DColorUv ||
					Name == Position3DNormalUv || Name == Position3DColorSkinned;
			}
		}

		protected override void CreateDefault()
		{
			switch (Name)
			{
			case Position2DUv:
					Initialize(new ShaderCreationData(ShaderCodeOpenGL.PositionUvOpenGLVertexCode,
						ShaderCodeOpenGL.PositionUvOpenGLFragmentCode, ShaderCodeDX11.PositionUvDx11,
						ShaderCodeDX9.Position2DUvDx9, VertexFormat.Position2DUv));
				break;
			case Position2DColor:
				Initialize(new ShaderCreationData(ShaderCodeOpenGL.PositionColorOpenGLVertexCode,
					ShaderCodeOpenGL.PositionColorOpenGLFragmentCode, ShaderCodeDX11.PositionColorDx11, 
					ShaderCodeDX9.Position2DColorDx9, VertexFormat.Position2DColor));
				break;
			case Position2DColorUv:
				Initialize(new ShaderCreationData(ShaderCodeOpenGL.PositionColorUvOpenGLVertexCode,
					ShaderCodeOpenGL.PositionColorUvOpenGLFragmentCode, ShaderCodeDX11.PositionColorUvDx11, 
					ShaderCodeDX9.Position2DColorUvDx9, VertexFormat.Position2DColorUv));
				break;
			case Position3DUv:
				Initialize(new ShaderCreationData(ShaderCodeOpenGL.PositionUvOpenGLVertexCode, 
					ShaderCodeOpenGL.PositionUvOpenGLFragmentCode, ShaderCodeDX11.PositionUvDx11, 
					ShaderCodeDX9.Position3DUvDx9, VertexFormat.Position3DUv));
				break;
			case Position3DColor:
				Initialize(new ShaderCreationData(ShaderCodeOpenGL.PositionColorOpenGLVertexCode,
					ShaderCodeOpenGL.PositionColorOpenGLFragmentCode, ShaderCodeDX11.PositionColorDx11, 
					ShaderCodeDX9.Position3DColorDx9, VertexFormat.Position3DColor));
				break;
			case Position3DColorUv:
				Initialize(new ShaderCreationData(ShaderCodeOpenGL.PositionColorUvOpenGLVertexCode,
					ShaderCodeOpenGL.PositionColorUvOpenGLFragmentCode, ShaderCodeDX11.PositionColorUvDx11, 
					ShaderCodeDX9.Position3DColorUvDx9, VertexFormat.Position3DColorUv));
				break;
			case Position3DNormalUv:
				Initialize(new ShaderCreationData(ShaderCodeOpenGL.PositionUvNormalOpenGLVertexCode,
					ShaderCodeOpenGL.PositionUvNormalOpenGLFragmentCode, ShaderCodeDX11.PositionNormalUvDx11, 
					ShaderCodeDX9.Position3DNormalUvDx9, VertexFormat.Position3DNormalUv));
				break;
			}
			Create();
		}
	}
}