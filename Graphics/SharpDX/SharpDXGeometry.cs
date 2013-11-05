using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using DXDevice = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using DXFormat = SharpDX.DXGI.Format;

namespace DeltaEngine.Graphics.SharpDX
{
	/// <summary>
	/// Geometry used for SharpDX devices.
	/// </summary>
	public class SharpDXGeometry : Geometry
	{
		protected SharpDXGeometry(string contentName, Device device)
			: base(contentName)
		{
			Initialize(device);
		}

		protected SharpDXGeometry(GeometryCreationData creationData, Device device)
			: base(creationData)
		{
			Initialize(device);
		}

		private void Initialize(Device device)
		{
			sharpDXDevice = (SharpDXDevice)device;
			nativeDevice = sharpDXDevice.NativeDevice;
			deviceContext = sharpDXDevice.Context;			
		}

		private SharpDXDevice sharpDXDevice;
		private DXDevice nativeDevice;
		private DeviceContext deviceContext;

		protected override void SetNativeData(byte[] vertexData, short[] indices)
		{
			if (vertexBuffer == null)
				CreateBuffers();
			DataStream verticesStream;
			deviceContext.MapSubresource(vertexBuffer, MapMode.WriteDiscard, MapFlags.None,
				out verticesStream);
			verticesStream.WriteRange(vertexData);
			deviceContext.UnmapSubresource(vertexBuffer, 0);
			DataStream indicesStream;
			deviceContext.MapSubresource(indexBuffer, MapMode.WriteDiscard, MapFlags.None,
				out indicesStream);
			indicesStream.WriteRange(indices);
			deviceContext.UnmapSubresource(indexBuffer, 0);
		}

		private SharpDXBuffer vertexBuffer;
		private SharpDXBuffer indexBuffer;

		private void CreateBuffers()
		{
			int vertexDataSize = NumberOfVertices * Format.Stride;
			vertexBuffer = new SharpDXBuffer(nativeDevice, vertexDataSize, BindFlags.VertexBuffer);
			int indexDataSize = NumberOfIndices * sizeof(short);
			indexBuffer = new SharpDXBuffer(nativeDevice, indexDataSize, BindFlags.IndexBuffer);
		}

		public override void Draw()
		{
			deviceContext.InputAssembler.SetVertexBuffers(0,
				new VertexBufferBinding(vertexBuffer, Format.Stride, 0));
			deviceContext.InputAssembler.SetIndexBuffer(indexBuffer, DXFormat.R16_UInt, 0);
			deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
			deviceContext.DrawIndexed(NumberOfIndices, 0, 0);
		}

		protected override void DisposeData()
		{
			vertexBuffer.Dispose();
			indexBuffer.Dispose();
		}
	}
}