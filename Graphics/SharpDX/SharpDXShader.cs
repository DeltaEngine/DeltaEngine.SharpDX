using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics.Vertices;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;
using DXDevice = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using Matrix = DeltaEngine.Datatypes.Matrix;
using ShaderFlags = DeltaEngine.Content.ShaderFlags;
using SharpDXFormat = SharpDX.DXGI.Format;

namespace DeltaEngine.Graphics.SharpDX
{
	/// <summary>
	/// Base simple SharpDX shader.
	/// </summary>
	public class SharpDXShader : ShaderWithFormat
	{
		private SharpDXShader(ShaderWithFormatCreationData creationData, SharpDXDevice device)
			: this((ShaderCreationData)creationData, device) {}

		public SharpDXShader(ShaderCreationData customShader, SharpDXDevice device)
			: base(customShader)
		{
			this.device = device;
			nativeDevice = device.NativeDevice;
			context = device.NativeDevice.ImmediateContext;
			TryCreateShader();
		}

		private readonly SharpDXDevice device;
		private readonly DXDevice nativeDevice;
		private readonly DeviceContext context;

		protected override sealed void CreateShader()
		{
			CreateVertexShader();
			CreatePixelShader();
			CreateVertexDeclaration();
			CreateShaderValuesWithBufferMustAlwaysBe16BytesAligned();
			CreateSamplerStates();
		}

		private void CreateVertexShader()
		{
			compiledVertexShader = ShaderBytecode.Compile(DX11Code, "VsMain", "vs_4_0");
			vertexShaderBytecode = compiledVertexShader.Bytecode;
			vertexShader = new VertexShader(nativeDevice, compiledVertexShader.Bytecode);
		}

		private CompilationResult compiledVertexShader;
		private byte[] vertexShaderBytecode;
		private VertexShader vertexShader;

		private void CreatePixelShader()
		{
			compiledPixelShader = ShaderBytecode.Compile(DX11Code, "PsMain", "ps_4_0");
			pixelShader = new PixelShader(nativeDevice, compiledPixelShader.Bytecode);
		}

		private CompilationResult compiledPixelShader;
		private PixelShader pixelShader;

		private void CreateVertexDeclaration()
		{
			int elementIndex = 0;
			var vertexElements = new InputElement[Format.Elements.Length];
			foreach (var vertexElement in Format.Elements)
				if (vertexElement.ElementType == VertexElementType.Position3D)
					vertexElements[elementIndex++] = GetVertexPosition3D(vertexElement.Offset);
				else if (vertexElement.ElementType == VertexElementType.Position2D)
					vertexElements[elementIndex++] = GetVertexPosition2D(vertexElement.Offset);
				else if (vertexElement.ElementType == VertexElementType.Color)
					vertexElements[elementIndex++] = GetVertexColor(vertexElement.Offset);
				else if (vertexElement.ElementType == VertexElementType.TextureUV)
					vertexElements[elementIndex++] = GetVertexTextureCoordinate(vertexElement.Offset);
				else if (vertexElement.ElementType == VertexElementType.LightMapUV)
					vertexElements[elementIndex++] = GetVertexLightMapUV(vertexElement.Offset);
				else if (vertexElement.ElementType == VertexElementType.Normal)
					vertexElements[elementIndex++] = GetVertexNormal3D(vertexElement.Offset);
				else if (vertexElement.ElementType == VertexElementType.SkinIndices)
					vertexElements[elementIndex++] = GetVertexSkinIndices(vertexElement.Offset);
				else if (vertexElement.ElementType == VertexElementType.SkinWeights)
					vertexElements[elementIndex++] = GetVertexSkinWeights(vertexElement.Offset);
			inputLayout = new InputLayout(nativeDevice, vertexShaderBytecode, vertexElements);
		}

		private InputLayout inputLayout;

		private static InputElement GetVertexPosition2D(int offset)
		{
			return new InputElement("SV_POSITION", 0, SharpDXFormat.R32G32_Float, offset, 0);
		}

		private static InputElement GetVertexPosition3D(int offset)
		{
			return new InputElement("SV_POSITION", 0, SharpDXFormat.R32G32B32_Float, offset, 0);
		}

		private static InputElement GetVertexNormal3D(int offset)
		{
			return new InputElement("NORMAL", 0, SharpDXFormat.R32G32B32_Float, offset, 0);
		}

