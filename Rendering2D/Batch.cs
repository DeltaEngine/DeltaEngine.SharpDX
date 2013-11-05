using System;
using DeltaEngine.Content;
using DeltaEngine.Core;
using DeltaEngine.Datatypes;
using DeltaEngine.Extensions;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.ScreenSpaces;

namespace DeltaEngine.Rendering2D
{
	/// <summary>
	/// For rendering sprites in batches
	/// </summary>
	public class Batch
	{
		public Batch(Material material, BlendMode blendMode, 
			int minimumNumberOfQuads = MinNumberOfQuads)
		{
			this.material = material;
			this.blendMode = blendMode;
			minimumNumberOfQuads = MathExtensions.Max(minimumNumberOfQuads, MinNumberOfQuads);
			var shader = material.Shader as ShaderWithFormat;
			hasUV = shader.Format.HasUV;
			hasColor = shader.Format.HasColor;
			indices = new short[minimumNumberOfQuads * IndicesPerQuad];
			if (!hasUV)
				verticesColor = new VertexPosition2DColor[minimumNumberOfQuads * VerticesPerQuad];
			else if (hasColor)
				verticesColorUV = new VertexPosition2DColorUV[minimumNumberOfQuads * VerticesPerQuad];
			else
				verticesUV = new VertexPosition2DUV[minimumNumberOfQuads * VerticesPerQuad];
		}

		public readonly Material material;
		public readonly BlendMode blendMode;
		private readonly bool hasUV;
		private readonly bool hasColor;
		private short[] indices;
		private const int MinNumberOfQuads = 16;
		private const int IndicesPerQuad = 6;
		private const int VerticesPerQuad = 4;
		private VertexPosition2DColor[] verticesColor;
		public VertexPosition2DColorUV[] verticesColorUV;
		private VertexPosition2DUV[] verticesUV;

		public bool IsBufferFullAndResizeIfPossible(int numberOfQuadsToAdd = 1)
		{
			bool isBufferFull = IsBufferFull(indices.Length, numberOfQuadsToAdd);
			if (!isBufferFull || indices.Length >= MaxNumberOfIndices)
				return isBufferFull;
			GrowIndicesAndVerticesToSmallestPowerOfTwoThatFits(numberOfQuadsToAdd);
			return IsBufferFull(indices.Length, numberOfQuadsToAdd);
		}
		private const int MaxNumberOfIndices = short.MaxValue;

		private bool IsBufferFull(int length, int numberOfQuadsToAdd)
		{
			return length - indicesIndex < IndicesPerQuad * numberOfQuadsToAdd;
		}

		private int indicesIndex;

		private void GrowIndicesAndVerticesToSmallestPowerOfTwoThatFits(int numberOfQuadsToAdd)
		{
			int newNumberOfIndices = indices.Length * 2;
			while (IsBufferFull(newNumberOfIndices, numberOfQuadsToAdd))
				newNumberOfIndices *= 2; //ncrunch: no coverage
			ResizeIndicesAndVertices(newNumberOfIndices);
		}

		private void ResizeIndicesAndVertices(int newNumberOfIndices)
		{
			if (newNumberOfIndices > MaxNumberOfIndices)
				newNumberOfIndices = MaxNumberOfIndices; //ncrunch: no coverage
			var newIndices = new short[newNumberOfIndices];
			Array.Copy(indices, newIndices, indices.Length);
			indices = newIndices;
			int newNumberOfVertices = newNumberOfIndices * VerticesPerQuad / IndicesPerQuad;
			if (!hasUV)
				ResizeColorVertices(newNumberOfVertices);
			else if (hasColor)
				ResizeColorUVVertices(newNumberOfVertices);
			else
				ResizeUVVertices(newNumberOfVertices);
		}

		private void ResizeColorVertices(int newNumberOfVertices)
		{
			var newVerticesColor = new VertexPosition2DColor[newNumberOfVertices];
			Array.Copy(verticesColor, newVerticesColor, verticesColor.Length);
			verticesColor = newVerticesColor;
		}

		private void ResizeColorUVVertices(int newNumberOfVertices)
		{
			var newVerticesColorUV = new VertexPosition2DColorUV[newNumberOfVertices];
			Array.Copy(verticesColorUV, newVerticesColorUV, verticesColorUV.Length);
			verticesColorUV = newVerticesColorUV;
		}

