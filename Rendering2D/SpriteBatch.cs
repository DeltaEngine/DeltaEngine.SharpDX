using DeltaEngine.Content;
using DeltaEngine.Datatypes;
using DeltaEngine.Graphics;
using DeltaEngine.Graphics.Vertices;
using DeltaEngine.ScreenSpaces;

namespace DeltaEngine.Rendering2D
{
	internal class SpriteBatch
	{
		public SpriteBatch(SpriteBatchKey key)
		{
			this.key = key;
		}

		public readonly SpriteBatchKey key;

		public void Reset()
		{
			verticesUVBatchIndex = 0;
			indicesUVIndex = 0;
			verticesUVColorBatchIndex = 0;
			indicesUVColorIndex = 0;
		}

		private int verticesUVBatchIndex;
		private int indicesUVIndex;
		private int verticesUVColorBatchIndex;
		private int indicesUVColorIndex;

		public bool AreBuffersFull()
		{
			return short.MaxValue - indicesUVIndex < IndicesPerSprite;
		}

		private const int IndicesPerSprite = 6;

		public bool AreColorBuffersFull()
		{
			return short.MaxValue - indicesUVColorIndex < IndicesPerSprite;
		}

		public void AddVerticesAndIndices(Sprite sprite)
		{
			indicesUVBatch[indicesUVIndex ++] = (short)verticesUVBatchIndex;
			indicesUVBatch[indicesUVIndex ++] = (short)(verticesUVBatchIndex + 1);
			indicesUVBatch[indicesUVIndex ++] = (short)(verticesUVBatchIndex + 2);
			indicesUVBatch[indicesUVIndex ++] = (short)verticesUVBatchIndex;
			indicesUVBatch[indicesUVIndex ++] = (short)(verticesUVBatchIndex + 2);
			indicesUVBatch[indicesUVIndex ++] = (short)(verticesUVBatchIndex + 3);
			AddVertices(sprite);
		}

		private readonly short[] indicesUVBatch = new short[short.MaxValue];

