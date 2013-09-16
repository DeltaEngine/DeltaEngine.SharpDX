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
			Create(ShaderWithFormat.UvVertexCode, ShaderWithFormat.UvFragmentCode,
				ShaderWithFormat.UvHlslCode, ShaderWithFormat.Dx9Position2DTexture,
				VertexFormat.Position2DUv, Shader.Position2DUv);
			Create(ShaderWithFormat.ColorVertexCode, ShaderWithFormat.ColorFragmentCode,
				ShaderWithFormat.ColorHlslCode, ShaderWithFormat.Dx9Position2DColor,
				VertexFormat.Position2DColor, Shader.Position2DColor);
			Create(ShaderWithFormat.ColorUvVertexCode, ShaderWithFormat.ColorUvFragmentCode,
				ShaderWithFormat.ColorUvHlslCode, ShaderWithFormat.Dx9Position2DColorTexture,
				VertexFormat.Position2DColorUv, Shader.Position2DColorUv);
			Create(ShaderWithFormat.UvVertexCode, ShaderWithFormat.UvFragmentCode,
				ShaderWithFormat.UvHlslCode, ShaderWithFormat.Dx9Position3DTexture,
				VertexFormat.Position3DUv, Shader.Position3DUv);
			Create(ShaderWithFormat.ColorVertexCode, ShaderWithFormat.ColorFragmentCode,
				ShaderWithFormat.ColorHlslCode, ShaderWithFormat.Dx9Position3DColor,
				VertexFormat.Position3DColor, Shader.Position3DColor);
			Create(ShaderWithFormat.ColorUvVertexCode, ShaderWithFormat.ColorUvFragmentCode,
				ShaderWithFormat.ColorUvHlslCode, ShaderWithFormat.Dx9Position3DColorTexture,
				VertexFormat.Position3DColorUv, Shader.Position3DColorUv);
			Create(ShaderWithFormat.UvLightmapVertexCode, ShaderWithFormat.UvLightmapFragmentCode,
				ShaderWithFormat.UvLightmapHlslCode, ShaderWithFormat.Dx9Position3DLightMap, 
				VertexFormat.Position3DTexturedLightmap, Shader.Position3DTexturedLightmap);
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