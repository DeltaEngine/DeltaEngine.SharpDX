using System.Collections.Generic;
using System.IO;
using DeltaEngine.Core;
using DeltaEngine.Graphics.Vertices;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using DXDevice = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;

namespace DeltaEngine.Graphics.SharpDX
{
	/// <summary>
	/// Basic functionality for all SharpDX based circular buffers to render small batches quickly.
	/// </summary>
	public class SharpDXCircularBuffer : CircularBuffer
	{
		public SharpDXCircularBuffer(Device device, ShaderWithFormat shader, BlendMode blendMode,
			VerticesMode drawMode = VerticesMode.Triangles)
			: base(device, shader, blendMode, drawMode) {}

		protected override void CreateNative()
		{
			sharpDXDevice = (SharpDXDevice)device;
			nativeDevice = sharpDXDevice.NativeDevice;
			deviceContext = sharpDXDevice.Context;
			nativeVertexBuffer = new SharpDXBuffer(nativeDevice, maxNumberOfVertices * vertexSize,
				BindFlags.VertexBuffer);
			nativeIndexBuffer = new SharpDXBuffer(nativeDevice, maxNumberOfIndices * indexSize,
				BindFlags.IndexBuffer);
		}

		private SharpDXDevice sharpDXDevice;
		private DXDevice nativeDevice;
		private DeviceContext deviceContext;
		private SharpDXBuffer nativeVertexBuffer;
		private SharpDXBuffer nativeIndexBuffer;

		protected override void DisposeNative()
		{
			nativeVertexBuffer.Dispose();
			nativeIndexBuffer.Dispose();
		}

		protected override void DisposeNextFrame()
		{
			buffersToDisposeNextFrame.Add(nativeVertexBuffer);
			if (UsesIndexBuffer)
				buffersToDisposeNextFrame.Add(nativeIndexBuffer);
		}

		private readonly List<SharpDXBuffer> buffersToDisposeNextFrame = new List<SharpDXBuffer>();

		protected override void AddDataNative<VertexType>(Chunk textureChunk, VertexType[] vertexData,
			short[] indices, int numberOfVertices, int numberOfIndices)
		{
			AddVertexDataNative(vertexData, numberOfVertices);
			if (!UsesIndexBuffer)
				return;
			if (indices == null)
				indices = ComputeIndices(textureChunk.NumberOfVertices, numberOfVertices);
			else if (totalIndicesCount > 0)
				indices = RemapIndices(indices, numberOfIndices);
			AddIndexDataNative(indices, numberOfIndices);
		}

		private void AddVertexDataNative<T>(T[] vertices, int numberOfVertices) where T : struct
		{
			DataStream dataStream;
			deviceContext.MapSubresource(nativeVertexBuffer, MapMode.WriteNoOverwrite, MapFlags.None,
				out dataStream);
			dataStream.Seek(totalVertexOffsetInBytes, SeekOrigin.Begin);
			dataStream.WriteRange(vertices, 0, numberOfVertices);
			deviceContext.UnmapSubresource(nativeVertexBuffer, 0);
		}

		private void AddIndexDataNative(short[] indices, int numberOfIndices)
		{
			DataStream dataStream;
			deviceContext.MapSubresource(nativeIndexBuffer, MapMode.WriteNoOverwrite, MapFlags.None,
				out dataStream);
			dataStream.Seek(totalIndexOffsetInBytes, SeekOrigin.Begin);
			dataStream.WriteRange(indices, 0, numberOfIndices);
			deviceContext.UnmapSubresource(nativeIndexBuffer, 0);
		}

		public override void DisposeUnusedBuffersFromPreviousFrame()
		{
			if (buffersToDisposeNextFrame.Count <= 0)
				return;
			foreach (var buffer in buffersToDisposeNextFrame)
				buffer.Dispose();
			buffersToDisposeNextFrame.Clear();
		}

		protected override void DrawChunk(Chunk chunk)
		{
			if (UsesIndexBuffer)
				DrawChunkWithIndices(chunk);
			else
				DrawChunkWithoutIndices(chunk);
		}

		private void DrawChunkWithIndices(Chunk chunk)
		{
			if (chunk.Texture != null)
				shader.SetDiffuseTexture(chunk.Texture);
			deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
			deviceContext.InputAssembler.SetVertexBuffers(0,
				new VertexBufferBinding(nativeVertexBuffer, vertexSize, 0));
			deviceContext.InputAssembler.SetIndexBuffer(nativeIndexBuffer, Format.R16_UInt,
				chunk.FirstIndexOffsetInBytes);
			deviceContext.DrawIndexed(chunk.NumberOfIndices, 0, 0);
		}

		private void DrawChunkWithoutIndices(Chunk chunk)
		{
			deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;
			deviceContext.InputAssembler.SetVertexBuffers(0,
				new VertexBufferBinding(nativeVertexBuffer, vertexSize, chunk.FirstVertexOffsetInBytes));
			deviceContext.Draw(chunk.NumberOfVertices, 0);
		}
	}
}