		private void AddVertices(Sprite sprite)
		{
			if (HasSomethingToRender(sprite))
				if (isAtlasRotated && rotation == 0)
					AddVerticesAtlasRotated();
				else if (isAtlasRotated)
					AddVerticesAtlasAndDrawAreaRotated(sprite.RotationCenter);
				else if (rotation == 0)
					AddVerticesNotRotated();
				else
					AddVerticesRotated(sprite.RotationCenter);
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

		private void AddVerticesAtlasRotated()
		{
			verticesUVBatch[verticesUVBatchIndex++] =
				new VertexPosition2DUV(screen.ToPixelSpaceRounded(drawArea.TopLeft), uv.BottomLeft);
			verticesUVBatch[verticesUVBatchIndex++] =
				new VertexPosition2DUV(screen.ToPixelSpaceRounded(drawArea.TopRight), uv.TopLeft);
			verticesUVBatch[verticesUVBatchIndex++] =
				new VertexPosition2DUV(screen.ToPixelSpaceRounded(drawArea.BottomRight), uv.TopRight);
			verticesUVBatch[verticesUVBatchIndex++] =
				new VertexPosition2DUV(screen.ToPixelSpaceRounded(drawArea.BottomLeft), uv.BottomRight);
		}

		private readonly VertexPosition2DUV[] verticesUVBatch =
			new VertexPosition2DUV[short.MaxValue * VerticesPerSprite / IndicesPerSprite];

		private const int VerticesPerSprite = 4;

		private void AddVerticesAtlasAndDrawAreaRotated(Vector2D rotationCenter)
		{
			verticesUVBatch[verticesUVBatchIndex++] = new VertexPosition2DUV(
					screen.ToPixelSpaceRounded(drawArea.TopLeft.RotateAround(rotationCenter, rotation)),
					uv.BottomLeft);
			verticesUVBatch[verticesUVBatchIndex++] = new VertexPosition2DUV(
					screen.ToPixelSpaceRounded(drawArea.TopRight.RotateAround(rotationCenter, rotation)),
					uv.TopLeft);
			verticesUVBatch[verticesUVBatchIndex++] = new VertexPosition2DUV(
					screen.ToPixelSpaceRounded(drawArea.BottomRight.RotateAround(rotationCenter, rotation)),
					uv.TopRight);
			verticesUVBatch[verticesUVBatchIndex++] = new VertexPosition2DUV(
					screen.ToPixelSpaceRounded(drawArea.BottomLeft.RotateAround(rotationCenter, rotation)),
					uv.BottomRight);
		}

		private void AddVerticesNotRotated()
		{
			verticesUVBatch[verticesUVBatchIndex++] =
				new VertexPosition2DUV(screen.ToPixelSpaceRounded(drawArea.TopLeft), uv.TopLeft);
			verticesUVBatch[verticesUVBatchIndex++] =
				new VertexPosition2DUV(screen.ToPixelSpaceRounded(drawArea.TopRight), uv.TopRight);
			verticesUVBatch[verticesUVBatchIndex++] =
				new VertexPosition2DUV(screen.ToPixelSpaceRounded(drawArea.BottomRight), uv.BottomRight);
			verticesUVBatch[verticesUVBatchIndex++] =
				new VertexPosition2DUV(screen.ToPixelSpaceRounded(drawArea.BottomLeft), uv.BottomLeft);
		}

		private void AddVerticesRotated(Vector2D rotationCenter)
		{
			verticesUVBatch[verticesUVBatchIndex++] =
				new VertexPosition2DUV(
					screen.ToPixelSpaceRounded(drawArea.TopLeft.RotateAround(rotationCenter, rotation)),
					uv.TopLeft);
			verticesUVBatch[verticesUVBatchIndex++] =
				new VertexPosition2DUV(
					screen.ToPixelSpaceRounded(drawArea.TopRight.RotateAround(rotationCenter, rotation)),
					uv.TopRight);
			verticesUVBatch[verticesUVBatchIndex++] =
				new VertexPosition2DUV(
					screen.ToPixelSpaceRounded(drawArea.BottomRight.RotateAround(rotationCenter, rotation)),
					uv.BottomRight);
			verticesUVBatch[verticesUVBatchIndex++] =
				new VertexPosition2DUV(
					screen.ToPixelSpaceRounded(drawArea.BottomLeft.RotateAround(rotationCenter, rotation)),
					uv.BottomLeft);
		}

		public void AddColorVerticesAndIndices(Sprite sprite)
		{
			indicesUVColorBatch[indicesUVColorIndex++] = (short)verticesUVColorBatchIndex;
			indicesUVColorBatch[indicesUVColorIndex++] = (short)(verticesUVColorBatchIndex + 1);
			indicesUVColorBatch[indicesUVColorIndex++] = (short)(verticesUVColorBatchIndex + 2);
			indicesUVColorBatch[indicesUVColorIndex++] = (short)verticesUVColorBatchIndex;
			indicesUVColorBatch[indicesUVColorIndex++] = (short)(verticesUVColorBatchIndex + 2);
			indicesUVColorBatch[indicesUVColorIndex++] = (short)(verticesUVColorBatchIndex + 3);
			AddColorVertices(sprite);
		}

		private readonly short[] indicesUVColorBatch = new short[short.MaxValue];

		private void AddColorVertices(Sprite sprite)
		{
			if (HasSomethingToRender(sprite))
				if (isAtlasRotated && rotation == 0)
					AddColorVerticesAtlasRotated(sprite.Color);
				else if (isAtlasRotated)
					AddColorVerticesAtlasAndDrawAreaRotated(sprite.Color, sprite.RotationCenter);
				else if (rotation == 0)
					AddColorVerticesNotRotated(sprite.Color);
				else
					AddColorVerticesRotated(sprite.Color, sprite.RotationCenter);
		}

		private void AddColorVerticesAtlasRotated(Color color)
		{
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopLeft),
					color, uv.BottomLeft);
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopRight),
					color, uv.TopLeft);
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomRight),
					color, uv.TopRight);
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomLeft),
					color, uv.BottomRight);
		}

		private void AddColorVerticesAtlasAndDrawAreaRotated(Color color, Vector2D rotationCenter)
		{
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(
					screen.ToPixelSpaceRounded(drawArea.TopLeft.RotateAround(rotationCenter, rotation)), color,
					uv.BottomLeft);
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(
					screen.ToPixelSpaceRounded(drawArea.TopRight.RotateAround(rotationCenter, rotation)),
					color, uv.TopLeft);
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(
					screen.ToPixelSpaceRounded(drawArea.BottomRight.RotateAround(rotationCenter, rotation)),
					color, uv.TopRight);
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(
					screen.ToPixelSpaceRounded(drawArea.BottomLeft.RotateAround(rotationCenter, rotation)),
					color, uv.BottomRight);
		}

		private void AddColorVerticesNotRotated(Color color)
		{
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopLeft),
					color, uv.TopLeft);
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(ScreenSpace.Current.ToPixelSpaceRounded(drawArea.TopRight),
					color, uv.TopRight);
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomRight),
					color, uv.BottomRight);
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(ScreenSpace.Current.ToPixelSpaceRounded(drawArea.BottomLeft),
					color, uv.BottomLeft);
		}

		private readonly VertexPosition2DColorUV[] verticesUVColorBatch =
			new VertexPosition2DColorUV[short.MaxValue * VerticesPerSprite / IndicesPerSprite];

		private void AddColorVerticesRotated(Color color, Vector2D rotationCenter)
		{
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(
					screen.ToPixelSpaceRounded(drawArea.TopLeft.RotateAround(rotationCenter, rotation)), color,
					uv.TopLeft);
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(
					screen.ToPixelSpaceRounded(drawArea.TopRight.RotateAround(rotationCenter, rotation)),
					color, uv.TopRight);
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(
					screen.ToPixelSpaceRounded(drawArea.BottomRight.RotateAround(rotationCenter, rotation)),
					color, uv.BottomRight);
			verticesUVColorBatch[verticesUVColorBatchIndex++] =
				new VertexPosition2DColorUV(
					screen.ToPixelSpaceRounded(drawArea.BottomLeft.RotateAround(rotationCenter, rotation)),
					color, uv.BottomLeft);
		}

		public void Draw(Drawing drawing)
		{
			if (verticesUVBatchIndex > 0)
				drawing.Add(key.Material, key.BlendMode, verticesUVBatch, indicesUVBatch,
					verticesUVBatchIndex, indicesUVIndex);
			if (verticesUVColorBatchIndex > 0)
				drawing.Add(key.Material, key.BlendMode, verticesUVColorBatch, indicesUVColorBatch,
					verticesUVColorBatchIndex, indicesUVColorIndex);
		}
	}
}