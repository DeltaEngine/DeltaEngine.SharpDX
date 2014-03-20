using System.Text;
using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics.Vertices;
using SlimDX;
using SlimDX.Direct3D9;
using Matrix = DeltaEngine.Datatypes.Matrix;
using SlimD3D9 = SlimDX.Direct3D9;
using SlimD3D11 = SlimDX.Direct3D11;
using VertexElementD3D9 = SlimDX.Direct3D9.VertexElement;

namespace DeltaEngine.Graphics.SlimDX
{
	/// <summary>
	/// SlimDX graphics shader.
	/// </summary>
	public class SlimDXShader : ShaderWithFormat
	{
		private SlimDXShader(ShaderWithFormatCreationData creationData, SlimDXDevice device)
			: this((ShaderCreationData)creationData, device) {}

		public SlimDXShader(ShaderCreationData customShader, SlimDXDevice device)
			: base(customShader)
		{
			this.device = device;
			TryCreateShader();
		}

		private readonly SlimDXDevice device;
		private SlimD3D9.Device NativeDevice
		{
			get { return device.NativeDevice; }
		}

		protected override sealed void CreateShader()
		{
			CreateVertexDeclaration();
			CreateVertexShader();
			CreatePixelShader();	
		}

		private void CreateVertexDeclaration()
		{
			int elementIndex = 0;
			var vertexElements = new VertexElementD3D9[Format.Elements.Length + 1];
			foreach (var vertexElement in Format.Elements)
				if (vertexElement.ElementType == VertexElementType.Position3D)
					vertexElements[elementIndex++] = GetVertexPosition3D(vertexElement.Offset);
				else if (vertexElement.ElementType == VertexElementType.Position2D)
					vertexElements[elementIndex++] = GetVertexPosition2D(vertexElement.Offset);
				else if (vertexElement.ElementType == VertexElementType.Normal)
					vertexElements[elementIndex++] = GetVertexNormal(vertexElement.Offset);
				else if (vertexElement.ElementType == VertexElementType.Color)
					vertexElements[elementIndex++] = GetVertexColor(vertexElement.Offset);
				else if (vertexElement.ElementType == VertexElementType.TextureUV)
					vertexElements[elementIndex++] = GetVertexTextureCoordinate(vertexElement.Offset);
				else if (vertexElement.ElementType == VertexElementType.LightMapUV)
					vertexElements[elementIndex++] = GetVertexLightMapUV(vertexElement.Offset);
				else if (vertexElement.ElementType == VertexElementType.SkinIndices)
					vertexElements[elementIndex++] = GetVertexSkindIndices(vertexElement.Offset);
				else if (vertexElement.ElementType == VertexElementType.SkinWeights)
					vertexElements[elementIndex++] = GetVertexSkindWeights(vertexElement.Offset);	
			vertexElements[elementIndex] = VertexElementD3D9.VertexDeclarationEnd;
			vertexDeclaration = new VertexDeclaration(NativeDevice, vertexElements);
		}

		private VertexDeclaration vertexDeclaration;

		private static VertexElementD3D9 GetVertexPosition3D(int offset)
		{
			return new VertexElementD3D9(0, (short)offset, DeclarationType.Float3,
				DeclarationMethod.Default, DeclarationUsage.Position, 0);
		}

		private static VertexElementD3D9 GetVertexNormal(int offset)
		{
			return new VertexElementD3D9(0, (short)offset, DeclarationType.Float3,
				DeclarationMethod.Default, DeclarationUsage.Normal, 0);
		}

		private static VertexElementD3D9 GetVertexPosition2D(int offset)
		{
			return new VertexElementD3D9(0, (short)offset, DeclarationType.Float2,
				DeclarationMethod.Default, DeclarationUsage.Position, 0);
		}

		private static VertexElementD3D9 GetVertexColor(int offset)
		{
			return new VertexElementD3D9(0, (short)offset, DeclarationType.Color,
				DeclarationMethod.Default, DeclarationUsage.Color, 0);
		}

		private static VertexElementD3D9 GetVertexTextureCoordinate(int offset)
		{
			return new VertexElementD3D9(0, (short)offset, DeclarationType.Float2,
				DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0);
		}

		private static VertexElementD3D9 GetVertexLightMapUV(int offset)
		{
			return new VertexElementD3D9(0, (short)offset, DeclarationType.Float2,
				DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 1);
		}

		private static VertexElementD3D9 GetVertexSkindIndices(int offset)
		{
			return new VertexElementD3D9(0, (short)offset, DeclarationType.Float2,
				DeclarationMethod.Default, DeclarationUsage.BlendIndices, 0);
		}

		private static VertexElementD3D9 GetVertexSkindWeights(int offset)
		{
			return new VertexElementD3D9(0, (short)offset, DeclarationType.Float2,
				DeclarationMethod.Default, DeclarationUsage.BlendWeight, 0);
		}

		private void CreateVertexShader()
		{
			byte[] shaderCodeInBytes = Encoding.UTF8.GetBytes(DX9Code);
			var byteCode = ShaderBytecode.Compile(shaderCodeInBytes, "VS",
				"vs_2_0", SlimD3D9.ShaderFlags.None);
			vertexShader = new VertexShader(NativeDevice, byteCode);
		}

		private VertexShader vertexShader;
		
