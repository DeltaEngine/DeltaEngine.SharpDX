namespace DeltaEngine.Graphics
{
	/// <summary>
	/// Default source code for the DirectX 9 shaders
	/// </summary>
	public static class ShaderCodeDX9
	{
		internal const string Position3DColorDx9 =
					"float4x4 WorldViewProjection;" + "struct VS_OUTPUT" + "{" + "float4 Pos       : POSITION;" +
						"float4 Color     : COLOR0;" + "};" +
						"VS_OUTPUT VS( float3 Pos : POSITION, float4 Color : COLOR )" + "{" +
						"VS_OUTPUT output = (VS_OUTPUT)0;" +
						"output.Pos = mul(float4(Pos, 1.0f), WorldViewProjection);" +
						"output.Color = float4(Color[2], Color[1], Color[0], Color[3]);" + "return output;" + "}" +
						"float4 PS( VS_OUTPUT input ) : COLOR0" + "{" + "return input.Color;" + "}";

		internal const string Position3DColorUvDx9 =
			"float4x4 WorldViewProjection;" + "struct VS_OUTPUT" + "{" + "float4 Pos       : POSITION;" +
				"float4 Color     : COLOR0;" + "float2 TextureUV : TEXCOORD0;" + "};" +
				"VS_OUTPUT VS( float3 Pos : POSITION, float4 Color : COLOR, float2 TextureUV : TEXCOORD0 )" +
				"{" + "VS_OUTPUT output = (VS_OUTPUT)0;" +
				"output.Pos = mul(float4(Pos, 1.0f), WorldViewProjection);" +
				"output.Color = float4(Color[2], Color[1], Color[0], Color[3]);" +
				"output.TextureUV = TextureUV;" + "return output;" + "}" + "sampler DiffuseTexture;" +
				"float4 PS( VS_OUTPUT input ) : COLOR0" + "{" +
				"return tex2D(DiffuseTexture, input.TextureUV) * input.Color;" + "}";

		internal const string Position3DUvDx9 =
			"float4x4 WorldViewProjection;" + "struct VS_OUTPUT" + "{" + "float4 Pos       : POSITION;" +
				"float2 TextureUV : TEXCOORD0;" + "};" +
				"VS_OUTPUT VS( float3 Pos : POSITION, float2 TextureUV : TEXCOORD0 )" + "{" +
				"VS_OUTPUT output = (VS_OUTPUT)0;" +
				"output.Pos = mul(float4(Pos, 1.0f), WorldViewProjection);" +
				"output.TextureUV = TextureUV;" + "return output;" + "}" + "sampler DiffuseTexture;" +
				"float4 PS( VS_OUTPUT input ) : COLOR0" + "{" +
				"return tex2D(DiffuseTexture, input.TextureUV);" + "}";

		internal const string Position2DColorDx9 =
			"float4x4 WorldViewProjection;" + "struct VS_OUTPUT" + "{" + "float4 Pos       : POSITION;" +
				"float4 Color     : COLOR0;" + "};" +
				"VS_OUTPUT VS( float2 Pos : POSITION, float4 Color : COLOR )" + "{" +
				"VS_OUTPUT output = (VS_OUTPUT)0;" +
				"output.Pos = mul(float4(Pos[0], Pos[1], 0.0f, 1.0f), WorldViewProjection);" +
				"output.Color = float4(Color[2], Color[1], Color[0], Color[3]);" + "return output;" + "}" +
				"float4 PS( VS_OUTPUT input ) : COLOR0" + "{" + "return input.Color;" + "}";

		internal const string Position2DColorUvDx9 =
			"float4x4 WorldViewProjection;" + "struct VS_OUTPUT" + "{" + "float4 Pos       : POSITION;" +
				"float4 Color     : COLOR0;" + "float2 TextureUV : TEXCOORD0;" + "};" +
				"VS_OUTPUT VS( float2 Pos : POSITION, float4 Color : COLOR, float2 TextureUV : TEXCOORD0 )" +
				"{" + "VS_OUTPUT output = (VS_OUTPUT)0;" +
				"output.Pos = mul(float4(Pos[0],Pos[1], 0.0f, 1.0f), WorldViewProjection);" +
				"output.Color = float4(Color[2], Color[1], Color[0], Color[3]);" +
				"output.TextureUV = TextureUV;" + "return output;" + "}" + "sampler DiffuseTexture;" +
				"float4 PS( VS_OUTPUT input ) : COLOR0" + "{" +
				"return tex2D(DiffuseTexture, input.TextureUV) * input.Color;" + "}";

		internal const string Position2DUvDx9 =
			"float4x4 WorldViewProjection;" + "struct VS_OUTPUT" + "{" + "float4 Pos       : POSITION;" +
				"float2 TextureUV : TEXCOORD0;" + "};" +
				"VS_OUTPUT VS( float2 Pos : POSITION, float2 TextureUV : TEXCOORD0 )" + "{" +
				"VS_OUTPUT output = (VS_OUTPUT)0;" +
				"output.Pos = mul(float4(Pos[0],Pos[1], 0.0f, 1.0f), WorldViewProjection);" +
				"output.TextureUV = TextureUV;" + "return output;" + "}" + "sampler DiffuseTexture;" +
				"float4 PS( VS_OUTPUT input ) : COLOR0" + "{" +
				"return tex2D(DiffuseTexture, input.TextureUV);" + "}";

		internal const string Dx9Position3DLightMap =
			"float4x4 WorldViewProjection;" + "struct VS_OUTPUT" + "{" + "float4 Pos       : POSITION;" +
				"float2 TextureUV : TEXCOORD0;" + "float2 LightMapUV: TEXCOORD1;" + "};" +
				"VS_OUTPUT VS( float3 Pos : POSITION, float2 TextureUV : TEXCOORD0, float2 LightMapUV : TEXCOORD1 )" +
				"{" + "VS_OUTPUT output = (VS_OUTPUT)0;" +
				"output.Pos = mul(float4(Pos, 1.0f), WorldViewProjection);" +
				"output.TextureUV = TextureUV;" + "output.LightMapUV = LightMapUV;" + "return output;" +
				"}" + "sampler DiffuseTexture : register(s0);" + "sampler LightmapTexture : register(s1);" +
				"float4 PS( VS_OUTPUT input ) : COLOR0" + "{" +
				"return tex2D(DiffuseTexture, input.TextureUV) * tex2D(LightmapTexture, input.LightMapUV);" +
				"}";
	}
}