		private static InputElement GetVertexColor(int offset)
		{
			return new InputElement("COLOR", 0, SharpDXFormat.R8G8B8A8_UNorm, offset, 0);
		}

		private static InputElement GetVertexTextureCoordinate(int offset)
		{
			return new InputElement("TEXCOORD", 0, SharpDXFormat.R32G32_Float, offset, 0);
		}

		private static InputElement GetVertexLightMapUV(int offset)
		{
			return new InputElement("TEXCOORD", 1, SharpDXFormat.R32G32_Float, offset, 0);
		}

		private static InputElement GetVertexSkinIndices(int offset)
		{
			return new InputElement("BLENDINDICES", 0, SharpDXFormat.R32G32_Float, offset, 0);
		}

		private static InputElement GetVertexSkinWeights(int offset)
		{
			return new InputElement("BLENDWEIGHT", 0, SharpDXFormat.R32G32_Float, offset, 0);
		}

		private void CreateShaderValuesWithBufferMustAlwaysBe16BytesAligned()
		{
			shaderBuffer = new SharpDXBuffer(nativeDevice, 2240, BindFlags.ConstantBuffer);
			shaderValues = new ShaderValues();
		}

		private Buffer shaderBuffer;
		private ShaderValues shaderValues;

		private class ShaderValues
		{
			public Matrix WorldViewProjection;
			public Matrix[] JointMatrices;
			public Matrix World;
			public Matrix WorldView;
			//public Matrix View;
			//public Matrix Normal;
			public float[] CameraPosition;
			public float[] FogColorChannels;
			public float FogStart;
			public float FogEnd;
			public float[] LightDirection;
			public float[] LightColor;
		}

		private void CreateSamplerStates()
		{
			linearClampSamplerState =
				new SharpDXSampler(nativeDevice, Filter.MinMagMipLinear, TextureAddressMode.Clamp);
			linearWrapSamplerState =
				new SharpDXSampler(nativeDevice, Filter.MinMagMipLinear, TextureAddressMode.Wrap);
			pointClampSamplerState =
				new SharpDXSampler(nativeDevice, Filter.MinMagMipPoint, TextureAddressMode.Clamp);
			pointWrapSamplerState =
				new SharpDXSampler(nativeDevice, Filter.MinMagMipPoint, TextureAddressMode.Wrap);
		}

		private SamplerState linearClampSamplerState;
		private SamplerState linearWrapSamplerState;
		private SamplerState pointClampSamplerState;
		private SamplerState pointWrapSamplerState;

		public override void Bind()
		{
			if (shaderBufferValuesLoaded)
				ApplyShaderValuesToShaderBuffer();
			context.VertexShader.Set(vertexShader);
			context.VertexShader.SetConstantBuffer(0, shaderBuffer);
			context.PixelShader.Set(pixelShader);
			context.PixelShader.SetConstantBuffer(0, shaderBuffer);
			context.PixelShader.SetShaderResource(0, null);
			context.InputAssembler.InputLayout = inputLayout;
		}

		private bool shaderBufferValuesLoaded;

		private void ApplyShaderValuesToShaderBuffer()
		{
			DataStream dataStream;
			context.MapSubresource(shaderBuffer, MapMode.WriteDiscard, MapFlags.None, out dataStream);
			WriteMatrixToShaderBufferStream(dataStream, shaderValues.WorldViewProjection);
			if (Flags.HasFlag(ShaderFlags.Skinned))
				WriteJointMatricesToShaderBufferStream(dataStream);
			if (Flags.HasFlag(ShaderFlags.Fog))
				WriteFogValuesToShaderBufferStream(dataStream);
			if (Flags.HasFlag(ShaderFlags.Lit))
				WriteLightingValuesToShaderBufferStream(dataStream);
			context.UnmapSubresource(shaderBuffer, 0);
		}

		private static void WriteMatrixToShaderBufferStream(DataStream shaderStream, Matrix value)
		{
			shaderStream.WriteRange(Matrix.Transpose(value).GetValues, 0, 16);
		}