		private void CreatePixelShader()
		{
			var byteCode = ShaderBytecode.Compile(Encoding.UTF8.GetBytes(DX9Code), "PS",
				"ps_2_0", SlimD3D9.ShaderFlags.None);
			pixelShader = new PixelShader(NativeDevice, byteCode);
		}

		private PixelShader pixelShader;

		public override void SetModelViewProjection(Matrix matrix)
		{
			vertexShaderCurrentRegister = 0;
			pixelShaderCurrentRegister = 0;
			SetVertexShaderConstant(Matrix.Transpose(matrix));
		}

		private int vertexShaderCurrentRegister;
		private int pixelShaderCurrentRegister;

		private void SetVertexShaderConstant(Matrix matrix)
		{
			NativeDevice.SetVertexShaderConstant(vertexShaderCurrentRegister, matrix.GetValues);
			IncrementVertexShaderRegister(16);
		}

		private void IncrementVertexShaderRegister(int numberOfFloats)
		{
			vertexShaderCurrentRegister += numberOfFloats / 4;
		}

		public override void SetModelViewProjection(Matrix model, Matrix view, Matrix projection)
		{
			vertexShaderCurrentRegister = 0;
			pixelShaderCurrentRegister = 0;
			worldMatrix = model;
			viewMatrix = view;
			worldViewProjectionMatrix = model * view * projection;
			SetVertexShaderConstant(Matrix.Transpose(worldViewProjectionMatrix));
		}

		private Matrix worldMatrix;
		private Matrix viewMatrix;
		private Matrix worldViewProjectionMatrix;

		public override void SetJointMatrices(Matrix[] jointMatrices)
		{
			for (int i = 0; i < 32; i++)
			{
				Matrix m = i < jointMatrices.Length ? jointMatrices[i] : Matrix.Identity;
				SetVertexShaderConstant(Matrix.Transpose(m));
			}
		}

		public override void SetDiffuseTexture(Image texture)
		{
			(texture as SlimDXImage).SetTextureAndSamplerState(0);
		}

		public override void SetLightmapTexture(Image texture)
		{
			(texture as SlimDXImage).SetTextureAndSamplerState(1);	
		}

		public override void SetSunLight(SunLight light)
		{
			SetVertexShaderConstant(Matrix.Transpose(worldMatrix));
			//SetVertexShaderConstant(viewMatrix);
			//SetVertexShaderConstant(Matrix.Invert(worldMatrix * viewMatrix));
			SetVertexShaderConstant(light.Direction);
			SetVertexShaderConstant(light.Color);
		}

		private void SetVertexShaderConstant(Vector3D vector, bool isVector = true)
		{
			var dxVector = new Vector4(vector.X, vector.Y, vector.Z, isVector ? 0.0f : 1.0f);
			NativeDevice.SetVertexShaderConstant(vertexShaderCurrentRegister, new[] { dxVector });
			IncrementVertexShaderRegister(4);
		}

		private void SetPixelShaderConstant(Color color)
		{
			var dxVector = new Vector4(color.RedValue, color.GreenValue, color.BlueValue, color.AlphaValue);
			NativeDevice.SetPixelShaderConstant(pixelShaderCurrentRegister, new[] { dxVector });
			IncrementPixelShaderRegister(4);
		}

		private void IncrementPixelShaderRegister(int numberOfFloats)
		{
			pixelShaderCurrentRegister += numberOfFloats / 4;
		}

		private void SetPixelShaderConstants(float value0, float value1, float value2, float value3)
		{
			var dxVector = new Vector4(value0, value1, value2, value3);
			NativeDevice.SetPixelShaderConstant(pixelShaderCurrentRegister, new[] { dxVector });
			IncrementPixelShaderRegister(4);			
		}

		public override void Bind()
		{
			NativeDevice.SetTexture(0, null);
			NativeDevice.VertexShader = vertexShader;
			NativeDevice.PixelShader = pixelShader;
			NativeDevice.VertexDeclaration = vertexDeclaration;
		}

		public override void BindVertexDeclaration() { }

		public override void ApplyFogSettings(FogSettings fogSettings)
		{
			SetVertexShaderConstant(Matrix.Transpose(worldMatrix));
			// SlimDX optimizes the World matrix to 4x3 we only need for our world position a float3
			IncrementVertexShaderRegister(-4);
			SetVertexShaderConstant(device.CameraInvertedViewMatrix.Translation, false);
			SetVertexShaderConstant(fogSettings.FogColor);
			SetVertexShaderConstants(fogSettings.FogStart, fogSettings.FogEnd, 0.0f, 0.0f);
		}

		private void SetVertexShaderConstant(Color color)
		{
			var dxVector = new Vector4(color.RedValue, color.GreenValue, color.BlueValue, color.AlphaValue);
			NativeDevice.SetVertexShaderConstant(vertexShaderCurrentRegister, new[] { dxVector });
			IncrementVertexShaderRegister(4);
		}

		private void SetVertexShaderConstants(float value0, float value1, float value2, float value3)
		{
			var dxVector = new Vector4(value0, value1, value2, value3);
			NativeDevice.SetVertexShaderConstant(vertexShaderCurrentRegister, new[] { dxVector });
			IncrementVertexShaderRegister(4);
		}

		protected override void DisposeData()
		{
			vertexDeclaration.Dispose();
			pixelShader.Dispose();
			vertexShader.Dispose();
		}
	}
}