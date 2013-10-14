using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Graphics.Vertices;
using NUnit.Framework;

namespace DeltaEngine.Graphics.Tests
{
	public class ShaderGenerationTests
	{
		[Test, Ignore]
		public void Create3DShaderContentFiles()
		{
			Create(ShaderCodeOpenGL.PositionUVOpenGLVertexCode, ShaderCodeOpenGL.PositionUVOpenGLFragmentCode,
				ShaderCodeDX11.PositionUVDX11, ShaderCodeDX9.Position2DUVDX9,
				VertexFormat.Position2DUV, Shader.Position2DUV);
			Create(ShaderCodeOpenGL.PositionColorOpenGLVertexCode, ShaderCodeOpenGL.PositionColorOpenGLFragmentCode,
				ShaderCodeDX11.PositionColorDX11, ShaderCodeDX9.Position2DColorDX9,
				VertexFormat.Position2DColor, Shader.Position2DColor);
			Create(ShaderCodeOpenGL.PositionColorUVOpenGLVertexCode, ShaderCodeOpenGL.PositionColorUVOpenGLFragmentCode,
				ShaderCodeDX11.PositionColorUVDX11, ShaderCodeDX9.Position2DColorUVDX9,
				VertexFormat.Position2DColorUV, Shader.Position2DColorUV);
			Create(ShaderCodeOpenGL.PositionUVOpenGLVertexCode, ShaderCodeOpenGL.PositionUVOpenGLFragmentCode,
				ShaderCodeDX11.PositionUVDX11, ShaderCodeDX9.Position3DUVDX9,
				VertexFormat.Position3DUV, Shader.Position3DUV);
			Create(ShaderCodeOpenGL.PositionColorOpenGLVertexCode, ShaderCodeOpenGL.PositionColorOpenGLFragmentCode,
				ShaderCodeDX11.PositionColorDX11, ShaderCodeDX9.Position3DColorDX9,
				VertexFormat.Position3DColor, Shader.Position3DColor);
			Create(ShaderCodeOpenGL.PositionColorUVOpenGLVertexCode, ShaderCodeOpenGL.PositionColorUVOpenGLFragmentCode,
				ShaderCodeDX11.PositionColorUVDX11, ShaderCodeDX9.Position3DColorUVDX9,
				VertexFormat.Position3DColorUV, Shader.Position3DColorUV);
			Create(ShaderCodeOpenGL.PositionUVLightmapVertexCode, ShaderCodeOpenGL.PositionUVLightmapFragmentCode,
				ShaderCodeDX11.UVLightmapHLSLCode, ShaderCodeDX9.DX9Position3DLightMap, 
				VertexFormat.Position3DNormalUVLightmap, Shader.Position3DNormalUVLightmap);
			Create(ShaderCodeOpenGL.ColorSkinnedVertexCode, ShaderCodeOpenGL.ColorSkinnedFragmentCode, "_",
				"_", VertexFormat.Position3DColorSkinned, Shader.Position3DColorSkinned);
			Create(ShaderCodeOpenGL.UVSkinnedVertexCode, ShaderCodeOpenGL.UVSkinnedFragmentCode, "_",
				"_", VertexFormat.Position3DUVSkinned, Shader.Position3DUVSkinned);
			Create(ShaderCodeOpenGL.PositionUVNormalOpenGLVertexCode,
				ShaderCodeOpenGL.PositionUVNormalOpenGLFragmentCode, ShaderCodeDX11.PositionNormalUVDX11,
				ShaderCodeDX9.Position3DNormalUVDX9, VertexFormat.Position3DNormalUV, Shader.Position3DNormalUV);				
		}

		private static void Create(string vertexCode, string fragmentCode, string dx11Code,
			string dx9Code, VertexFormat format, string name)
		{
			if (!Directory.Exists("Content"))
				Directory.CreateDirectory("Content");
			var data = new ShaderCreationData(vertexCode, fragmentCode, dx11Code, dx9Code, format);
			using (var file = File.Create(Path.Combine("Content", name + ".deltashader")))
				BinaryDataExtensions.Save(data, new BinaryWriter(file));
		}
	}
}