namespace DeltaEngine.Graphics
{
	/// <summary>
	/// Default source code for the DirectX 11 shaders
	/// </summary>
	public static class ShaderCodeDX11
	{
		public const string UVLightmapHLSLCode = @"
struct VertexInputType
{
	float4 position : SV_POSITION;
	float2 texCoord : TEXCOORD0;
	float2 lightMapUV : TEXCOORD1;
};
struct PixelInputType
{
	float4 position : SV_POSITION;
	float2 texCoord : TEXCOORD0;
	float2 lightMapUV : TEXCOORD1;
};
float4x4 WorldViewProjection;

PixelInputType VsMain(VertexInputType input)
{
	PixelInputType output;
	output.position = mul(input.position, WorldViewProjection);
	output.texCoord = input.texCoord;
	output.lightMapUV = input.lightMapUV;
	return output;
}

Texture2D DiffuseTexture : register(t0);
Texture2D Lightmap : register(t1);
SamplerState TextureSamplerState : register(s0);

float4 PsMain(PixelInputType input) : SV_TARGET
{
	return DiffuseTexture.Sample(TextureSamplerState, input.texCoord) *
		Lightmap.Sample(TextureSamplerState, input.lightMapUV);
}";

		public const string PositionUVDX11 = @"
struct VertexInputType
{
	float4 position : SV_POSITION;
	float2 texCoord : TEXCOORD;
};
struct PixelInputType
{
	float4 position : SV_POSITION;
	float2 texCoord : TEXCOORD;
};
float4x4 WorldViewProjection;

PixelInputType VsMain(VertexInputType input)
{
	PixelInputType output;
	output.position = mul(input.position, WorldViewProjection);
	output.texCoord = input.texCoord;
	return output;
}

Texture2D DiffuseTexture : register(t0);
SamplerState TextureSamplerState : register(s0);

float4 PsMain(PixelInputType input) : SV_TARGET
{
	return DiffuseTexture.Sample(TextureSamplerState, input.texCoord);
}";

		public const string PositionColorDX11 = @"
struct VertexInputType
{
	float4 position : SV_POSITION;
	float4 color : COLOR;
};
struct PixelInputType
{
	float4 position : SV_POSITION;
	float4 color : COLOR;
};
float4x4 WorldViewProjection;

PixelInputType VsMain(VertexInputType input)
{
	PixelInputType output;
	output.position = mul(input.position, WorldViewProjection);
	output.color = input.color;
	return output;
}

float4 PsMain(PixelInputType input) : SV_TARGET
{
	return input.color;
}";

		public const string PositionColorUVDX11 = @"
struct VertexInputType
{
	float4 position : SV_POSITION;
	float4 color : COLOR;
	float2 texCoord : TEXCOORD;
};
struct PixelInputType
{
	float4 position : SV_POSITION;
	float4 color : COLOR;
	float2 texCoord : TEXCOORD;
};
float4x4 WorldViewProjection;

PixelInputType VsMain(VertexInputType input)
{
	PixelInputType output;
	output.position = mul(input.position, WorldViewProjection);
	output.color = input.color;
	output.texCoord = input.texCoord;
	return output;
}

Texture2D DiffuseTexture : register(t0);
SamplerState TextureSamplerState : register(s0);

float4 PsMain(PixelInputType input) : SV_TARGET
{
	return DiffuseTexture.Sample(TextureSamplerState, input.texCoord) * input.color;
}";

		public const string PositionNormalUVDX11 = @"
cbuffer cbVSPerFrame
 {
	 float4 LightPosition;
	 float4 ViewPosition; 
 };
struct VertexInputType
{
	float4 position : SV_POSITION;
	float4 normal : NORMAL;
	float2 texCoord : TEXCOORD;
};
struct PixelInputType
{
	float4 position : SV_POSITION;
	float4 normal : NORMAL;
	float2 texCoord : TEXCOORD;
	float3 lightPos : POSITION1;
	float3 viewPos : POSITION2;
};
float4x4 WorldViewProjection;

PixelInputType VsMain(VertexInputType input)
{
	PixelInputType output;
	output.position = mul(input.position, WorldViewProjection);
	output.normal = input.normal;
	output.viewPos = normalize(ViewPosition.xyz - input.position);
	output.lightPos = LightPosition.xyz - input.position;
	output.texCoord = input.texCoord;
	return output;
}

Texture2D DiffuseTexture : register(t0);
SamplerState TextureSamplerState : register(s0);

float4 PsMain(PixelInputType input) : SV_TARGET
{
	return DiffuseTexture.Sample(TextureSamplerState, input.texCoord);
}";
	}
}