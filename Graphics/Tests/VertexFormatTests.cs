using DeltaEngine.Datatypes;
using DeltaEngine.Graphics.Vertices;
using NUnit.Framework;

namespace DeltaEngine.Graphics.Tests
{
	public class VertexFormatTests
	{
		[Test]
		public void VertexSizeInBytes()
		{
			Assert.AreEqual(VertexPosition2DUV.SizeInBytes, 16);
			Assert.AreEqual(VertexPosition2DColor.SizeInBytes, 12);
			Assert.AreEqual(VertexPosition2DColorUV.SizeInBytes, 20);
			Assert.AreEqual(VertexPosition3DUV.SizeInBytes, 20);
			Assert.AreEqual(VertexPosition3DColor.SizeInBytes, 16);
			Assert.AreEqual(VertexPosition3DColorUV.SizeInBytes, 24);
		}

		[Test]
		public void VertexPositionColorTextured2D()
		{
			var vertex = new VertexPosition2DColorUV(Point.Zero, Color.Red, Point.One);
			Assert.AreEqual(vertex.Position, Point.Zero);
			Assert.AreEqual(vertex.Color, Color.Red);
			Assert.AreEqual(vertex.UV, Point.One);
		}

		[Test]
		public void LerpPositionColorTextured2D()
		{
			var vertex = new VertexPosition2DColorUV(Point.UnitX, Color.White, Point.One);
			var vertex2 = new VertexPosition2DColorUV(Point.UnitY, Color.Black, Point.Zero);
			var lerpedVertex = vertex.Lerp(vertex2, 0.5f);
			Assert.AreEqual(lerpedVertex.Position, Point.Half);
			Assert.AreEqual(lerpedVertex.Color, new Color(127, 127, 127));
			Assert.AreEqual(lerpedVertex.UV, Point.One);
		}

		[Test]
		public void LerpPositionTextured2D()
		{
			var vertex = new VertexPosition2DUV(Point.UnitX, Point.One);
			var vertex2 = new VertexPosition2DUV(Point.UnitY, Point.Zero);
			var lerpedVertex = vertex.Lerp(vertex2, 0.5f);
			Assert.AreEqual(lerpedVertex.Position, Point.Half);
			Assert.AreEqual(lerpedVertex.UV, Point.One);
		}

		[Test]
		public void VertexPositionColorTextured3D()
		{
			Assert.AreEqual(VertexPosition3DColorUV.SizeInBytes, 24);
			var vertex = new VertexPosition3DColorUV(Vector.UnitX, Color.Red, Point.One);
			Assert.AreEqual(vertex.Format, VertexFormat.Position3DColorUv);
			Assert.AreEqual(vertex.Position, Vector.UnitX);
			Assert.AreEqual(vertex.Color, Color.Red);
			Assert.AreEqual(vertex.UV, Point.One);
		}

		[Test]
		public void LerpPositionColorTextured3D()
		{
			var vertex = new VertexPosition3DColorUV(Vector.UnitX, Color.White, Point.One);
			var vertex2 = new VertexPosition3DColorUV(Point.UnitY, Color.Black, Point.Zero);
			var lerpedVertex = vertex.Lerp(vertex2, 0.5f);
			Assert.AreEqual(lerpedVertex.Position, new Vector(0.5f, 0.5f, 0f));
			Assert.AreEqual(lerpedVertex.Color, new Color(127, 127, 127));
			Assert.AreEqual(lerpedVertex.UV, Point.One);
		}

		[Test]
		public void VertexElementPosition3D()
		{
			var element = new VertexElement(VertexElementType.Position3D);
			Assert.AreEqual(VertexElementType.Position3D, element.ElementType);
			Assert.AreEqual(3, element.ComponentCount);
			Assert.AreEqual(12, element.Size);
		}

		[Test]
		public void VertexPositionTextured3D()
		{
			Assert.AreEqual(VertexPosition3DUV.SizeInBytes, 20);
			var vertex = new VertexPosition3DUV(Vector.UnitX, Point.One);
			Assert.AreEqual(vertex.Format, VertexFormat.Position3DUv);
			Assert.AreEqual(vertex.Position, Vector.UnitX);
			Assert.AreEqual(vertex.UV, Point.One);
		}