		private void WriteFogValuesToShaderBufferStream(DataStream shaderBufferStream)
		{
			WriteMatrixToShaderBufferStream(shaderBufferStream, shaderValues.World);
			shaderBufferStream.WriteRange(shaderValues.CameraPosition, 0,
				shaderValues.CameraPosition.Length);
			shaderBufferStream.WriteRange(shaderValues.FogColorChannels, 0,
				shaderValues.FogColorChannels.Length);
			shaderBufferStream.Write(shaderValues.FogStart);
			shaderBufferStream.Write(shaderValues.FogEnd);
		}

		private void WriteJointMatricesToShaderBufferStream(DataStream shaderStream)
		{
			Matrix[] jointMatrices = shaderValues.JointMatrices;
			for (int i = 0; i < 32; i++)
			{
				Matrix m = i < jointMatrices.Length ? jointMatrices[i] : Matrix.Identity;
				WriteMatrixToShaderBufferStream(shaderStream, m);
			}
		}

		private void WriteLightingValuesToShaderBufferStream(DataStream dataStream)
		{
			//WriteMatrixToShaderBufferStream(dataStream, shaderValues.View);
			//WriteMatrixToShaderBufferStream(dataStream, shaderValues.Normal);
			WriteMatrixToShaderBufferStream(dataStream, shaderValues.World);
			dataStream.WriteRange(shaderValues.LightDirection, 0, shaderValues.LightDirection.Length);
			dataStream.WriteRange(shaderValues.LightColor, 0, shaderValues.LightColor.Length);
		}

		public override void BindVertexDeclaration() {}

		public override void ApplyFogSettings(FogSettings fogSettings)
		{
			Vector3D cameraPosition = device.CameraInvertedViewMatrix.Translation;
			shaderValues.CameraPosition = new[]
			{
				cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 1.0f
			};
			shaderValues.FogColorChannels = new[]
			{
				fogSettings.FogColor.RedValue, fogSettings.FogColor.GreenValue,
				fogSettings.FogColor.BlueValue, 1.0f
			};
			shaderValues.FogStart = fogSettings.FogStart;
			shaderValues.FogEnd = fogSettings.FogEnd;
		}

		public override void SetModelViewProjection(Matrix matrix)
		{
			shaderValues.WorldViewProjection = matrix;
			shaderBufferValuesLoaded = true;
		}

		public override void SetModelViewProjection(Matrix model, Matrix view, Matrix projection)
		{
			shaderValues.World = model;
			//shaderValues.View = view;
			shaderValues.WorldView = model * view;
			shaderValues.WorldViewProjection = shaderValues.WorldView * projection;
			shaderBufferValuesLoaded = true;
		}

		public override void SetJointMatrices(Matrix[] jointMatrices)
		{
			shaderValues.JointMatrices = jointMatrices;
		}

		public override void SetSunLight(SunLight light)
		{
			shaderValues.LightDirection = new[]
			{
				light.Direction.X, light.Direction.Y, light.Direction.Z, 0.0f
			};
			shaderValues.LightColor = new[]
			{
				light.Color.RedValue, light.Color.GreenValue, light.Color.BlueValue, light.Color.AlphaValue
			};
		}

		public override void SetDiffuseTexture(Image image)
		{
			context.PixelShader.SetShaderResource(0, (image as SharpDXImage).NativeResourceView);
			var sampler = GetSamplerState(!image.DisableLinearFiltering, image.AllowTiling);
			context.PixelShader.SetSampler(0, sampler);
		}

		public SamplerState GetSamplerState(bool linearFiltering, bool tiling)
		{
			return linearFiltering
				? (tiling ? linearWrapSamplerState : linearClampSamplerState)
				: (tiling ? pointWrapSamplerState : pointClampSamplerState);
		}

		public override void SetLightmapTexture(Image texture)
		{
			context.PixelShader.SetShaderResource(1, (texture as SharpDXImage).NativeResourceView);
		}

		protected override void DisposeData()
		{
			DisposeSamplerStates();
			compiledVertexShader.Dispose();
			compiledPixelShader.Dispose();
			vertexShader.Dispose();
			pixelShader.Dispose();
			shaderBuffer.Dispose();
			inputLayout.Dispose();
		}

		private void DisposeSamplerStates()
		{
			linearClampSamplerState.Dispose();
			linearWrapSamplerState.Dispose();
			pointClampSamplerState.Dispose();
			pointWrapSamplerState.Dispose();
		}
	}
}