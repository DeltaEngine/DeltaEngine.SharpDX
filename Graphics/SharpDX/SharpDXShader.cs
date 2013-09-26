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
using SharpDXMatrix = SharpDX.Matrix;
using SharpDXFormat = SharpDX.DXGI.Format;

namespace DeltaEngine.Graphics.SharpDX
{
	/// <summary>
	/// Base simple SharpDX shader.
	/// </summary>
	public class SharpDXShader : ShaderWithFormat
	{
		protected SharpDXShader(string contentName, SharpDXDevice device)
			: base(contentName)
		{
			nativeDevice = device.NativeDevice;
			context = device.NativeDevice.ImmediateContext;
		}

		private readonly DXDevice nativeDevice;
		private readonly DeviceContext context;

		public SharpDXShader(ShaderCreationData customShader, SharpDXDevice device)
			: base(customShader)
		{
			nativeDevice = device.NativeDevice;
			context = device.NativeDevice.ImmediateContext;
			Create();
		}

		protected override sealed void Create()
		{
			CreateVertexShader();
			CreatePixelShader();
			CreateVertexDeclaration();
			CreateMatrixBuffer();
		}

		private void CreateVertexShader()
		{
			var compiledShader = ShaderBytecode.Compile(Dx11Code, "VsMain", "vs_4_0");
			vertexShaderBytecode = compiledShader.Bytecode;
			vertexShader = new VertexShader(nativeDevice, compiledShader.Bytecode);
		}

		private byte[] vertexShaderBytecode;
		private VertexShader vertexShader;

		private void CreatePixelShader()
		{
			var compiledShader = ShaderBytecode.Compile(Dx11Code, "PsMain", "ps_4_0");
			pixelShader = new PixelShader(nativeDevice, compiledShader.Bytecode);
		}

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
					vertexElements[elementIndex++] = GetVertexLightMapUv(vertexElement.Offset);
				else if (vertexElement.ElementType == VertexElementType.Normal)
					vertexElements[elementIndex++] = GetVertexNormal3D(vertexElement.Offset);
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

		private static InputElement GetVertexLightMapUv(int offset)
		{
			return new InputElement("TEXCOORD", 1, SharpDXFormat.R32G32_Float, offset, 0);
		}

		private void CreateMatrixBuffer()
		{
			matrixBuffer = new SharpDXBuffer(nativeDevice, 64, BindFlags.ConstantBuffer);
		}

		private Buffer matrixBuffer;

		public override void Bind()
		{
			context.VertexShader.Set(vertexShader);
			context.VertexShader.SetConstantBuffer(0, matrixBuffer);
			context.PixelShader.Set(pixelShader);
			context.PixelShader.SetConstantBuffer(0, matrixBuffer);
			context.PixelShader.SetShaderResource(0, null);
			context.InputAssembler.InputLayout = inputLayout;
		}

		public override void BindVertexDeclaration() {}

		public override void SetModelViewProjectionMatrix(Matrix matrix)
		{
			DataStream dataStream;
			context.MapSubresource(matrixBuffer, MapMode.WriteDiscard, MapFlags.None, out dataStream);
			dataStream.WriteRange(Matrix.Transpose(matrix).GetValues, 0, 16);
			context.UnmapSubresource(matrixBuffer, 0);
		}

		public override void SetJointMatrices(Matrix[] jointMatrices)
		{
			// not supported yet
		}

		public override void SetDiffuseTexture(Image image)
		{
			context.PixelShader.SetShaderResource(0, (image as SharpDXImage).NativeResourceView);
			var sampler = image.DisableLinearFiltering ? GetPointSamplerLazy() : GetLinearSamplerLazy();
			context.PixelShader.SetSampler(0, sampler);
		}

		public SamplerState GetLinearSamplerLazy()
		{
			return linearSampler ??
				(linearSampler = new SharpDXSampler(nativeDevice, Filter.MinMagMipLinear));
		}

		private SamplerState linearSampler;

		public SamplerState GetPointSamplerLazy()
		{
			return pointSampler ??
				(pointSampler = new SharpDXSampler(nativeDevice, Filter.MinMagMipPoint));
		}

		private SamplerState pointSampler;

		public override void SetLightmapTexture(Image texture)
		{
			context.PixelShader.SetShaderResource(1, (texture as SharpDXImage).NativeResourceView);
		}

		public override void SetLightPosition(Vector3D vector)
		{
			// not implemented yet
		}

		public override void SetViewPosition(Vector3D vector)
		{
			// not implemented yet
		}

		protected override void DisposeData()
		{
			vertexShader.Dispose();
			pixelShader.Dispose();
			matrixBuffer.Dispose();
			inputLayout.Dispose();
		}
	}
}