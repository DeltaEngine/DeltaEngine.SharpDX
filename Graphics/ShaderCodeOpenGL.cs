namespace DeltaEngine.Graphics
{
	/// <summary>
	/// Default source code for the OpenGL shaders
	/// </summary>
	 
	public static class ShaderCodeOpenGL
	{
internal const string PositionUvOpenGLVertexCode = @"uniform mat4 ModelViewProjection;
attribute vec4 aPosition;
attribute vec2 aTextureUV;
varying vec2 vTexcoord;
void main()
{
	vTexcoord = aTextureUV;
	gl_Position = ModelViewProjection * aPosition;
}";

		internal const string PositionUvOpenGLFragmentCode = @"precision mediump float;
uniform sampler2D Texture;
varying vec2 vTexcoord;
void main()
{
	gl_FragColor = texture2D(Texture, vTexcoord);
}";

		internal const string PositionColorOpenGLVertexCode = @"uniform mat4 ModelViewProjection;
attribute vec4 aPosition;
attribute vec4 aColor;
varying vec4 diffuseColor;
void main()
{
	gl_Position = ModelViewProjection * aPosition;
	diffuseColor = aColor;
}";

		internal const string PositionColorOpenGLFragmentCode = @"precision mediump float;
varying vec4 diffuseColor;
void main()
{
	gl_FragColor = diffuseColor;
}";

		internal const string PositionColorUvOpenGLVertexCode = @"uniform mat4 ModelViewProjection;
attribute vec4 aPosition;
attribute vec4 aColor;
attribute vec2 aTextureUV;
varying vec4 diffuseColor;
varying vec2 diffuseTexCoord;
void main()
{
	gl_Position = ModelViewProjection * aPosition;
	diffuseColor = aColor;
	diffuseTexCoord = aTextureUV;
}";

		internal const string PositionColorUvOpenGLFragmentCode = @"precision mediump float;
uniform sampler2D Texture;
varying vec4 diffuseColor;
varying vec2 diffuseTexCoord;
void main()
{
	gl_FragColor = texture2D(Texture, diffuseTexCoord) * diffuseColor;
}";

		internal const string PositionUvLightmapVertexCode = @"uniform mat4 ModelViewProjection;
attribute vec4 aPosition;
attribute vec2 aTextureUV;
attribute vec2 aLightMapUV;
varying vec2 vDiffuseTexCoord;
varying vec2 vLightMapTexCoord;
void main()
{
	gl_Position = ModelViewProjection * aPosition;
	vDiffuseTexCoord = aTextureUV;
	vLightMapTexCoord = aLightMapUV;
}";

		internal const string PositionUvLightmapFragmentCode = @"precision mediump float;
uniform sampler2D Texture;
uniform sampler2D Lightmap;
varying vec2 vDiffuseTexCoord;
varying vec2 vLightMapTexCoord;
void main()
{
	gl_FragColor = texture2D(Texture, vDiffuseTexCoord) * texture2D(Lightmap, vLightMapTexCoord);
}";

		internal const string ColorSkinnedVertexCode = @"uniform mat4 ModelViewProjection;
uniform mat4 JointTransforms[20]; 
attribute vec4 aPosition;
attribute vec4 aColor;
attribute vec2 aSkinIndices;
attribute vec2 aSkinWeights;
varying vec4 diffuseColor;
void main()
{
	vec4 skinnedPosition = vec4(0.0, 0.0, 0.0, 0.0);
	for (int jointIndex = 0; jointIndex < 2; ++jointIndex)
	{
		int index = int(aSkinIndices[jointIndex]);
		float weight = aSkinWeights[jointIndex];
		skinnedPosition += (JointTransforms[index] * aPosition) * weight;
	} 
	gl_Position = ModelViewProjection * skinnedPosition;
	diffuseColor = aColor;
}";

		internal const string ColorSkinnedFragmentCode = PositionColorOpenGLFragmentCode;

		internal const string UvSkinnedVertexCode = @"uniform mat4 ModelViewProjection;
uniform mat4 JointTransforms[20];
attribute vec4 aPosition;
attribute vec2 aTextureUV;
attribute vec2 aSkinIndices;
attribute vec2 aSkinWeights;
varying vec2 vTexcoord;
void main()
{
	vec4 skinnedPosition = vec4(0.0, 0.0, 0.0, 0.0);
	for (int jointIndex = 0; jointIndex < 2; ++jointIndex)
	{
		int index = int(aSkinIndices[jointIndex]);
		float weight = aSkinWeights[jointIndex];
		skinnedPosition += (JointTransforms[index] * aPosition) * weight;
	} 
	gl_Position = ModelViewProjection * skinnedPosition;
	vTexcoord = aTextureUV;
}";

		internal const string UvSkinnedFragmentCode = PositionUvOpenGLFragmentCode;


		internal const string PositionUvNormalOpenGLVertexCode = @"uniform mat4 ModelViewProjection;
uniform vec4 viewPosition;
uniform vec4 lightPosition;
attribute vec4 aPosition;
attribute vec4 aNormal;
attribute vec2 aTextureUV;
varying vec2 vTexCoord;
varying vec3 vNormal;
varying vec3 vLightVec;
varying vec3 vCameraVec;
void main()
{
	vNormal = aNormal;
	vCameraVec = normalize(viewPosition.xyz - aPosition.xyz);
	vLightVec = lightPosition.xyz - aPosition.xyz;	
	gl_Position = ModelViewProjection * aPosition;
	vTexCoord = aTextureUV;	
}";

		internal const string PositionUvNormalOpenGLFragmentCode = @"precision mediump float;
uniform sampler2D Texture;
varying vec2 vTexCoord;
varying vec3 vNormal;
varying vec3 vLightVec;
varying vec3 vCameraVec;
const float MAX_DIST = 2.5;
const float MAX_DIST_SQUARED = MAX_DIST * MAX_DIST;
void main()
{
	vec3 normal = normalize(vNormal);
	vec3 cameraDir = normalize(vCameraVec);   
	float dist = min(dot(vLightVec, vLightVec), MAX_DIST_SQUARED) / MAX_DIST_SQUARED;  
	vec3 lightDir = normalize(vLightVec);
	vec4 diffuse = (max(0.0, dot(normal, lightDir)) * dist) * vec4(1.0f, 1.0f, 1.0f, 1.0f); 
	vec3 halfAngle = normalize(cameraDir + lightDir);
	float specularDot = dot(normal, halfAngle);
	vec4 specular = (pow(clamp(specularDot, 0.0, 1.0), 2.0) * dist) * vec4(1.0f, 1.0f, 1.0f, 1.0f);
	gl_FragColor = texture2D(Texture, vTexCoord);// * diffuse + specular;
}";

	}
}