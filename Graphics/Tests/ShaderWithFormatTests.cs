using System.IO;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Graphics.Mocks;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.Platforms;
using NUnit.Framework;

namespace DeltaEngine.Graphics.Tests
{
	public class ShaderWithFormatTests : TestWithMocksOrVisually
	{
		[Test, CloseAfterFirstFrame]
		public void LoadShaderAsContent()
		{
			var shaderColorUV = ContentLoader.Load<Shader>(Shader.Position2DColorUV) as ShaderWithFormat;
			var shaderColor = ContentLoader.Load<Shader>(Shader.Position2DColor) as ShaderWithFormat;
			var shaderUV = ContentLoader.Load<Shader>(Shader.Position2DUV) as ShaderWithFormat;
			var shader3DUV = ContentLoader.Load<Shader>(Shader.Position3DUV) as ShaderWithFormat;
			var shader3DColorUV = ContentLoader.Load<Shader>(Shader.Position3DColorUV) as ShaderWithFormat;
			Assert.AreEqual(Shader.Position2DColorUV, shaderColorUV.Name);
			Assert.AreEqual(VertexFormat.Position2DColorUV, shaderColorUV.Format);
			Assert.AreEqual(Shader.Position2DColor, shaderColor.Name);
			Assert.AreEqual(VertexFormat.Position2DColor, shaderColor.Format);
			Assert.AreEqual(Shader.Position2DUV, shaderUV.Name);
			Assert.AreEqual(VertexFormat.Position2DUV, shaderUV.Format);
			Assert.AreEqual(Shader.Position3DUV, shader3DUV.Name);
			Assert.AreEqual(VertexFormat.Position3DUV, shader3DUV.Format);
			Assert.AreEqual(Shader.Position3DColorUV, shader3DColorUV.Name);
			Assert.AreEqual(VertexFormat.Position3DColorUV, shader3DColorUV.Format);
		}

		[Test]
		public void InvalidVertexFormat()
		{
			Assert.Throws<ShaderWithFormat.InvalidVertexFormat>(
				() => ContentLoader.Create<Shader>(new ShaderCreationData("", "", "", "", null)));
			var data = new ShaderCreationData("", "", "", "", new VertexFormat(new VertexElement[0]));
			Assert.Throws<ShaderWithFormat.InvalidVertexFormat>(
				() => ContentLoader.Create<Shader>(data));
		}

		[Test]
		public void InvalidVertexAndPixelCode()
		{
			var data = new ShaderCreationData("", "", "", "", VertexFormat.Position2DColor);
			Assert.Throws<ShaderWithFormat.InvalidShaderCode>(() => ContentLoader.Create<Shader>(data));
		}

		[Test]
		public void ExpectDefaultShader2DIfNoShaderContentAvailable()
		{
			var position2DUVShader = ContentLoader.Load<NoDataShaderWithFormat>("Position2DUV");
			var position2DColorShader = ContentLoader.Load<NoDataShaderWithFormat>("Position2DColor");
			var position2DColorUVShader = ContentLoader.Load<NoDataShaderWithFormat>("Position2DColorUV");
			Assert.AreEqual(VertexFormat.Position2DUV, position2DUVShader.Format);
			Assert.AreEqual(VertexFormat.Position2DColor, position2DColorShader.Format);
			Assert.AreEqual(VertexFormat.Position2DColorUV, position2DColorUVShader.Format);
		}

		[Test]
		public void ExpectDefaultShader3DIfNoShaderContentAvailable()
		{
			var position3DUVShader = ContentLoader.Load<NoDataShaderWithFormat>("Position3DUV");
			var position3DColorShader = ContentLoader.Load<NoDataShaderWithFormat>("Position3DColor");
			var position3DColorUVShader = ContentLoader.Load<NoDataShaderWithFormat>("Position3DColorUV");
			var normalUVShader = ContentLoader.Load<NoDataShaderWithFormat>("Position3DNormalUV");
			Assert.AreEqual(VertexFormat.Position3DUV, position3DUVShader.Format);
			Assert.AreEqual(VertexFormat.Position3DColor, position3DColorShader.Format);
			Assert.AreEqual(VertexFormat.Position3DColorUV, position3DColorUVShader.Format);
			Assert.AreEqual(VertexFormat.Position3DNormalUV, normalUVShader.Format);
		}

		[Test]
		public void AllowDynamicCreationViaCreationData()
		{
			var data = new ShaderCreationData("AnyData", "AnyData", "AnyData", "AnyData",
				VertexFormat.Position2DColor);
			var shader = ContentLoader.Create<NoDataShaderWithFormat>(data);
			Assert.DoesNotThrow(() => shader.ReloadCreationData(data));
		}

		[Test]
		public void ExpectExceptionIfNoShaderDataAreSpecified()
		{
			var data = new ShaderCreationData("NoData", "NoData", "NoData", "NoData",
				VertexFormat.Position2DColor);
			var shader = ContentLoader.Create<NoDataShaderWithFormat>(data);
			Assert.Throws<ShaderWithFormat.NoShaderDataSpecified>(() => shader.ReloadCreationData(data));
		}

		private class NoDataShaderWithFormat : MockShader
		{
			public NoDataShaderWithFormat(string contentName, Device device)
				: base(contentName, device) { }

			public NoDataShaderWithFormat(ShaderCreationData creationData, Device device)
				: base(creationData, device) { }

			public void ReloadCreationData(ShaderCreationData creationData)
			{
				byte[] rawData = creationData.VertexCode == "NoData"
					? new byte[0] : BinaryDataExtensions.ToByteArrayWithTypeInformation(creationData);
				LoadData(new MemoryStream(rawData));
			}
		}
	}
}