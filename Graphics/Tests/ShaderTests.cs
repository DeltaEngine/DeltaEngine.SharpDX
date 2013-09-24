using System;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Graphics.Mocks;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Graphics.Tests
{
	public class ShaderTests : TestWithMocksOrVisually
	{
		[Test, CloseAfterFirstFrame]
		public void LoadShaderAsContent()
		{
			var shaderColorUv = ContentLoader.Load<Shader>(Shader.Position2DColorUv) as ShaderWithFormat;
			var shaderColor = ContentLoader.Load<Shader>(Shader.Position2DColor) as ShaderWithFormat;
			var shaderUv = ContentLoader.Load<Shader>(Shader.Position2DUv) as ShaderWithFormat;
			var shader3DUv = ContentLoader.Load<Shader>(Shader.Position3DUv) as ShaderWithFormat;
			var shader3DColorUv = ContentLoader.Load<Shader>(Shader.Position3DColorUv) as ShaderWithFormat;
			Assert.AreEqual(Shader.Position2DColorUv, shaderColorUv.Name);
			Assert.AreEqual(VertexFormat.Position2DColorUv, shaderColorUv.Format);
			Assert.AreEqual(Shader.Position2DColor, shaderColor.Name);
			Assert.AreEqual(VertexFormat.Position2DColor, shaderColor.Format);
			Assert.AreEqual(Shader.Position2DUv, shaderUv.Name);
			Assert.AreEqual(VertexFormat.Position2DUv, shaderUv.Format);
			Assert.AreEqual(Shader.Position3DUv, shader3DUv.Name);
			Assert.AreEqual(VertexFormat.Position3DUv, shader3DUv.Format);
			Assert.AreEqual(Shader.Position3DColorUv, shader3DColorUv.Name);
			Assert.AreEqual(VertexFormat.Position3DColorUv, shader3DColorUv.Format);
		}

		[Test]
		public void InvalidVertexFormat
			()
		{
			var data = new ShaderCreationData("", "", "", "", null);
			AssertActionThrowsInnerException<ShaderWithFormat.InvalidVertexFormat>(
				() => ContentLoader.Create<Shader>(data));
			data = new ShaderCreationData("", "", "", "", new VertexFormat(new VertexElement[0]));
			AssertActionThrowsInnerException<ShaderWithFormat.InvalidVertexFormat>(
				() => ContentLoader.Create<Shader>(data));
		}

		[Test]
		public void InvalidVertexAndPixelCode()
		{
			var data = new ShaderCreationData("", "", "", "", VertexFormat.Position2DColor);
			AssertActionThrowsInnerException<ShaderWithFormat.InvalidShaderCode>(
				() => ContentLoader.Create<Shader>(data));
		}

		private static void AssertActionThrowsInnerException<T>(Action action) where T : Exception
		{
			Assert.That(() => action(),
				Throws.TargetInvocationException.With.InnerException.TypeOf<T>());
		}
	}
}