		[Test]
		public void LerpPositionTextured3D()
		{
			var vertex = new VertexPosition3DUV(Vector.UnitX, Point.One);
			var vertex2 = new VertexPosition3DUV(Point.UnitY, Point.Zero);
			var lerpedVertex = vertex.Lerp(vertex2, 0.5f);
			Assert.AreEqual(lerpedVertex.Position, new Vector(0.5f, 0.5f, 0f));
			Assert.AreEqual(lerpedVertex.UV, Point.One);
		}

		[Test]
		public void LerpPositionColor3D()
		{
			var vertex = new VertexPosition3DColor(Vector.UnitX, Color.White);
			var vertex2 = new VertexPosition3DColor(Point.UnitY, Color.Black);
			var lerpedVertex = vertex.Lerp(vertex2, 0.5f);
			Assert.AreEqual(lerpedVertex.Position, new Vector(0.5f, 0.5f, 0f));
			Assert.AreEqual(lerpedVertex.Color, new Color(127, 127, 127));
		}

		[Test]
		public void VertexElementTextureUV()
		{
			var element = new VertexElement(VertexElementType.TextureUV);
			Assert.AreEqual(VertexElementType.TextureUV, element.ElementType);
			Assert.AreEqual(2, element.ComponentCount);
			Assert.AreEqual(8, element.Size);
		}

		[Test]
		public void VertexElementColor()
		{
			var element = new VertexElement(VertexElementType.Color);
			Assert.AreEqual(VertexElementType.Color, element.ElementType);
			Assert.AreEqual(4, element.ComponentCount);
			Assert.AreEqual(4, element.Size);
		}

		[Test]
		public void VertexFormatPosition3DTextureUVColor()
		{
			var elements = new[] {
				new VertexElement(VertexElementType.Position3D),
				new VertexElement(VertexElementType.TextureUV),
				new VertexElement(VertexElementType.Color)};
			var format = new VertexFormat(elements);
			Assert.AreEqual(24, format.Stride);
			Assert.AreEqual(0, elements[0].Offset);
			Assert.AreEqual(12, elements[1].Offset);
			Assert.AreEqual(20, elements[2].Offset);
		}

		[Test]
		public void VertexFormatGetVertexElement()
		{
			var elements = new[] {
				new VertexElement(VertexElementType.Position3D),
				new VertexElement(VertexElementType.TextureUV) };
			var format = new VertexFormat(elements);
			Assert.IsNull(format.GetElementFromType(VertexElementType.Color));
			Assert.IsNotNull(format.GetElementFromType(VertexElementType.TextureUV));
		}

		[Test]
		public void AreEqual()
		{
			var elements = new[] {
				new VertexElement(VertexElementType.Position3D),
				new VertexElement(VertexElementType.TextureUV) };
			var format = new VertexFormat(elements);
			Assert.IsTrue(VertexFormat.Position3DUv.Equals(format));
			Assert.IsTrue(VertexFormat.Position3DUv.Equals((object)format));
			Assert.AreEqual(VertexFormat.Position3DUv, format);
			Assert.IsTrue(VertexFormat.Position3DUv == format);
			Assert.IsTrue(VertexFormat.Position2DUv.Equals(VertexFormat.Position2DUv));
			Assert.IsFalse(VertexFormat.Position2DUv == VertexFormat.Position2DColor);
			Assert.AreEqual(VertexFormat.Position2DUv, VertexFormat.Position2DUv);
			Assert.AreNotEqual(VertexFormat.Position2DUv, VertexFormat.Position2DColor);
		}

		[Test]
		public void FormatToString()
		{
			var elements = new[] {
				new VertexElement(VertexElementType.Position3D),
				new VertexElement(VertexElementType.TextureUV) };
			var format = new VertexFormat(elements);
			Assert.AreEqual("VertexFormat: Position3D*3, TextureUV*2, Stride=20", format.ToString());
		}

		[Test]
		public void HasProperties()
		{
			var elements = new[] {
				new VertexElement(VertexElementType.Position3D),
				new VertexElement(VertexElementType.TextureUV) };
			var format = new VertexFormat(elements);
			Assert.IsTrue(format.HasUV);
			Assert.IsTrue(format.Is3D);
			Assert.IsFalse(format.HasColor);
		}
	}
}