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
			Create(ShaderCodeOpenGL.PositionUvOpenGLVertexCode, ShaderCodeOpenGL.PositionUvOpenGLFragmentCode,
				ShaderCodeDX11.PositionUvDx11, ShaderCodeDX9.Position2DUvDx9,
				VertexFormat.Position2DUv, Shader.Position2DUv);
			Create(ShaderCodeOpenGL.PositionColorOpenGLVertexCode, ShaderCodeOpenGL.PositionColorOpenGLFragmentCode,
				ShaderCodeDX11.PositionColorDx11, ShaderCodeDX9.Position2DColorDx9,
				VertexFormat.Position2DColor, Shader.Position2DColor);
			Create(ShaderCodeOpenGL.PositionColorUvOpenGLVertexCode, ShaderCodeOpenGL.PositionColorUvOpenGLFragmentCode,
				ShaderCodeDX11.PositionColorUvDx11, ShaderCodeDX9.Position2DColorUvDx9,
				VertexFormat.Position2DColorUv, Shader.Position2DColorUv);
			Create(ShaderCodeOpenGL.PositionUvOpenGLVertexCode, ShaderCodeOpenGL.PositionUvOpenGLFragmentCode,
				ShaderCodeDX11.PositionUvDx11, ShaderCodeDX9.Position3DUvDx9,
				VertexFormat.Position3DUv, Shader.Position3DUv);
			Create(ShaderCodeOpenGL.PositionColorOpenGLVertexCode, ShaderCodeOpenGL.PositionColorOpenGLFragmentCode,
				ShaderCodeDX11.PositionColorDx11, ShaderCodeDX9.Position3DColorDx9,
				VertexFormat.Position3DColor, Shader.Position3DColor);
			Create(ShaderCodeOpenGL.PositionColorUvOpenGLVertexCode, ShaderCodeOpenGL.PositionColorUvOpenGLFragmentCode,
				ShaderCodeDX11.PositionColorUvDx11, ShaderCodeDX9.Position3DColorUvDx9,
				VertexFormat.Position3DColorUv, Shader.Position3DColorUv);
			Create(ShaderCodeOpenGL.PositionUvLightmapVertexCode, ShaderCodeOpenGL.PositionUvLightmapFragmentCode,
				ShaderCodeDX11.UvLightmapHlslCode, ShaderCodeDX9.Dx9Position3DLightMap, 
				VertexFormat.Position3DTexturedLightmap, Shader.Position3DTexturedLightmap);
			Create(ShaderCodeOpenGL.ColorSkinnedVertexCode, ShaderCodeOpenGL.ColorSkinnedFragmentCode, "_",
				"_", VertexFormat.Position3DColorSkinned, Shader.Position3DColorSkinned);
			Create(ShaderCodeOpenGL.UvSkinnedVertexCode, ShaderCodeOpenGL.UvSkinnedFragmentCode, "_",
				"_", VertexFormat.Position3DUvSkinned, Shader.Position3DUvSkinned);
			Create(ShaderCodeOpenGL.PositionUvNormalOpenGLVertexCode,
				ShaderCodeOpenGL.PositionUvNormalOpenGLFragmentCode, ShaderCodeDX11.PositionNormalUvDx11,
				ShaderCodeDX9.Position3DNormalUvDx9, VertexFormat.Position3DNormalUv, Shader.Position3DNormalUv);				
		}

		private static void Create(string vertexCode, string fragmentCode, string dx11Code,
			string dx9Code, VertexFormat format, string name)
		{
			var data = new ShaderCreationData(vertexCode, fragmentCode, dx11Code, dx9Code, format);
			using (var file = File.Create(Path.Combine("Content", name + ".deltashader")))
				BinaryDataExtensions.Save(data, new BinaryWriter(file));
		}
	}
}