		private void ResizeUVVertices(int newNumberOfVertices)
		{
			var newVerticesUV = new VertexPosition2DUV[newNumberOfVertices];
			Array.Copy(verticesUV, newVerticesUV, verticesUV.Length);
			verticesUV = newVerticesUV;
		}

		public void AddIndicesAndVertices(Sprite sprite)
		{
			if (!HasSomethingToRender(sprite))
				return; //ncrunch: no coverage
			AddIndices();
			AddVertices(sprite);
		}

		public void AddIndices()
		{
			indices[indicesIndex++] = (short)verticesIndex;
			indices[indicesIndex++] = (short)(verticesIndex + 1);
			indices[indicesIndex++] = (short)(verticesIndex + 2);
			indices[indicesIndex++] = (short)verticesIndex;
			indices[indicesIndex++] = (short)(verticesIndex + 2);
			indices[indicesIndex++] = (short)(verticesIndex + 3);
		}

		public int verticesIndex;

		public void AddIndicesReversedWinding()
		{
			indices[indicesIndex++] = (short)verticesIndex;
			indices[indicesIndex++] = (short)(verticesIndex + 2);
			indices[indicesIndex++] = (short)(verticesIndex + 1);
			indices[indicesIndex++] = (short)verticesIndex;
			indices[indicesIndex++] = (short)(verticesIndex + 3);
			indices[indicesIndex++] = (short)(verticesIndex + 2);
		}

		private bool HasSomethingToRender(Sprite sprite)
		{
			var data = sprite.Get<RenderingData>();
			drawArea = data.DrawArea;
			uv = data.AtlasUV;
			isAtlasRotated = data.IsAtlasRotated;
			screen = ScreenSpace.Current;
			rotation = sprite.Get<float>();
			return data.HasSomethingToRender;
		}

		private Rectangle drawArea;
		private Rectangle uv;
		private bool isAtlasRotated;
		private ScreenSpace screen;
		private float rotation;

		private void AddVertices(Sprite sprite)
		{
			if (isAtlasRotated && rotation == 0)
				AddVerticesAtlasRotated(sprite); //ncrunch: no coverage
			else if (isAtlasRotated)
				AddVerticesAtlasAndDrawAreaRotated(sprite, sprite.RotationCenter); //ncrunch: no coverage
			else if (rotation == 0)
				AddVerticesNotRotated(sprite);
			else
				AddVerticesRotated(sprite, sprite.RotationCenter);
		}

