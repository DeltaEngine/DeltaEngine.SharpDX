using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Graphics.Mocks;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Graphics.Tests
{
	public class CircularBufferTests : TestWithMocksOrVisually
	{
		[SetUp]
		public void CreateBuffer()
		{
			buffer = new MockCircularBuffer(Resolve<Device>(),
				ContentLoader.Load<ShaderWithFormat>(Shader.Position2DUv), BlendMode.Normal,
				VerticesMode.Triangles);
			Assert.IsTrue(buffer.IsCreated);
		}

		private MockCircularBuffer buffer;

		private readonly int vertexSize = VertexPosition3DColor.SizeInBytes;

		[Test]
		public void CreateAndDisposeBuffer()
		{
			buffer.Dispose();
			Assert.IsFalse(buffer.IsCreated);
		}

		[Test]
		public void OffsetInitialization()
		{
			Assert.AreEqual(0, buffer.VertexOffset);
			Assert.AreEqual(0, buffer.IndexOffset);
		}

		[Test]
		public void OffsetIncrement()
		{
			const int VerticesCount = 32;
			const int IndicesCount = 48;
			var vertices = new VertexPosition2DUV[VerticesCount];
			var indices = new short[IndicesCount];
			buffer.Add(null, vertices, indices);
			Assert.AreEqual(VerticesCount * vertexSize, buffer.VertexOffset);
			Assert.AreEqual(IndicesCount * sizeof(short), buffer.IndexOffset);
		}

		[Test]
		public void OffsetSeveralIncrements()
		{
			const int VerticesCount = 32;
			const int IndicesCount = 48;
			var vertices = new VertexPosition2DUV[VerticesCount];
			var indices = new short[IndicesCount];
			for (int i = 1; i <= IncrementCount; i++)
			{
				buffer.Add(null, vertices, indices);
				Assert.AreEqual(i * VerticesCount * vertexSize, buffer.VertexOffset);
				Assert.AreEqual(i * IndicesCount * sizeof(short), buffer.IndexOffset);
			}
		}

		private const int IncrementCount = 4;

		[Test]
		public void DrawAndReset()
		{
			const int VerticesCount = 32;
			const int IndicesCount = 48;
			var vertices = new VertexPosition2DUV[VerticesCount];
			var indices = new short[IndicesCount];
			buffer.Add(null, vertices, indices);
			Assert.IsFalse(buffer.HasDrawn);
			buffer.DrawAllTextureChunks();
			Assert.IsTrue(buffer.HasDrawn);
			Assert.AreEqual(512, buffer.VertexOffset);
			Assert.AreEqual(96, buffer.IndexOffset);
		}

		[Test]
		public void DataBiggerThanHalfOfTheBufferSize()
		{
			const int VerticesCount = 12288;
			const int IndicesCount = 16384;
			var vertices = new VertexPosition2DUV[VerticesCount];
			var indices = new short[IndicesCount];
			buffer.Add(null, vertices, indices);
			Assert.AreEqual(VerticesCount * vertexSize, buffer.VertexOffset);
			Assert.AreEqual(IndicesCount * sizeof(short), buffer.IndexOffset);
			Assert.IsFalse(buffer.HasDrawn);
		}

		[Test]
		public void MakeBufferResize()
		{
			const int VerticesCount = 12288;
			const int IndicesCount = 16384;
			var vertices = new VertexPosition2DUV[VerticesCount];
			var indices = new short[IndicesCount];
			buffer.Add(null, vertices, indices);
			Assert.IsFalse(buffer.HasDrawn);
			buffer.Add(null, vertices, indices);
			Assert.IsTrue(buffer.HasDrawn);
		}

		[Test]
		public void LoadDataWithDifferentSize()
		{
			const int Data1VerticesCount = 100;
			const int Data1IndicesCount = 150;
			const int Data2VerticesCount = 400;
			const int Data2IndicesCount = 500;
			var vertices1 = new VertexPosition2DUV[Data1VerticesCount];
			var indices1 = new short[Data1IndicesCount];
			var vertices2 = new VertexPosition2DUV[Data2VerticesCount];
			var indices2 = new short[Data2IndicesCount];
			buffer.Add(null, vertices1, indices1);
			Assert.AreEqual(Data1VerticesCount * vertexSize, buffer.VertexOffset);
			Assert.AreEqual(Data1IndicesCount * sizeof(short), buffer.IndexOffset);
			buffer.Add(null, vertices2, indices2);
			Assert.AreEqual((Data1VerticesCount + Data2VerticesCount) * vertexSize, buffer.VertexOffset);
			Assert.AreEqual((Data1IndicesCount + Data2IndicesCount) * sizeof(short), buffer.IndexOffset);
		}

		[Test]
		public void DataBiggerThanBufferSize()
		{
			var verticesCount = buffer.MaxNumberOfVertices * 3;
			var indicesCount = buffer.MaxNumberOfVertices * 4;
			var vertices = new VertexPosition2DUV[verticesCount];
			var indices = new short[indicesCount];
			buffer.Add(null, vertices, indices);
			Assert.AreEqual(verticesCount * vertexSize, buffer.VertexOffset);
			Assert.AreEqual(indicesCount * sizeof(short), buffer.IndexOffset);
		}
	}
}