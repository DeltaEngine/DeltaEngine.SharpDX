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
				string.IsNullOrEmpty(data.DX9Code) || string.IsNullOrEmpty(data.DX11Code))
				throw new InvalidShaderCode();
			Format = data.Format;
			OpenGLVertexCode = data.VertexCode;
			OpenGLFragmentCode = data.FragmentCode;
			DX11Code = data.DX11Code;
			DX9Code = data.DX9Code;
		}

		internal class InvalidVertexFormat : Exception {}

		internal class InvalidShaderCode : Exception {}

		public VertexFormat Format { get; private set; }
		protected string OpenGLVertexCode { get; private set; }
		protected string OpenGLFragmentCode { get; private set; }
		protected string DX11Code { get; private set; }
		protected string DX9Code { get; private set; }

		protected override void LoadData(Stream fileData)
		{
			if (fileData.Length == 0)
				throw new NoShaderDataSpecified();
			var data = new BinaryReader(fileData).Create();
			Initialize(data as ShaderCreationData);
			Create();
		}

		public class NoShaderDataSpecified : Exception {}

		protected abstract void Create();

		protected override bool AllowCreationIfContentNotFound
		{
			get
			{
				return Name == Position2DUV || Name == Position3DUV || Name == Position2DColor ||
					Name == Position3DColor || Name == Position2DColorUV || Name == Position3DColorUV ||
					Name == Position3DNormalUV || Name == Position3DColorSkinned;
			}
		}

		protected override void CreateDefault()
		{
			switch (Name)
			{
			case Position2DUV:
					Initialize(new ShaderCreationData(ShaderCodeOpenGL.PositionUVOpenGLVertexCode,
						ShaderCodeOpenGL.PositionUVOpenGLFragmentCode, ShaderCodeDX11.PositionUVDX11,
						ShaderCodeDX9.Position2DUVDX9, VertexFormat.Position2DUV));
				break;
			case Position2DColor:
				Initialize(new ShaderCreationData(ShaderCodeOpenGL.PositionColorOpenGLVertexCode,
					ShaderCodeOpenGL.PositionColorOpenGLFragmentCode, ShaderCodeDX11.PositionColorDX11, 
					ShaderCodeDX9.Position2DColorDX9, VertexFormat.Position2DColor));
				break;
			case Position2DColorUV:
				Initialize(new ShaderCreationData(ShaderCodeOpenGL.PositionColorUVOpenGLVertexCode,
					ShaderCodeOpenGL.PositionColorUVOpenGLFragmentCode, ShaderCodeDX11.PositionColorUVDX11, 
					ShaderCodeDX9.Position2DColorUVDX9, VertexFormat.Position2DColorUV));
				break;
			case Position3DUV:
				Initialize(new ShaderCreationData(ShaderCodeOpenGL.PositionUVOpenGLVertexCode, 
					ShaderCodeOpenGL.PositionUVOpenGLFragmentCode, ShaderCodeDX11.PositionUVDX11, 
					ShaderCodeDX9.Position3DUVDX9, VertexFormat.Position3DUV));
				break;
			case Position3DColor:
				Initialize(new ShaderCreationData(ShaderCodeOpenGL.PositionColorOpenGLVertexCode,
					ShaderCodeOpenGL.PositionColorOpenGLFragmentCode, ShaderCodeDX11.PositionColorDX11, 
					ShaderCodeDX9.Position3DColorDX9, VertexFormat.Position3DColor));
				break;
			case Position3DColorUV:
				Initialize(new ShaderCreationData(ShaderCodeOpenGL.PositionColorUVOpenGLVertexCode,
					ShaderCodeOpenGL.PositionColorUVOpenGLFragmentCode, ShaderCodeDX11.PositionColorUVDX11, 
					ShaderCodeDX9.Position3DColorUVDX9, VertexFormat.Position3DColorUV));
				break;
			case Position3DNormalUV:
				Initialize(new ShaderCreationData(ShaderCodeOpenGL.PositionUVNormalOpenGLVertexCode,
					ShaderCodeOpenGL.PositionUVNormalOpenGLFragmentCode, ShaderCodeDX11.PositionNormalUVDX11, 
					ShaderCodeDX9.Position3DNormalUVDX9, VertexFormat.Position3DNormalUV));
				break;
			}
			Create();
		}
	}
}