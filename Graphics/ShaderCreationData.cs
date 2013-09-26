using DeltaEngine.Content;
using DeltaEngine.Graphics.Vertices;

namespace DeltaEngine.Graphics
{
	/// <summary>
	/// Creates a shader directly from vertex and fragment shader code for OpenGL frameworks plus
	/// Hlsl code for DirectX frameworks. If you only provide shader code for a specific framework, 
	/// it breaks multiplatform compatibility. Use this only for testing and use content normally. 
	/// </summary>
	public class ShaderCreationData : ContentCreationData
	{
		private ShaderCreationData() {} //ncrunch: no coverage

		public ShaderCreationData(string vertexCode, string fragmentCode, string dx11Code,
			string dx9Code, VertexFormat format)
		{
			VertexCode = vertexCode;
			FragmentCode = fragmentCode;
			Dx11Code = dx11Code;
			Dx9Code = dx9Code;
			Format = format;
		}
		 
		public string VertexCode { get; private set; }
		public string FragmentCode { get; private set; }
		public string Dx11Code { get; private set; }
		public string Dx9Code { get; private set; }
		public VertexFormat Format { get; private set; }
	}
}