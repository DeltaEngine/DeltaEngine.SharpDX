namespace DeltaEngine.Graphics
{
	/// <summary>
	/// Default source code for the DirectX 11 shaders
	/// </summary>
	public static class ShaderCodeDX11
	{
	

		internal const string UvLightmapHlslCode = @"
struct VertexInputType
{
	float4 position : SV_POSITION;
	float2 texCoord : TEXCOORD0;
	float2 lightMapUv : TEXCOORD1;
};

struct PixelInputType
{
	float4 position : SV_POSITION;
	float2 texCoord : TEXCOORD0;
	float2 lightMapUv : TEXCOORD1;
};

float4x4 WorldViewProjection;

PixelInputType VsMain(VertexInputType input)
{
	PixelInputType output;
	output.position = mul(input.position, WorldViewProjection);
	output.texCoord = input.texCoord;
	output.lightMapUv = input.lightMapUv;
	return output;
}

Texture2D DiffuseTexture : register(t0);
Texture2D Lightmap : register(t1);
SamplerState TextureSamplerState : register(s0);

float4 PsMain(PixelInputType input) : SV_TARGET
{
	return DiffuseTexture.Sample(TextureSamplerState, input.texCoord) *
		Lightmap.Sample(TextureSamplerState, input.lightMapUv);
}";

		internal const string PositionUvDx11 = @"
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

		internal const string PositionColorDx11 = @"
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

		internal const string PositionColorUvDx11 = @"
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
	}
}