		//ncrunch: no coverage start
		private void AddVerticesAtlasRotated(Sprite sprite)
		{
			if (!hasUV)
			{
				var color = sprite.Color;
				verticesColor[verticesIndex++] = new VertexPosition2DColor(
					ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopLeft), color);
				verticesColor[verticesIndex++] = new VertexPosition2DColor(
					ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopRight), color);
				verticesColor[verticesIndex++] = new VertexPosition2DColor(
						ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomRight), color);
				verticesColor[verticesIndex++] = new VertexPosition2DColor(
					ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomLeft), color);
			}
			else if (hasColor)
			{
				var color = sprite.Color;
				verticesColorUV[verticesIndex++] = new VertexPosition2DColorUV(
					ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopLeft), color, uv.BottomLeft);
				verticesColorUV[verticesIndex++] = new VertexPosition2DColorUV(
					ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopRight), color, uv.TopLeft);
				verticesColorUV[verticesIndex++] = new VertexPosition2DColorUV(
					ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomRight), color, uv.TopRight);
				verticesColorUV[verticesIndex++] = new VertexPosition2DColorUV(
					ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomLeft), color, uv.BottomRight);
			}
			else
			{
				verticesUV[verticesIndex++] =
					new VertexPosition2DUV(screen.ToPixelSpaceRounded(drawArea.TopLeft), uv.BottomLeft);
				verticesUV[verticesIndex++] =
					new VertexPosition2DUV(screen.ToPixelSpaceRounded(drawArea.TopRight), uv.TopLeft);
				verticesUV[verticesIndex++] =
					new VertexPosition2DUV(screen.ToPixelSpaceRounded(drawArea.BottomRight), uv.TopRight);
				verticesUV[verticesIndex++] =
					new VertexPosition2DUV(screen.ToPixelSpaceRounded(drawArea.BottomLeft), uv.BottomRight);
			}
		}

		private void AddVerticesAtlasAndDrawAreaRotated(Sprite sprite, Vector2D rotationCenter)
		{
			float sin = MathExtensions.Sin(rotation);
			float cos = MathExtensions.Cos(rotation);
			if (!hasUV)
			{
				var color = sprite.Color;
				verticesColor[verticesIndex++] = new VertexPosition2DColor(
					ScreenSpace.Current.ToPixelSpaceRounded(
					drawArea.TopLeft.RotateAround(rotationCenter, sin, cos)), color);
				verticesColor[verticesIndex++] = new VertexPosition2DColor(
					ScreenSpace.Current.ToPixelSpaceRounded(
					drawArea.TopRight.RotateAround(rotationCenter, sin, cos)), color);
				verticesColor[verticesIndex++] = new VertexPosition2DColor(
						ScreenSpace.Current.ToPixelSpaceRounded(
						drawArea.BottomRight.RotateAround(rotationCenter, sin, cos)), color);
				verticesColor[verticesIndex++] = new VertexPosition2DColor(
					ScreenSpace.Current.ToPixelSpaceRounded(
					drawArea.BottomLeft.RotateAround(rotationCenter, sin, cos)), color);
			}
			else if (hasColor)
			{
				var color = sprite.Color;
				verticesColorUV[verticesIndex++] = new VertexPosition2DColorUV(
					ScreenSpace.Current.ToPixelSpaceRounded(
					drawArea.TopLeft.RotateAround(rotationCenter, sin, cos)), color, uv.BottomLeft);
				verticesColorUV[verticesIndex++] = new VertexPosition2DColorUV(
					ScreenSpace.Current.ToPixelSpaceRounded(
					drawArea.TopRight.RotateAround(rotationCenter, sin, cos)), color, uv.TopLeft);
				verticesColorUV[verticesIndex++] = new VertexPosition2DColorUV(
					ScreenSpace.Current.ToPixelSpaceRounded(
					drawArea.BottomRight.RotateAround(rotationCenter, sin, cos)), color, uv.TopRight);
				verticesColorUV[verticesIndex++] = new VertexPosition2DColorUV(
					ScreenSpace.Current.ToPixelSpaceRounded(
					drawArea.BottomLeft.RotateAround(rotationCenter, sin, cos)), color, uv.BottomRight);
			}
			else
			{
				verticesUV[verticesIndex++] = new VertexPosition2DUV(
					screen.ToPixelSpaceRounded(drawArea.TopLeft.RotateAround(rotationCenter, sin, cos)),
					uv.BottomLeft);
				verticesUV[verticesIndex++] = new VertexPosition2DUV(
					screen.ToPixelSpaceRounded(drawArea.TopRight.RotateAround(rotationCenter, sin, cos)),
					uv.TopLeft);
				verticesUV[verticesIndex++] = new VertexPosition2DUV(
					screen.ToPixelSpaceRounded(drawArea.BottomRight.RotateAround(rotationCenter, sin, cos)),
					uv.TopRight);
				verticesUV[verticesIndex++] = new VertexPosition2DUV(
					screen.ToPixelSpaceRounded(drawArea.BottomLeft.RotateAround(rotationCenter, sin, cos)),
					uv.BottomRight);
			}
		} //ncrunch: no coverage end

		private void AddVerticesNotRotated(Sprite sprite)
		{
			if (!hasUV)
			{
				var color = sprite.Color;
				verticesColor[verticesIndex++] = new VertexPosition2DColor(
					ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopLeft), color);
				verticesColor[verticesIndex++] = new VertexPosition2DColor(
					ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopRight), color);
				verticesColor[verticesIndex++] = new VertexPosition2DColor(
					ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomRight), color);
				verticesColor[verticesIndex++] = new VertexPosition2DColor(
					ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomLeft), color);
			}
			else if (hasColor)
			{
				var color = sprite.Color;
				verticesColorUV[verticesIndex++] = new VertexPosition2DColorUV(
					ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopLeft), color, uv.TopLeft);
				verticesColorUV[verticesIndex++] = new VertexPosition2DColorUV(
					ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopRight), color, uv.TopRight);
				verticesColorUV[verticesIndex++] = new VertexPosition2DColorUV(
					ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomRight), color, uv.BottomRight);
				verticesColorUV[verticesIndex++] = new VertexPosition2DColorUV(
					ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomLeft), color, uv.BottomLeft);
			}
			else
			{
				verticesUV[verticesIndex++] =
					new VertexPosition2DUV(screen.ToPixelSpaceRounded(drawArea.TopLeft), uv.TopLeft);
				verticesUV[verticesIndex++] =
					new VertexPosition2DUV(screen.ToPixelSpaceRounded(drawArea.TopRight), uv.TopRight);
				verticesUV[verticesIndex++] =
					new VertexPosition2DUV(screen.ToPixelSpaceRounded(drawArea.BottomRight), uv.BottomRight);
				verticesUV[verticesIndex++] =
					new VertexPosition2DUV(screen.ToPixelSpaceRounded(drawArea.BottomLeft), uv.BottomLeft);
			}
		}

		private void AddVerticesRotated(Sprite sprite, Vector2D rotationCenter)
		{
			float sin = MathExtensions.Sin(rotation);
			float cos = MathExtensions.Cos(rotation);
			if (!hasUV)
			{
				var color = sprite.Color;
				verticesColor[verticesIndex++] = new VertexPosition2DColor(
					ScreenSpace.Current.ToPixelSpaceRounded(
					drawArea.TopLeft.RotateAround(rotationCenter, sin, cos)), color);
				verticesColor[verticesIndex++] = new VertexPosition2DColor(
					ScreenSpace.Current.ToPixelSpaceRounded(
					drawArea.TopRight.RotateAround(rotationCenter, sin, cos)), color);
				verticesColor[verticesIndex++] = new VertexPosition2DColor(
						ScreenSpace.Current.ToPixelSpaceRounded(
						drawArea.BottomRight.RotateAround(rotationCenter, sin, cos)), color);
				verticesColor[verticesIndex++] = new VertexPosition2DColor(
					ScreenSpace.Current.ToPixelSpaceRounded(
					drawArea.BottomLeft.RotateAround(rotationCenter, sin, cos)), color);
			}
			else if (hasColor)
			{
				var color = sprite.Color;
				verticesColorUV[verticesIndex++] = new VertexPosition2DColorUV(
					ScreenSpace.Current.ToPixelSpaceRounded(
					drawArea.TopLeft.RotateAround(rotationCenter, sin, cos)), color, uv.TopLeft);
				verticesColorUV[verticesIndex++] = new VertexPosition2DColorUV(
					ScreenSpace.Current.ToPixelSpaceRounded(
					drawArea.TopRight.RotateAround(rotationCenter, sin, cos)), color, uv.TopRight);
				verticesColorUV[verticesIndex++] = new VertexPosition2DColorUV(
					ScreenSpace.Current.ToPixelSpaceRounded(
					drawArea.BottomRight.RotateAround(rotationCenter, sin, cos)), color, uv.BottomRight);
				verticesColorUV[verticesIndex++] = new VertexPosition2DColorUV(
					ScreenSpace.Current.ToPixelSpaceRounded(
					drawArea.BottomLeft.RotateAround(rotationCenter, sin, cos)), color, uv.BottomLeft);
			}
			else
			{
				verticesUV[verticesIndex++] = new VertexPosition2DUV(
					screen.ToPixelSpaceRounded(drawArea.TopLeft.RotateAround(rotationCenter, sin, cos)),
					uv.TopLeft);
				verticesUV[verticesIndex++] = new VertexPosition2DUV(
					screen.ToPixelSpaceRounded(drawArea.TopRight.RotateAround(rotationCenter, sin, cos)),
					uv.TopRight);
				verticesUV[verticesIndex++] = new VertexPosition2DUV(
					screen.ToPixelSpaceRounded(drawArea.BottomRight.RotateAround(rotationCenter, sin, cos)),
					uv.BottomRight);
				verticesUV[verticesIndex++] = new VertexPosition2DUV(
					screen.ToPixelSpaceRounded(drawArea.BottomLeft.RotateAround(rotationCenter, sin, cos)),
					uv.BottomLeft);
			}
		}

		public void Draw(Drawing drawing)
		{
			if (indicesIndex == 0)
				return;
			if (verticesUV != null)
				drawing.Add(material, blendMode, verticesUV, indices, verticesIndex, indicesIndex);
			else if (verticesColorUV != null)
				drawing.Add(material, blendMode, verticesColorUV, indices, verticesIndex, indicesIndex);
			else if (verticesColor != null)
				drawing.Add(material, blendMode, verticesColor, indices, verticesIndex, indicesIndex);
		}

		public void Reset()
		{
			verticesIndex = 0;
			indicesIndex = 0;
		}
	}
}