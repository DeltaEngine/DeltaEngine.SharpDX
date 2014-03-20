using SlimDX.Direct3D9;
using DeviceD3D9 = SlimDX.Direct3D9.Device;
using VertexFormatD3D9 = SlimDX.Direct3D9.VertexFormat;

namespace DeltaEngine.Graphics.SlimDX
{
	/// <summary>
	/// SlimDX Geometry.
	/// </summary>
	public class SlimDXGeometry : Geometry
	{
		protected SlimDXGeometry(string contentName, Device device)
			: base(contentName)
		{
			dxDevice = (SlimDXDevice)device;
			nativeDevice = dxDevice.NativeDevice;
			dxDevice.DisposeNativeBuffers += DisposeData;
			dxDevice.ReloadNativeBuffersData += ReloadBuffersData;
		}

		private SlimDXGeometry(GeometryCreationData creationData, Device device)
			: base(creationData)
		{
			dxDevice = (SlimDXDevice)device;
			nativeDevice = dxDevice.NativeDevice;
			dxDevice.DisposeNativeBuffers += DisposeData;
			dxDevice.ReloadNativeBuffersData += ReloadBuffersData;
		}

		private readonly SlimDXDevice dxDevice;
		private readonly DeviceD3D9 nativeDevice;

		protected override void SetNativeData(byte[] vertexData, short[] indices)
		{
			if (vertexBuffer == null || vertexBuffer.Disposed)
				CreateBuffers();
			cachedVertexData = vertexData;
			cachedIndices = indices;
			var vertexStream = vertexBuffer.Lock(0, vertexData.Length, LockFlags.None);
			vertexStream.WriteRange(vertexData, 0, 0);
			vertexBuffer.Unlock();
			var indexStream = indexBuffer.Lock(0, indices.Length * sizeof(short), LockFlags.None);
			indexStream.WriteRange(indices, 0, 0);
			indexBuffer.Unlock();
		}

		private byte[] cachedVertexData;
		private short[] cachedIndices;

		private void CreateBuffers()
		{
			int vertexDataSize = NumberOfVertices * Format.Stride;
			vertexBuffer = new VertexBuffer(nativeDevice, vertexDataSize, Usage.Dynamic,
				VertexFormatD3D9.None, Pool.Default);
			int indexDataSize = NumberOfIndices * sizeof(short);
			indexBuffer = new IndexBuffer(nativeDevice, indexDataSize, Usage.Dynamic, Pool.Default, true);
		}

		private VertexBuffer vertexBuffer;
		private IndexBuffer indexBuffer;

		public override void Draw()
		{
			const int VerticesPerTriangle = 3;
			nativeDevice.SetStreamSource(0, vertexBuffer, 0, Format.Stride);
			nativeDevice.Indices = indexBuffer;
			nativeDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, NumberOfVertices, 0,
				NumberOfIndices / VerticesPerTriangle);
		}

		protected override void DisposeData()
		{
			if (vertexBuffer != null)
				vertexBuffer.Dispose();
			if (vertexBuffer != null)
				indexBuffer.Dispose();
		}

		private void ReloadBuffersData()
		{
			SetNativeData(cachedVertexData, cachedIndices);
		}
	}
}