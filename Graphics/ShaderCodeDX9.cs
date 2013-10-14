namespace DeltaEngine.Graphics
{
	/// <summary>
	/// Default source code for the DirectX 9 shaders
	/// </summary>
	public static class ShaderCodeDX9
	{
		public const string Position3DColorDX9 =  @"
float4x4 WorldViewProjection;
struct VS_OUTPUT
{
	float4 Pos       : POSITION;
	float4 Color     : COLOR0;
};

VS_OUTPUT VS( float3 Pos : POSITION, float4 Color : COLOR )
{ 
	VS_OUTPUT output = (VS_OUTPUT)0; 
	output.Pos = mul(float4(Pos, 1.0f), WorldViewProjection); 
	output.Color = float4(Color[2], Color[1], Color[0], Color[3]);  
	return output;  
} 

float4 PS( VS_OUTPUT input ) : COLOR0  
{  
	return input.Color;  
}";

		public const string Position3DColorUVDX9 = @"
float4x4 WorldViewProjection;
struct VS_OUTPUT  
{  
	float4 Pos       : POSITION;
	float4 Color     : COLOR0;  
	float2 TextureUV : TEXCOORD0;  
};
 
VS_OUTPUT VS( float3 Pos : POSITION, float4 Color : COLOR, float2 TextureUV : TEXCOORD0 ) 
{ 
	VS_OUTPUT output = (VS_OUTPUT)0;
	output.Pos = mul(float4(Pos, 1.0f), WorldViewProjection);
	output.Color = float4(Color[2], Color[1], Color[0], Color[3]);
	output.TextureUV = TextureUV;  
	return output;  
}

sampler DiffuseTexture; 
float4 PS( VS_OUTPUT input ) : COLOR0  
{
	return tex2D(DiffuseTexture, input.TextureUV) * input.Color;  
}";

		public const string Position3DUVDX9 = @"
float4x4 WorldViewProjection;
struct VS_OUTPUT  
{  
	float4 Pos				: POSITION; 
	float2 TextureUV : TEXCOORD0;  
};

VS_OUTPUT VS( float3 Pos : POSITION, float2 TextureUV : TEXCOORD0 )  
{ 
	VS_OUTPUT output = (VS_OUTPUT)0; 
	output.Pos = mul(float4(Pos, 1.0f), WorldViewProjection); 
	output.TextureUV = TextureUV;  
	return output;  
}

sampler DiffuseTexture;
float4 PS( VS_OUTPUT input ) : COLOR0  
{
	return tex2D(DiffuseTexture, input.TextureUV);  
}";

		public const string Position2DColorDX9 = @"
float4x4 WorldViewProjection;
struct VS_OUTPUT  
{  
	float4 Pos       : POSITION; 
	float4 Color     : COLOR0;  
}; 

VS_OUTPUT VS( float2 Pos : POSITION, float4 Color : COLOR )  
{ 
	VS_OUTPUT output = (VS_OUTPUT)0; 
	output.Pos = mul(float4(Pos[0], Pos[1], 0.0f, 1.0f), WorldViewProjection); 
	output.Color = float4(Color[2], Color[1], Color[0], Color[3]);  
	return output;  
}
 
float4 PS( VS_OUTPUT input ) : COLOR0  
{  
	return input.Color;  
}";

		public const string Position2DColorUVDX9 = @"
float4x4 WorldViewProjection;  struct VS_OUTPUT  
{  
	float4 Pos       : POSITION;
	float4 Color     : COLOR0;  
	float2 TextureUV : TEXCOORD0;  
}; 

VS_OUTPUT VS( float2 Pos : POSITION, float4 Color : COLOR, float2 TextureUV : TEXCOORD0 ) 
{
	VS_OUTPUT output = (VS_OUTPUT)0;
	output.Pos = mul(float4(Pos[0],Pos[1], 0.0f, 1.0f), WorldViewProjection);
	output.Color = float4(Color[2], Color[1], Color[0], Color[3]);
	output.TextureUV = TextureUV;  
	return output;  
}

sampler DiffuseTexture;
float4 PS( VS_OUTPUT input ) : COLOR0  
{ 
	return tex2D(DiffuseTexture, input.TextureUV) * input.Color;  
}";

		public const string Position2DUVDX9 = @"
float4x4 WorldViewProjection;  
struct VS_OUTPUT  
{  
	float4 Pos       : POSITION;
	float2 TextureUV : TEXCOORD0;  
}; 

VS_OUTPUT VS( float2 Pos : POSITION, float2 TextureUV : TEXCOORD0 )  
{ 
	VS_OUTPUT output = (VS_OUTPUT)0; 
	output.Pos = mul(float4(Pos[0],Pos[1], 0.0f, 1.0f), WorldViewProjection); 
	output.TextureUV = TextureUV;  
	return output;  
}  

sampler DiffuseTexture; 
float4 PS( VS_OUTPUT input ) : COLOR0  
{ 
	return tex2D(DiffuseTexture, input.TextureUV);  
}";

		public const string DX9Position3DLightMap = @"
float4x4 WorldViewProjection;  
struct VS_OUTPUT  
{  
	float4 Pos       : POSITION;
	float2 TextureUV : TEXCOORD0;
	float2 LightMapUV: TEXCOORD1;  
}; 

VS_OUTPUT VS( float3 Pos : POSITION, float2 TextureUV : TEXCOORD0, float2 LightMapUV : TEXCOORD1 ) 
{  
	VS_OUTPUT output = (VS_OUTPUT)0;
	output.Pos = mul(float4(Pos, 1.0f), WorldViewProjection); 
	output.TextureUV = TextureUV;  
	output.LightMapUV = LightMapUV;  
	return output; 
}  

sampler DiffuseTexture : register(s0);  
sampler LightmapTexture : register(s1);
float4 PS( VS_OUTPUT input ) : COLOR0  
{ 
	return tex2D(DiffuseTexture, input.TextureUV) * tex2D(LightmapTexture, input.LightMapUV); 
}";

		public const string Position3DNormalUVDX9 = @"
float4x4 WorldViewProjection;
float4 ViewPosition;
float4 LightPosition;
struct VS_OUTPUT  
{  
	float4 Pos       : POSITION;
	float3 Normal		 : TEXCOORD0;	
	float3 View			 : TEXCOORD1;
	float3 Light		 : TEXCOORD2;
	float2 TextureUV : TEXCOORD4;
}; 

VS_OUTPUT VS( float3 Pos : POSITION, float3 Normal : NORMAL, float2 TextureUV : TEXCOORD0 ) 
{  
	VS_OUTPUT output = (VS_OUTPUT)0;
	output.Pos = mul(float4(Pos, 1.0f), WorldViewProjection); 
	output.Normal = Normal;
	output.View = normalize(ViewPosition.xyz - Pos);
	output.Light = LightPosition.xyz - Pos;
	output.TextureUV = TextureUV;	
	return output; 
}  

sampler DiffuseTexture;
float4 PS( VS_OUTPUT input ) : COLOR0  
{ 
	return tex2D(DiffuseTexture, input.TextureUV);
}";
	}
}