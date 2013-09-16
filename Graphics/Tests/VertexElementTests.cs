using System.IO;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics.Vertices;
using NUnit.Framework;

namespace DeltaEngine.Graphics.Tests
{
	public class VertexElementTests
	{
		[Test]
		public void CreationWithInvalidTypeThrows()
		{
			const VertexElementType invalidType = (VertexElementType)999;
			Assert.Throws<VertexElement.ElementTypeNotYetSupported>(
				() => new VertexElement(invalidType));
		}

		[Test]
		public void ElementToString()
		{
			var element = new VertexElement(VertexElementType.TextureUV);
			Assert.AreEqual("TextureUV*2", element.ToString());
		}

		[Test]
		public void SaveDataColor()
		{
			var element = new VertexElement(VertexElementType.TextureUV);
			using(var stream = new MemoryStream())
			{
				var writer = new BinaryWriter(stream);
				element.SaveData(writer, Color.Red);
				Assert.AreEqual(stream.Length, 4);
			}
		}

		[Test]
		public void SaveDataVector()
		{
			var element = new VertexElement(VertexElementType.TextureUV);
			using (var stream = new MemoryStream())
			{
				var writer = new BinaryWriter(stream);
				element.SaveData(writer, Vector.UnitX);
				Assert.AreEqual(stream.Length, 12);
			}
		}

		[Test]
		public void SaveDataPoint()
		{
			var element = new VertexElement(VertexElementType.TextureUV);
			using (var stream = new MemoryStream())
			{
				var writer = new BinaryWriter(stream);
				element.SaveData(writer, Point.UnitX);
				Assert.AreEqual(stream.Length, 8);
			}
		}
	}
}